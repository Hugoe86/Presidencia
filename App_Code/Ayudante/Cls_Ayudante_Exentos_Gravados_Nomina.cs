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
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Empleados.Negocios;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Calculo_Deducciones.Negocio;
using Presidencia.Cat_Terceros.Negocio;
using Presidencia.Finiquitos.Negocio;
using Presidencia.Nomina_Percepciones_Deducciones;
using System.Collections.Generic;

namespace Presidencia.Ayudante_Exentos_Gravados
{
    public class Cls_Ayudante_Exentos_Gravados_Nomina
    {
        #region (Generación de Recálculo)
        /// ***********************************************************************************
        /// Nombre: Calcular_Exento_Gravado
        /// 
        /// Descripción: Este método hace el recálculo de los montos que le aplican al empleado 
        ///              por concepto de sus percepciones y deducciones.
        /// 
        /// Parámetros: Dt_Percepciones.- Listado de percepciones que le aplican al empleado.
        ///             Dt_Deducciones.- Listado de deducciones que le aplican al empleado.
        ///             No_Empleado.- Identificador del empleado para uso de empleados de RH.
        ///             Nomina_ID.- Nomina en la que se genera el finiquito.
        ///             No_Nomina.- Periodo en el que se genera el finiquito.
        ///             Tipo_Nomina_ID.- Tipo de nomiona a la que pertenece el empleado.
        ///             Fecha_Fin_Periodo.- Fecha en la que termina el periodo en el que se genera 
        ///                                 el finiquito.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 17/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        public void Calcular_Exento_Gravado(ref DataTable Dt_Percepciones, ref DataTable Dt_Deducciones, String No_Empleado,
            String Nomina_ID, String No_Nomina, String Tipo_Nomina_ID, String Fecha_Fin_Periodo)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacena la información del empleado a generar sus finiquito.
            Cls_Cat_Tipos_Nominas_Negocio INF_TIPO_NOMINA = null;//Variable que almacena la información del tipo de nómina.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//variable que almacena la información de la zona económica.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacena la información del parámetro de la nómina.
            Cls_Cat_Nom_Percepciones_Deducciones_Business INF_PERCEPCION_DEDUCCION = null;//Variable que almacena la información de las percepciones y deducciones.
            Cls_Cat_Nom_Terceros_Negocio INF_TERCERO = null;//Variable que almacena la información del partido politico al que pertence el empleado.
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            Double Cantidad = 0.0;//Variable que almacena la cantidad de cada concepto que le aplica al empleado.
            Double Grava = 0.0;//Variable que almacena la cantidad que grava la percepción.
            Double Exenta = 0.0;//Variable que almacena la cantidad que exenta el concepto de percepcion.
            Double Ingresos_Gravables_Empleado = 0.0;//Variable que almacena la cantidad total que grava el empleado.
            Double Gravable_Prima_Vacacional = 0.0;//Variable que almacena la cantidad que grava la prima vacacional.
            Double Gravable_Aguinaldo = 0.0;//Variable que almacena la cantidad que grava el aguinaldo.
            Double Gravable_Prima_Antiguedad = 0.0;//Variable que almacena la cantidad que grava la prima de antiguedad.
            Double Gravable_Indemnizacion = 0.0;//Variable que almacena la cantidad que grava la indemnización.
            Double Exenta_Prima_Antiguedad = 0.0;//Variable que almacena la cantidad que exenta la prima de antiguedad.
            Double Exenta_Indemnizacion = 0.0;//Variable que almacena la cantidad que exenta la indemnización.
            Double Gravable_Tiempo_Extra = 0.0;//Variable que almacena la cantidad que grava el tiempo extra.
            Double Gravable_Dias_Festivos = 0.0;//Variable que almacena la cantidad que grava los dias festivos.
            Double Exenta_Tiempo_Extra = 0.0;//Variable que almacena la cantidad que exenta el tiempo extra.
            Double Exenta_Dias_Festivos = 0.0;//Variable que almacena la cantidad que exenta los dias festivos.
            Double Total_Percepciones = 0.0;//Variable que almacena el total de percepciones.
            Double Total_Deducciones = 0.0;//Variable que almacena el total de deducciones.
            Double Total_Indemnizacion = 0.0;//Variable que almacena el total de indemnización.
            Double Total_Aguinaldo =0.0;//Variable que almacena el total de aguinaldo.
            Double Total_Prima_Vacacional = 0;//Variable que almacena el total de prima vacacional.
            Double OJ_Aguinaldo = 0.0;//Variable que almacena la cantidad de orden judicial por concepto de aguinaldo.
            Double OJ_PV = 0.0;//Variable que almacena la cantidad a retener por concepto de orden judicial de la prima vacacional.
            Double OJ_Indemnizacion = 0.0;//Variable que almacena la cantidad a retener por concepto de orden judicial indemnización.

            try
            {
                INF_EMPLEADO = Consultar_Informacion_Empleado(No_Empleado);//Consultamos la información del empleado.
                INF_TIPO_NOMINA = Consultar_Tipo_Nomina(INF_EMPLEADO.P_Tipo_Nomina_ID);//Consultamos la información de los tipos de nomina.
                INF_ZONA_ECONOMICA = Consultar_Zona_Economica();//Consultamos la información de la zona económica.
                INF_PARAMETRO  = Consultar_Parametros_Nomina();//Consulta la información del parámetro de la nómina.
                INF_TERCERO = Consultar_Terceros(INF_EMPLEADO.P_Terceros_ID);//Consultamos la información de los partidos politicos.

                //*****************************************************************************************
                //*********************************** [ PERCEPCIONES ] *************************************
                //*****************************************************************************************
                Dt_Percepciones.Columns.Add("NOMBRE", typeof(String));
                Dt_Percepciones.Columns.Add("TIPO_ASIGNACION", typeof(String));

                if (Dt_Percepciones is DataTable)
                {
                    if (Dt_Percepciones.Rows.Count > 0)
                    {
                        foreach (DataRow PERCEPCIONES in Dt_Percepciones.Rows)
                        {
                            if (PERCEPCIONES is DataRow)
                            {
                                INF_PERCEPCION_DEDUCCION = Consultar_Percepcion_Deduccion(PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim());

                                if (!String.IsNullOrEmpty(PERCEPCIONES["Monto"].ToString().Trim()))
                                {
                                    Cantidad = Convert.ToDouble(PERCEPCIONES["Monto"].ToString().Trim());

                                    if (!String.IsNullOrEmpty(PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim()))
                                    {
                                        if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Prevision_Social_Multiple))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Sueldo_Normal))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Fondo_Retiro))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", 0);
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Vacaciones))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Quinquenio))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Prima_Vacacional))
                                        {                                            
                                            Exenta = (INF_TIPO_NOMINA.P_Dias_Exenta_Prima_Vacacional * INF_ZONA_ECONOMICA.P_Salario_Diario);
                                            Grava = (Cantidad - Exenta);

                                            if (Exenta > Cantidad) {
                                                Exenta = Cantidad;
                                                Grava = 0;
                                            }

                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", Grava);
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", Exenta);

                                            Gravable_Prima_Vacacional = Grava;
                                            Total_Prima_Vacacional = Cantidad;
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Prima_Dominical))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Aguinaldo))
                                        {
                                            Exenta = (30 * INF_ZONA_ECONOMICA.P_Salario_Diario);
                                            Grava = (Cantidad - Exenta);

                                            if (Exenta >= Cantidad)
                                            {
                                                Grava = 0;
                                                Exenta = Cantidad;
                                            }

                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", Grava);
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", Exenta);

                                            Gravable_Aguinaldo = Grava;
                                            Total_Aguinaldo = Cantidad;
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Dias_Festivos))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);
                                            Gravable_Dias_Festivos = Grava;
                                            Exenta_Dias_Festivos = Exenta;
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Horas_Extra))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);
                                            Gravable_Tiempo_Extra = Grava;
                                            Exenta_Tiempo_Extra = Exenta;
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Dia_Doble))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Dia_Domingo))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Ajuste_ISR))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Incapacidades))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", 0);
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Subsidio))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", 0);
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Prima_Antiguedad))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);
                                            Gravable_Prima_Antiguedad = Grava;
                                            Exenta_Prima_Antiguedad = Exenta;
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Indemnizacion))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", ((Cantidad < 0) ? 0 : Cantidad));
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);

                                            Grava = ((Cantidad < 0) ? 0 : Cantidad);

                                            Gravable_Indemnizacion = Grava;
                                            Exenta_Indemnizacion = Exenta;
                                            Total_Indemnizacion = Cantidad;
                                        }
                                        else if (PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Percepcion_Vacaciones_Pendientes_Pagar))
                                        {
                                            PERCEPCIONES["Grava"] = String.Format("{0:0.00}", 0);
                                            PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);
                                        }
                                        else
                                        {
                                            if (INF_PERCEPCION_DEDUCCION.P_TIPO.Trim().ToUpper().Equals("PERCEPCION"))
                                            {
                                                if (INF_PERCEPCION_DEDUCCION.P_GRAVABLE.ToString().Trim().Contains("1"))
                                                {
                                                    Grava = (INF_PERCEPCION_DEDUCCION.P_PORCENTAJE_GRAVABLE / 100) * Cantidad;
                                                    if (Grava < 0) Grava = 0;

                                                    PERCEPCIONES["Grava"] = String.Format("{0:0.00}", Grava);
                                                    PERCEPCIONES["Exenta"] = String.Format("{0:0.00}", 0);
                                                }
                                            }
                                        }

                                        PERCEPCIONES["NOMBRE"] = INF_PERCEPCION_DEDUCCION.P_NOMBRE;
                                        PERCEPCIONES["TIPO_ASIGNACION"] = INF_PERCEPCION_DEDUCCION.P_TIPO_ASIGNACION;

                                        if (INF_PERCEPCION_DEDUCCION.P_TIPO.Trim().ToUpper().Equals("PERCEPCION"))
                                        {
                                            Total_Percepciones += Cantidad;
                                            Ingresos_Gravables_Empleado += ((Grava < 0) ? 0 : Grava);
                                        }

                                        Grava = 0.0;
                                        Exenta = 0.0;
                                    }
                                }
                            }
                        }
                    }
                }

                Dt_Percepciones.Columns["Percepcion_Deduccion"].ColumnName = "PERCEPCION_DEDUCCION_ID";
                Dt_Percepciones.Columns["Grava"].ColumnName = "Gravado";
                Dt_Percepciones.Columns["Exenta"].ColumnName = "Exento";
                Dt_Percepciones.Columns["Monto"].ColumnName = "Cantidad";

                //*****************************************************************************************
                //*********************************** [ DEDUCCIONES ] *************************************
                //*****************************************************************************************
                Dt_Deducciones.Columns.Add("NOMBRE", typeof(String));
                Dt_Deducciones.Columns.Add("TIPO_ASIGNACION", typeof(String));

                //Establecemos información que se usara para los recálculos del finiquito.
                Calculo_Deducciones.P_Nomina_ID = Nomina_ID;
                Calculo_Deducciones.P_No_Nomina = Convert.ToInt32(No_Nomina);
                Calculo_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                //Establecemos los valores de totales de los conceptos.
                Calculo_Deducciones.Total_Percepciones = Total_Percepciones;
                Calculo_Deducciones.Gravable_Aguinaldo = Gravable_Aguinaldo;
                Calculo_Deducciones.Gravable_Prima_Vacacional = Gravable_Prima_Vacacional;
                Calculo_Deducciones.Gravable_Prima_Antiguedad = Gravable_Prima_Antiguedad;
                Calculo_Deducciones.Gravable_Indemnizacion = Gravable_Indemnizacion;
                Calculo_Deducciones.Exenta_Prima_Antiguedad = Exenta_Prima_Antiguedad;
                Calculo_Deducciones.Exenta_Indemnizacion = Exenta_Indemnizacion;
                Calculo_Deducciones.Gravable_Dias_Festivos = Gravable_Dias_Festivos;
                Calculo_Deducciones.Gravable_Tiempo_Extra = Gravable_Tiempo_Extra;
                Calculo_Deducciones.Exenta_Tiempo_Extra = Exenta_Tiempo_Extra;
                Calculo_Deducciones.Exenta_Dias_Festivos = Exenta_Dias_Festivos;

                Calculo_Deducciones.Ingresos_Gravables_Empleado = Ingresos_Gravables_Empleado;
                Calculo_Deducciones.Fecha_Generar_Nomina = Convert.ToDateTime(Fecha_Fin_Periodo);

                Cantidad = 0;

                if (Dt_Deducciones is DataTable)
                {
                    if (Dt_Deducciones.Rows.Count > 0)
                    {
                        foreach (DataRow DEDUCCIONES in Dt_Deducciones.Rows)
                        {
                            if (DEDUCCIONES is DataRow)
                            {
                                if (!String.IsNullOrEmpty(DEDUCCIONES["Monto"].ToString().Trim()))
                                {
                                    //Obtenemos la cantidad.
                                    Cantidad = Convert.ToDouble(DEDUCCIONES["Monto"].ToString().Trim());

                                    if (!String.IsNullOrEmpty(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim()))
                                    {
                                        if (DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Deduccion_ISR))
                                        {
                                            Cantidad = Obtener_Cantidad(Calculo_Deducciones.Calcular_ISR_Total(INF_EMPLEADO.P_Empleado_ID));
                                            DEDUCCIONES["Monto"] = String.Format("{0:0.00}", Cantidad);
                                        }

                                        if (DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Deduccion_Orden_Judicial_Indemnizacion))
                                        {
                                            Cantidad = Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Indemnizacion(INF_EMPLEADO.P_Empleado_ID,
                                                Total_Indemnizacion, Calculo_Deducciones);
                                            DEDUCCIONES["Monto"] = String.Format("{0:0.00}", Cantidad);

                                            OJ_Indemnizacion = Cantidad;
                                        }

                                        if (DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Deduccion_Orden_Judicial_Aguinaldo))
                                        {
                                            Cantidad = Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Aguinaldo(INF_EMPLEADO.P_Empleado_ID,
                                                Total_Aguinaldo, Calculo_Deducciones);
                                            DEDUCCIONES["Monto"] = String.Format("{0:0.00}", Cantidad);

                                            OJ_Aguinaldo = Cantidad;
                                        }

                                        if (DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Deduccion_Orden_Judicial_Prima_Vacacional))
                                        {
                                            Cantidad = Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Prima_Vacacional(INF_EMPLEADO.P_Empleado_ID,
                                                Total_Prima_Vacacional, Calculo_Deducciones);
                                            DEDUCCIONES["Monto"] = String.Format("{0:0.00}", Cantidad);

                                            OJ_PV = Cantidad;
                                        }

                                        INF_PERCEPCION_DEDUCCION = Consultar_Percepcion_Deduccion(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim());
                                        DEDUCCIONES["NOMBRE"] = INF_PERCEPCION_DEDUCCION.P_NOMBRE;
                                        DEDUCCIONES["TIPO_ASIGNACION"] = INF_PERCEPCION_DEDUCCION.P_TIPO_ASIGNACION;

                                        if (INF_PERCEPCION_DEDUCCION.P_TIPO.Trim().ToUpper().Equals("DEDUCCION"))
                                        {
                                            if (!(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Deduccion_Tipo_Desc_Orden_Judicial)) &&
                                                !(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_TERCERO.P_Percepcion_Deduccion_ID)))
                                            {
                                                Total_Deducciones += Cantidad;
                                                Cantidad = 0;
                                            }
                                            
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Cantidad = 0;

                if (Dt_Deducciones is DataTable)
                {
                    if (Dt_Deducciones.Rows.Count > 0)
                    {
                        foreach (DataRow DEDUCCIONES in Dt_Deducciones.Rows)
                        {
                            if (DEDUCCIONES is DataRow)
                            {
                                if (!String.IsNullOrEmpty(DEDUCCIONES["Monto"].ToString().Trim()))
                                {
                                    if (!String.IsNullOrEmpty(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim()))
                                    {
                                        if (DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_PARAMETRO.P_Deduccion_Tipo_Desc_Orden_Judicial))
                                        {
                                            Cantidad = Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial(INF_EMPLEADO.P_Empleado_ID, Calculo_Deducciones,
                                               OJ_Aguinaldo, OJ_PV, OJ_Indemnizacion,
                                               Total_Aguinaldo, Total_Prima_Vacacional, Total_Indemnizacion,
                                               Total_Deducciones);

                                            DEDUCCIONES["Monto"] = String.Format("{0:0.00}", Cantidad);
                                        }

                                        INF_PERCEPCION_DEDUCCION = Consultar_Percepcion_Deduccion(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim());
                                        DEDUCCIONES["NOMBRE"] = INF_PERCEPCION_DEDUCCION.P_NOMBRE;
                                        DEDUCCIONES["TIPO_ASIGNACION"] = INF_PERCEPCION_DEDUCCION.P_TIPO_ASIGNACION;

                                        if (INF_PERCEPCION_DEDUCCION.P_TIPO.Trim().ToUpper().Equals("DEDUCCION"))
                                        {
                                            Total_Deducciones += Cantidad;
                                            Cantidad = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Cantidad = 0;

                if (Dt_Deducciones is DataTable)
                {
                    if (Dt_Deducciones.Rows.Count > 0)
                    {
                        foreach (DataRow DEDUCCIONES in Dt_Deducciones.Rows)
                        {
                            if (DEDUCCIONES is DataRow)
                            {
                                if (!String.IsNullOrEmpty(DEDUCCIONES["Monto"].ToString().Trim()))
                                {
                                    if (!String.IsNullOrEmpty(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim()))
                                    {
                                        if (DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim().Equals(INF_TERCERO.P_Percepcion_Deduccion_ID))
                                        {
                                            Cantidad = Obtener_Cantidad_Deducciones_Retencion_Tercero(INF_EMPLEADO.P_Empleado_ID,
                                                INF_TERCERO, Total_Percepciones, Total_Deducciones);

                                            DEDUCCIONES["Monto"] = String.Format("{0:0.00}", Cantidad);
                                        }

                                        INF_PERCEPCION_DEDUCCION = Consultar_Percepcion_Deduccion(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim());
                                        DEDUCCIONES["NOMBRE"] = INF_PERCEPCION_DEDUCCION.P_NOMBRE;
                                        DEDUCCIONES["TIPO_ASIGNACION"] = INF_PERCEPCION_DEDUCCION.P_TIPO_ASIGNACION;

                                        if (INF_PERCEPCION_DEDUCCION.P_TIPO.Trim().ToUpper().Equals("DEDUCCION"))
                                        {
                                            Total_Deducciones += Cantidad;
                                            Cantidad = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Dt_Deducciones.Columns["Percepcion_Deduccion"].ColumnName = "PERCEPCION_DEDUCCION_ID";
                Dt_Deducciones.Columns["Grava"].ColumnName = "Gravado";
                Dt_Deducciones.Columns["Exenta"].ColumnName = "Exento";
                Dt_Deducciones.Columns["Monto"].ColumnName = "Cantidad";
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar el calculo de exentos y gravados. Error: [" + Ex.Message + "");
            }
        }
        #endregion

        #region (Consulta Información)
        /// ***********************************************************************************
        /// Nombre: Consultar_Informacion_Empleado
        /// 
        /// Descripción: Consulta la información general del empleado.
        /// 
        /// Parámetros: No_Empleado.- Identificador interno del sistema para las operaciones que
        ///                           se realizan sobre los empelados.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        protected Cls_Cat_Empleados_Negocios Consultar_Informacion_Empleado(String No_Empleado)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();//Variable que almacenara la información del empleado.
            DataTable Dt_Empleado = null;//Variable que almacena el registro búscado del empleado.

            try
            {
                Obj_Empleados.P_No_Empleado = No_Empleado;
                Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();//Consultamos la información del empleado.

                if (Dt_Empleado is DataTable) {
                    if (Dt_Empleado.Rows.Count > 0) {
                        foreach (DataRow EMPLEADO in Dt_Empleado.Rows) {
                            if (EMPLEADO is DataRow) {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Tipo_Nomina_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Zona_ID = EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()))
                                    INF_EMPLEADO.P_Salario_Diario = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Terceros_ID = EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las información del empleado. Error: [" + Ex.Message + "]");
            }
            return INF_EMPLEADO;
        }
        /// ***********************************************************************************
        /// Nombre: Consultar_Zona_Economica
        /// 
        /// Descripción: Consulta la información general de la zona económica a la que pertenece
        ///              el empleado.
        /// 
        /// Parámetros: Zona_ID.- Identificador de la zona económica.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        protected Cls_Cat_Nom_Zona_Economica_Negocio Consultar_Zona_Economica()
        {
            Cls_Cat_Nom_Zona_Economica_Negocio Obj_Zona_Economica = new Cls_Cat_Nom_Zona_Economica_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = new Cls_Cat_Nom_Zona_Economica_Negocio();//Variable que almacena la información de la zona económica.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacenara la información del parámetro de la nómina.
            DataTable Dt_Zona_Economica = null;//Variable que almacena la información del registro búscado.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL PARÁMETRO DE LA NÓMINA.
                INF_PARAMETRO = Consultar_Parametros_Nomina();

                //CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONÓMICA ESTABLECIDA COMO PARÁMETRO.
                Obj_Zona_Economica.P_Zona_ID = INF_PARAMETRO.P_Zona_ID;
                Dt_Zona_Economica = Obj_Zona_Economica.Consulta_Datos_Zona_Economica();//consulta la información de la zona económica.

                if (Dt_Zona_Economica is DataTable) {
                    if (Dt_Zona_Economica.Rows.Count > 0) {
                        foreach (DataRow ZONA_ECONOMICA in Dt_Zona_Economica.Rows) {
                            if (ZONA_ECONOMICA is DataRow) {
                                if (!String.IsNullOrEmpty(ZONA_ECONOMICA[Cat_Nom_Zona_Economica.Campo_Zona_ID].ToString().Trim()))
                                    INF_ZONA_ECONOMICA.P_Zona_ID = ZONA_ECONOMICA[Cat_Nom_Zona_Economica.Campo_Zona_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(ZONA_ECONOMICA[Cat_Nom_Zona_Economica.Campo_Salario_Diario].ToString().Trim()))
                                    INF_ZONA_ECONOMICA.P_Salario_Diario = Convert.ToDouble(ZONA_ECONOMICA[Cat_Nom_Zona_Economica.Campo_Salario_Diario].ToString().Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la zona economica. Error: [" + ex.Message + "]");
            }
            return INF_ZONA_ECONOMICA;
        }
        /// ***********************************************************************************
        /// Nombre: Consultar_Tipo_Nomina
        /// 
        /// Descripción: Consulta la información del tipo de nómina a la que pertence el empleado.
        /// 
        /// Parámetros: Tipo_Nomina_ID.- identificador del tipo de nómina.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        protected Cls_Cat_Tipos_Nominas_Negocio Consultar_Tipo_Nomina(String Tipo_Nomina_ID)
        {
            Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión a la capa de negocios.
            Cls_Cat_Tipos_Nominas_Negocio INF_TIPO_NOMINA = new Cls_Cat_Tipos_Nominas_Negocio();//Variable que almacena la información del tipo de nómina.
            DataTable Dt_Tipo_Nomina = null;//Variable que almacena el registro del tipo de nómina búscado.

            try
            {
                Obj_Tipos_Nominas.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Tipo_Nomina = Obj_Tipos_Nominas.Consulta_Datos_Tipo_Nomina();//Consultamos la información del tipo de nómina.

                if (Dt_Tipo_Nomina is DataTable) {
                    if (Dt_Tipo_Nomina.Rows.Count > 0) {
                        foreach (DataRow TIPO_NOMINA in Dt_Tipo_Nomina.Rows) {
                            if (TIPO_NOMINA is DataRow) {

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Exenta_Prima_Vacacional = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Exenta_Aguinaldo = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Aguinaldo = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Prima_Vacacional_1= Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Prima_Vacacional_2 = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Prima_Antiguedad = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim();

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Aplica_ISR = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString().Trim();

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Tipo_Nomina_ID = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString().Trim();

                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Errro al consultar la información del empleado. Error: [" + Ex.Message + "]");
            }
            return INF_TIPO_NOMINA;
        }
        /// ***********************************************************************************
        /// Nombre: Consultar_Parametros_Nomina
        /// 
        /// Descripción: Consulta la información del parámetro de la nómina.
        /// 
        /// Parámetros: No Áplica.
        /// 
        /// Usuario Creo: Juan alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        protected Cls_Cat_Nom_Parametros_Negocio Consultar_Parametros_Nomina()
        {
            Cls_Cat_Nom_Parametros_Negocio Obj_Parametros = new Cls_Cat_Nom_Parametros_Negocio();//Variable de conexión a la capa de negocios.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = new Cls_Cat_Nom_Parametros_Negocio();//Variable que almacena la información del parámetro de nómina.
            DataTable Dt_Parametro = null;//Variable que almacena el registro del parámetro de la nómina.
            String Mi_SQL = "";//Variable que almacenará la consulta a ejecutar.
            DataTable Dt_Retencion_Terceros = null;//Variable que almacenará el registro de retencion a terceros.

            try
            {
                Dt_Parametro = Obj_Parametros.Consulta_Parametros();

                if (Dt_Parametro is DataTable)
                {
                    if (Dt_Parametro.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Zona_ID].ToString()))
                            INF_PARAMETROS.P_Zona_ID = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Zona_ID].ToString();

                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString()))
                            INF_PARAMETROS.P_Percepcion_Quinquenio = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString()))
                            INF_PARAMETROS.P_Percepcion_Prima_Vacacional = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString()))
                            INF_PARAMETROS.P_Percepcion_Prima_Dominical = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString()))
                            INF_PARAMETROS.P_Percepcion_Aguinaldo = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString()))
                            INF_PARAMETROS.P_Percepcion_Dias_Festivos = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString()))
                            INF_PARAMETROS.P_Percepcion_Horas_Extra = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString()))
                            INF_PARAMETROS.P_Percepcion_Dia_Doble = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString()))
                            INF_PARAMETROS.P_Percepcion_Dia_Domingo = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString()))
                            INF_PARAMETROS.P_Percepcion_Ajuste_ISR = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString()))
                            INF_PARAMETROS.P_Percepcion_Incapacidades = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString()))
                            INF_PARAMETROS.P_Percepcion_Subsidio = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString()))
                            INF_PARAMETROS.P_Percepcion_Prima_Antiguedad = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString()))
                            INF_PARAMETROS.P_Percepcion_Indemnizacion = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString()))
                            INF_PARAMETROS.P_Percepcion_Vacaciones_Pendientes_Pagar = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString()))
                            INF_PARAMETROS.P_Percepcion_Vacaciones = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString()))
                            INF_PARAMETROS.P_Percepcion_Fondo_Retiro = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString()))
                            INF_PARAMETROS.P_Percepcion_Sueldo_Normal = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString()))
                            INF_PARAMETROS.P_Percepcion_Prevision_Social_Multiple = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString();

                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString()))
                            INF_PARAMETROS.P_Deduccion_Faltas = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString()))
                            INF_PARAMETROS.P_Deduccion_Retardos = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString()))
                            INF_PARAMETROS.P_Deduccion_Fondo_Retiro = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISR].ToString()))
                            INF_PARAMETROS.P_Deduccion_ISR = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISR].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString()))
                            INF_PARAMETROS.P_Deduccion_IMSS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString()))
                            INF_PARAMETROS.P_Deduccion_Vacaciones_Tomadas_Mas = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString()))
                            INF_PARAMETROS.P_Deduccion_Aguinaldo_Pagado_Mas = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString()))
                            INF_PARAMETROS.P_Deduccion_Prima_Vac_Pagada_Mas = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString()))
                            INF_PARAMETROS.P_Deduccion_Sueldo_Pagado_Mas = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo].ToString()))
                            INF_PARAMETROS.P_Deduccion_Orden_Judicial_Aguinaldo = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional].ToString()))
                            INF_PARAMETROS.P_Deduccion_Orden_Judicial_Prima_Vacacional = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion].ToString()))
                            INF_PARAMETROS.P_Deduccion_Orden_Judicial_Indemnizacion = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial].ToString()))
                            INF_PARAMETROS.P_Deduccion_Tipo_Desc_Orden_Judicial = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString()))
                            INF_PARAMETROS.P_Deduccion_ISSEG = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString();

                    }//Fin de la validación de que existan algún registro del parámetro.
                }//Fin de la Validación de los parámetros consultados.
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la informacion de los parametros de nomina. Error: [" + Ex.Message + "]");
            }
            return INF_PARAMETROS;
        }
        /// ***********************************************************************************
        /// Nombre: Consultar_Terceros
        /// 
        /// Descripción: Consulta la información de la tabla de terceros que existe en el sistema.
        /// 
        /// Parámetros: Tercero_ID.- Identificador del registro de tercero.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        protected Cls_Cat_Nom_Terceros_Negocio Consultar_Terceros(String Tercero_ID)
        {
            Cls_Cat_Nom_Terceros_Negocio Obj_Terceros = new Cls_Cat_Nom_Terceros_Negocio();//Variable de conexión a la capa de negocios.
            Cls_Cat_Nom_Terceros_Negocio INF_TERCERO = new Cls_Cat_Nom_Terceros_Negocio();//Variable que almacena la información del partido politico.
            DataTable Dt_Tercero = null;//Variable que almacena el registro del partido politico búscado.

            try
            {
                Obj_Terceros.P_Tercero_ID = Tercero_ID;
                Dt_Tercero = Obj_Terceros.Consultar_Terceros_Nombre();

                if (Dt_Tercero is DataTable) {
                    if (Dt_Tercero.Rows.Count > 0) {
                        foreach (DataRow TERCERO in Dt_Tercero.Rows) {
                            if (TERCERO is DataRow) {
                                if (!String.IsNullOrEmpty(TERCERO[Cat_Nom_Terceros.Campo_Nombre].ToString()))
                                    INF_TERCERO.P_Nombre = TERCERO[Cat_Nom_Terceros.Campo_Nombre].ToString();

                                if (!String.IsNullOrEmpty(TERCERO[Cat_Nom_Terceros.Campo_Contacto].ToString()))
                                    INF_TERCERO.P_Contacto = TERCERO[Cat_Nom_Terceros.Campo_Contacto].ToString();

                                if (!String.IsNullOrEmpty(TERCERO[Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID].ToString()))
                                    INF_TERCERO.P_Percepcion_Deduccion_ID = TERCERO[Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID].ToString();

                                if (!String.IsNullOrEmpty(TERCERO[Cat_Nom_Terceros.Campo_Porcentaje_Retencion].ToString()))
                                    INF_TERCERO.P_Porcentaje_Retencion = Convert.ToDouble(TERCERO[Cat_Nom_Terceros.Campo_Porcentaje_Retencion].ToString());

                                if (!String.IsNullOrEmpty(TERCERO[Cat_Nom_Terceros.Campo_Tercero_ID].ToString()))
                                    INF_TERCERO.P_Tercero_ID = TERCERO[Cat_Nom_Terceros.Campo_Tercero_ID].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la información del tercero. Error: [" + Ex.Message + "]");
            }
            return INF_TERCERO;
        }
        /// ***********************************************************************************
        /// Nombre: Consultar_Percepcion_Deduccion
        /// 
        /// Descripción: Consulta la información de las percepciones y deducciones.
        /// 
        /// Parámetros: Percepcion_Deduccion_ID. Identificador de la percepción o deducción.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        protected Cls_Cat_Nom_Percepciones_Deducciones_Business Consultar_Percepcion_Deduccion(String Percepcion_Deduccion_ID)
        {
            Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepcion_Deduccion = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexión con la capa de negocios.
            Cls_Cat_Nom_Percepciones_Deducciones_Business INF_PERCEPCION_DEDUCCION = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable que almacena la información de percepción deducción.
            DataTable Dt_Percepcion_Deduccion = null;//Variable que almacena el registro búscado de percepción y deducción.

            try
            {
                Obj_Percepcion_Deduccion.P_PERCEPCION_DEDUCCION_ID = Percepcion_Deduccion_ID;
                Dt_Percepcion_Deduccion = Obj_Percepcion_Deduccion.Consultar_Percepciones_Deducciones_General();

                if (Dt_Percepcion_Deduccion is DataTable) {
                    if (Dt_Percepcion_Deduccion.Rows.Count > 0) {
                        foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepcion_Deduccion.Rows) {
                            if (PERCEPCION_DEDUCCION is DataRow) {

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim())) {
                                    INF_PERCEPCION_DEDUCCION.P_NOMBRE = "[" + PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " +
                                        PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();
                                }

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString().Trim()))
                                    INF_PERCEPCION_DEDUCCION.P_TIPO_ASIGNACION = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString().Trim();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString().Trim()))
                                    INF_PERCEPCION_DEDUCCION.P_TIPO = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString().Trim();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                    INF_PERCEPCION_DEDUCCION.P_PERCEPCION_DEDUCCION_ID = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString().Trim()))
                                    INF_PERCEPCION_DEDUCCION.P_GRAVABLE = Convert.ToDouble(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString().Trim());

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString().Trim()))
                                    INF_PERCEPCION_DEDUCCION.P_PORCENTAJE_GRAVABLE = Convert.ToDouble(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString().Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la informacion de la percepción y/ deducción. Error: [" + Ex.Message + "]");
            }
            return INF_PERCEPCION_DEDUCCION;
        }
        #endregion

        #region (Orden Judicial)
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Indemnizacion
        /// 
        /// DESCRIPCIÓN: Obtiene el monto por concepto de Orden Judicial, si es que la misma aplicara al empleado por
        ///              concepto de su indemnización.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del sistema para el control de las operaciones sobre los empleados.
        ///             INDEMNIZACION.- Cantidad que le corresponde al empleado por concepto de INDEMNIZACÍÓN.
        ///             Calculos.- Objeto que almacena los valores que se ocuparan para realizar el calculo de orden 
        ///                        judicial por concepto de indemnización.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 2/Febrero/2011
        /// USUARIO MODIFICO:Juan Alberto Hernández Negrete.
        /// FECHA MODIFICO: 16/Agosto/2011
        /// CAUSA MODIFICACION: Ajustar Calculo de Orden Judicialcon los cambios que hubo en el calculo al momento de revizar el diseño
        ///                     con el personal de recursos humanos.
        ///*****************************************************************************************************************************
        private Double Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Indemnizacion(String Empleado_ID, Double INDEMNIZACION,
            Cls_Ope_Nom_Deducciones_Negocio Calculos)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            DataTable Dt_Empleados = null;//Variable que almacenar'a una lista de los empleados.
            Double Cantidad_Retencion_Orden_Judicial = 0.0;//Cantidad que se le retendra al empleado.
            Double Cant_Porc_Retener_Indemnizacion = 0.0;//Cantidad o Porcentaje que se descontara al empleado por concepto de retención de orden judicial.
            String Tipo_Desc_Retencion_OJ_Indemnizacion = String.Empty;//Parámetro que nos indica si la retención será por un monto fijo, o por un porcentaje de retención.
            String OJ_Bruto_Neto_Indemnizacion = String.Empty;//Variable que almacena el valor si la retención se le hará al empleado sobre el NETO O EL BRUTO de su sueldo.
            Int32 Contador_Beneficiarios = 1;//Variable que llevara la cuenta de beneficiarios.
            Double Cantidad_ISR_Indemnizacion = 0.0;//Cantidad de ISR a retener al empleado por concepto de indemnización.
            Double Cantidad_Total_Orden_Judicial = 0.0;//Variable que almacena la cantidad que se le descontara al empleado por concepto de indemnización.

            try
            {
                //Cosnulta de el tipo de retencion que tiene el empleado, de acuerdo al partido al que pertenece.
                Mi_SQL = "SELECT " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + ".*" +
                        " FROM " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial +
                        " WHERE " + Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID + "='" + Empleado_ID + "'";

                //Ejecutamos la consulta.
                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Validamos los resultados de la consulta realizada.
                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow PARAMTROS_OJ in Dt_Empleados.Rows)
                        {
                            if (PARAMTROS_OJ is DataRow)
                            {
                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Indemnizacion].ToString()))
                                    Cant_Porc_Retener_Indemnizacion = Convert.ToDouble(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Indemnizacion].ToString().Trim());

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Indemnizacion].ToString()))
                                    Tipo_Desc_Retencion_OJ_Indemnizacion = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Indemnizacion].ToString().Trim();

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Indemnizacion].ToString()))
                                    OJ_Bruto_Neto_Indemnizacion = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Indemnizacion].ToString();

                                //Si aplica, validamos si dicha retención se hará por un monto fijo ó por un porcentaje
                                //sobre el total de sus percepciones.
                                if (Tipo_Desc_Retencion_OJ_Indemnizacion.Equals("CANTIDAD"))
                                {
                                    //Obtenemos el monto a descontar al empleado por concepto de orden judicial.
                                    Cantidad_Total_Orden_Judicial += Cant_Porc_Retener_Indemnizacion;
                                }
                                else if (Tipo_Desc_Retencion_OJ_Indemnizacion.Equals("PORCENTAJE"))
                                {
                                    if (OJ_Bruto_Neto_Indemnizacion.ToString().Trim().ToUpper().Equals("BRUTO"))
                                    {
                                        Double Bruto = INDEMNIZACION;
                                        Cant_Porc_Retener_Indemnizacion = Cant_Porc_Retener_Indemnizacion / 100;
                                        Cantidad_Retencion_Orden_Judicial = (Bruto * Cant_Porc_Retener_Indemnizacion);
                                        //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                        //retención de orden judicial.
                                        Cantidad_Total_Orden_Judicial += Cantidad_Retencion_Orden_Judicial;
                                    }
                                    else if (OJ_Bruto_Neto_Indemnizacion.ToString().Trim().ToUpper().Equals("NETO"))
                                    {
                                        Cantidad_ISR_Indemnizacion = Obtener_Cantidad_ISR_Particular(Calculos.Calcular_ISPT_Percepciones_Por_Retiro(Empleado_ID));
                                        Cantidad_ISR_Indemnizacion = (Calculos.Gravable_Indemnizacion * Cantidad_ISR_Indemnizacion) / (Calculos.Gravable_Indemnizacion +
                                            Calculos.Gravable_Prima_Antiguedad);

                                        Double Neto = INDEMNIZACION - Cantidad_ISR_Indemnizacion;
                                        if (Neto < 0) Neto = 0;

                                        Cant_Porc_Retener_Indemnizacion = Cant_Porc_Retener_Indemnizacion / 100;
                                        Cantidad_Retencion_Orden_Judicial = (Neto * Cant_Porc_Retener_Indemnizacion);

                                        //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                        //retención de orden judicial.
                                        Cantidad_Total_Orden_Judicial += Cantidad_Retencion_Orden_Judicial;
                                    }
                                }
                                Contador_Beneficiarios++;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Obtener Cantidad Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Total_Orden_Judicial;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Aguinaldo
        /// 
        /// DESCRIPCIÓN: Obtiene el monto de Orden Judicial por concepto del pago del aguinaldo. Se consideran las [N] beneficiarios
        ///              que tenga el empleado.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del sistema para el control de las operaciones sobre los empleados.
        ///             AGUINALDO.- Cantidad de aguinaldo que le corresponde al empleado.
        ///             Calculos.- Objeto que almacena los valores que se ocuparan para realizar el calculo de orden 
        ///                        judicial por concepto de indemnización.
        ///
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 2/Febrero/2011
        /// USUARIO MODIFICO:Juan Alberto Hernández Negrete.
        /// FECHA MODIFICO:16/Agosto/2011
        /// CAUSA MODIFICACION: Ajustar Calculo de Orden Judicialcon los cambios que hubo en el calculo al momento de revizar el diseño
        ///                     con el personal de recursos humanos.
        ///*****************************************************************************************************************************
        private Double Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Aguinaldo(String Empleado_ID, Double AGUINALDO,
            Cls_Ope_Nom_Deducciones_Negocio Calculos)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            DataTable Dt_Empleados = null;//Variable que almacenar'a una lista de los empleados.
            Double Cantidad_Retencion_Orden_Judicial = 0.0;//Cantidad que se le retendra al empleado.
            Double Cant_Porc_Retener_Aguinaldo = 0.0;//Cantidad o Porcentaje que se descontara al empleado por concepto de retención de orden judicial.
            String Tipo_Desc_Retencion_OJ_Aguinaldo = String.Empty;//Parámetro que nos indica si la retención será por un monto fijo, o por un porcentaje de retención.
            String OJ_Bruto_Neto_Aguinaldo = String.Empty;//Variable que almacena el valor si la retención se le hará al empleado sobre el NETO O EL BRUTO de su sueldo.
            Int32 Contador_Beneficiarios = 1;
            Double Cantidad_ISR_Aguinaldo = 0.0;
            Double Cantidad_Total_Orden_Judicial = 0.0;

            try
            {
                //Cosnulta de el tipo de retencion que tiene el empleado, de acuerdo al partido al que pertenece.
                Mi_SQL = "SELECT " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + ".*" +
                        " FROM " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial +
                        " WHERE " + Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID + "='" + Empleado_ID + "'";

                //Ejecutamos la consulta.
                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Validamos los resultados de la consulta realizada.
                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow PARAMTROS_OJ in Dt_Empleados.Rows)
                        {
                            if (PARAMTROS_OJ is DataRow)
                            {
                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Aguinaldo].ToString()))
                                    Cant_Porc_Retener_Aguinaldo = Convert.ToDouble(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Aguinaldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Aguinaldo].ToString()))
                                    Tipo_Desc_Retencion_OJ_Aguinaldo = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Aguinaldo].ToString().Trim();

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Aguinaldo].ToString()))
                                    OJ_Bruto_Neto_Aguinaldo = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Aguinaldo].ToString();

                                //Si aplica, validamos si dicha retención se hará por un monto fijo ó por un porcentaje
                                //sobre el total de sus percepciones.
                                if (Tipo_Desc_Retencion_OJ_Aguinaldo.Equals("CANTIDAD"))
                                {
                                    //Obtenemos el monto a descontar al empleado por concepto de orden judicial.
                                    Cantidad_Total_Orden_Judicial += Cant_Porc_Retener_Aguinaldo;
                                }
                                else if (Tipo_Desc_Retencion_OJ_Aguinaldo.Equals("PORCENTAJE"))
                                {
                                    if (OJ_Bruto_Neto_Aguinaldo.ToString().Trim().ToUpper().Equals("BRUTO"))
                                    {
                                        Double Bruto = AGUINALDO;
                                        Cant_Porc_Retener_Aguinaldo = Cant_Porc_Retener_Aguinaldo / 100;
                                        Cantidad_Retencion_Orden_Judicial = (Bruto * Cant_Porc_Retener_Aguinaldo);
                                        //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                        //retención de orden judicial.
                                        Cantidad_Total_Orden_Judicial += Cantidad_Retencion_Orden_Judicial;
                                    }
                                    else if (OJ_Bruto_Neto_Aguinaldo.ToString().Trim().ToUpper().Equals("NETO"))
                                    {
                                        Cantidad_ISR_Aguinaldo = Obtener_Cantidad_ISR_Particular(Calculos.Calcular_ISPT_Prima_Vacacional_Aguinaldo(Empleado_ID));
                                        Cantidad_ISR_Aguinaldo = (Calculos.Gravable_Aguinaldo * Cantidad_ISR_Aguinaldo) / (Calculos.Gravable_Aguinaldo +
                                            Calculos.Gravable_Prima_Vacacional);

                                        Double Neto = AGUINALDO - Cantidad_ISR_Aguinaldo;
                                        if (Neto < 0) Neto = 0;

                                        Cant_Porc_Retener_Aguinaldo = Cant_Porc_Retener_Aguinaldo / 100;
                                        Cantidad_Retencion_Orden_Judicial = (Neto * Cant_Porc_Retener_Aguinaldo);

                                        //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                        //retención de orden judicial.
                                        Cantidad_Total_Orden_Judicial += Cantidad_Retencion_Orden_Judicial;
                                    }
                                }
                                Contador_Beneficiarios++;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Obtener Cantidad Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Total_Orden_Judicial;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Prima_Vacacional
        /// 
        /// DESCRIPCIÓN: Obtiene el monto que se le descontara al empleado por concepto de orden judicial que se le aplicara
        ///              a la cantidad de prima vacacional.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del sistema para el control de las operaciones sobre los empleados.
        ///             PRIMA_VACACIONAL.- Cantidad de prima vacacional que le corresponde al empleado.
        ///             Calculos.- Objeto que almacena los valores que se ocuparan para realizar el calculo de orden 
        ///                        judicial por concepto de prima vacacional.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 2/Febrero/2011
        /// USUARIO MODIFICO:Juan Alberto Hernández Negrete.
        /// FECHA MODIFICO: 16/Agosto/2011
        /// CAUSA MODIFICACION: Ajustar Calculo de Orden Judicialcon los cambios que hubo en el calculo al momento de revizar el diseño
        ///                     con el personal de recursos humanos.
        ///*****************************************************************************************************************************
        private Double Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Prima_Vacacional(String Empleado_ID, Double PRIMA_VACACIONAL,
            Cls_Ope_Nom_Deducciones_Negocio Calculos)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            DataTable Dt_Empleados = null;//Variable que almacenar'a una lista de los empleados.
            Double Cantidad_Retencion_Orden_Judicial = 0.0;//Cantidad que se le retendra al empleado.
            Double Cant_Porc_Retener_Prima_Vacacional = 0.0;//Cantidad o Porcentaje que se descontara al empleado por concepto de retención de orden judicial.
            String Tipo_Desc_Retencion_OJ_Prima_Vacacional = String.Empty;//Parámetro que nos indica si la retención será por un monto fijo, o por un porcentaje de retención.
            String OJ_Bruto_Neto_Prima_Vacacional = String.Empty;//Variable que almacena el valor si la retención se le hará al empleado sobre el NETO O EL BRUTO de su sueldo.
            Int32 Contador_Beneficiarios = 1;
            Double Cantidad_ISR_Prima_Vacacional = 0.0;
            Double Cantidad_Total_Orden_Judicial = 0.0;

            try
            {
                //Cosnulta de el tipo de retencion que tiene el empleado, de acuerdo al partido al que pertenece.
                Mi_SQL = "SELECT " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + ".*" +
                        " FROM " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial +
                        " WHERE " + Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID + "='" + Empleado_ID + "'";

                //Ejecutamos la consulta.
                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Validamos los resultados de la consulta realizada.
                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow PARAMTROS_OJ in Dt_Empleados.Rows)
                        {
                            if (PARAMTROS_OJ is DataRow)
                            {
                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_PV].ToString()))
                                    Cant_Porc_Retener_Prima_Vacacional = Convert.ToDouble(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_PV].ToString().Trim());

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_PV].ToString()))
                                    Tipo_Desc_Retencion_OJ_Prima_Vacacional = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_PV].ToString().Trim();

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_PV].ToString()))
                                    OJ_Bruto_Neto_Prima_Vacacional = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_PV].ToString();

                                //Si aplica, validamos si dicha retención se hará por un monto fijo ó por un porcentaje
                                //sobre el total de sus percepciones.
                                if (Tipo_Desc_Retencion_OJ_Prima_Vacacional.Equals("CANTIDAD"))
                                {
                                    //Obtenemos el monto a descontar al empleado por concepto de orden judicial.
                                    Cantidad_Total_Orden_Judicial += Cant_Porc_Retener_Prima_Vacacional;
                                }
                                else if (Tipo_Desc_Retencion_OJ_Prima_Vacacional.Equals("PORCENTAJE"))
                                {
                                    if (OJ_Bruto_Neto_Prima_Vacacional.ToString().Trim().ToUpper().Equals("BRUTO"))
                                    {
                                        Double Bruto = PRIMA_VACACIONAL;
                                        Cant_Porc_Retener_Prima_Vacacional = Cant_Porc_Retener_Prima_Vacacional / 100;
                                        Cantidad_Retencion_Orden_Judicial = (Bruto * Cant_Porc_Retener_Prima_Vacacional);
                                        //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                        //retención de orden judicial.
                                        Cantidad_Total_Orden_Judicial += Cantidad_Retencion_Orden_Judicial;
                                    }
                                    else if (OJ_Bruto_Neto_Prima_Vacacional.ToString().Trim().ToUpper().Equals("NETO"))
                                    {
                                        Cantidad_ISR_Prima_Vacacional = Obtener_Cantidad_ISR_Particular(Calculos.Calcular_ISPT_Prima_Vacacional(Empleado_ID));

                                        Double Neto = PRIMA_VACACIONAL - Cantidad_ISR_Prima_Vacacional;
                                        if (Neto < 0) Neto = 0;

                                        Cant_Porc_Retener_Prima_Vacacional = Cant_Porc_Retener_Prima_Vacacional / 100;
                                        Cantidad_Retencion_Orden_Judicial = (Neto * Cant_Porc_Retener_Prima_Vacacional);

                                        //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                        //retención de orden judicial.
                                        Cantidad_Total_Orden_Judicial += Cantidad_Retencion_Orden_Judicial;
                                    }
                                }
                                Contador_Beneficiarios++;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Obtener Cantidad Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Total_Orden_Judicial;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial
        /// 
        /// DESCRIPCIÓN: Obtiene la cantidad de orden judicial que se le descontara al empleado.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador interno del sistema para controlar las operaciones sobre los empleados.
        ///             Calculos.- Objeto que almacena los valores que son necesarios para realizaras las operaciones.
        ///             OJ_AGUINALDO.- Cantidad de orden judicial que se le descontara al empleado por concepto de aguinaldo.
        ///             OJ_PRIMA_VACACIONAL.- Cantidad de orden judicial que se le descontara al empleado por conceptode prima vacacional.
        ///             OJ_INDEMNIZACION.- Cantidad de orden judicial que se le descontara al empleado por concepto de indemnización.
        ///             TOTAL_AGUINALDO.- Cantidad de aguinaldo que le corresponde al empleado.
        ///             TOTAL_PV.- Cantidad de prima vacacional que le corresponde al empleado.
        ///             OJ_INDEMNIZACION.- Cantidad de indemnización que le corresponde al empleado.
        ///             Total_Deducciones.- Cantidad total de deducciones que tiene el empleado sin contar retencion de terceros para
        ///                                 este cálculo.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 2/Febrero/2011
        /// USUARIO MODIFICO:Juan Alberto Hernández Negrete.
        /// FECHA MODIFICO: 16/Agosto/2011
        /// CAUSA MODIFICACION: Ajustar Calculo de Orden Judicialcon los cambios que hubo en el calculo al momento de revizar el diseño
        ///                     con el personal de recursos humanos.
        ///*****************************************************************************************************************************
        private Double Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial(String Empleado_ID, Cls_Ope_Nom_Deducciones_Negocio Calculos,
            Double OJ_AGUINALDO, Double OJ_PRIMA_VACACIONAL, Double OJ_INDEMNIZACION,
            Double TOTAL_AGUINALDO, Double TOTAL_PV, Double TOTAL_INDEMNIZACION,
            Double Total_Deducciones)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            DataTable Dt_Empleados = null;//Variable que almacenar'a una lista de los empleados.
            Double Cantidad_Retencion_Orden_Judicial = 0.0;//Cantidad que se le retendra al empleado.
            Double Cant_Porc_Retener_Sueldo = 0.0;//Cantidad o Porcentaje que se descontara al empleado por concepto de retención de orden judicial.
            String Tipo_Desc_Retencion_OJ_Sueldo = String.Empty;//Parámetro que nos indica si la retención será por un monto fijo, o por un porcentaje de retención.
            String OJ_Bruto_Neto_Sueldo = String.Empty;//Variable que almacena el valor si la retención se le hará al empleado sobre el NETO O EL BRUTO de su sueldo.
            Int32 Contador_Beneficiarios = 1;
            Double Cantidad_ISR_Sueldo = 0.0;
            Double Cantidad_Total_Orden_Judicial = 0.0;
            Double Total_Percepciones_Empleado = 0.0;//Total de percepciones del empleado, en su nómina actual.
            Double Total_Deducciones_Empleado = 0.0;//Total de deducciones del empleado, en su nómina actual.

            try
            {
                //Cosnulta de el tipo de retencion que tiene el empleado, de acuerdo al partido al que pertenece.
                Mi_SQL = "SELECT " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + ".*" +
                        " FROM " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial +
                        " WHERE " + Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID + "='" + Empleado_ID + "'";

                //Ejecutamos la consulta.
                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Validamos los resultados de la consulta realizada.
                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow PARAMTROS_OJ in Dt_Empleados.Rows)
                        {
                            if (PARAMTROS_OJ is DataRow)
                            {
                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo].ToString()))
                                    Cant_Porc_Retener_Sueldo = Convert.ToDouble(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Sueldo].ToString()))
                                    Tipo_Desc_Retencion_OJ_Sueldo = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Sueldo].ToString().Trim();

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Sueldo].ToString()))
                                    OJ_Bruto_Neto_Sueldo = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Sueldo].ToString();

                                //Si aplica, validamos si dicha retención se hará por un monto fijo ó por un porcentaje
                                //sobre el total de sus percepciones.
                                if (Tipo_Desc_Retencion_OJ_Sueldo.Equals("CANTIDAD"))
                                {
                                    //Obtenemos el monto a descontar al empleado por concepto de orden judicial.
                                    Cantidad_Total_Orden_Judicial += Cant_Porc_Retener_Sueldo;
                                }
                                else if (Tipo_Desc_Retencion_OJ_Sueldo.Equals("PORCENTAJE"))
                                {
                                    if (OJ_Bruto_Neto_Sueldo.ToString().Trim().ToUpper().Equals("BRUTO"))
                                    {
                                        Double Bruto = Calculos.Total_Percepciones - (TOTAL_AGUINALDO + TOTAL_PV + TOTAL_INDEMNIZACION);
                                        Cant_Porc_Retener_Sueldo = Cant_Porc_Retener_Sueldo / 100;
                                        Cantidad_Retencion_Orden_Judicial = (Bruto * Cant_Porc_Retener_Sueldo);
                                        //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                        //retención de orden judicial.
                                        Cantidad_Total_Orden_Judicial += Cantidad_Retencion_Orden_Judicial;
                                    }
                                    else if (OJ_Bruto_Neto_Sueldo.ToString().Trim().ToUpper().Equals("NETO"))
                                    {
                                        Total_Percepciones_Empleado = Calculos.Total_Percepciones - (TOTAL_AGUINALDO + TOTAL_PV + TOTAL_INDEMNIZACION);
                                        Total_Deducciones_Empleado = Total_Deducciones - (OJ_AGUINALDO + OJ_PRIMA_VACACIONAL + OJ_INDEMNIZACION);

                                        Double Neto = (Total_Percepciones_Empleado - Total_Deducciones_Empleado);

                                        if (Neto < 0) Neto = 0;

                                        Cant_Porc_Retener_Sueldo = Cant_Porc_Retener_Sueldo / 100;
                                        Cantidad_Retencion_Orden_Judicial = (Neto * Cant_Porc_Retener_Sueldo);

                                        //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                        //retención de orden judicial.
                                        Cantidad_Total_Orden_Judicial += Cantidad_Retencion_Orden_Judicial;
                                    }
                                }
                                Contador_Beneficiarios++;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Obtener Cantidad Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Total_Orden_Judicial;
        }
        #endregion 

        #region (Retencion de Terceros)
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Tercero
        /// 
        /// DESCRIPCIÓN: Realiza el calculo de la Retención de Terceros. Este calculo se realiza por separado ya que es necesario
        ///              conocer la cantidad total de Percepciones y  la cantidad total de Deducciones. Que tiene el empleado.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del sistema para realizar las operaciones sobre los empleados.
        ///             INF_TERCERO.- Informacion de los parámetros que aplican al empleado por concepto de retención de terceros.
        ///             Tota_Percepciones.- Cantidad total de percepiones que tiene el empleado.
        ///             Total_Deducciones.- Cantidad total de deducciones que tiene el empleado.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 19/Enero/2011
        /// USUARIO MODIFICO: 16/Agosto/"011
        /// FECHA MODIFICO: Ajustar el cálculo para que se pueda realizar el recálculo de la retención de terceros.
        ///*****************************************************************************************************************************
        private Double Obtener_Cantidad_Deducciones_Retencion_Tercero(String Empleado_ID, Cls_Cat_Nom_Terceros_Negocio INF_TERCERO,
            Double Tota_Percepciones, Double Total_Deducciones)
        {
            Double Cantidad = 0.0;//Cantidad que se le retendra al empleado.
            Double Neto = 0.0;

            try
            {
                Neto = Tota_Percepciones - Total_Deducciones;
                Cantidad = Neto * (INF_TERCERO.P_Porcentaje_Retencion / 100);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Obtener Cantidad Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
            return Cantidad;
        }
        #endregion

        #region (Métodos Generales)
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_ISR_Particular
        /// 
        /// DESCRIPCIÓN: Obtiene la cantidad de la tabla de resultados.
        /// 
        /// PARÁMETROS: Dt_Resultados_Calculo_Actual.- Resultados del calculo de la percepción.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 14/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private Double Obtener_Cantidad_ISR_Particular(DataTable Dt_Resultados)
        {
            Double Cantidad = 0.0;//Variable que almacenará la cantidad que le aplica según la percepción.

            try
            {
                if (Dt_Resultados is DataTable)
                {
                    if (Dt_Resultados.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(Dt_Resultados.Rows[0]["Cantidad"].ToString().Trim()))
                            Cantidad = Convert.ToDouble(Dt_Resultados.Rows[0]["Cantidad"].ToString().Trim());
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: [" + Ex.Message + "]");
            }
            return Cantidad;
        }
        /// ***************************************************************************************************************************
        /// NOMBRE: Obtener_Cantidad
        /// 
        /// DESCRIPCIÓN: Obtiene la cantidad de una tabla de la tabla que es pasada como parámetro.
        /// 
        /// PARÁMTROS: Dt_Resultado.- La tabla de cuál se obtendrá la cantidad.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 16/Agosto/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// ***************************************************************************************************************************
        protected Double Obtener_Cantidad(DataTable Dt_Resultado)
        {
            Double Cantidad = 0.0;//Variable que almacena la cantidad que se lee de la tabla que es pasada como parámetro.

            try
            {
                if (Dt_Resultado is DataTable)
                {
                    if (Dt_Resultado.Rows.Count > 0)
                    {
                        foreach (DataRow FILA in Dt_Resultado.Rows)
                        {
                            if (FILA is DataRow)
                            {
                                if (!String.IsNullOrEmpty(FILA["Cantidad"].ToString().Trim()))
                                    Cantidad = Convert.ToDouble(FILA["Cantidad"].ToString().Trim());//Obtenemos la cantidad.
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad de la tabla de resultados. Error: [" + Ex.Message + "]");
            }
            return Cantidad;
        }
        #endregion
    }
}
