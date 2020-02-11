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
using Presidencia.Reportes_Predial_Convenios.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Operacion_Predial_Parametros.Negocio;

namespace Presidencia.Reportes_Predial_Convenios.Datos
{

    public class Cls_Rep_Pre_Convenios_Datos
    {


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Convenios_Predial_Reporte
        ///DESCRIPCIÓN          : Obtiene los Convenios de predial dados de alta en la base de datos
        ///                         filtrados por fecha y estatus para el reporte de convenios
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio con informacion para la consulta
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 10-ene-2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Convenios_Predial_Reporte(Cls_Rep_Pre_Convenios_Negocio Datos)
        {
            Cls_Ope_Pre_Parametros_Negocio Consulta_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
            DataTable Dt_Predial = new DataTable();
            String Mi_SQL = "";
            try
            {
                int Anio_Corriente = Consulta_Parametros.Consultar_Anio_Corriente();

                // formar consulta con los montos pagados por convenio
                Mi_SQL = "WITH PAGOS AS ( SELECT "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", CASE"
                    + " WHEN CEIL(SUM(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ")) - SUM(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ") < 0.5"
                    + " THEN CEIL(SUM(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + "))"
                    + " ELSE FLOOR(SUM(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + "))"
                    + " END PAGADO"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO'"
                    + " GROUP BY " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + " ORDER BY " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio 
                    + ")";

                // formar consulta para obtener el primer periodo del convenio (bimestres incluidos en el primer pago)
                Mi_SQL += ",PRIMER_PERIODO AS "
                    + "(SELECT * FROM" 
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " DESC"
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9))"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 1, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 12)"
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + " WHERE LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") > 4"
                    + " AND LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") < 14"
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9))"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 1, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 12)"
                    + " ) WHERE RANK = 1)";

                // formar consulta para obtener el ultimo periodo del convenio (bimestres incluidos en el ultimo pago)
                Mi_SQL += ",ULTIMO_PERIODO AS "
                    + "(SELECT * FROM"
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " DESC"
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) DESC"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 5, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 6) DESC"
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + " WHERE LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") > 4"
                    + " AND LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") < 14"
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) DESC"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 5, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 6) DESC"
                    + " ) WHERE RANK = 1)";

                // formar consulta para obtener el primer periodo rezago del convenio
                Mi_SQL += ",PRIMER_PERIODO_REZAGO AS "
                    + "(SELECT * FROM"
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " DESC"
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9))"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 1, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 12)"
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + " WHERE LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") > 4"
                    + " AND LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") < 14"
                    + " AND TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9)) < " + Anio_Corriente
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9))"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 1, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 12)"
                    + " ) WHERE RANK = 1)";

                // formar consulta para obtener el ultimo periodo de rezago del convenio
                Mi_SQL += ",ULTIMO_PERIODO_REZAGO AS "
                    + "(SELECT * FROM"
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " DESC"
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) DESC"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 5, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 6) DESC"
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + " WHERE LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") > 4"
                    + " AND LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") < 14"
                    + " AND TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) < " + Anio_Corriente
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) DESC"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 5, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 6) DESC"
                    + " ) WHERE RANK = 1)";

                // formar consulta para obtener la primer parcialidad con estatus POR PAGAR de cada convenio
                Mi_SQL += ",PARCIALIDADES_POR_PAGAR AS "
                    + "(SELECT * FROM"
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " DESC"
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + "WHERE DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR'"
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " DESC"
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + " ) WHERE RANK = 1)";

                // formar consulta principal 
                Mi_SQL += "SELECT "
                    + "(SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id
                    + ") AS CUENTA_PREDIAL, ";
                // agregar nombre del solicitante, si es nulo, traer el nombre del contribuyente en el convenio, 
                // si este ultimo es nulo tambien, traer el propietario de la cuenta
                Mi_SQL += "NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Solicitante
                    + ", (SELECT " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = NVL("
                    + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id
                    + ",(SELECT " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                    + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                    + " WHERE " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = "
                    + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id
                    + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'"
                    + " AND ROWNUM = 1))"
                    + ")) AS NOMBRE_SOLICITANTE";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura
                    + ",0) " + Ope_Pre_Convenios_Predial.Campo_No_Reestructura
                    + ", " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Estatus;
                Mi_SQL += ", (SELECT " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento
                    + " FROM "+Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio
                    + " AND ROWNUM = 1 AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR') SIGUIENTE_PARCIALIDAD";
                Mi_SQL += ",(SUBSTR(PRIMER_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", 0, 6) | | '-' | |"
                    + " SUBSTR(ULTIMO_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", 8)) PERIODO";
                //Periodo Rezago
                //Mi_SQL += ", (SELECT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Periodo_Rezago
                //    + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + ", " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial
                //    + " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " = " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " AND ROWNUM = 1) AS PERIODO_REZAGO";
                Mi_SQL += ",(SUBSTR(PRIMER_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", 0, 6) | | '-' | |"
                    + " SUBSTR(ULTIMO_PERIODO_REZAGO." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", 8)) PERIODO_REZAGO";
                //Fin periodo Rezago
                //Periodo Corriente
                Mi_SQL += ",(SELECT COUNT(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago 
                    + ") FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio 
                    + " AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO'"
                    + " AND NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0) = NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0)) AS PAGOS_HECHOS";
                Mi_SQL += ",(SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente 
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + ") AS PERIODO_CORRIENTE";
                //Fin periodo Corriente
                Mi_SQL += ", TO_CHAR(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Fecha + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"')" 
                    + Ope_Pre_Convenios_Predial.Campo_Fecha;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago + ",0) " 
                    + Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente + ",0) " 
                    + Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Predial + ",0) " 
                    + Ope_Pre_Convenios_Predial.Campo_Total_Predial;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Recargos + ",0) + "
                    + "NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Moratorios + ",0)" 
                    + " SUMA_RECARGOS";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Recargos + ",0) " 
                    + Ope_Pre_Convenios_Predial.Campo_Total_Recargos;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Moratorios + ",0) " 
                    + Ope_Pre_Convenios_Predial.Campo_Total_Moratorios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Honorarios + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Honorarios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Descuento + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Descuento;
                Mi_SQL += ",NVL(PAGOS.PAGADO, 0) PAGADO";
                Mi_SQL += ", CASE WHEN TRIM(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Estatus
                    + ") = 'TERMINADO' THEN 0 ELSE "
                    + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Sub_Total + " -  NVL(PAGOS.PAGADO,0) END POR_PAGAR";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Sub_Total
                    + ",0) TOTAL_CONVENIO";

                Mi_SQL += " FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial
                    + " LEFT JOIN PAGOS ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = PAGOS."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(PAGOS."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN PRIMER_PERIODO ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = PRIMER_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(PRIMER_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN ULTIMO_PERIODO ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = ULTIMO_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(ULTIMO_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN PRIMER_PERIODO_REZAGO ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = PRIMER_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(PRIMER_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN ULTIMO_PERIODO_REZAGO ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = ULTIMO_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(ULTIMO_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN PARCIALIDADES_POR_PAGAR ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)";


                Mi_SQL += " WHERE ";
                if (Datos.P_No_Convenio != "" && Datos.P_No_Convenio != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "' AND ";
                }
                if (Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Datos.P_Cuenta_Predial_ID + "' AND ";
                }
                if (Datos.P_Solo_Convenios == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                        + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) = 0 " + " AND ";
                }
                else if (Datos.P_Solo_Reestructuras == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." 
                        + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) > 0 " + " AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Estatus + " = '" + Datos.P_Estatus + "' AND ";
                }
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Fecha + " >= '" + Datos.P_Desde_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Fecha + " <= '" + Datos.P_Hasta_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }

                // eliminar AND o WHERE del final del string
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                // si se proporcionó un orden agregarlo, si no, ordenar por NO_CONVENIO
                if (!string.IsNullOrEmpty(Datos.P_Ordenar_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Datos.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                        + Ope_Pre_Convenios_Predial.Campo_No_Convenio;
                }

                Dt_Predial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Convenios de Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Predial;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Convenios_Predial_Detallado_Reporte
        ///DESCRIPCIÓN          : Obtiene los Convenios de predial dados de alta en la base de datos
        ///                         filtrados por fecha y estatus para el reporte de convenios
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio con informacion para la consulta
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 10-ene-2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Convenios_Predial_Detallado_Reporte(Cls_Rep_Pre_Convenios_Negocio Datos)
        {
            Cls_Ope_Pre_Parametros_Negocio Consulta_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
            DataTable Dt_Predial = new DataTable();
            String Mi_SQL = "";
            try
            {
                int Anio_Corriente = Consulta_Parametros.Consultar_Anio_Corriente();

                Mi_SQL = "WITH PAGOS AS ( SELECT "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", CASE"
                    + " WHEN CEIL(SUM(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ")) - SUM(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ") < 0.5"
                    + " THEN CEIL(SUM(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + "))"
                    + " ELSE FLOOR(SUM(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + "))"
                    + " END PAGADO"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO'"
                    + " GROUP BY " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + " ORDER BY " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ")";

                // formar consulta para obtener el primer periodo del convenio (bimestres incluidos en el primer pago)
                Mi_SQL += ",PRIMER_PERIODO AS "
                    + "(SELECT * FROM"
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9))"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 1, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 12)"
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + " WHERE LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") > 4"
                    + " AND LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") < 14"
                    + " AND DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO'"
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9))"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 1, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 12)"
                    + ") WHERE RANK = 1)";

                // formar consulta para obtener el ultimo periodo del convenio (bimestres incluidos en el ultimo pago)
                Mi_SQL += ",ULTIMO_PERIODO AS "
                    + "(SELECT * FROM"
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) DESC"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 5, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 6) DESC"
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + " WHERE LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") > 4"
                    + " AND LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") < 14"
                    + " AND DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO'"
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) DESC"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 5, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 6) DESC"
                    + ") WHERE RANK = 1)";

                // formar consulta para obtener el primer periodo rezago del convenio
                Mi_SQL += ",PRIMER_PERIODO_REZAGO AS "
                    + "(SELECT * FROM"
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9))"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 1, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 12)"
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + " WHERE LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") > 4"
                    + " AND LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") < 14"
                    + " AND TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9)) < " + Anio_Corriente
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 3, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 9))"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), 1, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 12)"
                    + ") WHERE RANK = 1)";

                // formar consulta para obtener el ultimo periodo de rezago del convenio
                Mi_SQL += ",ULTIMO_PERIODO_REZAGO AS "
                    + "(SELECT * FROM"
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) DESC"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 5, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 6) DESC"
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + " WHERE LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") > 4"
                    + " AND LENGTH(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ") < 14"
                    + " AND TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) < " + Anio_Corriente
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", TO_NUMBER(SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 3)) DESC"
                    + ", SUBSTR(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + "), LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 5, LENGTH(TRIM(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ")) - 6) DESC"
                    + ") WHERE RANK = 1)";

                // formar consulta para obtener la primer parcialidad con estatus POR PAGAR de cada convenio
                Mi_SQL += ",PARCIALIDADES_POR_PAGAR AS "
                    + "(SELECT * FROM"
                    + " (SELECT DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + ", RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " DESC"
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0)"
                    + " WHERE DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR'"
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " DESC"
                    + ", DET." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + " ) WHERE RANK = 1)";

                // formar consulta principal 
                Mi_SQL += "SELECT "
                    + "(SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id
                    + ") AS CUENTA_PREDIAL, ";
                // agregar nombre del solicitante, si es nulo, traer el nombre del contribuyente en el convenio, 
                // si este ultimo es nulo tambien, traer el propietario de la cuenta
                Mi_SQL += "NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Solicitante
                    + ", (SELECT " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = NVL("
                    + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id
                    + ",(SELECT " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                    + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                    + " WHERE " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = "
                    + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id
                    + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'"
                    + " AND ROWNUM = 1))"
                    + ")) AS NOMBRE_SOLICITANTE";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura
                    + ",0) " + Ope_Pre_Convenios_Predial.Campo_No_Reestructura
                    + ", " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Estatus;
                Mi_SQL += ", (SELECT " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio
                    + " AND ROWNUM = 1 AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR') SIGUIENTE_PARCIALIDAD";
                Mi_SQL += ",(SUBSTR(PRIMER_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", 0, 6) | | '-' | |"
                    + " SUBSTR(ULTIMO_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", 8)) PERIODO";
                Mi_SQL += ",(SELECT COUNT(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                    + ") FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio
                    + " AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO'"
                    + " AND NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ",0) = NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0)) AS PAGOS_HECHOS";
                //Periodo Rezago
                //Mi_SQL += ", (SELECT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Periodo_Rezago
                //    + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + ", " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial
                //    + " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " = " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " AND ROWNUM = 1) AS PERIODO_REZAGO";
                Mi_SQL += ",(SUBSTR(PRIMER_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", 0, 6) | | '-' | |"
                    + " SUBSTR(ULTIMO_PERIODO_REZAGO." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo
                    + ", 8)) PERIODO_REZAGO";
                //Fin periodo Rezago
                //Periodo Corriente
                Mi_SQL += ",(SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + ") AS PERIODO_CORRIENTE";
                //Fin periodo Corriente
                Mi_SQL += ", (SELECT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                    + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos
                    + " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " = (SELECT " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " FROM " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + " WHERE " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " AND ROWNUM = 1)) AS NO_RECIBO";
                Mi_SQL += ", (SELECT " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja
                    + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + ", " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial
                    + " WHERE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_ID + " = " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " = " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " AND ROWNUM = 1) AS NO_CAJA";
                Mi_SQL += ", (SELECT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha
                    + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos
                    + " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " = (SELECT " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " FROM " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + " WHERE " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago + " = PRIMER_PERIODO." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " AND ROWNUM=1)) AS FECHA";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Predial + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Predial;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Recargos + ",0) + "
                    + "NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Moratorios + ",0)"
                    + " SUMA_RECARGOS";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Recargos + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Recargos;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Moratorios + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Moratorios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Honorarios + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Honorarios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Total_Descuento + ",0) "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Descuento;
                Mi_SQL += ",NVL(PAGOS.PAGADO, 0) PAGADO";
                Mi_SQL += "," + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Sub_Total + " -  NVL(PAGOS.PAGADO,0) POR_PAGAR";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Sub_Total
                    + ",0) TOTAL_CONVENIO";

                Mi_SQL += " FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial
                    + " LEFT JOIN PAGOS ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = PAGOS."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(PAGOS."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN PRIMER_PERIODO ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = PRIMER_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(PRIMER_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN ULTIMO_PERIODO ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = ULTIMO_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(ULTIMO_PERIODO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN PRIMER_PERIODO_REZAGO ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = PRIMER_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(PRIMER_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN ULTIMO_PERIODO_REZAGO ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = ULTIMO_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(ULTIMO_PERIODO_REZAGO."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN PARCIALIDADES_POR_PAGAR ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = NVL(PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0)";


                Mi_SQL += " WHERE ";
                if (Datos.P_No_Convenio != "" && Datos.P_No_Convenio != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "' AND ";
                }
                if (Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Datos.P_Cuenta_Predial_ID + "' AND ";
                }
                if (Datos.P_Solo_Convenios == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                        + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) = 0 " + " AND ";
                }
                else if (Datos.P_Solo_Reestructuras == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                        + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ",0) > 0 " + " AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Estatus + " = '" + Datos.P_Estatus + "' AND ";
                }
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Fecha + " >= '" + Datos.P_Desde_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Fecha + " <= '" + Datos.P_Hasta_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }

                // eliminar AND o WHERE del final del string
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Mi_SQL += " ORDER BY " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio;

                Dt_Predial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Convenios de Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Predial;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Convenios_Traslado_Reporte
        ///DESCRIPCIÓN          : Obtiene los Convenios de traslado dados de alta en la base de datos
        ///                         filtrados por fecha y estatus para el reporte de convenios
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio con informacion para la consulta
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 12-ene-2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Convenios_Traslado_Reporte(Cls_Rep_Pre_Convenios_Negocio Datos)
        {
            DataTable Dt_Predial = new DataTable();
            String Mi_SQL = "";
            try
            {
                // formar consulta con los montos pagados por convenio
                Mi_SQL = "WITH PAGOS AS ( SELECT "
                    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Multas
                    + " + " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto
                    + " + " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios
                    + " + " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios
                    + ") PAGADO"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'PAGADO'"
                    + " GROUP BY " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura
                    + " ORDER BY " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + ")";

                // formar consulta con los sumatoria por convenio de los montos en la tabla de detalles
                Mi_SQL += ", TOTALES_CONVENIO AS ( SELECT "
                    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto
                    + ") TOTAL_IMPUESTO"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios
                    + ") TOTAL_RECARGOS"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios
                    + ") TOTAL_MORATORIOS"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios
                    + " + " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios
                    + ") SUMA_RECARGOS"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Multas
                    + ") TOTAL_MULTAS"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio
                    + " GROUP BY " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura
                    + ")";

                // formar consulta para obtener la primer parcialidad con estatus POR PAGAR de cada convenio
                Mi_SQL += ",PARCIALIDADES_POR_PAGAR AS "
                    + "(SELECT * FROM "
                    + "( SELECT DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago
                    + ", DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura
                    + ",RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + " DESC"
                    + ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + ",0)"
                    + "WHERE DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'POR PAGAR'"
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + " DESC"
                    + ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago
                    + " ) WHERE RANK = 1)";

                // formar consulta principal 
                Mi_SQL += "SELECT "
                    + "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = "
                    + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID
                    + ") AS CUENTA_PREDIAL, ";
                // agregar nombre del solicitante, si es nulo, traer el nombre del contribuyente en el convenio, 
                // si este ultimo es nulo tambien, traer el propietario de la cuenta
                Mi_SQL += "NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Campo_Solicitante
                    + ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE "
                    + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = NVL("
                    + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_Propietario_ID
                    + ",(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                    + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                    + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = "
                    + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID
                    + " AND " + Cat_Pre_Propietarios.Campo_Tipo + "='PROPIETARIO'"
                    + " AND ROWNUM=1))"
                    + ")) AS NOMBRE_SOLICITANTE";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura
                    + ",0) " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura
                    + ", " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus;
                Mi_SQL += ", (NVL2(PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + ","
                    + " PARCIALIDADES_POR_PAGAR." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago
                    + " | | ' de ' | | " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Numero_Parcialidades
                    + ", '-')) SIGUIENTE_PARCIALIDAD";
                Mi_SQL += ", TO_CHAR(" + Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"')"
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha;
                Mi_SQL += ",TOTALES_CONVENIO.TOTAL_IMPUESTO"
                    + ",TOTALES_CONVENIO.TOTAL_RECARGOS"
                    + ",TOTALES_CONVENIO.TOTAL_MORATORIOS"
                    + ",TOTALES_CONVENIO.SUMA_RECARGOS"
                    + ",TOTALES_CONVENIO.TOTAL_MULTAS";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Ordinarios + ",0) "
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Ordinarios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Moratorios + ",0) "
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Moratorios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Multas + ",0) "
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Multas;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Descuento + ",0) "
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Descuento;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Campo_Sub_Total
                    + ",0) TOTAL_CONVENIO";
                Mi_SQL += ",NVL(PAGADO, 0) PAGADO";
                Mi_SQL += "," + Ope_Pre_Convenios_Traslados_Dominio.Campo_Sub_Total + " -  NVL(PAGADO,0) POR_PAGAR";

                Mi_SQL += " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio
                    + " LEFT JOIN PAGOS ON " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = PAGOS."
                    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura + ", 0) = NVL(PAGOS."
                    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN PARCIALIDADES_POR_PAGAR ON " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura + ", 0) = NVL(PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN TOTALES_CONVENIO ON " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = TOTALES_CONVENIO."
                    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura + ", 0) = NVL(TOTALES_CONVENIO."
                    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + ", 0)";


                Mi_SQL += " WHERE ";
                if (Datos.P_No_Convenio != "" && Datos.P_No_Convenio != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "' AND ";
                }
                if (Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "' AND ";
                }
                if (Datos.P_Solo_Convenios == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                        + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura + ",0) = 0 " + " AND ";
                }
                else if (Datos.P_Solo_Reestructuras == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                        + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura + ",0) > 0 " + " AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL += Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " = '" + Datos.P_Estatus + "' AND ";
                }
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha + " >= '" + Datos.P_Desde_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha + " <= '" + Datos.P_Hasta_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }

                // eliminar AND o WHERE del final del string
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Mi_SQL += " ORDER BY " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "."
                    + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio;

                Dt_Predial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Convenios de traslado de dominio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Predial;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Convenios_Der_Supervision_Reporte
        ///DESCRIPCIÓN          : Obtiene los Convenios de Derechos de supervision dados de alta en la base de datos
        ///                         filtrados por fecha y estatus para el reporte de convenios
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio con informacion para la consulta
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 12-ene-2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Convenios_Der_Supervision_Reporte(Cls_Rep_Pre_Convenios_Negocio Datos)
        {
            DataTable Dt_Predial = new DataTable();
            String Mi_SQL = "";
            try
            {
                // formar consulta con los montos pagados por convenio
                Mi_SQL = "WITH PAGOS AS ( SELECT "
                    + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Multas
                    + " + " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Impuesto
                    + " + " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Ordinarios
                    + " + " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Moratorios
                    + ") PAGADO"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'PAGADO'"
                    + " GROUP BY " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura
                    + " ORDER BY " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + ")";

                // formar consulta con los sumatoria por convenio de los montos en la tabla de detalles
                Mi_SQL += ", TOTALES_CONVENIO AS ( SELECT "
                    + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Impuesto
                    + ") TOTAL_IMPUESTO"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Ordinarios
                    + ") TOTAL_RECARGOS"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Moratorios
                    + ") TOTAL_MORATORIOS"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Ordinarios
                    + " + " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Moratorios
                    + ") SUMA_RECARGOS"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Multas
                    + ") TOTAL_MULTAS"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision
                    + " GROUP BY " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura
                    + ")";

                // formar consulta para obtener la primer parcialidad con estatus POR PAGAR de cada convenio
                Mi_SQL += ",PARCIALIDADES_POR_PAGAR AS "
                    + "(SELECT * FROM "
                    + "( SELECT DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago
                    + ", DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura
                    + ",RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + " DESC"
                    + ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + ",0)"
                    + "WHERE DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR'"
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + " DESC"
                    + ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago
                    + " ) WHERE RANK = 1)";

                // formar consulta principal 
                Mi_SQL += "SELECT "
                    + "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = "
                    + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_Cuenta_Predial_ID
                    + ") AS CUENTA_PREDIAL, ";
                // agregar nombre del solicitante, si es nulo, traer el nombre del contribuyente en el convenio, 
                // si este ultimo es nulo tambien, traer el propietario de la cuenta
                Mi_SQL += "NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Campo_Solicitante
                    + ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE "
                    + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = NVL("
                    + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_Propietario_ID
                    + ",(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                    + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                    + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = "
                    + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_Cuenta_Predial_ID
                    + " AND " + Cat_Pre_Propietarios.Campo_Tipo + "='PROPIETARIO'"
                    + " AND ROWNUM=1))"
                    + ")) AS NOMBRE_SOLICITANTE";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura
                    + ",0) " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura
                    + ", " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus;
                Mi_SQL += ", (NVL2(PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + ","
                    + " PARCIALIDADES_POR_PAGAR." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago
                    + " | | ' de ' | | " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Numero_Parcialidades
                    + ", '-')) SIGUIENTE_PARCIALIDAD";
                Mi_SQL += ", TO_CHAR(" + Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"')"
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha;
                Mi_SQL += ",TOTALES_CONVENIO.TOTAL_IMPUESTO"
                    + ",TOTALES_CONVENIO.TOTAL_RECARGOS"
                    + ",TOTALES_CONVENIO.TOTAL_MORATORIOS"
                    + ",TOTALES_CONVENIO.SUMA_RECARGOS"
                    + ",TOTALES_CONVENIO.TOTAL_MULTAS";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Campo_Descuento_Recargos_Ordinarios + ",0) "
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_Descuento_Recargos_Ordinarios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Campo_Descuento_Recargos_Moratorios + ",0) "
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_Descuento_Recargos_Moratorios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Campo_Descuento_Multas + ",0) "
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_Descuento_Multas;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Campo_Total_Descuento + ",0) "
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_Total_Descuento;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Campo_Sub_Total
                    + ",0) TOTAL_CONVENIO";
                Mi_SQL += ",NVL(PAGADO, 0) PAGADO";
                Mi_SQL += "," + Ope_Pre_Convenios_Derechos_Supervision.Campo_Sub_Total + " -  NVL(PAGADO,0) POR_PAGAR";

                Mi_SQL += " FROM " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision
                    + " LEFT JOIN PAGOS ON " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = PAGOS."
                    + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + ", 0) = NVL(PAGOS."
                    + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN PARCIALIDADES_POR_PAGAR ON " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + ", 0) = NVL(PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN TOTALES_CONVENIO ON " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = TOTALES_CONVENIO."
                    + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + ", 0) = NVL(TOTALES_CONVENIO."
                    + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + ", 0)";


                Mi_SQL += " WHERE ";
                if (Datos.P_No_Convenio != "" && Datos.P_No_Convenio != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "' AND ";
                }
                if (Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Derechos_Supervision.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "' AND ";
                }
                if (Datos.P_Solo_Convenios == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                        + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + ",0) = 0 " + " AND ";
                }
                else if (Datos.P_Solo_Reestructuras == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                        + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + ",0) > 0 " + " AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL += Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " = '" + Datos.P_Estatus + "' AND ";
                }
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha + " >= '" + Datos.P_Desde_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha + " <= '" + Datos.P_Hasta_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }

                // eliminar AND o WHERE del final del string
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Mi_SQL += " ORDER BY " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "."
                    + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio;

                Dt_Predial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Convenios Derechos de supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Predial;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Convenios_Fraccionamiento_Reporte
        ///DESCRIPCIÓN          : Obtiene los Convenios de Fraccionamiento dados de alta en la base de datos
        ///                         filtrados por fecha y estatus para el reporte de convenios
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio con informacion para la consulta
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 12-ene-2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Convenios_Fraccionamiento_Reporte(Cls_Rep_Pre_Convenios_Negocio Datos)
        {
            DataTable Dt_Predial = new DataTable();
            String Mi_SQL = "";
            try
            {
                // formar consulta con los montos pagados por convenio
                Mi_SQL = "WITH PAGOS AS ( SELECT "
                    + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas
                    + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto
                    + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios
                    + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios
                    + ") PAGADO"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'PAGADO'"
                    + " GROUP BY " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura
                    + " ORDER BY " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + ")";

                // formar consulta con los sumatoria por convenio de los montos en la tabla de detalles
                Mi_SQL += ", TOTALES_CONVENIO AS ( SELECT "
                    + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto
                    + ") TOTAL_IMPUESTO"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios
                    + ") TOTAL_RECARGOS"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios
                    + ") TOTAL_MORATORIOS"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios
                    + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios
                    + ") SUMA_RECARGOS"
                    + ", SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas
                    + ") TOTAL_MULTAS"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos
                    + " GROUP BY " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura
                    + ")";

                // formar consulta para obtener la primer parcialidad con estatus POR PAGAR de cada convenio
                Mi_SQL += ",PARCIALIDADES_POR_PAGAR AS "
                    + "(SELECT * FROM "
                    + "( SELECT DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago
                    + ", DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura
                    + ",RANK() OVER (PARTITION BY DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " DESC"
                    + ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago
                    + ") AS RANK"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " DET "
                    + " LEFT JOIN " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " CONV"
                    + " ON CONV." + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = "
                    + " DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + " AND NVL(CONV." + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + ",0) ="
                    + " NVL(DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ",0)"
                    + "WHERE DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'"
                    + " ORDER BY DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + ", DET." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " DESC"
                    + ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago
                    + " ) WHERE RANK = 1)";

                // formar consulta principal 
                Mi_SQL += "SELECT "
                    + "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = "
                    + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID
                    + ") AS CUENTA_PREDIAL, ";
                // agregar nombre del solicitante, si es nulo, traer el nombre del contribuyente en el convenio, 
                // si este ultimo es nulo tambien, traer el propietario de la cuenta
                Mi_SQL += "NVL(" + Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante
                    + ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE "
                    + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = NVL("
                    + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID
                    + ",(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                    + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                    + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = "
                    + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID
                    + " AND " + Cat_Pre_Propietarios.Campo_Tipo + "='PROPIETARIO'"
                    + " AND ROWNUM=1))"
                    + ")) AS NOMBRE_SOLICITANTE";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura
                    + ",0) " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura
                    + ", " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus;
                Mi_SQL += ", (NVL2(PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + ","
                    + " PARCIALIDADES_POR_PAGAR." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago
                    + " | | ' de ' | | " + Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades
                    + ", '-')) SIGUIENTE_PARCIALIDAD";
                Mi_SQL += ", TO_CHAR(" + Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"')"
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha;
                Mi_SQL += ",TOTALES_CONVENIO.TOTAL_IMPUESTO"
                    + ",TOTALES_CONVENIO.TOTAL_RECARGOS"
                    + ",TOTALES_CONVENIO.TOTAL_MORATORIOS"
                    + ",TOTALES_CONVENIO.SUMA_RECARGOS"
                    + ",TOTALES_CONVENIO.TOTAL_MULTAS";
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Ordinarios + ",0) "
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Ordinarios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Moratorios + ",0) "
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Moratorios;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Multas + ",0) "
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Multas;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Descuento + ",0) "
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Descuento;
                Mi_SQL += ", NVL(" + Ope_Pre_Convenios_Fraccionamientos.Campo_Sub_Total
                    + ",0) TOTAL_CONVENIO";
                Mi_SQL += ",NVL(PAGADO, 0) PAGADO";
                Mi_SQL += "," + Ope_Pre_Convenios_Fraccionamientos.Campo_Sub_Total + " -  NVL(PAGADO,0) POR_PAGAR";

                Mi_SQL += " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos
                    + " LEFT JOIN PAGOS ON " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = PAGOS."
                    + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + ", 0) = NVL(PAGOS."
                    + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN PARCIALIDADES_POR_PAGAR ON " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + ", 0) = NVL(PARCIALIDADES_POR_PAGAR."
                    + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ", 0)"
                    + " LEFT JOIN TOTALES_CONVENIO ON " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = TOTALES_CONVENIO."
                    + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio
                    + " AND NVL(" + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + ", 0) = NVL(TOTALES_CONVENIO."
                    + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ", 0)";


                Mi_SQL += " WHERE ";
                if (Datos.P_No_Convenio != "" && Datos.P_No_Convenio != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "' AND ";
                }
                if (Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "' AND ";
                }
                if (Datos.P_Solo_Convenios == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                        + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + ",0) = 0 " + " AND ";
                }
                else if (Datos.P_Solo_Reestructuras == true)
                {
                    Mi_SQL += " NVL(" + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                        + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + ",0) > 0 " + " AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = '" + Datos.P_Estatus + "' AND ";
                }
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha + " >= '" + Datos.P_Desde_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha + " <= '" + Datos.P_Hasta_Fecha.ToString("dd-MM-yyyy") + "' AND ";
                }

                // eliminar AND o WHERE del final del string
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Mi_SQL += " ORDER BY " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "."
                    + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio;

                Dt_Predial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Convenios Derechos de supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Predial;
        }

    }

}