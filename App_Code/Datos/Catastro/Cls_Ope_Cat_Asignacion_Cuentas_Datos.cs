using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Operacion_Cat_Asignacion_Cuentas.Negocio;
using System.Data.OracleClient;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Ope_Cat_Asignacion_Cuentas_Datos
/// </summary>

namespace Presidencia.Operacion_Cat_Asignacion_Cuentas.Datos
{
    public class Cls_Ope_Cat_Asignacion_Cuentas_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuentas_Prediales
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Cuentas_Prediales(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += "'ALTA' AS ACCION, ";
                Mi_SQL += "CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += "COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                Mi_SQL += "CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE_NOTIFICACION, ";
                Mi_SQL += "COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA_NOTIFICACION, ";
                Mi_SQL += "ESTADOS." + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO_CUENTA, ";
                Mi_SQL += "TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS TIPO_PREDIO, ";
                Mi_SQL += "CIUDADES." + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD_CUENTA, ";
                Mi_SQL += "PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS PROPIETARIO_ID, ";
                Mi_SQL += "(PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_PROPIETARIO, ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + " ";
                Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPOS_PREDIO ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " ESTADOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CIUDADES ON ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " = CIUDADES." + Cat_Pre_Ciudades.Campo_Estado_ID + " ";
                Mi_SQL += " AND CIUDADES." + Cat_Pre_Ciudades.Campo_Ciudad_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " PROPIETARIOS ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                Mi_SQL += " WHERE ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " IN ";
                Mi_SQL += "(SELECT CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA WHERE ";
                Mi_SQL += "CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + " IS NOT NULL AND ";
                if (Cuentas.P_Efecto_Anio != null && Cuentas.P_Efecto_Anio.Trim() != "")
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))<(" + Cuentas.P_Efecto_Anio + ")) OR ";
                }
                else 
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))<(TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')))) OR ";
                }



                if (Cuentas.P_Efecto_Anio != null && Cuentas.P_Efecto_Anio.Trim() != "")
                {
                    Mi_SQL += "((TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))=(" + Cuentas.P_Efecto_Anio + ")) AND ";
                }
                else 
                {
                    Mi_SQL += "((TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))=(TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')))) AND ";
                }


                if (Cuentas.P_Efecto_Bimestre != null && Cuentas.P_Efecto_Bimestre.Trim() != "")
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", 0, 1))=(" + Cuentas.P_Efecto_Bimestre + "))) AND ";
                }
                else 
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", 0, 1))=(TO_NUMBER(TO_CHAR(SYSDATE,'MM'))/2))) AND ";
                }
                Mi_SQL += "CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " NOT IN ";
                Mi_SQL += "(SELECT " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " WHERE " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_Estatus + " IN ('VIGENTE','PROCESO')) AND ROWNUM<100) AND ";
                if (Cuentas.P_Calle != null && Cuentas.P_Calle.Trim() != "")
                {
                    Mi_SQL += " CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " LIKE '%" + Cuentas.P_Calle + "%' AND ";
                }
                if (Cuentas.P_Colonia != null && Cuentas.P_Colonia.Trim() != "")
                {
                    Mi_SQL += " COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Cuentas.P_Colonia + "%' AND ";
                }
                if (Cuentas.P_No_Ext != null && Cuentas.P_No_Ext.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " LIKE '%" + Cuentas.P_No_Ext + "%' AND ";
                }
                if (Cuentas.P_No_Int != null && Cuentas.P_No_Int.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " LIKE '%" + Cuentas.P_No_Int + "%' AND ";
                }
                if (Cuentas.P_Cuenta_Predial != null && Cuentas.P_Cuenta_Predial.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Cuentas.P_Cuenta_Predial + "%' AND ";
                }
                if (Cuentas.P_Tipo_Predio != null && Cuentas.P_Tipo_Predio.Trim() != "")
                {
                    Mi_SQL += " TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " = '" + Cuentas.P_Tipo_Predio + "' AND ";
                }


               // Mi_SQL += "(";
                if (Cuentas.P_Superficie_Terreno != null && Cuentas.P_Superficie_Terreno.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " >= " + Cuentas.P_Superficie_Terreno + " AND ";
                }
                if (Cuentas.P_Superficie_Terreno_Menor != null && Cuentas.P_Superficie_Terreno_Menor.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " <= " + Cuentas.P_Superficie_Terreno_Menor + " AND ";
                }
               

                if (Cuentas.P_Propietario != null && Cuentas.P_Propietario.Trim() != "")
                {

             
                    Mi_SQL += " (PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") LIKE '%" + Cuentas.P_Propietario + "%' AND ";
                }

                //Mi_SQL += "(";
                if (Cuentas.P_Superficie_Construccion != null && Cuentas.P_Superficie_Construccion.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " >= " + Cuentas.P_Superficie_Construccion + " AND ";
                }
                if (Cuentas.P_Superficie_Construccion_Menor != null && Cuentas.P_Superficie_Construccion_Menor.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " <= " + Cuentas.P_Superficie_Construccion_Menor + " AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                //Mi_SQL += ")";


                if (Mi_SQL.EndsWith("()"))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                }


                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " ASC, CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " ASC, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " ASC, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " ASC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Consultar_Claves_Catastrales: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuentas_Asignadas
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que han sido asignadas
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio Cls_Ope_Cat_Asignacion_Cuentas_Negocio 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Cuentas_Asignadas(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion+", ";
                Mi_SQL += "CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += "COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                Mi_SQL += "CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE_NOTIFICACION, ";
                Mi_SQL += "COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA_NOTIFICACION, ";
                Mi_SQL += "ESTADOS." + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO_CUENTA, ";
                Mi_SQL += "CIUDADES." + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD_CUENTA, ";
                Mi_SQL += "PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS PROPIETARIO_ID, ";
                Mi_SQL += "(PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_PROPIETARIO, ";
                Mi_SQL += "(EMPLEADO." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || EMPLEADO." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || EMPLEADO." + Cat_Empleados.Campo_Nombre + ") AS PERITO_INTERNO, ";
                Mi_SQL += Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + "." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " AS PERITO_INTERNO_ID, ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + " ";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " ON " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " ON " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " = " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + "." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADO ON " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + "." + Cat_Cat_Peritos_Internos.Campo_Empleado_Id + " = EMPLEADO." + Cat_Empleados.Campo_Empleado_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPOS_PREDIO ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " ESTADOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CIUDADES ON ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " = CIUDADES." + Cat_Pre_Ciudades.Campo_Estado_ID + " ";
                Mi_SQL += " AND CIUDADES." + Cat_Pre_Ciudades.Campo_Ciudad_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " PROPIETARIOS ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                Mi_SQL += " WHERE ";
                if (Cuentas.P_Calle != null && Cuentas.P_Calle.Trim() != "")
                {
                    Mi_SQL += " CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " LIKE '%" + Cuentas.P_Calle + "%' AND ";
                }
                if (Cuentas.P_Colonia != null && Cuentas.P_Colonia.Trim() != "")
                {
                    Mi_SQL += " COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Cuentas.P_Colonia + "%' AND ";
                }
                if (Cuentas.P_Perito != null && Cuentas.P_Perito.Trim() != "")
                {
                    Mi_SQL += "(EMPLEADO." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || EMPLEADO." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || EMPLEADO." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Cuentas.P_Perito + "%' AND ";
                }
                if (Cuentas.P_Cuenta_Predial != null && Cuentas.P_Cuenta_Predial.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Cuentas.P_Cuenta_Predial + "%' AND ";
                }
                if (Cuentas.P_Perito_Interno_Id != null && Cuentas.P_Perito_Interno_Id.Trim() != "")
                {
                    Mi_SQL += " " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " = '" + Cuentas.P_Perito_Interno_Id + "' AND ";
                }
                if (Cuentas.P_Avaluo)
                {
                    Mi_SQL += " " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " NOT IN (SELECT AVALUO." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AVALUO WHERE AVALUO."+Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion+" IS NOT NULL) AND ";
                    Mi_SQL += " " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " NOT IN (SELECT AVALUO_U." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AVALUO_U WHERE AVALUO_U." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " IS NOT NULL) AND ";
                }
                if (Cuentas.P_Tipo_Predio != null && Cuentas.P_Tipo_Predio.Trim() != "")
                {
                    Mi_SQL += " TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " = '" + Cuentas.P_Tipo_Predio + "' AND ";
                }
                if (Cuentas.P_Propietario != null && Cuentas.P_Propietario.Trim() != "")
                {
                    Mi_SQL += " (PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") LIKE '%" + Cuentas.P_Propietario + "%' AND ";
                }
                //Mi_SQL += "ROWNUM<30";
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " ASC, CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " ASC, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " ASC, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " ASC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Consultar_Cuentas_Asignadas: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Cuenta_Asignada
        ///DESCRIPCIÓN: Da de alta en la Base de Datos los valores del I.N.P.R.
        ///PARAMENTROS:     
        ///             1. Cuentas.       Instancia de la Clase de Negocio de los valores I.n.p.r.
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Cuenta_Asignada(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            String No_Asignacion = "";
            No_Asignacion = Obtener_ID_Consecutivo(Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas, "TO_NUMBER(" + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ")", "", 10);
            try
            {
                foreach (DataRow Dr_Renglon in Cuentas.P_Dt_Cuentas.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "(";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ", ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + ", ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ", ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Estatus + ", ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Asignacion + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id].ToString() + "', '";
                        Mi_sql += Cuentas.P_Perito_Interno_Id + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Asignacion_Cuentas.Campo_Estatus].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Asignacion = (Convert.ToDouble(No_Asignacion) + 1).ToString("0000000000");
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Valor_Inpr: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Cuenta_Asignada
        ///DESCRIPCIÓN: Da de alta en la Base de Datos los valores del I.N.P.R.
        ///PARAMENTROS:     
        ///             1. Cuentas.       Instancia de la Clase de Negocio de los valores I.n.p.r.
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Entregas(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            String Folio_Predial = "";
            Folio_Predial = Obtener_ID_Consecutivo(Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas, Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial, Ope_Cat_Asignacion_Cuentas.Campo_Anio + "=" + Cuentas.P_Anio + " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + "='" + Cuentas.P_No_Entrega + "' ", 10);
            try
            {
                foreach (DataRow Dr_Renglon in Cuentas.P_Dt_Cuentas.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "UPDATE " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " SET ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial + "= '"+ Folio_Predial +"', ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= " + Cuentas.P_Anio + ", ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Estatus + "= 'DETERMINACION', ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + "='"+Cuentas.P_No_Entrega+"', ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Fecha_Modifico + "=SYSDATE ";
                        Mi_sql += " WHERE " + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + "='" + Dr_Renglon[Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Folio_Predial = (Convert.ToDouble(Folio_Predial) + 1).ToString("0000000000");
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Entregas: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Motivo_Avaluo
        ///DESCRIPCIÓN: Modifica un motivo de Avaluo
        ///PARAMENTROS:     
        ///             1. Motivo_Avaluo.   Instancia de la Clase de Negocio de Motivos de Avaluo 
        ///                                 con los datos del que van a ser
        ///                                 modificado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Cuenta_Asignada(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuenta)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            try
            {
                Mi_sql = "UPDATE " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas;
                Mi_sql += " SET " + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " = '" + Cuenta.P_Perito_Interno_Id + "', ";
                Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Asignacion_Cuentas.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = '" + Cuenta.P_No_Asignacion + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Error al intentar Reasignar el perito interno [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuentas_Prediales
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Cuentas_Entregar(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += "'ALTA' AS ACCION, ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Estatus + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_Estatus + ", ";
                //////////  Consultar folio de catastro
                Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                Mi_SQL += " THEN 'AR' ELSE ";
                Mi_SQL += "'AU' END AS FOLIO_CATASTRO_TIPO, ";

                Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                Mi_SQL += " THEN (SELECT TO_NUMBER(AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo + ") ||'/'||TO_NUMBER(AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + ") FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") ELSE ";
                Mi_SQL += "(SELECT TO_NUMBER(AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ") ||'/'||TO_NUMBER(AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + ") FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") END AS FOLIO_CATASTRO, ";
                ///////////
                Mi_SQL += "CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += "COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                Mi_SQL += "CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE_NOTIFICACION, ";
                Mi_SQL += "COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA_NOTIFICACION, ";
                Mi_SQL += "ESTADOS." + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO_CUENTA, ";
                Mi_SQL += "TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS TIPO_PREDIO, ";
                Mi_SQL += "CIUDADES." + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD_CUENTA, ";
                Mi_SQL += "PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS PROPIETARIO_ID, ";
                Mi_SQL += "(PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_PROPIETARIO, ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + " ";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " CUENTA ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " ON CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPOS_PREDIO ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " ESTADOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CIUDADES ON ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " = CIUDADES." + Cat_Pre_Ciudades.Campo_Estado_ID + " ";
                Mi_SQL += " AND CIUDADES." + Cat_Pre_Ciudades.Campo_Ciudad_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " PROPIETARIOS ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                Mi_SQL += " WHERE ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " IN ";
                Mi_SQL += "(SELECT CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA WHERE ";
                Mi_SQL += "CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + " IS NOT NULL AND ";
                if (Cuentas.P_Efecto_Anio != null && Cuentas.P_Efecto_Anio.Trim() != "")
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))<(" + Cuentas.P_Efecto_Anio + ")) OR ";
                }
                else
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))<(TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')))) OR ";
                }



                if (Cuentas.P_Efecto_Anio != null && Cuentas.P_Efecto_Anio.Trim() != "")
                {
                    Mi_SQL += "((TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))=(" + Cuentas.P_Efecto_Anio + ")) AND ";
                }
                else
                {
                    Mi_SQL += "((TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))=(TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')))) AND ";
                }


                if (Cuentas.P_Efecto_Bimestre != null && Cuentas.P_Efecto_Bimestre.Trim() != "")
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", 0, 1))=(" + Cuentas.P_Efecto_Bimestre + "))) AND ";
                }
                else
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", 0, 1))=(TO_NUMBER(TO_CHAR(SYSDATE,'MM'))/2))) AND ";
                }
                Mi_SQL += "CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " NOT IN ";
                Mi_SQL += "(SELECT " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " WHERE " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_Estatus + " IN ('VIGENTE','PROCESO')) AND ROWNUM<100) AND ";
                if (Cuentas.P_Calle != null && Cuentas.P_Calle.Trim() != "")
                {
                    Mi_SQL += " CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " LIKE '%" + Cuentas.P_Calle + "%' AND ";
                }
                if (Cuentas.P_Colonia != null && Cuentas.P_Colonia.Trim() != "")
                {
                    Mi_SQL += " COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Cuentas.P_Colonia + "%' AND ";
                }
                if (Cuentas.P_No_Ext != null && Cuentas.P_No_Ext.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " LIKE '%" + Cuentas.P_No_Ext + "%' AND ";
                }
                if (Cuentas.P_No_Int != null && Cuentas.P_No_Int.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " LIKE '%" + Cuentas.P_No_Int + "%' AND ";
                }
                if (Cuentas.P_Cuenta_Predial != null && Cuentas.P_Cuenta_Predial.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Cuentas.P_Cuenta_Predial + "%' AND ";
                }
                if (Cuentas.P_Tipo_Predio != null && Cuentas.P_Tipo_Predio.Trim() != "")
                {
                    Mi_SQL += " TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " = '" + Cuentas.P_Tipo_Predio + "' AND ";
                }
                if (Cuentas.P_Anio_Nulo)
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " IS NULL AND ";
                }
                else
                {
                    if (Cuentas.P_Anio != null && Cuentas.P_Anio.Trim() != "")
                    {
                        Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " = " + Cuentas.P_Anio + " AND ";
                    }
                }
                if (Cuentas.P_No_Entrega != null && Cuentas.P_No_Entrega.Trim() != "")
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " = '" + Cuentas.P_No_Entrega + "' AND ";
                }
                if (Cuentas.P_Estatus != null && Cuentas.P_Estatus.Trim() != "")
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Estatus + " " + Cuentas.P_Estatus + " AND ";
                }
                // Mi_SQL += "(";
                if (Cuentas.P_Superficie_Terreno != null && Cuentas.P_Superficie_Terreno.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " >= " + Cuentas.P_Superficie_Terreno + " AND ";
                }
                if (Cuentas.P_Superficie_Terreno_Menor != null && Cuentas.P_Superficie_Terreno_Menor.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " <= " + Cuentas.P_Superficie_Terreno_Menor + " AND ";
                }


                if (Cuentas.P_Propietario != null && Cuentas.P_Propietario.Trim() != "")
                {


                    Mi_SQL += " (PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") LIKE '%" + Cuentas.P_Propietario + "%' AND ";
                }

                //Mi_SQL += "(";
                if (Cuentas.P_Superficie_Construccion != null && Cuentas.P_Superficie_Construccion.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " >= " + Cuentas.P_Superficie_Construccion + " AND ";
                }
                if (Cuentas.P_Superficie_Construccion_Menor != null && Cuentas.P_Superficie_Construccion_Menor.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " <= " + Cuentas.P_Superficie_Construccion_Menor + " AND ";
                }

                Mi_SQL += "(CUENTA."+Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion +" IN (SELECT AR."+Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion+" FROM "+Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V+" AR WHERE AR."+Ope_Cat_Avaluo_Rustico_V.Campo_Estatus+"='AUTORIZADO') OR ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AR WHERE AR." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "='AUTORIZADO')) AND ";

                if (Cuentas.P_Max_Reg != null && Cuentas.P_Max_Reg.Trim() != "")
                {
                    Mi_SQL += " ROWNUM<=" + Cuentas.P_Max_Reg.Trim() + " AND ";
                }

                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                //Mi_SQL += ")";


                if (Mi_SQL.EndsWith("()"))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                }


                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " ASC, CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " ASC, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " ASC, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " ASC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Consultar_Claves_Catastrales: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, String Filtro, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                if (Filtro != "" && Filtro != null)
                {
                    Mi_SQL += " WHERE " + Filtro;
                }
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (Exception Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Avaluos_Fecha
        ///DESCRIPCIÓN: Modificara la fecha de los avaluos que ya esten vencidos por fecha.
        ///PARAMETROS:     
        ///            
        ///CREO: Alejandro Leyva Alvarado.
        ///FECHA_CREO: 18/Octubre/2012 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static Boolean Modificar_Avaluos_Fecha(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuenta)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            try
            {
                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V;
                Mi_sql += " SET " + Ope_Cat_Avaluo_Rustico_V.Campo_Fecha_Creo + " = '" + Cuenta.P_Fecha_Avaluo.ToString("d-M-yyyy") + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_V.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_V.Campo_Fecha_Modifico + " = SYSDATE";
                if (Cuenta.P_Folio_Inicial != null && Cuenta.P_Folio_Inicial.Trim() != "")
                {
                    Mi_sql += " WHERE TO_NUMBER(" + Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo + ") >= " + Cuenta.P_Folio_Inicial + " AND ";
                }

                if (Cuenta.P_Folio_Final != null && Cuenta.P_Folio_Final.Trim() != "")
                {
                    Mi_sql += " TO_NUMBER(" + Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo + ") <= " + Cuenta.P_Folio_Final + " AND ";
                }
                if (Cuenta.P_Anio_Avaluo != null && Cuenta.P_Anio_Avaluo.Trim() != "")
                {
                    Mi_sql += " " + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + " = " + Cuenta.P_Anio_Avaluo + " ";
                }
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av;
                Mi_sql += " SET " + Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Creo + " = '" + Cuenta.P_Fecha_Avaluo.ToString("d-M-yyyy") + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Modifico + " = SYSDATE";
                if (Cuenta.P_Folio_Inicial != null && Cuenta.P_Folio_Inicial.Trim() != "")
                {
                    Mi_sql += " WHERE TO_NUMBER(" + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ") >= " + Cuenta.P_Folio_Inicial + " AND ";
                }

                if (Cuenta.P_Folio_Final != null && Cuenta.P_Folio_Final.Trim() != "")
                {
                    Mi_sql += " TO_NUMBER(" + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ") <= " + Cuenta.P_Folio_Final + " AND ";
                }
                if (Cuenta.P_Anio_Avaluo != null && Cuenta.P_Anio_Avaluo.Trim() != "")
                {
                    Mi_sql += " " + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + " = " + Cuenta.P_Anio_Avaluo + " ";
                }
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Error al intentar Modificar los avaluos [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuentas_Determinaciones
        ///DESCRIPCIÓN: Consulta las tablas para las determinaciones.
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Asignación de Cuentas
        ///                                 con los datos que servirán de filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 19/Oct/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Cuentas_Determinaciones(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += "'ALTA' AS ACCION, ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ", ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Estatus + " AS " + Ope_Cat_Asignacion_Cuentas.Campo_Estatus + ", ";
                //////////  Consultar folio de catastro
                Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                Mi_SQL += " THEN 'RUSTICO' ELSE ";
                Mi_SQL += "'URBANO' END AS FOLIO_CATASTRO_TIPO, ";

                Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                Mi_SQL += " THEN (SELECT TO_NUMBER(AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo + ") ||'/'||TO_NUMBER(AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + ") FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") ELSE ";
                Mi_SQL += "(SELECT TO_NUMBER(AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ") ||'/'||TO_NUMBER(AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + ") FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") END AS FOLIO_CATASTRO, ";

                Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                Mi_SQL += " THEN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Valor_Total_Predio + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") ELSE ";
                Mi_SQL += "(SELECT AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_Total_Predio + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") END AS VALOR_PREDIO, ";
                ///////////
                Mi_SQL += "CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += "COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                Mi_SQL += "CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE_NOTIFICACION, ";
                Mi_SQL += "COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA_NOTIFICACION, ";
                Mi_SQL += "ESTADOS." + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO_CUENTA, ";
                Mi_SQL += "TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS TIPO_PREDIO, ";
                Mi_SQL += "CIUDADES." + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD_CUENTA, ";
                Mi_SQL += "PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS PROPIETARIO_ID, ";
                Mi_SQL += "(PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_PROPIETARIO, ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", ";
                Mi_SQL += "SUBSTR(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3) AS ANIO_EFECTOS, ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + " ";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " CUENTA ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " ON CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPOS_PREDIO ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " ESTADOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CIUDADES ON ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " = CIUDADES." + Cat_Pre_Ciudades.Campo_Estado_ID + " ";
                Mi_SQL += " AND CIUDADES." + Cat_Pre_Ciudades.Campo_Ciudad_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " PROPIETARIOS ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                Mi_SQL += " WHERE ";
                if (Cuentas.P_Anio != null && Cuentas.P_Anio.Trim() != "")
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " = " + Cuentas.P_Anio + " AND ";
                }
                if (Cuentas.P_No_Entrega != null && Cuentas.P_No_Entrega.Trim() != "")
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " = '" + Cuentas.P_No_Entrega + "' AND ";
                }

                Mi_SQL += "(CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "='AUTORIZADO') OR ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AR WHERE AR." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "='AUTORIZADO')) AND ";

                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                //Mi_SQL += ")";


                if (Mi_SQL.EndsWith("()"))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                }


                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial + " ASC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }


                Mi_SQL = "(SELECT VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Referencia
                   + ", TO_NUMBER(VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_No_Avaluo + ")|| '/'||VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Anio_Avaluo + " AS FOLIO_CATASTRO"
                   + ", 'URBANO' AS FOLIO_CATASTRO_TIPO"
                   + ", VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Superficie_M2
                   + ", VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Factor
                   + ", VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Parcial
                   + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_M2 + ",0.00) AS VALOR_M2"
                   + " FROM  " + Ope_Cat_Calc_Valor_Const_Av.Tabla_Ope_Cat_Calc_Valor_Const_Av + " VC"
                   + " LEFT OUTER JOIN " + Cat_Cat_Tab_Val_Construccion.Tabla_Cat_Cat_Tab_Val_Construccion + " TV"
                   + " ON VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Construccion_Id + "=TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_Construccion_Id
                   + " LEFT OUTER JOIN " + Cat_Cat_Calidad_Construccion.Tabla_Cat_Cat_Calidad_Construccion + " CC"
                   + " ON TV." + Cat_Cat_Tab_Val_Construccion.Campo_Calidad_Id + "=CC." + Cat_Cat_Calidad_Construccion.Campo_Calidad_Id
                   + " WHERE TO_NUMBER("
                   + Ope_Cat_Calc_Valor_Const_Av.Campo_No_Avaluo + ")||'/'||" + Ope_Cat_Calc_Valor_Const_Av.Campo_Anio_Avaluo + " IN (SELECT TO_NUMBER(AU."
                   + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ")||'/'||AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + " FROM "
                   + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " IN "
                   + " (SELECT AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC WHERE AC."
                   + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " = '" + Cuentas.P_No_Entrega + "' AND AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " = " + Cuentas.P_Anio + "))) UNION ";



                Mi_SQL += "(SELECT VC." + Ope_Cat_Calc_Valor_Const_Arv.Campo_Croquis + " AS " +Ope_Cat_Calc_Valor_Const_Av.Campo_Referencia
                   + ", TO_NUMBER(VC." + Ope_Cat_Calc_Valor_Const_Arv.Campo_No_Avaluo + ")|| '/'||VC." + Ope_Cat_Calc_Valor_Const_Arv.Campo_Anio_Avaluo + " AS FOLIO_CATASTRO"
                   + ", 'RUSTICO' AS FOLIO_CATASTRO_TIPO"
                   + ", VC." + Ope_Cat_Calc_Valor_Const_Arv.Campo_Superficie_M2
                   + ", VC." + Ope_Cat_Calc_Valor_Const_Arv.Campo_Factor
                   + ", VC." + Ope_Cat_Calc_Valor_Const_Arv.Campo_Valor_Parcial
                   + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_M2 + ",0.00) AS VALOR_M2"
                   + " FROM  " + Ope_Cat_Calc_Valor_Const_Arv.Tabla_Ope_Cat_Calc_Valor_Const_Arv + " VC"
                   + " LEFT OUTER JOIN " + Cat_Cat_Tab_Val_Construccion.Tabla_Cat_Cat_Tab_Val_Construccion + " TV"
                   + " ON VC." + Ope_Cat_Calc_Valor_Const_Arv.Campo_Valor_Construccion_Id + "=TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_Construccion_Id
                   + " LEFT OUTER JOIN " + Cat_Cat_Calidad_Construccion.Tabla_Cat_Cat_Calidad_Construccion + " CC"
                   + " ON TV." + Cat_Cat_Tab_Val_Construccion.Campo_Calidad_Id + "=CC." + Cat_Cat_Calidad_Construccion.Campo_Calidad_Id
                   + " WHERE TO_NUMBER("
                   + Ope_Cat_Calc_Valor_Const_Arv.Campo_No_Avaluo + ")||'/'||" + Ope_Cat_Calc_Valor_Const_Arv.Campo_Anio_Avaluo + " IN (SELECT TO_NUMBER(AU."
                   + Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo + ")||'/'||AU." + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + " FROM "
                   + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AU WHERE AU." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " IN "
                   + " (SELECT AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC WHERE AC."
                   + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " = '" + Cuentas.P_No_Entrega + "' AND AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " = " + Cuentas.P_Anio + ")))";

                Mi_SQL += " ORDER BY 2 ASC, 1 ASC";

                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Cuentas.P_Dt_Construccion = dataset.Tables[0];
                }

                Mi_SQL = "(SELECT VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Seccion
                    + ", TO_NUMBER(VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_No_Avaluo + ")|| '/'||VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Anio_Avaluo + " AS FOLIO_CATASTRO"
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Orden
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Superficie_M2
                    + ", 'URBANO' AS FOLIO_CATASTRO_TIPO"
                    + ", NVL(VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + ",0) AS " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Factor
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Factor_EF
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Valor_Parcial
                    + " FROM  " + Ope_Cat_Calc_Valor_Terreno_Av.Tabla_Ope_Cat_Calc_Valor_Terreno_Av + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos + " VT"
                    + " ON VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Valor_Tramo_Id + "=VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id
                    + " WHERE TO_NUMBER("
                    + Ope_Cat_Calc_Valor_Terreno_Av.Campo_No_Avaluo + ")||'/'||" + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Anio_Avaluo + " IN (SELECT TO_NUMBER(AU."
                    + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ")||'/'||AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + " FROM "
                    + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " IN "
                    + " (SELECT AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC WHERE AC."
                    + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " = '" + Cuentas.P_No_Entrega + "' AND AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " = " + Cuentas.P_Anio + "))) UNION ";

                Mi_SQL += "(SELECT NVL(TC." + Cat_Cat_Tipos_Constru_Rustico.Campo_Identificador + ",'') AS " + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Seccion
                    + ", TO_NUMBER(VC." + Ope_Cat_Calc_Terreno_Arv.Campo_No_Avaluo + ")|| '/'||VC." + Ope_Cat_Calc_Terreno_Arv.Campo_Anio_Avaluo + " AS FOLIO_CATASTRO"
                    + ", 1 AS " + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Orden
                    + ", VC." + Ope_Cat_Calc_Terreno_Arv.Campo_Superficie + " AS " + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Superficie_M2
                    + ", 'RUSTICO' AS FOLIO_CATASTRO_TIPO"
                    + ", NVL(CR." + Cat_Cat_Tab_Val_Const_Rustico.Campo_Valor_M2 + ",0) AS " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo
                    + ", VC." + Ope_Cat_Calc_Terreno_Arv.Campo_Factor
                    + ", 1 AS " + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Factor_EF
                    + ", VC." + Ope_Cat_Calc_Terreno_Arv.Campo_Valor_Parcial
                    + " FROM  " + Ope_Cat_Calc_Terreno_Arv.Tabla_Ope_Cat_Calc_Terreno_Arv + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tipos_Constru_Rustico.Tabla_Cat_Cat_Tipos_Constru_Rustico + " TC"
                    + " ON VC." + Ope_Cat_Calc_Terreno_Arv.Campo_Tipo_Constru_Rustico_Id + "=TC." + Cat_Cat_Tipos_Constru_Rustico.Campo_Tipo_Constru_Rustico_Id
                    + " LEFT OUTER JOIN " + Cat_Cat_Tab_Val_Const_Rustico.Tabla_Cat_Cat_Tab_Val_Const_Rustico + " CR"
                    + " ON VC." + Ope_Cat_Calc_Terreno_Arv.Campo_Valor_Constru_Rustico_Id + "=CR." + Cat_Cat_Tab_Val_Const_Rustico.Campo_Valor_Constru_Rustico_Id
                    + " WHERE TO_NUMBER("
                    + Ope_Cat_Calc_Terreno_Arv.Campo_No_Avaluo + ")||'/'||" + Ope_Cat_Calc_Terreno_Arv.Campo_Anio_Avaluo + " IN (SELECT TO_NUMBER(AU."
                    + Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo + ")||'/'||AU." + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + " FROM "
                    + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AU WHERE AU." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " IN "
                    + " (SELECT AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC WHERE AC."
                    + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " = '" + Cuentas.P_No_Entrega + "' AND AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " = " + Cuentas.P_Anio + "))) ";



                    Mi_SQL += " ORDER BY 2 ASC, 5 ASC, 3 ASC";

                    dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        Cuentas.P_Dt_Terreno = dataset.Tables[0];
                    }


                    Mi_SQL = "SELECT ";
                    Mi_SQL += "'ALTA' AS ACCION, ";
                    Mi_SQL += "PREDIO." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                    //////////  Consultar folio de catastro
                    Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                    Mi_SQL += " THEN 'RUSTICO' ELSE ";
                    Mi_SQL += "'URBANO' END AS FOLIO_CATASTRO_TIPO, ";

                    Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                    Mi_SQL += " THEN (SELECT TO_NUMBER(AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo + ") ||'/'||TO_NUMBER(AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + ") FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") ELSE ";
                    Mi_SQL += "(SELECT TO_NUMBER(AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ") ||'/'||TO_NUMBER(AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + ") FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") END AS FOLIO_CATASTRO, ";

                    Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                    Mi_SQL += " THEN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Valor_Total_Predio + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") ELSE ";
                    Mi_SQL += "(SELECT AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_Total_Predio + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") END AS VALOR_PREDIO, ";


                    Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                    Mi_SQL += " THEN (SELECT SUM(CCR." + Ope_Cat_Calc_Valor_Const_Arv.Campo_Superficie_M2 + ") FROM " + Ope_Cat_Calc_Valor_Const_Arv.Tabla_Ope_Cat_Calc_Valor_Const_Arv + " CCR WHERE CCR."+Ope_Cat_Calc_Valor_Const_Arv.Campo_Anio_Avaluo+"||'/'||CCR."+Ope_Cat_Calc_Valor_Const_Arv.Campo_No_Avaluo+" IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + "||'/'||AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo +" FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ")) ELSE ";
                    Mi_SQL += "(SELECT SUM(CCU."+Ope_Cat_Calc_Valor_Const_Av.Campo_Superficie_M2+") FROM "+Ope_Cat_Calc_Valor_Const_Av.Tabla_Ope_Cat_Calc_Valor_Const_Av+" CCU WHERE CCU."+Ope_Cat_Calc_Valor_Const_Av.Campo_Anio_Avaluo+"||'/'||CCU."+Ope_Cat_Calc_Valor_Const_Av.Campo_No_Avaluo+" IN (SELECT AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "||'/'||AU."+Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo+" FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ")) END AS CALC_CONSTRUCCION ";
                    ///////////
                    Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " CUENTA ";
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " PREDIO ON CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + "=PREDIO." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " WHERE ";
                    if (Cuentas.P_Anio != null && Cuentas.P_Anio.Trim() != "")
                    {
                        Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " = " + Cuentas.P_Anio + " AND ";
                    }
                    if (Cuentas.P_No_Entrega != null && Cuentas.P_No_Entrega.Trim() != "")
                    {
                        Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " = '" + Cuentas.P_No_Entrega + "' AND ";
                    }

                    Mi_SQL += "(CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "='AUTORIZADO') OR ";
                    Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AR WHERE AR." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "='AUTORIZADO')) AND ";

                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    //Mi_SQL += ")";


                    if (Mi_SQL.EndsWith("()"))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                    }


                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                    Mi_SQL += " ORDER BY " + Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial + " ASC";
                    dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        Cuentas.P_Dt_Totales = dataset.Tables[0];
                    }


            }
            catch (Exception Ex)
            {

                String Message = "Consultar_Claves_Catastrales: [" + Ex.Message + "].";
            }
            return Tabla;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultas_Colonias_Actualizar
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultas_Colonias_Actualizar(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT CO." + Cat_Ate_Colonias.Campo_Nombre;
                Mi_SQL += ", (SELECT ";
                Mi_SQL += "NVL(COUNT(CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "),0) FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CP WHERE ";
                Mi_SQL += "CP." + Cat_Pre_Cuentas_Predial.Campo_Efectos + " IS NOT NULL AND ";
                Mi_SQL += "((TO_NUMBER(SUBSTR(CP." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3)))<" + Cuentas.P_Efecto_Anio + " OR ";
                Mi_SQL += "((TO_NUMBER(SUBSTR(CP." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))=" + Cuentas.P_Efecto_Anio + " AND ";
                Mi_SQL += "(TO_NUMBER(SUBSTR(CP." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", 0, 1))<=" + Cuentas.P_Efecto_Bimestre + ")))) AND ";
                Mi_SQL += " CP." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + "=CO." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += ") AS PREDIOS_EVALUAR, 'A' AS GRUPO FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " CO ORDER BY CO." + Cat_Ate_Colonias.Campo_Nombre + " ASC";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Consultas_Colonias_Actualizar: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Predios_Estrategicos
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: David Herrera Rincon
        ///FECHA_CREO: 24/Octubre/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Predios_Estrategicos(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT TO_NUMBER(CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial + ") AS " + Ope_Cat_Asignacion_Cuentas.Campo_Folio_Predial + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                Mi_SQL += " THEN 'AR' ELSE ";
                Mi_SQL += "'AU' END AS FOLIO_CATASTRO_TIPO, ";


                Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                Mi_SQL += " THEN (SELECT SUM(CCR." + Ope_Cat_Calc_Valor_Const_Arv.Campo_Superficie_M2 + ") FROM " + Ope_Cat_Calc_Valor_Const_Arv.Tabla_Ope_Cat_Calc_Valor_Const_Arv + " CCR WHERE CCR." + Ope_Cat_Calc_Valor_Const_Arv.Campo_Anio_Avaluo + "||'/'||CCR." + Ope_Cat_Calc_Valor_Const_Arv.Campo_No_Avaluo + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + "||'/'||AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ")) ELSE ";
                Mi_SQL += "(SELECT SUM(CCU." + Ope_Cat_Calc_Valor_Const_Av.Campo_Superficie_M2 + ") FROM " + Ope_Cat_Calc_Valor_Const_Av.Tabla_Ope_Cat_Calc_Valor_Const_Av + " CCU WHERE CCU." + Ope_Cat_Calc_Valor_Const_Av.Campo_Anio_Avaluo + "||'/'||CCU." + Ope_Cat_Calc_Valor_Const_Av.Campo_No_Avaluo + " IN (SELECT AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "||'/'||AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ")) END AS CALC_CONSTRUCCION, ";


                Mi_SQL += "CASE WHEN CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IS NOT NULL)";
                Mi_SQL += " THEN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Valor_Total_Predio + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") ELSE ";
                Mi_SQL += "(SELECT AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_Total_Predio + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ") END AS VALOR_PREDIO, ";


                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ",";
                Mi_SQL += "CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE_NOTIFICACION, ";
                Mi_SQL += "COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA_NOTIFICACION, ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                Mi_SQL += " 0.00 AS CUOTA_NUEVA, 0.00 AS DIFERENCIA, ";
                Mi_SQL += "(PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_PROPIETARIO, ";
                Mi_SQL += "CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += "COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA ";

                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " CUENTA ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " ON CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPOS_PREDIO ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " ESTADOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CIUDADES ON ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " = CIUDADES." + Cat_Pre_Ciudades.Campo_Estado_ID + " ";
                Mi_SQL += " AND CIUDADES." + Cat_Pre_Ciudades.Campo_Ciudad_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " PROPIETARIOS ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                Mi_SQL += " WHERE ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " IN ";
                Mi_SQL += "(SELECT CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA WHERE ";
                Mi_SQL += "CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + " IS NOT NULL AND ";
                if (Cuentas.P_Efecto_Anio != null && Cuentas.P_Efecto_Anio.Trim() != "")
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))<(" + Cuentas.P_Efecto_Anio + ")) OR ";
                }
                else
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))<(TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')))) OR ";
                }



                if (Cuentas.P_Efecto_Anio != null && Cuentas.P_Efecto_Anio.Trim() != "")
                {
                    Mi_SQL += "((TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))=(" + Cuentas.P_Efecto_Anio + ")) AND ";
                }
                else
                {
                    Mi_SQL += "((TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3))=(TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')))) AND ";
                }


                if (Cuentas.P_Efecto_Bimestre != null && Cuentas.P_Efecto_Bimestre.Trim() != "")
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", 0, 1))=(" + Cuentas.P_Efecto_Bimestre + "))) AND ";
                }
                else
                {
                    Mi_SQL += "(TO_NUMBER(SUBSTR(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", 0, 1))=(TO_NUMBER(TO_CHAR(SYSDATE,'MM'))/2))) AND ";
                }
                Mi_SQL += "CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " NOT IN ";
                Mi_SQL += "(SELECT " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " WHERE " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + "." + Ope_Cat_Asignacion_Cuentas.Campo_Estatus + " IN ('VIGENTE','PROCESO')) AND ROWNUM<100) AND ";
                if (Cuentas.P_Calle != null && Cuentas.P_Calle.Trim() != "")
                {
                    Mi_SQL += " CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " LIKE '%" + Cuentas.P_Calle + "%' AND ";
                }
                if (Cuentas.P_Colonia != null && Cuentas.P_Colonia.Trim() != "")
                {
                    Mi_SQL += " COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Cuentas.P_Colonia + "%' AND ";
                }
                if (Cuentas.P_No_Ext != null && Cuentas.P_No_Ext.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " LIKE '%" + Cuentas.P_No_Ext + "%' AND ";
                }
                if (Cuentas.P_No_Int != null && Cuentas.P_No_Int.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " LIKE '%" + Cuentas.P_No_Int + "%' AND ";
                }
                if (Cuentas.P_Cuenta_Predial != null && Cuentas.P_Cuenta_Predial.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Cuentas.P_Cuenta_Predial + "%' AND ";
                }
                if (Cuentas.P_Tipo_Predio != null && Cuentas.P_Tipo_Predio.Trim() != "")
                {
                    Mi_SQL += " TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " = '" + Cuentas.P_Tipo_Predio + "' AND ";
                }
                if (Cuentas.P_Anio_Nulo)
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " IS NULL AND ";
                }
                else
                {
                    if (Cuentas.P_Anio != null && Cuentas.P_Anio.Trim() != "")
                    {
                        Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " = " + Cuentas.P_Anio + " AND ";
                    }
                }
                if (Cuentas.P_No_Entrega != null && Cuentas.P_No_Entrega.Trim() != "")
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " = '" + Cuentas.P_No_Entrega + "' AND ";
                }
                if (Cuentas.P_Estatus != null && Cuentas.P_Estatus.Trim() != "")
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Estatus + " " + Cuentas.P_Estatus + " AND ";
                }
                // Mi_SQL += "(";
                if (Cuentas.P_Superficie_Terreno != null && Cuentas.P_Superficie_Terreno.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " >= " + Cuentas.P_Superficie_Terreno + " AND ";
                }
                if (Cuentas.P_Superficie_Terreno_Menor != null && Cuentas.P_Superficie_Terreno_Menor.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " <= " + Cuentas.P_Superficie_Terreno_Menor + " AND ";
                }


                if (Cuentas.P_Propietario != null && Cuentas.P_Propietario.Trim() != "")
                {


                    Mi_SQL += " (PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") LIKE '%" + Cuentas.P_Propietario + "%' AND ";
                }

                //Mi_SQL += "(";
                //------------------------------------------------------------------------------------------------------------------------------------------------
                if (Cuentas.P_Tipo_Reporte != null && Cuentas.P_Tipo_Reporte.Trim() == "ESTRATEGICO")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " >= 2000 AND ";
                }
                else if (Cuentas.P_Tipo_Reporte != null && Cuentas.P_Tipo_Reporte.Trim() == "URBANO")
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "='AUTORIZADO' AND AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " IS NOT NULL) AND ";
                }
                else if (Cuentas.P_Tipo_Reporte != null && Cuentas.P_Tipo_Reporte.Trim() == "RUSTICO")
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AU." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AU WHERE AU." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AU." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "='AUTORIZADO' AND AU." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " IS NOT NULL) AND ";
                }
                else if (Cuentas.P_Tipo_Reporte != null && Cuentas.P_Tipo_Reporte.Trim() == "MUNICIPIO")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + "=100 AND ";
                }

                if (Cuentas.P_Con_Movimiento)
                {
                    Mi_SQL += " CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Cuenta_Predial_Id + " IN (";
                    Mi_SQL += "SELECT OV." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " OV WHERE OV." + Ope_Pre_Ordenes_Variacion.Campo_Anio + "=CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_Anio;
                    Mi_SQL += " AND OV." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + "= 'ACEPTADA' AND OV." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " IN (";
                    Mi_SQL += "SELECT MOV." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " MOV WHERE MOV." + Cat_Pre_Movimientos.Campo_Estatus + "='VIGENTE' AND MOV." + Cat_Pre_Movimientos.Campo_Identificador + "='ACV'";
                    Mi_SQL += ")) AND ";
                }
                //------------------------------------------------------------------------------------------------------------------------------------------------
                if (Cuentas.P_Superficie_Construccion_Menor != null && Cuentas.P_Superficie_Construccion_Menor.Trim() != "")
                {
                    Mi_SQL += " " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " <= " + Cuentas.P_Superficie_Construccion_Menor + " AND ";
                }

                Mi_SQL += "(CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "='AUTORIZADO') OR ";
                Mi_SQL += "CUENTA." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN (SELECT AR." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AR WHERE AR." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "='AUTORIZADO')) AND ";

                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                //Mi_SQL += ")";


                if (Mi_SQL.EndsWith("()"))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                }


                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " ASC, CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " ASC, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " ASC, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " ASC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Consultar_Predios_Estrategicos: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultas_Ejercicio_Fiscal
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultas_Ejercicio_Fiscal(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {


                Mi_SQL = "SELECT (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Primera_Entrega + ") AS FECHA_ENTREGA";
                Mi_SQL += ", (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Primera_Entrega_Real + ") AS FECHA_ENTREGA_REAL";
                //Mi_SQL += ", " + Cat_Cat_Calendario_Entregas.Campo_Anio ;
                Mi_SQL += ", NVL((SELECT SUM(CP." + Cat_Cat_Cuotas_Perito.Campo_1_Entrega + ")";
                Mi_SQL += " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= CP." + Cat_Cat_Cuotas_Perito.Campo_Anio + "),0) AS AVALUOS_ENTREGAR";
                Mi_SQL += ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ")";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " AND ";
                Mi_SQL += " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + "='1_ENTREGA') AS AVALUOS_ENTREGADOS";
                Mi_SQL += " FROM " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas + " CE";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= '" + Cuentas.P_Anio_Ejercicio_Fiscal + "' ";


                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Error al consultar los Ejercicios Fiscales: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultas_Ejercicio_Fiscal_Segunda_Entrega
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultas_Ejercicio_Fiscal_Segunda_Entrega(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {


                Mi_SQL = "SELECT (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Segunda_Entrega + ") AS FECHA_ENTREGA";
                Mi_SQL += ", (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Segunda_Entrega_Real + ") AS FECHA_ENTREGA_REAL";
                Mi_SQL += ", NVL((SELECT SUM(CP." + Cat_Cat_Cuotas_Perito.Campo_2_Entrega + ")";
                Mi_SQL += " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= CP." + Cat_Cat_Cuotas_Perito.Campo_Anio + "),0) AS AVALUOS_ENTREGAR";
                Mi_SQL += ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ")";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " AND ";
                Mi_SQL += " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + "='2_ENTREGA') AS AVALUOS_ENTREGADOS";
                Mi_SQL += " FROM " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas + " CE";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= '" + Cuentas.P_Anio_Ejercicio_Fiscal + "' ";


                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Error al consultar los Ejercicios Fiscales: [" + Ex.Message + "].";
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultas_Ejercicio_Fiscal_Tercera_Entrega
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultas_Ejercicio_Fiscal_Tercera_Entrega(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {


                Mi_SQL = "SELECT (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Tercera_Entrega + ") AS FECHA_ENTREGA";
                Mi_SQL += ", (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Tercera_Entrega_Real + ") AS FECHA_ENTREGA_REAL";
                Mi_SQL += ", NVL((SELECT SUM(CP." + Cat_Cat_Cuotas_Perito.Campo_3_Entrega + ")";
                Mi_SQL += " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= CP." + Cat_Cat_Cuotas_Perito.Campo_Anio + "),0) AS AVALUOS_ENTREGAR";
                Mi_SQL += ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ")";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " AND ";
                Mi_SQL += " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + "='3_ENTREGA') AS AVALUOS_ENTREGADOS";
                Mi_SQL += " FROM " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas + " CE";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= '" + Cuentas.P_Anio_Ejercicio_Fiscal + "' ";


                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Error al consultar los Ejercicios Fiscales: [" + Ex.Message + "].";
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultas_Ejercicio_Fiscal_Cuarta_Entrega
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultas_Ejercicio_Fiscal_Cuarta_Entrega(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {


                Mi_SQL = "SELECT (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Cuarta_Entrega + ") AS FECHA_ENTREGA";
                Mi_SQL += ", (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Cuarta_Entrega_Real + ") AS FECHA_ENTREGA_REAL";
                Mi_SQL += ", NVL((SELECT SUM(CP." + Cat_Cat_Cuotas_Perito.Campo_4_Entrega + ")";
                Mi_SQL += " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= CP." + Cat_Cat_Cuotas_Perito.Campo_Anio + "),0) AS AVALUOS_ENTREGAR";
                Mi_SQL += ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ")";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " AND ";
                Mi_SQL += " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + "='4_ENTREGA') AS AVALUOS_ENTREGADOS";
                Mi_SQL += " FROM " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas + " CE";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= '" + Cuentas.P_Anio_Ejercicio_Fiscal + "' ";


                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Error al consultar los Ejercicios Fiscales: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultas_Ejercicio_Fiscal_Quinta_Entrega
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultas_Ejercicio_Fiscal_Quinta_Entrega(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {


                Mi_SQL = "SELECT (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Quinta_Entrega + ") AS FECHA_ENTREGA";
                Mi_SQL += ", (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Quinta_Entrega_Real + ") AS FECHA_ENTREGA_REAL";
                Mi_SQL += ", NVL((SELECT SUM(CP." + Cat_Cat_Cuotas_Perito.Campo_5_Entrega + ")";
                Mi_SQL += " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= CP." + Cat_Cat_Cuotas_Perito.Campo_Anio + "),0) AS AVALUOS_ENTREGAR";
                Mi_SQL += ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ")";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " AND ";
                Mi_SQL += " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + "='5_ENTREGA') AS AVALUOS_ENTREGADOS";
                Mi_SQL += " FROM " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas + " CE";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= '" + Cuentas.P_Anio_Ejercicio_Fiscal + "' ";


                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Error al consultar los Ejercicios Fiscales: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultas_Ejercicio_Fiscal_Sexta_Entrega
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultas_Ejercicio_Fiscal_Sexta_Entrega(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {


                Mi_SQL = "SELECT (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Sexta_Entrega + ") AS FECHA_ENTREGA";
                Mi_SQL += ", (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Sexta_Entrega_Real + ") AS FECHA_ENTREGA_REAL";
                Mi_SQL += ", NVL((SELECT SUM(CP." + Cat_Cat_Cuotas_Perito.Campo_6_Entrega + ")";
                Mi_SQL += " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= CP." + Cat_Cat_Cuotas_Perito.Campo_Anio + "),0) AS AVALUOS_ENTREGAR";
                Mi_SQL += ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ")";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " AND ";
                Mi_SQL += " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + "='6_ENTREGA') AS AVALUOS_ENTREGADOS";
                Mi_SQL += " FROM " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas + " CE";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= '" + Cuentas.P_Anio_Ejercicio_Fiscal + "' ";


                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Error al consultar los Ejercicios Fiscales: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Veces_Rechazo_1
        ///DESCRIPCIÓN: Consulta los veces de rechazos de los avaluo y el nombre de los perito que rechazaron
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Veces_Rechazo_1(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Rechazo)
        {
            DataTable Tabla = new DataTable();
            DataTable Temp_Tabla = new DataTable();
            int index = 0;
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " CE." + Cat_Empleados.Campo_Nombre + " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Paterno;
                /* SE AGREGO EL APELLIDO MATERNO POR QUE PUEDE EXISTIR DOS PERSONAS CON EL MISMO NOMBRE Y APELLIDO PATERNO*/
                Mi_SQL += " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE,";                
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "1_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " + ";
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "1_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " AS VECES_RECHAZO " + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " CCPI ";
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " CE";
                Mi_SQL += " ON " + " CE." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += "= CCPI." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += " WHERE CCPI." + Cat_Empleados.Campo_Estatus + " = " + " 'VIGENTE' " + "ORDER BY " + "CE." + Cat_Empleados.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Message = "Consultar_Veces_Rechazo: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Veces_Rechazo_1
        ///DESCRIPCIÓN: Consulta los veces de rechazos de los avaluo y el nombre de los perito que rechazaron
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Veces_Rechazo_2(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Rechazo)
        {
            DataTable Tabla = new DataTable();
            DataTable Temp_Tabla = new DataTable();
            int index = 0;
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " CE." + Cat_Empleados.Campo_Nombre + " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Paterno;
                /* SE AGREGO EL APELLIDO MATERNO POR QUE PUEDE EXISTIR DOS PERSONAS CON EL MISMO NOMBRE Y APELLIDO PATERNO*/
                Mi_SQL += " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE,";                
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "2_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " + ";
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "2_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " AS VECES_RECHAZO " + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " CCPI ";
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " CE";
                Mi_SQL += " ON " + " CE." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += "= CCPI." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += " WHERE CCPI." + Cat_Empleados.Campo_Estatus + " = " + " 'VIGENTE' " + "ORDER BY " + "CE." + Cat_Empleados.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Message = "Consultar_Veces_Rechazo: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Veces_Rechazo_3
        ///DESCRIPCIÓN: Consulta los veces de rechazos de los avaluo y el nombre de los perito que rechazaron
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Veces_Rechazo_3(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Rechazo)
        {
            DataTable Tabla = new DataTable();
            DataTable Temp_Tabla = new DataTable();
            int index = 0;
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " CE." + Cat_Empleados.Campo_Nombre + " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Paterno;
                /* SE AGREGO EL APELLIDO MATERNO POR QUE PUEDE EXISTIR DOS PERSONAS CON EL MISMO NOMBRE Y APELLIDO PATERNO*/
                Mi_SQL += " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE,";                
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "3_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " + ";
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "3_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " AS VECES_RECHAZO " + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " CCPI ";
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " CE";
                Mi_SQL += " ON " + " CE." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += "= CCPI." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += " WHERE CCPI." + Cat_Empleados.Campo_Estatus + " = " + " 'VIGENTE' " + "ORDER BY " + "CE." + Cat_Empleados.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Message = "Consultar_Veces_Rechazo: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Veces_Rechazo_4
        ///DESCRIPCIÓN: Consulta los veces de rechazos de los avaluo y el nombre de los perito que rechazaron
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Veces_Rechazo_4(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Rechazo)
        {
            DataTable Tabla = new DataTable();
            DataTable Temp_Tabla = new DataTable();
            int index = 0;
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " CE." + Cat_Empleados.Campo_Nombre + " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Paterno;
                /* SE AGREGO EL APELLIDO MATERNO POR QUE PUEDE EXISTIR DOS PERSONAS CON EL MISMO NOMBRE Y APELLIDO PATERNO*/
                Mi_SQL += " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE,";                
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "4_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " + ";
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "4_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " AS VECES_RECHAZO " + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " CCPI ";
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " CE";
                Mi_SQL += " ON " + " CE." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += "= CCPI." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += " WHERE CCPI." + Cat_Empleados.Campo_Estatus + " = " + " 'VIGENTE' " + "ORDER BY " + "CE." + Cat_Empleados.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Message = "Consultar_Veces_Rechazo: [" + Ex.Message + "].";
            }
            return Tabla;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Veces_Rechazo_5
        ///DESCRIPCIÓN: Consulta los veces de rechazos de los avaluo y el nombre de los perito que rechazaron
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Veces_Rechazo_5(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Rechazo)
        {
            DataTable Tabla = new DataTable();
            DataTable Temp_Tabla = new DataTable();
            int index = 0;
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " CE." + Cat_Empleados.Campo_Nombre + " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Paterno;
                /* SE AGREGO EL APELLIDO MATERNO POR QUE PUEDE EXISTIR DOS PERSONAS CON EL MISMO NOMBRE Y APELLIDO PATERNO*/
                Mi_SQL += " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE,";                
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "5_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " + ";
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "5_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " AS VECES_RECHAZO " + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " CCPI ";
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " CE";
                Mi_SQL += " ON " + " CE." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += "= CCPI." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += " WHERE CCPI." + Cat_Empleados.Campo_Estatus + " = " + " 'VIGENTE' " + "ORDER BY " + "CE." + Cat_Empleados.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Message = "Consultar_Veces_Rechazo: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Veces_Rechazo_1
        ///DESCRIPCIÓN: Consulta los veces de rechazos de los avaluo y el nombre de los perito que rechazaron
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Veces_Rechazo_6(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Rechazo)
        {
            DataTable Tabla = new DataTable();
            DataTable Temp_Tabla = new DataTable();

            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " CE." + Cat_Empleados.Campo_Nombre + " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Paterno;
                /* SE AGREGO EL APELLIDO MATERNO POR QUE PUEDE EXISTIR DOS PERSONAS CON EL MISMO NOMBRE Y APELLIDO PATERNO*/
                Mi_SQL += " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE,";                
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "6_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " + ";
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "6_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " AS VECES_RECHAZO " + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " CCPI ";
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " CE";
                Mi_SQL += " ON " + " CE." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += "= CCPI." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += " WHERE CCPI." + Cat_Empleados.Campo_Estatus + " = " + " 'VIGENTE' " + "ORDER BY " + "CE." + Cat_Empleados.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Message = "Consultar_Veces_Rechazo: [" + Ex.Message + "].";
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Veces_Rechazo_1
        ///DESCRIPCIÓN: Consulta los veces de rechazos de los avaluo y el nombre de los perito que rechazaron
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Veces_Rechazo_7(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Rechazo)
        {
            DataTable Tabla = new DataTable();
            DataTable Temp_Tabla = new DataTable();
            int index = 0;
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " CE." + Cat_Empleados.Campo_Nombre + " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Paterno;
                /* SE AGREGO EL APELLIDO MATERNO POR QUE PUEDE EXISTIR DOS PERSONAS CON EL MISMO NOMBRE Y APELLIDO PATERNO*/
                Mi_SQL += " ||' '|| " + "CE." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE,";                
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "7_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " + ";
                Mi_SQL += " NVL (" + "(SELECT SUM(" + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " OPAU ";
                Mi_SQL += " WHERE " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " =";
                Mi_SQL += " CCPI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id;
                Mi_SQL += " AND " + "OPAU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion;
                Mi_SQL += " IN (SELECT " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " OCAC ";
                Mi_SQL += " WHERE " + "OCAC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " = ";
                Mi_SQL += " OPAU." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion;
                Mi_SQL += " AND " + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega;
                Mi_SQL += " = '" + "7_ENTREGA' AND " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "= 2012)),0) ";
                Mi_SQL += " AS VECES_RECHAZO " + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " CCPI ";
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " CE";
                Mi_SQL += " ON " + " CE." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += "= CCPI." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL += " WHERE CCPI." + Cat_Empleados.Campo_Estatus + " = " + " 'VIGENTE' " + "ORDER BY " + "CE." + Cat_Empleados.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Message = "Consultar_Veces_Rechazo: [" + Ex.Message + "].";
            }
            return Tabla;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultas_Ejercicio_Fiscal_Septima_Entrega
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultas_Ejercicio_Fiscal_Septima_Entrega(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {


                Mi_SQL = "SELECT (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Septima_Entrega + ") AS FECHA_ENTREGA";
                Mi_SQL += ", (CE." + Cat_Cat_Calendario_Entregas.Campo_Fecha_Septima_Entrega_Real + ") AS FECHA_ENTREGA_REAL";
                Mi_SQL += ", NVL((SELECT SUM(CP." + Cat_Cat_Cuotas_Perito.Campo_7_Entrega + ")";
                Mi_SQL += " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= CP." + Cat_Cat_Cuotas_Perito.Campo_Anio + "),0) AS AVALUOS_ENTREGAR";
                Mi_SQL += ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + ")";
                Mi_SQL += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC ";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_Anio + " AND ";
                Mi_SQL += " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + "='7_ENTREGA') AS AVALUOS_ENTREGADOS";
                Mi_SQL += " FROM " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas + " CE";
                Mi_SQL += " WHERE CE." + Cat_Cat_Calendario_Entregas.Campo_Anio + "= '" + Cuentas.P_Anio_Ejercicio_Fiscal + "' ";


                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Error al consultar los Ejercicios Fiscales: [" + Ex.Message + "].";
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultas_Ejercicio_Fiscal_Septima_Entrega
        ///DESCRIPCIÓN: Consulta los Cuentas Prediales que son candidatas a reevaluar
        ///PARAMENTROS:     
        ///             1. Cuentas.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultas_Avaluos_Asisgnados_Atendidos(Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            String Mi_SQL1 = "";
            String Mi_SQL2 = "";
            try
            {
                Mi_SQL = "SELECT COUNT(" + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + ")";
                Mi_SQL += " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av;
                Mi_SQL += " WHERE " + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "=" + Cuentas.P_Anio;
                Object Suma_Urbano = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mi_SQL1 = "SELECT COUNT(" + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + ")";
                Mi_SQL1 += " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V;
                Mi_SQL += " WHERE " + Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo + "=" + Cuentas.P_Anio;
                Object Suma_Rustico = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL1);

                Int64 Total = Convert.ToInt64(Suma_Urbano) + Convert.ToInt64(Suma_Rustico);


                Mi_SQL2 = "SELECT COUNT(" + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + ")AS NO_CUENTAS_ASIGNADAS";
                Mi_SQL2 += ", " + Total + " as NO_CUENTAS_ATENDIDAS";
                Mi_SQL2 += " FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas;
                Mi_SQL2 += " WHERE " + Ope_Cat_Asignacion_Cuentas.Campo_Anio + "=" + Cuentas.P_Anio + " OR ";
                Mi_SQL2 += Ope_Cat_Asignacion_Cuentas.Campo_Anio + " IS null";




                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL2);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Error al consultar los Ejercicios Fiscales: [" + Ex.Message + "].";
            }
            return Tabla;
        }
    }

}

