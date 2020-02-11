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
using System.Collections.Generic;
using Presidencia.Nomina_Percepciones_Deducciones;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Calculo_Percepciones.Negocio;
using Presidencia.Calculo_Deducciones.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Recibos_Empleados.Negocio;
using Presidencia.Faltas_Empleado.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Negocio;
using Presidencia.Calculo_Percepciones.Datos;
using Presidencia.Calculo_Deducciones.Datos;
using System.Text;
using Presidencia.Ayudante_Informacion;
using Presidencia.Archivos_Historial_Nomina_Generada;
using Presidencia.IMSS.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Puestos.Negocios;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Cat_Terceros.Negocio;

namespace Presidencia.Finiquitos.Negocio
{
    public class Cls_Ope_Nom_Finiquitos_Negocio
    {
        ///VARIABLES GLOBALES PARA LA GENERACIÓN DE LA NÓMINA
        Double Total_Grava_Prima_Dominical = 0.0;//Variable que almacenrá el total que gravará la prima dominical del empleado.
        Double Total_Grava_Prima_Vacacional = 0.0;//Variable que almacenrá el total que gravará la prima vacacional del empleado.
        Double Total_Grava_Aguinaldo = 0.0;//Variable que almacenrá el total que gravará el aguinaldo del empleado.
        Double Total_Ingresos_Gravables_Empleado = 0.0;//Variable que almacenrá el total de ingresos gravables del empleado.
        Double Total_Exenta_Empleado = 0.0;//Variable que almacena la cantidad los ingresosexentos del empelado.  
        Double Total_Grava_Prima_Antiguedad = 0.0;//Variable que almacena la cantidad que grava la prima de antiguedad.
        Double Total_Grava_Indemnizacion = 0.0;//Variable que almacena la cantidad que grava la indemnización del empleado.
        Double Total_Exenta_Prima_Antiguedad = 0.0;//Variable almacena el exento de la prima de antiguedad.
        Double Total_Exenta_Indemnizacion = 0.0;//Variable que almacena el exento de la indemnizacion.
        Double Total_Grava_Tiempo_Extra = 0.0;//Variable que almacena la cantidad que grava el tiempo extra.
        Double Total_Grava_Dias_Festivos = 0.0;//Variable que almacena la cantidad que gravan los dias festivos.
        Double Total_Exenta_Tiempo_Extra = 0.0;//Variable que almacena la cantidad que exenta el tiempo extra.
        Double Total_Exenta_Dias_Festivos = 0.0;//Variable que almacena la cantidad que exentan los dias festivos.
        Double Total_Percepciones = 0.0;//Variable que almacenrá el total de las percepciones recibidas por el empleado.
        Double Total_Deducciones = 0.0;//Variable que almacenrá el total de deducciones aplicadas al empleado.
        public DateTime Fecha_Catorcena_Generar_Nomina = new DateTime();//Fecha Final de la catorcena de cuál se desea generar la nómina.
        Double Total_Grava_Sueldo = 0.0;
        Double Total_Aguinaldo = 0.0;
        Double Total_Prima_Vacacional = 0.0;
        Double Total_Indemnizacion = 0.0;
        Double Total_Sueldo = 0.0;
        Double Total_OJ_Aguinaldo = 0.0;
        Double Total_OJ_PV = 0.0;
        Double Total_OJ_Indemnizacion = 0.0;

        #region (Variables Privadas)
        private String Nomina_ID = "";                  //Variable que almacenará el Identificador de la nómina.
        private Int32 No_Nomina = 0;                    //Variable que almacena el numero de catorcena de la cual se desea generar la nómina.
        private String Detalle_Nomina_ID = "";          //Variable que identifica el perido seleccionado para generar la nómina.
        private String Tipo_Nomina_ID = "";             //Variable que almacena el tipo de nómina de la cual se desea generar la nómina.
        private String Tipo_Salario = String.Empty;
        #endregion

        #region (Variables Públicas)
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        public Int32 P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        public String P_Detalle_Nomina_ID
        {
            get { return Detalle_Nomina_ID; }
            set { Detalle_Nomina_ID = value; }
        }

        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }

        public String P_Tipo_Salario {
            get { return Tipo_Salario; }
            set { Tipo_Salario = value; }
        }

        #endregion

        ///-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        ///-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        ///***************************************************************    FINIQUITOS NÓMINA    *******************************************************************************
        ///-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        ///-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        #region (Percepciones y/o Deducciones, Generales del Empleado)
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Percepiones_IO_Deducciones
        /// DESCRIPCIÓN: Obtiene una Lista de todas percepciones y/o deducciones que aplica para el empleado.
        /// 
        /// PARÁMETROS: Empleado_ID .- Clave del empleado del cuál se está calculando la nómina.
        ///             Fecha_Pago_Nomina .- Fecha que se tomara para obtener las fecha del cálculo de la catorcena actual.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 4/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        public List<List<Cls_Percepciones_Deducciones>> Obtener_Percepiones_IO_Deducciones(String Empleado_ID, DateTime Fecha_Pago_Nomina)
        {
            ///Datos para la generación de del alta del recibo de nómina del empleado.
            Cls_Cat_Empleados_Negocios Informacion_Empleado = new Cls_Cat_Empleados_Negocios();//Conexion con la capa de negocios.
            Cls_Ope_Nom_Percepciones_Negocio Informacion_Salario_Empleado = new Cls_Ope_Nom_Percepciones_Negocio();//Variable de conexion con la capa de negocios.
            Cls_Ope_Nom_Recibos_Empleados_Negocio Recibo_Nomina_Empleado = new Cls_Ope_Nom_Recibos_Empleados_Negocio();//Variable de conexion con la capa de negocios.
            List<List<Cls_Percepciones_Deducciones>> Coleccion_Listas_Percepciones_Deducciones = new List<List<Cls_Percepciones_Deducciones>>();//Almacenar una lista de deducciones y/o percepciones que se utillizan para la generación de la nómina.
            ///Concepto SINDICATO [FIJAS - CALCULADAS]
            List<Cls_Percepciones_Deducciones> Sindicato_Percepciones_Fijas_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Percepciones Fijas del empleado.
            List<Cls_Percepciones_Deducciones> Sindicato_Deducciones_Fijas_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Deducciones Fijas del empleado.
            List<Cls_Percepciones_Deducciones> Sindicato_Percepciones_Calculadas_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Percepciones Calculadas del empleado.
            List<Cls_Percepciones_Deducciones> Sindicato_Deducciones_Calculadas_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Deducciones Calculadas del empleado.
            ///Concepto TIPO NÓMINA [FIJAS - VARIABLES -CALCULADAS]
            List<Cls_Percepciones_Deducciones> Tipo_Nomina_Percepciones_Fijas_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Percepciones Fijas del empleado.
            List<Cls_Percepciones_Deducciones> Tipo_Nomina_Deducciones_Fijas_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Deducciones Fijas del empleado.
            List<Cls_Percepciones_Deducciones> Tipo_Nomina_Percepciones_Variables_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Percepciones Variables del empleado.
            List<Cls_Percepciones_Deducciones> Tipo_Nomina_Deducciones_Variables_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Deducciones Variables del empleado.
            List<Cls_Percepciones_Deducciones> Tipo_Nomina_Percepciones_Calculadas_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Percepciones Calculadas del empleado.
            List<Cls_Percepciones_Deducciones> Tipo_Nomina_Deducciones_Calculadas_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Deducciones Calculadas del empleado.
            ///Lista de Percepción y/o Deducción [RETENCION DE ISR ó SUBSIDIO AL EMPLEADO]
            List<Cls_Percepciones_Deducciones> ISR_Retener_Subsidio_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista que almacenará un solo registro que almacena la información correspondiente al concepto Retención ISR o Subsidio.
            List<Cls_Percepciones_Deducciones> Lista_Temporal_Percepciones_Calculadas = new List<Cls_Percepciones_Deducciones>();//Lista temporal que almacenará una lista de Percepciones Calculadas por Tipo Nómina.
            List<Cls_Percepciones_Deducciones> Lista_Temporal_Deducciones_Calculadas = new List<Cls_Percepciones_Deducciones>();//Lista temporal que almacenará una lista de Deducciones Calculadas por Tipo Nómina.
            ///Lista de Deducciones que corresponden a los Préstamos que el empleado tiene actualmente.
            List<Cls_Percepciones_Deducciones> Prestamos_Deducciones_Calculadas_Empleado = new List<Cls_Percepciones_Deducciones>();//Lista de Deducciones Calculadas del empleado.
            ///Lista de Deducción por concepto de Retención a Terceros.
            List<Cls_Percepciones_Deducciones> Retencion_Terceros = new List<Cls_Percepciones_Deducciones>();//Variable que corresponde a la retención de terceros que se aplicará la emplaedo.
            ///DEDUCCIÓN POR CONCEPTO DE RETENCIÓN DE ORDEN JUDICIAL.
            List<Cls_Percepciones_Deducciones> Retencion_Orden_Judicial_Sueldo = new List<Cls_Percepciones_Deducciones>();//Estructura de tipo lista que almacenrá si el empleado tiene alguna percepción que le corresponda a este concepto.
            List<Cls_Percepciones_Deducciones> Retencion_Orden_Judicial_Aguinaldo = new List<Cls_Percepciones_Deducciones>();//Estructura de tipo lista que almacenrá si el empleado tiene alguna percepción que le corresponda a este concepto.
            List<Cls_Percepciones_Deducciones> Retencion_Orden_Judicial_Prima_Vacacional = new List<Cls_Percepciones_Deducciones>();//Estructura de tipo lista que almacenrá si el empleado tiene alguna percepción que le corresponda a este concepto.
            List<Cls_Percepciones_Deducciones> Retencion_Orden_Judicial_Indemnizacion = new List<Cls_Percepciones_Deducciones>();//Estructura de tipo lista que almacenrá si el empleado tiene alguna percepción que le corresponda a este concepto.
            ///Variable de Tipo Estructura requeridas para la generación de la nómina.
            DataTable Dt_Percepciones_Deducciones = null;//Variable que almacenará una lista de percepciones y deducciones según los filtros seleccionados.
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Percepciones_Deducciones Percepcion_Subsidio_Empleado = null;//Concepto que corresponde a la cantidad que se le otorgara al empleado como subsidio.
            Cls_Percepciones_Deducciones Deduccion_Retencion_ISR = null;//Concepto que corresponde a la catidad que se le retendrá al empleado como concepto de ISR.

            try
            {
                //Obetenemos la fecha en la cual se generara la nómina.
                Fecha_Catorcena_Generar_Nomina = Fecha_Pago_Nomina;

                ///EN ESTE PUNTO SE CONSULTARAN TODAS LAS PERCEPCIONES Y/O DEDUCCIONES QUE TIENE EL EMPLEADO.
                ///Inicio...

                #region (Consulta Percepciones y/o Deducciones Empleado Por Sindicato)
                ///Paso I .- Obtenemos Percepciones y/o Deducciones [FIJAS - CALCULADAS] (Sindicato)
                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "PERCEPCION", "FIJA", "SINDICATO");
                Sindicato_Percepciones_Fijas_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);

                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "DEDUCCION", "FIJA", "SINDICATO");
                Sindicato_Deducciones_Fijas_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);

                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "PERCEPCION", "OPERACION", "SINDICATO");
                Sindicato_Percepciones_Calculadas_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);

                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "DEDUCCION", "OPERACION", "SINDICATO");
                Sindicato_Deducciones_Calculadas_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);
                #endregion

                #region (Consulta Percepciones y/o Deducciones Empleado Por Tipo Nómina)
                ///Paso II.- Obtenemos Percepcionesy/o Deducciones [FIJAS - VARIABLES- CALCULADAS] (Tipo Nomina)
                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "PERCEPCION", "FIJA", "TIPO_NOMINA");
                Tipo_Nomina_Percepciones_Fijas_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);

                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "DEDUCCION", "FIJA", "TIPO_NOMINA");
                Tipo_Nomina_Deducciones_Fijas_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);

                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "PERCEPCION", "VARIABLE", "TIPO_NOMINA");
                Tipo_Nomina_Percepciones_Variables_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);

                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "DEDUCCION", "VARIABLE", "TIPO_NOMINA");
                Tipo_Nomina_Deducciones_Variables_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);

                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "PERCEPCION", "OPERACION", "TIPO_NOMINA");
                Tipo_Nomina_Percepciones_Calculadas_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);

                Dt_Percepciones_Deducciones = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "DEDUCCION", "OPERACION", "TIPO_NOMINA");
                Tipo_Nomina_Deducciones_Calculadas_Empleado = Obtener_Lista_Percepciones_Deducciones(Dt_Percepciones_Deducciones);
                #endregion

                ///...

                #region (Percepciones Obtener Montos)
                ///******************************************************************************************************************************************
                ///**************************************************** PERCEPCIONES DEDUCCIONES FIJAS *************************************************
                ///******************************************************************************************************************************************
                ///Percepciones Fijas Concepto [TIPO_NOMINA - SINDICATO]
                //CONCEPTO [ SINDICATO]
                Sindicato_Percepciones_Fijas_Empleado = Obtener_Percepciones_Deucciones_Fijas_Aplica(Sindicato_Percepciones_Fijas_Empleado);
                //CONCEPTO [ TIPO_NOMINA ]
                Tipo_Nomina_Percepciones_Fijas_Empleado = Obtener_Percepciones_Deucciones_Fijas_Aplica(Tipo_Nomina_Percepciones_Fijas_Empleado);
                ///******************************************************************************************************************************************
                ///**************************************************** PERCEPCIONES VARIABLES *************************************************
                ///******************************************************************************************************************************************
                //Obtenemos las percepciones y/o deducciones variables del Empleado [Nota: Solo Aplica para el concepto TIPO_NOMINA].
                Tipo_Nomina_Percepciones_Variables_Empleado = Obtener_Cantidad_Percepciones_Variables(Tipo_Nomina_Percepciones_Variables_Empleado, Empleado_ID);
                ///******************************************************************************************************************************************
                ///**************************************************** PERCEPCIONES CALCULADAS *************************************************
                ///******************************************************************************************************************************************
                //Obtenemos las percepciones y/o deducciones Calculadas TIPO_NOMINA
                Lista_Temporal_Percepciones_Calculadas = Tipo_Nomina_Percepciones_Calculadas_Empleado;//Guardamos temporalmente la lista que almacena las Percepciones Calculadas por tipo de nómina. [Cálculo del Subsidio]
                Tipo_Nomina_Percepciones_Calculadas_Empleado = Obtener_Cantidad_Percepciones_Calculadas_Aplica(Tipo_Nomina_Percepciones_Calculadas_Empleado, Empleado_ID);
                //Obtenemos las Percepciones y/o Deducciones Calculadas SINDICATO.
                Sindicato_Percepciones_Calculadas_Empleado = Obtener_Cantidad_Percepciones_Calculadas_Aplica(Sindicato_Percepciones_Calculadas_Empleado, Empleado_ID);
                ///SE REALIZA UN BARRIDO SUMANDO LOS MONTOS DE CADA DEDUCCIÓN QUE LE APLICARA AL EMPLEADO EN EL CÁLCULO DE SU NÓMINA. 
                Leer_Percepciones_Deducciones_Empleados(Sindicato_Percepciones_Fijas_Empleado, Empleado_ID, "PERCEPCION");
                Leer_Percepciones_Deducciones_Empleados(Tipo_Nomina_Percepciones_Fijas_Empleado, Empleado_ID, "PERCEPCION");
                Leer_Percepciones_Deducciones_Empleados(Tipo_Nomina_Percepciones_Variables_Empleado, Empleado_ID, "PERCEPCION");
                Leer_Percepciones_Deducciones_Empleados(Tipo_Nomina_Percepciones_Calculadas_Empleado, Empleado_ID, "PERCEPCION");
                Leer_Percepciones_Deducciones_Empleados(Sindicato_Percepciones_Calculadas_Empleado, Empleado_ID, "PERCEPCION");
                #endregion

                #region (Deducciones Obtener Montos)
                ///******************************************************************************************************************************************
                ///**************************************************** DEDUCCIONES FIJAS *******************************************************************
                ///******************************************************************************************************************************************
                //CONCEPTO [ SINDICATO ]
                Sindicato_Deducciones_Fijas_Empleado = Obtener_Percepciones_Deucciones_Fijas_Aplica(Sindicato_Deducciones_Fijas_Empleado);
                //CONCEPTO [ TIPO_NOMINA ]
                Tipo_Nomina_Deducciones_Fijas_Empleado = Obtener_Percepciones_Deucciones_Fijas_Aplica(Tipo_Nomina_Deducciones_Fijas_Empleado);
                ///******************************************************************************************************************************************

                ///******************************************************************************************************************************************
                ///**************************************************** DEDUCCIONES VARIABLES *************************************************
                ///******************************************************************************************************************************************
                Tipo_Nomina_Deducciones_Variables_Empleado = Obtener_Cantidad_Deducciones_Variables(Tipo_Nomina_Deducciones_Variables_Empleado, Empleado_ID);
                ///******************************************************************************************************************************************

                ///******************************************************************************************************************************************
                ///**************************************************** DEDUCCIONES CALCULADAS *************************************************
                ///******************************************************************************************************************************************
                //Obtenemos las percepciones y/o deducciones Calculadas TIPO_NOMINA
                Retencion_Terceros = Tipo_Nomina_Deducciones_Calculadas_Empleado;//Almacenamos temporalmente la lista de Deducciones Calculadas por Tipo de Nómina. [Cálculo Retencion de Terceros].
                Lista_Temporal_Deducciones_Calculadas = Tipo_Nomina_Deducciones_Calculadas_Empleado;//Almacenamos temporalmente la lista de Deducciones Calculadas por Tipo de Nómina. [Cálculo de ISR].

                Tipo_Nomina_Deducciones_Calculadas_Empleado = Obtener_Cantidad_Deducciones_Calculadas_Aplica(Tipo_Nomina_Deducciones_Calculadas_Empleado, Empleado_ID);
                //Obtenemos las Percepciones y/o Deducciones Calculadas SINDICATO.
                Sindicato_Deducciones_Calculadas_Empleado = Obtener_Cantidad_Deducciones_Calculadas_Aplica(Sindicato_Deducciones_Calculadas_Empleado, Empleado_ID);
                ///******************************************************************************************************************************************

                //****************************************************************************************************************************************************************
                ///**************************************************** PRESTAMOS DEDUCCIONES CALCULADAS *************************************************************************
                //****************************************************************************************************************************************************************
                Prestamos_Deducciones_Calculadas_Empleado = Obtener_Prestamos_Deducciones_Calculadas(Prestamos_Deducciones_Calculadas_Empleado, Empleado_ID);
                //****************************************************************************************************************************************************************

                ///***************************************************************************************************************************************************************
                ///**************************************************** RETENCIÓN POR ORDEN JUDICIAL *****************************************************************************
                ///***************************************************************************************************************************************************************
                Retencion_Orden_Judicial_Indemnizacion = Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Indemnizacion(Lista_Temporal_Deducciones_Calculadas, Empleado_ID);
                Retencion_Orden_Judicial_Aguinaldo = Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Aguinaldo(Lista_Temporal_Deducciones_Calculadas, Empleado_ID);
                Retencion_Orden_Judicial_Prima_Vacacional = Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Prima_Vacacional(Lista_Temporal_Deducciones_Calculadas, Empleado_ID);
                //****************************************************************************************************************************************************************

                //****************************************************************************************************************************************************************
                ///SE REALIZA UN BARRIDO SUMANDO LOS MONTOS DE CADA DEDUCCIÓN QUE LE APLICARA AL EMPLEADO EN EL CÁLCULO DE SU NÓMINA. ********************************************
                //****************************************************************************************************************************************************************
                Leer_Percepciones_Deducciones_Empleados(Sindicato_Deducciones_Fijas_Empleado, Empleado_ID, "DEDUCCION");
                Leer_Percepciones_Deducciones_Empleados(Tipo_Nomina_Deducciones_Fijas_Empleado, Empleado_ID, "DEDUCCION");
                Leer_Percepciones_Deducciones_Empleados(Tipo_Nomina_Deducciones_Variables_Empleado, Empleado_ID, "DEDUCCION");
                Leer_Percepciones_Deducciones_Empleados(Tipo_Nomina_Deducciones_Calculadas_Empleado, Empleado_ID, "DEDUCCION");
                Leer_Percepciones_Deducciones_Empleados(Sindicato_Deducciones_Calculadas_Empleado, Empleado_ID, "DEDUCCION");
                Leer_Percepciones_Deducciones_Empleados(Prestamos_Deducciones_Calculadas_Empleado, Empleado_ID, "DEDUCCION");
                Leer_Percepciones_Deducciones_Empleados(Retencion_Orden_Judicial_Indemnizacion, Empleado_ID, "DEDUCCION");
                Leer_Percepciones_Deducciones_Empleados(Retencion_Orden_Judicial_Aguinaldo, Empleado_ID, "DEDUCCION");
                Leer_Percepciones_Deducciones_Empleados(Retencion_Orden_Judicial_Prima_Vacacional, Empleado_ID, "DEDUCCION");
                //****************************************************************************************************************************************************************

                ///******************************************************************************************************************************************
                ///*************************************** BÚSQUEDA DEDUCCIÓN CORRESPONDE ISR ***************************************************************
                Deduccion_Retencion_ISR = Identificar_Percepcion_Dedeuccion(Lista_Temporal_Deducciones_Calculadas);
                ///******************************************************************************************************************************************

                ///*************************************** BÚSQUEDA PERCEPCIÓN CORRESPONDE SUBSIDIO  ********************************************************
                Percepcion_Subsidio_Empleado = Identificar_Percepcion_Dedeuccion(Lista_Temporal_Percepciones_Calculadas);
                ///******************************************************************************************************************************************

                ///*********************** OBTENER LA CANTIDAD DE ISR A RETENER AL EMPLEADO O SUBSIDIO ******************************************************
                ISR_Retener_Subsidio_Empleado = Obtener_ISR_Cas_Empleado(Deduccion_Retencion_ISR, Percepcion_Subsidio_Empleado, Empleado_ID);
                ///******************************************************************************************************************************************

                if (ISR_Retener_Subsidio_Empleado.Count == 2)
                {
                    if (ISR_Retener_Subsidio_Empleado.ElementAt(0) != null)
                        Leer_Percepciones_Deducciones_Empleados(ISR_Retener_Subsidio_Empleado, Empleado_ID, ISR_Retener_Subsidio_Empleado.ElementAt(0).P_Es_ISR_Subsidio);
                    if (ISR_Retener_Subsidio_Empleado.ElementAt(1) != null)
                        Leer_Percepciones_Deducciones_Empleados(ISR_Retener_Subsidio_Empleado, Empleado_ID, ISR_Retener_Subsidio_Empleado.ElementAt(1).P_Es_ISR_Subsidio);
                }

                ///***************************************************************************************************************************************************************
                ///******************************************* RETENCIÓN POR ORDEN JUDICIAL SUELDO*****************************************************************************
                ///***************************************************************************************************************************************************************
                Retencion_Orden_Judicial_Sueldo = Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial(Lista_Temporal_Deducciones_Calculadas, Empleado_ID);
                Leer_Percepciones_Deducciones_Empleados(Retencion_Orden_Judicial_Sueldo, Empleado_ID, "DEDUCCION");//Lee la deducción.
                ///******************************************************************************************************************************************

                ///******************************************************************************************************************************************
                ///**************************************************** RETENCIÓN TERCEROS DEDUCCIONES CALCULADAS *******************************************
                ///******************************************************************************************************************************************
                Retencion_Terceros = Obtener_Cantidad_Deducciones_Retencion_Tercero(Retencion_Terceros, Empleado_ID);
                Leer_Percepciones_Deducciones_Empleados(Retencion_Terceros, Empleado_ID, "DEDUCCION");
                ///******************************************************************************************************************************************
                #endregion

                ///Cierre...
                ///EN ESTE PUNTO YA SE CONOCEN TODAS LAS PERCEPCIONES Y/O DEDUCCIONES QUE APLICARAN EN EL CÁLCULO DEL FINIQUITO DEL EMPLEADO.


                #region (Obtener Lista de Percepciones y/o Deducciones)
                ///****************************  Generación del Recibo de Nómina  ******************************************
                //AGREGAR LAS LISTAS A LA COLECCION DE LISTAS QUE ALMACENARA TODAS LAS LISTAS DE PERCEPCIONES Y/O DEDUCCIONES
                //PERCEPCIONES
                Coleccion_Listas_Percepciones_Deducciones.Add(Sindicato_Percepciones_Fijas_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Tipo_Nomina_Percepciones_Fijas_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Tipo_Nomina_Percepciones_Variables_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Tipo_Nomina_Percepciones_Calculadas_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Sindicato_Percepciones_Calculadas_Empleado);
                //DEDUCCIONES
                Coleccion_Listas_Percepciones_Deducciones.Add(Sindicato_Deducciones_Fijas_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Tipo_Nomina_Deducciones_Fijas_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Tipo_Nomina_Deducciones_Variables_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Tipo_Nomina_Deducciones_Calculadas_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Sindicato_Deducciones_Calculadas_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Prestamos_Deducciones_Calculadas_Empleado);
                Coleccion_Listas_Percepciones_Deducciones.Add(Retencion_Terceros);
                Coleccion_Listas_Percepciones_Deducciones.Add(Retencion_Orden_Judicial_Sueldo);
                Coleccion_Listas_Percepciones_Deducciones.Add(Retencion_Orden_Judicial_Indemnizacion);
                Coleccion_Listas_Percepciones_Deducciones.Add(Retencion_Orden_Judicial_Aguinaldo);
                Coleccion_Listas_Percepciones_Deducciones.Add(Retencion_Orden_Judicial_Prima_Vacacional);
                //RETENCIÓN ISR Ó SUBSIDIO AL EMPLEADO
                Coleccion_Listas_Percepciones_Deducciones.Add(ISR_Retener_Subsidio_Empleado);
                #endregion
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar la nómina del empleado. Error: [" + Ex.Message + "]");
            }
            return Coleccion_Listas_Percepciones_Deducciones;
        }
        #endregion

        #region (Percepcion y/o Deducciones Fijas)
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Percepciones_Deucciones_Fijas_Aplica
        /// DESCRIPCIÓN: Recorre y Válida que las Percepciones y/o Deducciones Fijas del Empleado Apliquén para el periodo actual.
        /// 
        /// PARÁMETROS: Listas_Percepciones_Deucciones_Fijas .- Lista de Percepciones y/o Deducciones Fijas.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 14/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Percepciones_Deucciones_Fijas_Aplica(List<Cls_Percepciones_Deducciones> Listas_Percepciones_Deucciones_Fijas)
        {
            List<Cls_Percepciones_Deducciones> Listas_Percepciones_Deucciones_Fijas_Auxiliar = new List<Cls_Percepciones_Deducciones>();//Sirve para almacenar la percepciones y/o deducciones que si aplican.
            try
            {
                //Recorremos la lista de Percepciones y/o Deducciones para validar que apliquen al periodo actual.
                foreach (Cls_Percepciones_Deducciones Percepcion_Deduccion_Fijas in Listas_Percepciones_Deucciones_Fijas)
                {
                    //Paso II.- Validamos si la percepcion grava.
                    if (Percepcion_Deduccion_Fijas.P_Gravable == 1)
                    {
                        //Paso III.- Si Grava obtenemos la cantidad que grava.
                        Percepcion_Deduccion_Fijas.P_Grava = ((Percepcion_Deduccion_Fijas.P_Porcentaje_Gravable / 100) * Percepcion_Deduccion_Fijas.P_Monto);
                    }

                    if (Percepcion_Deduccion_Fijas.P_Tipo.Trim().ToUpper().Equals("DEDUCCION"))
                    {
                        if (Percepcion_Deduccion_Fijas.P_Saldo > 0)
                        {
                            //Si la deducción tiene algún monto se descontara el [30%] del saldo.
                            Percepcion_Deduccion_Fijas.P_Monto = Percepcion_Deduccion_Fijas.P_Saldo;
                        }
                    }

                    //Agregamos el concepto fijo a la lista que las almacenara temporalmente.
                    Listas_Percepciones_Deucciones_Fijas_Auxiliar.Add(Percepcion_Deduccion_Fijas);

                }//Fin del If válida el monto o cantidad que el empleado percibirá
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al validar si la percepcion y/deduccion fija aplica o no [FINIQUITO]. Error: [" + Ex.Message + "]");
            }
            return Listas_Percepciones_Deucciones_Fijas_Auxiliar;
        }
        #endregion

        #region (Percepcion y/o Deducciones Variables FINIQUITO)
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Percepciones_Variables                                                                      
        /// DESCRIPCIÓN: Obtener la cantidad varible que se le otorgara al empleado.                                                         
        ///                                                                                                             
        /// PARÁMETROS: Lista_Percepciones_Var .- Lista de percepciones Variables que le aplican al empleado.                   
        ///             Empleado_ID .- Identificador del Empleado para las operaciones internas del sistem.                                
        ///                                                                                                              
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.                                                                   
        /// FECHA CREÓ: 4/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Cantidad_Percepciones_Variables(List<Cls_Percepciones_Deducciones> Lista_Percepciones_Var, String Empleado_ID)
        {
            Double Cantidad = 0.0;//Variable que almacena la cantidad a otorgar o deducir al empleado.
            List<Cls_Percepciones_Deducciones> Lista_Perc_Var_Auxiliar = new List<Cls_Percepciones_Deducciones>();//Lista de Percepciones Variables.
            Double Cantidad_Grava = 0.0;//Variable que almacenará la cantidad que gravará la percepción.

            try
            {
                foreach (Cls_Percepciones_Deducciones Percepcion in Lista_Percepciones_Var)
                {
                    //Paso I.- Obtenemos la cantidad variable que el empleado percibirá.
                    Cantidad = Obtener_Cantidad_Percepcion_Variable_Aplica(Empleado_ID, Percepcion.P_Clabe);

                    //Paso II.- Validamos si la percepcion grava.
                    if (Percepcion.P_Gravable == 1)
                    {
                        //Paso III.- Si Grava obtenemos la cantidad que grava.
                        Cantidad_Grava = ((Percepcion.P_Porcentaje_Gravable / 100) * Cantidad);
                    }

                    //Paso IV.- Creamos un objeto para establecer datos como [ID Percepcion, Nombre, Monto, Exento].
                    Cls_Percepciones_Deducciones Objeto_Percepcion_Variable = new Cls_Percepciones_Deducciones();

                    //Establecemos los valores
                    Objeto_Percepcion_Variable.P_Nombre = Percepcion.P_Nombre;
                    Objeto_Percepcion_Variable.P_Clabe = Percepcion.P_Clabe;
                    Objeto_Percepcion_Variable.P_Monto = Cantidad;
                    Objeto_Percepcion_Variable.P_Grava = Cantidad_Grava;
                    Objeto_Percepcion_Variable.P_Aplica = Percepcion.P_Aplica;
                }//Fin del foreach
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad por concepto de percepción variable [FINIQUITO]. Error [" + Ex.Message + "]");
            }
            return Lista_Perc_Var_Auxiliar;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Variables
        /// DESCRIPCIÓN: Obtener la cantidad varible que se le deducira al empleado.     
        /// 
        /// PARÁMETROS: Lista_Deducciones_Var .- Lista de deducciones Variables que le aplican al empleado. 
        ///             No_Empleado .- Número único que identifica al empleado.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 4/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Cantidad_Deducciones_Variables(List<Cls_Percepciones_Deducciones> Lista_Deducciones_Var, String Empleado_ID)
        {
            Double Cantidad = 0.0;//Almacenará la cantidad a otorgar o deducir al empleado.
            List<Cls_Percepciones_Deducciones> Lista_Dedu_Var_Auxiliar = new List<Cls_Percepciones_Deducciones>();//Lista de Deducciones que se le aplicaran al empleado.
            Cls_Percepciones_Deducciones Objeto_Deduccion_Variable = null;

            try
            {
                foreach (Cls_Percepciones_Deducciones Deduccion in Lista_Deducciones_Var)
                {
                    //Paso I.- Obtenemos la cantidad que se le deducirá al empleado.
                    Cantidad = Obtener_Cantidad_Deduccion_Variable_Aplica(Empleado_ID, Deduccion.P_Clabe);

                    //Paso II.- Creamos un objeto para establecer datos como [ID Percepcion, Nombre, Monto, Exento].
                    Objeto_Deduccion_Variable = new Cls_Percepciones_Deducciones();
                    Objeto_Deduccion_Variable.P_Nombre = Deduccion.P_Nombre;
                    Objeto_Deduccion_Variable.P_Clabe = Deduccion.P_Clabe;
                    Objeto_Deduccion_Variable.P_Monto = Cantidad;
                    Objeto_Deduccion_Variable.P_Aplica = Deduccion.P_Aplica;

                    //Paso III.- Válidamos que exista una cantidad. De lo contrario la deducción no aplicá 
                    if (Cantidad > 0)
                    {
                        Lista_Dedu_Var_Auxiliar.Add(Objeto_Deduccion_Variable);
                    }//Fin del If válida el monto o cantidad que el empleado percibirá
                }//Fin del foreach
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad que se le retendrá al empleado por concepto dededucción variable [FINIQUITO]. Error: [" + Ex.Message + "]");
            }
            return Lista_Dedu_Var_Auxiliar;
        }
        ///********************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Cantidad_Deduccion_Variable_Aplica
        /// DESCRIPCION : Consulta las Deducciones variables por Empleado_ID y por la Deducción a aplicar.
        ///               Valida el Estatus de la Deduccion Variable y si tiene un Estatus de Autorizado se obtiene la 
        ///               cantidad a descontar al empleado.
        ///               
        /// PARAMETROS  : Empleado_ID.- Empleado al cuál le aplica la deducción. 
        ///               Percepcion_Deduccion_ID.- Deduccion Variable que se le aplicará al empleado.
        ///               
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///********************************************************************************************************************************
        private Double Obtener_Cantidad_Deduccion_Variable_Aplica(String Empleado_ID, String Percepcion_Deduccion_ID)
        {
            DataTable Dt_Deduccion_Var_Empl_Det = null;//Variable que almacenará la deducción consultada.
            Double Cantidad = 0.0;//Variable que almacenará la cantidad a descontar al empleado.

            try
            {
                //Paso III.- ejecutamos la consulta.
                Dt_Deduccion_Var_Empl_Det = Cls_Ope_Nom_Deducciones_Datos.Consultar_Deducciones_Var_Empleado_Periodo_Nominal(
                    Empleado_ID, Nomina_ID, No_Nomina, Percepcion_Deduccion_ID);

                //Paso IV.- Válidamos la estructura para validar que existan registros con la busqueda realizada.
                if (Dt_Deduccion_Var_Empl_Det is DataTable)
                {
                    foreach (DataRow Deduccion_Variable in Dt_Deduccion_Var_Empl_Det.Rows)
                    {
                        if (Deduccion_Variable is DataRow)
                        {
                            if (!string.IsNullOrEmpty(Deduccion_Variable[Ope_Nom_Deduc_Var_Emp_Det.Campo_Cantidad].ToString()))
                            {
                                Cantidad = Convert.ToDouble(Deduccion_Variable[Ope_Nom_Deduc_Var_Emp_Det.Campo_Cantidad].ToString());
                            }
                        }
                    }//Fin foreach 
                }//Fin validación DataTable 
            }//Fin del try
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad por concepto de deducción variable. Error: " + Ex.Message + "]");
            }
            return Cantidad;
        }
        ///********************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Cantidad_Deduccion_Variable_Aplica
        /// DESCRIPCION : Consulta las Percepciones variables por Empleado_ID y por la Percepcion a aplicar.
        ///               Valida el Estatus de la Deduccion Variable y si tiene un Estatus de Autorizado se obtiene la 
        ///               cantidad que el empleado percibira.
        ///               
        /// PARAMETROS  : Empleado_ID.- Empleado al cuál le aplica la deducción. 
        ///               Percepcion_Deduccion_ID.- Percepción Variable que se le aplicará al empleado.
        ///               
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///********************************************************************************************************************************
        private Double Obtener_Cantidad_Percepcion_Variable_Aplica(String Empleado_ID, String Percepcion_Deduccion_ID)
        {
            DataTable Dt_Percepcion_Var_Empl_Det = null;//Variable que almacenará la deducción consultada.
            Double Cantidad = 0.0;//Variable que almacenará la cantidad a descontar al empleado.

            try
            {
                //Paso III.- ejecutamos la consulta.
                Dt_Percepcion_Var_Empl_Det = Cls_Ope_Nom_Percepciones_Datos.Consultar_Percepciones_Var_Empleado_Periodo_Nominal(
                    Empleado_ID, Nomina_ID, No_Nomina, Percepcion_Deduccion_ID);

                //Paso IV.- Válidamos la estructura para validar que existan registros con la busqueda realizada.
                if (Dt_Percepcion_Var_Empl_Det is DataTable)
                {
                    foreach (DataRow Percepcion_Variable in Dt_Percepcion_Var_Empl_Det.Rows)
                    {
                        if (Percepcion_Variable is DataRow)
                        {
                            if (!string.IsNullOrEmpty(Percepcion_Variable[Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad].ToString()))
                            {
                                Cantidad = Convert.ToDouble(Percepcion_Variable[Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad].ToString());
                            }
                        }
                    }//Fin foreach 
                }//Fin validación DataTable 
            }//Fin del try
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad por concepto de percepción variable. Error: [" + Ex.Message + "]");
            }
            return Cantidad;
        }
        #endregion

        #region (Percepcion y/o Deducciones Calculadas)

        #region (Nomina FINIQUITO)
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Percepciones_Calculadas_Aplica
        /// DESCRIPCIÓN: Obtiene una Lista de Percepciones Calculadas con los datos actualizados [Cantidad, Gravado y Exento].
        /// 
        /// PARÁMETROS: Lista_Percepciones_Calculadas .- Lista de las Percepciones Calculadas.
        ///             Empleado_ID .- Clave única que identifica al empleado.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 13/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Cantidad_Percepciones_Calculadas_Aplica(List<Cls_Percepciones_Deducciones> Lista_Percepciones_Calculadas,
                                                                                                   String Empleado_ID)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            Cls_Ope_Nom_Percepciones_Negocio Calculo_Percepciones = new Cls_Ope_Nom_Percepciones_Negocio();//Variable de conexión con la capa de negocios.
            List<Cls_Percepciones_Deducciones> Lista_Percepciones_Calculadas_Temporal = new List<Cls_Percepciones_Deducciones>();//Variable que almacenará una lista de percepciones calculadas.
            DataTable Dt_Resultados_Calculo_Actual = null;//Estructura que almacenará los resultados de percepción calculada.
            DataTable Dt_Parametro = null;//Variable que almacenará el parámetro de la nómina. 
            DateTime? Fecha_Prima_Vacacional_1 = null;//Variable que almacena la fecha en la que se dará la rpima vacacional 1ra. parte el empleado.
            DateTime? Fecha_Prima_Vacacional_2 = null;//Variable que almacena la fecha en la que se dará la rpima vacacional 2da. parte el empleado.

            //************************** [ PERCEPCIONES ] *************************************
            String PERCEPCION_SUELDO_NORMAL = String.Empty;
            String PERCEPCION_PREVISION_SOCIAL_MULTIPLE = String.Empty;
            String PERCEPCION_PRIMA_DOMINICAL = String.Empty;
            String PERCEPCION_DIAS_FESTIVOS = String.Empty;
            String PERCEPCION_HORAS_EXTRA = String.Empty;
            String PERCEPCION_DIA_DOBLE = String.Empty;
            //String PERCEPCION_DIA_DOMINGO = String.Empty;
            String PERCEPCION_FONDO_RETIRO = String.Empty;
            String PERCEPCION_AJUSTE_ISR = String.Empty;
            String PERCEPCION_INCAPACIDAD = String.Empty;
            String PERCEPCION_QUINQUENIO = String.Empty;
            String PERCEPCION_PRIMA_VACACIONAL = String.Empty;
            String PERCEPCION_AGUINALDO = String.Empty;
            String PERCEPCION_PRIMA_ANTIGUEDAD = String.Empty;
            String PERCEPCION_INDEMNIZACION = String.Empty;
            String PERCEPCION_VACACIONES = String.Empty;
            String PERCEPCION_VACACIONES_PENDIENTES_PAGAR = String.Empty;
            //---------------------------------------------------------------------------------

            //VARIABLES DE NEGOCIO.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            Cls_Cat_Puestos_Negocio INF_PUESTO = null;//Variable que almacenara la información del puesto.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACION DEL PUESTO QUE TIENE ASIGNADO EL EMPLEADO.
                INF_PUESTO = _Informacion_Puestos(INF_EMPLEADO.P_Puesto_ID);

                //Establecemos la fecha de pago del periodo nominal actual.
                Calculo_Percepciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;//Variable que almacena la fecha de fin del periodo.
                Calculo_Percepciones.P_Nomina_ID = Nomina_ID;//Variable que almacena la nomina actual.
                Calculo_Percepciones.P_No_Nomina = No_Nomina;//Variable que almacena el número del periodo actual.
                Calculo_Percepciones.P_Detalle_Nomina_ID = Detalle_Nomina_ID;//Variable que almacena el identificador del periodo actual.
                Calculo_Percepciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;//Variable que almacena el tipo de nómina al que pertence el empleado.

                //Creamos la consulta para consultar los parámetros de la nómina.
                Mi_SQL = "SELECT " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ".*" +
                         " FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;
                //Ejecutamos la consulta del parámetro de la nómina.
                Dt_Parametro = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Parametro is DataTable)
                {
                    if (Dt_Parametro.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString()))
                            PERCEPCION_QUINQUENIO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString()))
                            PERCEPCION_PRIMA_DOMINICAL = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString()))
                            PERCEPCION_DIAS_FESTIVOS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString()))
                            PERCEPCION_HORAS_EXTRA = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString()))
                            PERCEPCION_DIA_DOBLE = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString();
                        //if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString()))
                        //    PERCEPCION_DIA_DOMINGO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString()))
                            PERCEPCION_AJUSTE_ISR = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString()))
                            PERCEPCION_INCAPACIDAD = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString()))
                            PERCEPCION_SUELDO_NORMAL = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString()))
                            PERCEPCION_PRIMA_VACACIONAL = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString()))
                            PERCEPCION_AGUINALDO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString()))
                            PERCEPCION_PRIMA_ANTIGUEDAD = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString()))
                            PERCEPCION_INDEMNIZACION = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString()))
                            PERCEPCION_VACACIONES_PENDIENTES_PAGAR = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString()))
                            PERCEPCION_SUELDO_NORMAL = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString()))
                            PERCEPCION_VACACIONES = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString()))
                            PERCEPCION_FONDO_RETIRO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString()))
                            PERCEPCION_PREVISION_SOCIAL_MULTIPLE = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString();

                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1].ToString()))
                            Fecha_Prima_Vacacional_1 = Convert.ToDateTime(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1].ToString());
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2].ToString()))
                            Fecha_Prima_Vacacional_2 = Convert.ToDateTime(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2].ToString());

                    }//Fin de la validación de que existan algún registro del parámetro.
                }//Fin de la Validación de los parámetros consultados.


                foreach (Cls_Percepciones_Deducciones Percepcion_Calculada in Lista_Percepciones_Calculadas)
                {
                    if (Percepcion_Calculada is Cls_Percepciones_Deducciones)
                    {
                        if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_QUINQUENIO))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Quinquenios(Empleado_ID);//Ejecutamos el calculo correspondiente al Quinquenio.
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_PRIMA_DOMINICAL))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Prima_Dominical(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_DIAS_FESTIVOS))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Pago_Dias_Festivos(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_HORAS_EXTRA))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Tiempo_Extra(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_DIA_DOBLE))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calculo_Dia_Doble(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        //else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_DIA_DOMINGO))
                        //{
                        //    Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calculo_Dia_Doble(Empleado_ID);
                        //    Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        //}
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_AJUSTE_ISR))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Pago_Ajuste_ISR_Finiquito(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_INCAPACIDAD))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Incapacidades(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_SUELDO_NORMAL))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Sueldo_Normal(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);

                            //Obtenemos el total que se pago por concepto de sueldo.
                            Total_Sueldo = Obtener_Cantidad_Resultado(Dt_Resultados_Calculo_Actual);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_PRIMA_VACACIONAL))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Prima_Vacacional_Finiquito(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                            //Obtenemos el total que se pago por concepto de prima vacacional.
                            Total_Prima_Vacacional = Obtener_Cantidad_Resultado(Dt_Resultados_Calculo_Actual);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_AGUINALDO))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Aguinaldo_Finiquito(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);

                            //Obtenemos el total que se pago por concepto de Aguinaldo.
                            Total_Aguinaldo = Obtener_Cantidad_Resultado(Dt_Resultados_Calculo_Actual);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_PRIMA_ANTIGUEDAD))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calculo_Prima_Antiguedad(Empleado_ID, Fecha_Catorcena_Generar_Nomina, Tipo_Salario);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_INDEMNIZACION))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calculo_Indemnizacion(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);

                            //Obtenemos el total que se pago por concepto de indemnización.
                            Total_Indemnizacion = Obtener_Cantidad_Resultado(Dt_Resultados_Calculo_Actual);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_VACACIONES_PENDIENTES_PAGAR))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Vacaciones_Pendientes_Pagar(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_VACACIONES))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Vacaciones(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_FONDO_RETIRO))
                        {
                            if (!String.IsNullOrEmpty(INF_PUESTO.P_Aplica_Fondo_Retiro))
                            {
                                if (INF_PUESTO.P_Aplica_Fondo_Retiro.Trim().ToUpper().Equals("SI"))
                                {
                                    Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Fondo_Retiro(Empleado_ID);
                                    Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                                }
                            }
                        }
                        else if (Percepcion_Calculada.P_Clabe.Equals(PERCEPCION_PREVISION_SOCIAL_MULTIPLE))
                        {
                            Dt_Resultados_Calculo_Actual = Calculo_Percepciones.Calcular_Prevision_Social_Multiple(Empleado_ID);
                            Cargar_Elemento_Lista_Percepciones_Calculadas(ref Lista_Percepciones_Calculadas_Temporal, Dt_Resultados_Calculo_Actual, Percepcion_Calculada);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Obtener Cantidad Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
            return Lista_Percepciones_Calculadas_Temporal;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Cargar_Elemento_Lista
        /// DESCRIPCIÓN: Carga los datos a la lista como como Cantidad, Gravado y Exento de la Percepción Calculada.
        /// 
        /// PARÁMETROS: Lista_Percepciones_Calculadas_Temporal.- Lista sobre la que se cargaran las percepciones calculadas.
        ///             Dt_Resultados_Calculo_Actual.- Resultados del calculo de la percepción.
        ///             Percepcion_Calculada.- Percepcion calculada a actualizar su información.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 14/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private void Cargar_Elemento_Lista_Percepciones_Calculadas(ref List<Cls_Percepciones_Deducciones> Lista_Percepciones_Calculadas_Temporal,
                                           DataTable Dt_Resultados_Calculo_Actual, Cls_Percepciones_Deducciones Percepcion_Calculada)
        {
            Cls_Percepciones_Deducciones Objeto_Percepcion_Calculada = new Cls_Percepciones_Deducciones();//Variable que se agregara a la lista de Percepciones Calculadas.
            Double Cantidad = 0.0;//Variable que almacenará la cantidad que le aplica según la percepción.
            Double Grava = 0.0;//Variable que almacenrá la cantidad que grava el quinquenio.
            Double Exenta = 0.0;//Variable que almacenará la cantidad que exenta el quinquenio.
            DataTable Dt_Periodo_Catorcenal = null;//Almacenará el registro del periodo actual.
            Int32 No_Nomina = 0;//Almacenará el número de nómina actual.

            try
            {
                if (Dt_Resultados_Calculo_Actual is DataTable)
                {
                    if (Dt_Resultados_Calculo_Actual.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(Dt_Resultados_Calculo_Actual.Rows[0]["Calculo"].ToString().Trim()))
                            Cantidad = Convert.ToDouble(Dt_Resultados_Calculo_Actual.Rows[0]["Calculo"].ToString().Trim());
                        if (!String.IsNullOrEmpty(Dt_Resultados_Calculo_Actual.Rows[0]["Grava"].ToString().Trim()))
                            Grava = Convert.ToDouble(Dt_Resultados_Calculo_Actual.Rows[0]["Grava"].ToString().Trim());
                        if (!String.IsNullOrEmpty(Dt_Resultados_Calculo_Actual.Rows[0]["Exenta"].ToString().Trim()))
                            Exenta = Convert.ToDouble(Dt_Resultados_Calculo_Actual.Rows[0]["Exenta"].ToString().Trim());

                        //Establecemos los valores al objeto que almacena la informacion de la percepción.
                        Objeto_Percepcion_Calculada.P_Clabe = Percepcion_Calculada.P_Clabe;
                        Objeto_Percepcion_Calculada.P_Nombre = Percepcion_Calculada.P_Nombre;
                        Objeto_Percepcion_Calculada.P_Monto = Cantidad;
                        Objeto_Percepcion_Calculada.P_Grava = Grava;
                        Objeto_Percepcion_Calculada.P_Exenta = Exenta;
                        Objeto_Percepcion_Calculada.P_Aplica = Percepcion_Calculada.P_Aplica;

                        //Agregamos la percepción a la lista de percepciones.
                        Lista_Percepciones_Calculadas_Temporal.Add(Objeto_Percepcion_Calculada);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Cargar la Lista de Percepciones Calculadas [FINIQUITO]. Error: [" + Ex.Message + "]");
            }
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Calculadas_Aplica
        /// DESCRIPCIÓN: Obtiene una Lista de Deducciones Calculadas con los datos actualizados [Cantidad].
        /// 
        /// PARÁMETROS: Lista_Percepciones_Calculadas .- Lista de las Deducciones Calculadas.
        ///             Empleado_ID .- Clave única que identifica al empleado.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 13/Enero/2011
        /// USUARIO MODIFICO:Juan Alberto Hernández Negrete.
        /// FECHA MODIFICO:23/Abril/2011 14:28 p.m.
        /// CAUSA MODIFICACION: Agregar el concepto de IMSS.
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Cantidad_Deducciones_Calculadas_Aplica(List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas,
                                                                                                   String Empleado_ID)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas_Temporal = new List<Cls_Percepciones_Deducciones>();//Variable que almacenará una lista de percepciones calculadas.
            DataTable Dt_Parametro = null;//Variable que almacenará el parámetro de la nómina. 
            Double Cantidad = 0.0;//Cantidad que se le retendrá al empleado.
            String Tipo_IMSS = String.Empty;

            //********************************* [ DEDUCCIONES ] *****************************************
            String DEDUCCIONES_FALTAS = String.Empty;
            String DEDUCCIONES_RETARDOS = String.Empty;
            String DEDUCCION_FONDO_RETIRO = String.Empty;
            String DEDUCCION_IMSS = String.Empty;
            String DEDUCCION_ISSEG = String.Empty;
            String DEDUCCION_VACACIONES_TOMADAS_MAS = String.Empty;
            String DEDUCCION_AGUINALDO_PAGADO_MAS = String.Empty;
            String DEDUCCION_PRIMA_VACACIONAL_PAGADO_MAS = String.Empty;
            String DEDUCCION_SUELDO_PAGADO_MAS = String.Empty;
            //-------------------------------------------------------------------------------------------

            //VARIABLES DE NEGOCIO.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            Cls_Cat_Puestos_Negocio INF_PUESTO = null;//Variable que almacenara la información del puesto.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACION DEL PUESTO QUE TIENE ASIGNADO EL EMPLEADO.
                INF_PUESTO = _Informacion_Puestos(INF_EMPLEADO.P_Puesto_ID);

                //Establecemos algunos valores para poder realizar los cálculos de las deducciones.
                Calculo_Deducciones.P_Nomina_ID = Nomina_ID;//Nomina actual.
                Calculo_Deducciones.P_No_Nomina = No_Nomina;//Periodo nominal actual.
                Calculo_Deducciones.P_Detalle_Nomina_ID = Detalle_Nomina_ID;//Identificador del periodo actual.
                Calculo_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;//Tipo de nómina al que pertenece el empleado.

                //Se estable las cantidades que que gravaron y exentaron las percepciones calculadas previamente.
                Calculo_Deducciones.Total_Percepciones = Total_Percepciones;//Total de percepciones.
                Calculo_Deducciones.Gravable_Aguinaldo = Total_Grava_Aguinaldo;//Total gravo el aguinaldo.
                Calculo_Deducciones.Gravable_Prima_Vacacional = Total_Grava_Prima_Vacacional;//Total grava la prima vacacional 
                Calculo_Deducciones.Gravable_Prima_Antiguedad = Total_Grava_Prima_Antiguedad;//Total grava la prima de antiguedad.
                Calculo_Deducciones.Gravable_Indemnizacion = Total_Grava_Indemnizacion;//Total grava el cálculo de indemnización.
                Calculo_Deducciones.Exenta_Prima_Antiguedad = Total_Exenta_Prima_Antiguedad;//Total exenta la prima de antiguedad.
                Calculo_Deducciones.Exenta_Indemnizacion = Total_Exenta_Indemnizacion;//Total exenta la indemnización.
                Calculo_Deducciones.Gravable_Dias_Festivos = Total_Grava_Dias_Festivos;//Total gravan los dias festivos.
                Calculo_Deducciones.Gravable_Tiempo_Extra = Total_Grava_Tiempo_Extra;//Total grava el tiempo extra.
                Calculo_Deducciones.Exenta_Tiempo_Extra = Total_Exenta_Tiempo_Extra;//Total exenta el tiempo extra.
                Calculo_Deducciones.Exenta_Dias_Festivos = Total_Exenta_Dias_Festivos;//Total exenta los dias festivos.
                Calculo_Deducciones.Gravable_Sueldo = Total_Grava_Sueldo;

                Calculo_Deducciones.Ingresos_Gravables_Empleado = Total_Ingresos_Gravables_Empleado;//Total de los ingresos gravables del empleado.
                Calculo_Deducciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;//Fecha de fin del periodo nominal actual.

                //Se consulta el párámetro de la nómina.
                Mi_SQL = "SELECT " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ".*" +
                         " FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;
                //Se ejecuta la consulta del parámetro de la nómina.
                Dt_Parametro = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Válidamos la Búsqueda de Parámetros.
                if (Dt_Parametro is DataTable)
                {
                    if (Dt_Parametro.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Tipo_IMSS].ToString().Trim()))
                            Tipo_IMSS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Tipo_IMSS].ToString().Trim();

                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString().Trim()))
                            DEDUCCIONES_FALTAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString().Trim()))
                            DEDUCCIONES_RETARDOS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString().Trim()))
                            DEDUCCION_FONDO_RETIRO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString().Trim()))
                            DEDUCCION_IMSS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString().Trim()))
                            DEDUCCION_VACACIONES_TOMADAS_MAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString().Trim()))
                            DEDUCCION_AGUINALDO_PAGADO_MAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString().Trim()))
                            DEDUCCION_PRIMA_VACACIONAL_PAGADO_MAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString().Trim()))
                            DEDUCCION_SUELDO_PAGADO_MAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString().Trim()))
                            DEDUCCION_ISSEG = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString().Trim();

                    }//Fin de la validación de que existan algún registro del parámetro.
                }//Fin de la Validación de los parámetros consultados.


                foreach (Cls_Percepciones_Deducciones Deduccion_Calculada in Lista_Deducciones_Calculadas)
                {
                    if (Deduccion_Calculada is Cls_Percepciones_Deducciones)
                    {
                        if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCIONES_FALTAS))
                        {
                            Cantidad = Calculo_Deducciones.Calcular_Inasistencias(Empleado_ID);//Ejecutamos el calculo correspondiente al Quinquenio.
                            Deduccion_Calculada.P_Monto = Cantidad;
                            Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                        }
                        else if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCIONES_RETARDOS))
                        {
                            Cantidad = Calculo_Deducciones.Calcular_Retardos(Empleado_ID);
                            Deduccion_Calculada.P_Monto = Cantidad;
                            Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                        }
                        else if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_FONDO_RETIRO))
                        {
                            if (!String.IsNullOrEmpty(INF_PUESTO.P_Aplica_Fondo_Retiro))
                            {
                                if (INF_PUESTO.P_Aplica_Fondo_Retiro.Trim().ToUpper().Equals("SI"))
                                {
                                    Cantidad = Calculo_Deducciones.Retencion_Fondo_Retiro(Empleado_ID);
                                    Deduccion_Calculada.P_Monto = Cantidad;
                                    Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                }
                            }
                        }
                        else if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_IMSS))
                        {
                            if (Tipo_IMSS.ToUpper().Trim().Equals("NUEVO"))
                            {
                                Cantidad = Calculo_Deducciones.Calcular_IMSS(Empleado_ID);
                            }
                            else
                            {
                                Cantidad = Calculo_Deducciones.Calculo_IMSS_Actual(Empleado_ID);
                            }
                            Deduccion_Calculada.P_Monto = Cantidad;
                            Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                        }
                        else if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_VACACIONES_TOMADAS_MAS))
                        {
                            Cantidad = Calculo_Deducciones.Vacaciones_Tomadas_Mas(Empleado_ID);
                            Deduccion_Calculada.P_Monto = Cantidad;
                            Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                        }
                        else if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_AGUINALDO_PAGADO_MAS))
                        {
                            Cantidad = Calculo_Deducciones.Aguinaldo_Pagado_Mas(Empleado_ID);
                            Deduccion_Calculada.P_Monto = Cantidad;
                            Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                        }
                        else if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_PRIMA_VACACIONAL_PAGADO_MAS))
                        {
                            Cantidad = Calculo_Deducciones.Prima_Vacacional_Pagada_Mas(Empleado_ID);
                            Deduccion_Calculada.P_Monto = Cantidad;
                            Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                        }
                        else if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_SUELDO_PAGADO_MAS))
                        {
                            //
                        }
                        else if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_ISSEG))
                        {
                            Cantidad = Calculo_Deducciones.Calcular_ISSEG(Empleado_ID);
                            Deduccion_Calculada.P_Monto = Cantidad;
                            Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Obtener Cantidad Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
            return Lista_Deducciones_Calculadas_Temporal;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Cargar_Elemento_Lista
        /// DESCRIPCIÓN: Carga los datos a la lista como como Cantidad, Gravado y Exento de la Percepción Calculada.
        /// 
        /// PARÁMETROS: Lista_Deducciones_Calculadas_Temporal.- Lista sobre la que se cargaran las deducciones calculadas.
        ///             Dt_Resultados_Calculo_Actual.- Resultados del calculo de la deduccion.
        ///             Deduccion_Calculada.- Deduccion calculada a actualizar su información.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 14/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private void Cargar_Elemento_Lista_Deducciones_Calculadas(ref List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas_Temporal,
                                                                   Cls_Percepciones_Deducciones Deduccion_Calculada)
        {
            Cls_Percepciones_Deducciones Objeto_Deduccion_Calculada = new Cls_Percepciones_Deducciones();//Variable que se agregara a la lista de Percepciones Calculadas.
            DataTable Dt_Periodo_Catorcenal = null;//Almacenará el registro del periodo actual.
            Int32 No_Nomina = 0;//Almacenará el número de nómina actual.
            try
            {
                //Establecemos los valores de la deducción a considerar para el cálculo de la deducción.
                Objeto_Deduccion_Calculada.P_Clabe = Deduccion_Calculada.P_Clabe;
                Objeto_Deduccion_Calculada.P_Nombre = Deduccion_Calculada.P_Nombre;
                Objeto_Deduccion_Calculada.P_Monto = Deduccion_Calculada.P_Monto;
                Objeto_Deduccion_Calculada.P_Aplica = Deduccion_Calculada.P_Aplica;

                //Agregamos la deducción a la lista de las deducciones que se considerara para el finiquito.
                Lista_Deducciones_Calculadas_Temporal.Add(Objeto_Deduccion_Calculada);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Cargar la Lista de Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Prestamos_Deducciones_Calculadas
        /// DESCRIPCIÓN: Carga los datos a la lista como como Cantidad que se lededucira al empleado.
        /// 
        /// PARÁMETROS: Lista_Percepciones_Calculadas_Temporal.- Lista sobre la que se cargaran las deducciones calculadas.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 14/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Prestamos_Deducciones_Calculadas(List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas, String Empleado_ID)
        {
            List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas_Temporal = new List<Cls_Percepciones_Deducciones>();//Variable que almacenará una lista de percepciones calculadas.
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Prestamos = null;//Variable que almacenara la lista de prestamos que tiene el empleado actualmente.

            try
            {
                //Establecemos la fecha del fin del periodo nominal actual.
                Calculo_Deducciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;
                //Consultamos si el empleado tiene algun prestamo actualmente.
                Dt_Prestamos = Calculo_Deducciones.Pago_Prestamo_Finiquito(Empleado_ID);

                //Validamos la búsqueda de prestamos.
                if (Dt_Prestamos is DataTable)
                {
                    if (Dt_Prestamos.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon in Dt_Prestamos.Rows)
                        {
                            if (Renglon is DataRow)
                            {
                                Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Renglon);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener los prestamos del empleado. Error: [" + Ex.Message + "]");
            }
            return Lista_Deducciones_Calculadas_Temporal;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Cargar_Elemento_Lista
        /// DESCRIPCIÓN: Carga los datos a la lista como como Cantidad, Gravado y Exento de la Percepción Calculada.
        /// 
        /// PARÁMETROS: Lista_Percepciones_Calculadas_Temporal.- Lista sobre la que se cargaran las percepciones calculadas.
        ///             Dr_PRESTAMO.- Registro del prestamo.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 14/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private void Cargar_Elemento_Lista_Deducciones_Calculadas(ref List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas_Temporal,
            DataRow Dr_PRESTAMO)
        {
            Cls_Percepciones_Deducciones Objeto_Deduccion_Calculada = new Cls_Percepciones_Deducciones();//Variable que se agregara a la lista de Percepciones Calculadas.
            Cls_Cat_Nom_Percepciones_Deducciones_Business Cls_Percepciones_Deducciones_Consulta = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexión con la capa de negocios.
            DataTable Dt_Percepcion_Deduccion = null;//Almacenará el registro del periodo actual.

            try
            {
                if (Dr_PRESTAMO is DataRow)
                {
                    Dt_Percepcion_Deduccion = Cls_Percepciones_Deducciones_Consulta.Busqueda_Percepcion_Deduccion_Por_ID(Dr_PRESTAMO["Deduccion"].ToString().Trim());

                    if (Dt_Percepcion_Deduccion is DataTable)
                    {
                        if (Dt_Percepcion_Deduccion.Rows.Count > 0)
                        {
                            foreach (DataRow DEDUCCION in Dt_Percepcion_Deduccion.Rows)
                            {
                                if (DEDUCCION is DataRow)
                                {
                                    if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                        Objeto_Deduccion_Calculada.P_Clabe = DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                                    if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim()))
                                        Objeto_Deduccion_Calculada.P_Nombre = DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();

                                    if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString().Trim()))
                                        Objeto_Deduccion_Calculada.P_Aplica = DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString().Trim();

                                    if (!String.IsNullOrEmpty(Dr_PRESTAMO["Cantidad"].ToString().Trim()))
                                        Objeto_Deduccion_Calculada.P_Monto = Convert.ToDouble(Dr_PRESTAMO["Cantidad"].ToString().Trim());
                                    else Objeto_Deduccion_Calculada.P_Monto = 0.00;

                                    Lista_Deducciones_Calculadas_Temporal.Add(Objeto_Deduccion_Calculada);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Cargar la Lista de Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Tercero
        /// DESCRIPCIÓN: Realiza el calculo de la Retención de Terceros. Este calculo se realiza por separado ya que es necesario
        ///              conocer la cantidad total de Percepciones y  la cantidad total de Deducciones. Que tiene el empleado.
        /// 
        /// PARÁMETROS: Lista_Deducciones_Calculadas .- Lista de Percepciones Deducciones la cual se recorrera y se validara que 
        ///                                             corresponda a las deduccion de retencion de terceros..
        ///             Empleado_ID .- Identificador único que identifica al empleado en las operacione sinternas del sistema.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 19/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Cantidad_Deducciones_Retencion_Tercero(List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas,
                                                                                                  String Empleado_ID)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas_Temporal = new List<Cls_Percepciones_Deducciones>();//Variable que almacenará una lista de percepciones calculadas.
            DataTable Dt_Retencion_Terceros = null;//Variable que almacenará el registro de retencion a terceros.

            //************************************ [ DEDUCCIÓN ] ***************************************
            String DEDUCCION_TERCERO = "";//Deduccion que corresponde a la Deducción Retención Terceros.
            Double Cantidad = 0.0;//Cantidad que se le retendra al empleado.

            try
            {
                //Establecemos los valores de las variables que se utilizaran en el cálculo de la deducción.
                Calculo_Deducciones.Total_Percepciones = Total_Percepciones;//Total de percepciones del empleado.
                Calculo_Deducciones.Total_Deducciones = Total_Deducciones;//Total de deducciones que se le retendrá al empleado.
                Calculo_Deducciones.Ingresos_Gravables_Empleado = Total_Ingresos_Gravables_Empleado;//Total de ingresos gravables del empleado.
                Calculo_Deducciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;//Fecha de fin del periodo nominal actual.

                //Cosnulta de el tipo de retencion que tiene el empleado, de acuerdo al partido al que pertenece-.
                Mi_SQL = "SELECT " + Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID +
                         " FROM " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros +
                         " WHERE " + Cat_Nom_Terceros.Campo_Tercero_ID +
                         " IN " +
                         " (SELECT " + Cat_Empleados.Campo_Terceros_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "')";

                //Ejecutamos la consulta.
                Dt_Retencion_Terceros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Válidamos la Búsqueda de Retención a Terceros.
                if (Dt_Retencion_Terceros is DataTable)
                    if (Dt_Retencion_Terceros.Rows.Count > 0)
                        foreach (DataRow RETENCION_TERCERO in Dt_Retencion_Terceros.Rows)
                            if (!String.IsNullOrEmpty(RETENCION_TERCERO[Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                DEDUCCION_TERCERO = RETENCION_TERCERO[Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                foreach (Cls_Percepciones_Deducciones Deduccion_Calculada in Lista_Deducciones_Calculadas)
                {
                    if (Deduccion_Calculada is Cls_Percepciones_Deducciones)
                    {
                        if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_TERCERO))
                        {
                            Cantidad = Calculo_Deducciones.Calcular_Retenciones_Terceros(Empleado_ID);
                            Deduccion_Calculada.P_Monto = Cantidad;
                            Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Obtener Cantidad Percepciones Calculadas. Error: [" + Ex.Message + "]");
            }
            return Lista_Deducciones_Calculadas_Temporal;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial
        /// DESCRIPCIÓN: Obtiene el monto por concepto de Orden Judicial, si es que la misma aplicá para el empleado.
        /// 
        /// PARÁMETROS: Lista_Deducciones_Calculadas .- Lista de Percepciones Deducciones la cual se recorrera y se validara que 
        ///                                             corresponda a las deduccion de retencion de terceros..
        ///             Empleado_ID .- Identificador único que identifica al empleado en las operacione sinternas del sistema.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 2/Febrero/2011
        /// USUARIO MODIFICO:Juan Alberto Hernández Negrete.
        /// FECHA MODIFICO:23/Abril/2011
        /// CAUSA MODIFICACION: Ajustar Calculo de Orden Judicialcon los cambios que hubo en el calculo al momento de revizar el diseño
        ///                     con el personal de recursos humanos.
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Indemnizacion(List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas,
                                                                                          String Empleado_ID)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Cat_Empleados_Negocios Empleados_Negocio = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capá de negocios.
            List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas_Temporal = new List<Cls_Percepciones_Deducciones>();//Variable que almacenará una lista de percepciones calculadas.
            DataTable Dt_Retencion_Orden_Judicial = null;//Variable que almacenará el registro de retencion de Orden Judicial.
            DataTable Dt_Empleados = null;//Variable que almacenar'a una lista de los empleados.
            String DEDUCCION_ORDEN_JUDICAL = "";//Deduccion que corresponde a la Deducción Retención por Orden Judicial.
            Double Cantidad_Retencion_Orden_Judicial = 0.0;//Cantidad que se le retendra al empleado.
            Double Cant_Porc_Retener_Indemnizacion = 0.0;//Cantidad o Porcentaje que se descontara al empleado por concepto de retención de orden judicial.
            String Tipo_Desc_Retencion_OJ_Indemnizacion = "";//Parámetro que nos indica si la retención será por un monto fijo, o por un porcentaje de retención.
            String OJ_Bruto_Neto_Indemnizacion = String.Empty;//Variable que almacena el valor si la retención se le hará al empleado sobre el NETO O EL BRUTO de su sueldo.
            Int32 Contador_Beneficiarios = 1;
            Double Cantidad_ISR_Indemnizacion = 0.0;

            try
            {
                Calculo_Deducciones.P_Nomina_ID = Nomina_ID;
                Calculo_Deducciones.P_No_Nomina = No_Nomina;
                Calculo_Deducciones.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Calculo_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;

                //Se estable las cantidades que que gravaron y exentaron las percepciones calculadas previamente.
                Calculo_Deducciones.Total_Percepciones = Total_Percepciones;//Total de percepciones.
                Calculo_Deducciones.Gravable_Aguinaldo = Total_Grava_Aguinaldo;//Total gravo el aguinaldo.
                Calculo_Deducciones.Gravable_Prima_Vacacional = Total_Grava_Prima_Vacacional;//Total grava la prima vacacional 
                Calculo_Deducciones.Gravable_Prima_Antiguedad = Total_Grava_Prima_Antiguedad;//Total grava la prima de antiguedad.
                Calculo_Deducciones.Gravable_Indemnizacion = Total_Grava_Indemnizacion;//Total grava el cálculo de indemnización.
                Calculo_Deducciones.Exenta_Prima_Antiguedad = Total_Exenta_Prima_Antiguedad;//Total exenta la prima de antiguedad.
                Calculo_Deducciones.Exenta_Indemnizacion = Total_Exenta_Indemnizacion;//Total exenta la indemnización.
                Calculo_Deducciones.Gravable_Dias_Festivos = Total_Grava_Dias_Festivos;//Total gravan los dias festivos.
                Calculo_Deducciones.Gravable_Tiempo_Extra = Total_Grava_Tiempo_Extra;//Total grava el tiempo extra.
                Calculo_Deducciones.Exenta_Tiempo_Extra = Total_Exenta_Tiempo_Extra;//Total exenta el tiempo extra.
                Calculo_Deducciones.Exenta_Dias_Festivos = Total_Exenta_Dias_Festivos;//Total exenta los dias festivos.
                Calculo_Deducciones.Gravable_Sueldo = Total_Grava_Sueldo;

                Calculo_Deducciones.Ingresos_Gravables_Empleado = Total_Ingresos_Gravables_Empleado;//Total de los ingresos gravables del empleado.
                Calculo_Deducciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;//Fecha de fin del periodo nominal actual.

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

                                //Consultamos la tabla de parámetros para obtener el identicador de la Orden judicial del concepto a retener al empleado.
                                Mi_SQL = "SELECT " +
                                                     Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion +
                                         " FROM " +
                                                     Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;
                                //Ejecutamos la consulta, para obtener el identificador de la deducción por concepto de Orden Judicial.
                                Dt_Retencion_Orden_Judicial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                                //Validamos que la consulta ó  búsqueda realizada tenga algún resultado.
                                if (Dt_Retencion_Orden_Judicial != null)
                                {
                                    if (Dt_Retencion_Orden_Judicial.Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(Dt_Retencion_Orden_Judicial.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion].ToString()))
                                        {
                                            //Obtenemos el identificador por concepto de Orden Judicial.
                                            DEDUCCION_ORDEN_JUDICAL = Dt_Retencion_Orden_Judicial.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion].ToString();

                                            //Recorremos la lista de Deducciones, para comparar y si existe alguna coincidencia con el parámetro consultado
                                            //se realiza el cálculo.
                                            foreach (Cls_Percepciones_Deducciones Deduccion_Calculada in Lista_Deducciones_Calculadas)
                                            {
                                                //Validamos que el objeto actual no sea null.
                                                if (Deduccion_Calculada != null)
                                                {
                                                    //Comparamos los identificadores, con el parámetro consultador obtener si el empleado
                                                    //tiene asignada dicha deducción.
                                                    if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_ORDEN_JUDICAL))
                                                    {
                                                        //Si aplica, validamos si dicha retención se hará por un monto fijo ó por un porcentaje
                                                        //sobre el total de sus percepciones.
                                                        if (Tipo_Desc_Retencion_OJ_Indemnizacion.Equals("CANTIDAD"))
                                                        {
                                                            //Obtenemos el monto a descontar al empleado por concepto de orden judicial.
                                                            Deduccion_Calculada.P_Monto += Cant_Porc_Retener_Indemnizacion;
                                                            //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                            //retención de orden judicial.
                                                            if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                            {
                                                                Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                Total_OJ_Indemnizacion = Deduccion_Calculada.P_Monto;
                                                            }
                                                        }
                                                        else if (Tipo_Desc_Retencion_OJ_Indemnizacion.Equals("PORCENTAJE"))
                                                        {
                                                            if (OJ_Bruto_Neto_Indemnizacion.ToString().Trim().ToUpper().Equals("BRUTO"))
                                                            {
                                                                Double Bruto = Total_Indemnizacion;
                                                                Cant_Porc_Retener_Indemnizacion = Cant_Porc_Retener_Indemnizacion / 100;
                                                                Cantidad_Retencion_Orden_Judicial = (Bruto * Cant_Porc_Retener_Indemnizacion);
                                                                //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                                //retención de orden judicial.
                                                                Deduccion_Calculada.P_Monto += Cantidad_Retencion_Orden_Judicial;

                                                                if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                                {
                                                                    Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                    Total_OJ_Indemnizacion = Deduccion_Calculada.P_Monto;
                                                                }
                                                            }
                                                            else if (OJ_Bruto_Neto_Indemnizacion.ToString().Trim().ToUpper().Equals("NETO"))
                                                            {
                                                                Cantidad_ISR_Indemnizacion = Obtener_Cantidad_ISR_Particular(Calculo_Deducciones.Calcular_ISPT_Percepciones_Por_Retiro(Empleado_ID));
                                                                Cantidad_ISR_Indemnizacion = (Total_Grava_Indemnizacion * Cantidad_ISR_Indemnizacion) / (Total_Grava_Indemnizacion + Total_Grava_Prima_Antiguedad);

                                                                Double Neto = Total_Indemnizacion - Cantidad_ISR_Indemnizacion;
                                                                if (Neto < 0) Neto = 0;
                                                                Cant_Porc_Retener_Indemnizacion = Cant_Porc_Retener_Indemnizacion / 100;
                                                                Cantidad_Retencion_Orden_Judicial = (Neto * Cant_Porc_Retener_Indemnizacion);
                                                                //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                                //retención de orden judicial.
                                                                Deduccion_Calculada.P_Monto += Cantidad_Retencion_Orden_Judicial;
                                                                if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                                {
                                                                    Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                    Total_OJ_Indemnizacion = Deduccion_Calculada.P_Monto;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
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
            return Lista_Deducciones_Calculadas_Temporal;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial
        /// DESCRIPCIÓN: Obtiene el monto por concepto de Orden Judicial, si es que la misma aplicá para el empleado.
        /// 
        /// PARÁMETROS: Lista_Deducciones_Calculadas .- Lista de Percepciones Deducciones la cual se recorrera y se validara que 
        ///                                             corresponda a las deduccion de retencion de terceros..
        ///             Empleado_ID .- Identificador único que identifica al empleado en las operacione sinternas del sistema.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 2/Febrero/2011
        /// USUARIO MODIFICO:Juan Alberto Hernández Negrete.
        /// FECHA MODIFICO:23/Abril/2011
        /// CAUSA MODIFICACION: Ajustar Calculo de Orden Judicialcon los cambios que hubo en el calculo al momento de revizar el diseño
        ///                     con el personal de recursos humanos.
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Aguinaldo(List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas,
                                                                                          String Empleado_ID)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Cat_Empleados_Negocios Empleados_Negocio = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capá de negocios.
            List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas_Temporal = new List<Cls_Percepciones_Deducciones>();//Variable que almacenará una lista de percepciones calculadas.
            DataTable Dt_Retencion_Orden_Judicial = null;//Variable que almacenará el registro de retencion de Orden Judicial.
            DataTable Dt_Empleados = null;//Variable que almacenar'a una lista de los empleados.
            String DEDUCCION_ORDEN_JUDICAL = "";//Deduccion que corresponde a la Deducción Retención por Orden Judicial.
            Double Cantidad_Retencion_Orden_Judicial = 0.0;//Cantidad que se le retendra al empleado.
            Double Cant_Porc_Retener_Aguinaldo = 0.0;//Cantidad o Porcentaje que se descontara al empleado por concepto de retención de orden judicial.
            String Tipo_Desc_Retencion_OJ_aguinaldo = "";//Parámetro que nos indica si la retención será por un monto fijo, o por un porcentaje de retención.
            String OJ_Bruto_Neto_Aguinaldo = String.Empty;//Variable que almacena el valor si la retención se le hará al empleado sobre el NETO O EL BRUTO de su sueldo.
            Int32 Contador_Beneficiarios = 1;
            Double Cantidad_ISR_Aguinaldo = 0.0;

            try
            {
                Calculo_Deducciones.P_Nomina_ID = Nomina_ID;
                Calculo_Deducciones.P_No_Nomina = No_Nomina;
                Calculo_Deducciones.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Calculo_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;

                //Se estable las cantidades que que gravaron y exentaron las percepciones calculadas previamente.
                Calculo_Deducciones.Total_Percepciones = Total_Percepciones;//Total de percepciones.
                Calculo_Deducciones.Gravable_Aguinaldo = Total_Grava_Aguinaldo;//Total gravo el aguinaldo.
                Calculo_Deducciones.Gravable_Prima_Vacacional = Total_Grava_Prima_Vacacional;//Total grava la prima vacacional 
                Calculo_Deducciones.Gravable_Prima_Antiguedad = Total_Grava_Prima_Antiguedad;//Total grava la prima de antiguedad.
                Calculo_Deducciones.Gravable_Indemnizacion = Total_Grava_Indemnizacion;//Total grava el cálculo de indemnización.
                Calculo_Deducciones.Exenta_Prima_Antiguedad = Total_Exenta_Prima_Antiguedad;//Total exenta la prima de antiguedad.
                Calculo_Deducciones.Exenta_Indemnizacion = Total_Exenta_Indemnizacion;//Total exenta la indemnización.
                Calculo_Deducciones.Gravable_Dias_Festivos = Total_Grava_Dias_Festivos;//Total gravan los dias festivos.
                Calculo_Deducciones.Gravable_Tiempo_Extra = Total_Grava_Tiempo_Extra;//Total grava el tiempo extra.
                Calculo_Deducciones.Exenta_Tiempo_Extra = Total_Exenta_Tiempo_Extra;//Total exenta el tiempo extra.
                Calculo_Deducciones.Exenta_Dias_Festivos = Total_Exenta_Dias_Festivos;//Total exenta los dias festivos.
                Calculo_Deducciones.Gravable_Sueldo = Total_Grava_Sueldo;

                Calculo_Deducciones.Ingresos_Gravables_Empleado = Total_Ingresos_Gravables_Empleado;//Total de los ingresos gravables del empleado.
                Calculo_Deducciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;//Fecha de fin del periodo nominal actual.

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
                                    Tipo_Desc_Retencion_OJ_aguinaldo = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Aguinaldo].ToString().Trim();

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Aguinaldo].ToString()))
                                    OJ_Bruto_Neto_Aguinaldo = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Aguinaldo].ToString();

                                //Consultamos la tabla de parámetros para obtener el identicador de la Orden judicial del concepto a retener al empleado.
                                Mi_SQL = "SELECT " +
                                                     Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo +
                                         " FROM " +
                                                     Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;
                                //Ejecutamos la consulta, para obtener el identificador de la deducción por concepto de Orden Judicial.
                                Dt_Retencion_Orden_Judicial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                                //Validamos que la consulta ó  búsqueda realizada tenga algún resultado.
                                if (Dt_Retencion_Orden_Judicial != null)
                                {
                                    if (Dt_Retencion_Orden_Judicial.Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(Dt_Retencion_Orden_Judicial.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo].ToString()))
                                        {
                                            //Obtenemos el identificador por concepto de Orden Judicial.
                                            DEDUCCION_ORDEN_JUDICAL = Dt_Retencion_Orden_Judicial.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo].ToString();

                                            //Recorremos la lista de Deducciones, para comparar y si existe alguna coincidencia con el parámetro consultado
                                            //se realiza el cálculo.
                                            foreach (Cls_Percepciones_Deducciones Deduccion_Calculada in Lista_Deducciones_Calculadas)
                                            {
                                                //Validamos que el objeto actual no sea null.
                                                if (Deduccion_Calculada != null)
                                                {
                                                    //Comparamos los identificadores, con el parámetro consultador obtener si el empleado
                                                    //tiene asignada dicha deducción.
                                                    if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_ORDEN_JUDICAL))
                                                    {
                                                        //Si aplica, validamos si dicha retención se hará por un monto fijo ó por un porcentaje
                                                        //sobre el total de sus percepciones.
                                                        if (Tipo_Desc_Retencion_OJ_aguinaldo.Equals("CANTIDAD"))
                                                        {
                                                            //Obtenemos el monto a descontar al empleado por concepto de orden judicial.
                                                            Deduccion_Calculada.P_Monto += Cant_Porc_Retener_Aguinaldo;
                                                            //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                            //retención de orden judicial.
                                                            if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                            {
                                                                Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                Total_OJ_Aguinaldo = Deduccion_Calculada.P_Monto;
                                                            }
                                                        }
                                                        else if (Tipo_Desc_Retencion_OJ_aguinaldo.Equals("PORCENTAJE"))
                                                        {
                                                            if (OJ_Bruto_Neto_Aguinaldo.ToString().Trim().ToUpper().Equals("BRUTO"))
                                                            {
                                                                Double Bruto = Total_Aguinaldo;
                                                                Cant_Porc_Retener_Aguinaldo = Cant_Porc_Retener_Aguinaldo / 100;
                                                                Cantidad_Retencion_Orden_Judicial = (Bruto * Cant_Porc_Retener_Aguinaldo);
                                                                //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                                //retención de orden judicial.
                                                                Deduccion_Calculada.P_Monto += Cantidad_Retencion_Orden_Judicial;

                                                                if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                                {
                                                                    Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                    Total_OJ_Aguinaldo = Deduccion_Calculada.P_Monto;
                                                                }
                                                            }
                                                            else if (OJ_Bruto_Neto_Aguinaldo.ToString().Trim().ToUpper().Equals("NETO"))
                                                            {
                                                                Cantidad_ISR_Aguinaldo = Obtener_Cantidad_ISR_Particular(Calculo_Deducciones.Calcular_ISPT_Prima_Vacacional_Aguinaldo(Empleado_ID));
                                                                Cantidad_ISR_Aguinaldo = (Total_Grava_Aguinaldo * Cantidad_ISR_Aguinaldo) / (Total_Grava_Aguinaldo + Total_Grava_Prima_Vacacional);

                                                                Double Neto = Total_Aguinaldo - Cantidad_ISR_Aguinaldo;
                                                                if (Neto < 0) Neto = 0;
                                                                Cant_Porc_Retener_Aguinaldo = Cant_Porc_Retener_Aguinaldo / 100;
                                                                Cantidad_Retencion_Orden_Judicial = (Neto * Cant_Porc_Retener_Aguinaldo);
                                                                //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                                //retención de orden judicial.
                                                                Deduccion_Calculada.P_Monto += Cantidad_Retencion_Orden_Judicial;
                                                                if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                                {
                                                                    Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                    Total_OJ_Aguinaldo = Deduccion_Calculada.P_Monto;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
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
            return Lista_Deducciones_Calculadas_Temporal;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial
        /// DESCRIPCIÓN: Obtiene el monto por concepto de Orden Judicial, si es que la misma aplicá para el empleado.
        /// 
        /// PARÁMETROS: Lista_Deducciones_Calculadas .- Lista de Percepciones Deducciones la cual se recorrera y se validara que 
        ///                                             corresponda a las deduccion de retencion de terceros..
        ///             Empleado_ID .- Identificador único que identifica al empleado en las operacione sinternas del sistema.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 2/Febrero/2011
        /// USUARIO MODIFICO:Juan Alberto Hernández Negrete.
        /// FECHA MODIFICO:23/Abril/2011
        /// CAUSA MODIFICACION: Ajustar Calculo de Orden Judicialcon los cambios que hubo en el calculo al momento de revizar el diseño
        ///                     con el personal de recursos humanos.
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial_Prima_Vacacional(List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas,
                                                                                          String Empleado_ID)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Cat_Empleados_Negocios Empleados_Negocio = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capá de negocios.
            List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas_Temporal = new List<Cls_Percepciones_Deducciones>();//Variable que almacenará una lista de percepciones calculadas.
            DataTable Dt_Retencion_Orden_Judicial = null;//Variable que almacenará el registro de retencion de Orden Judicial.
            DataTable Dt_Empleados = null;//Variable que almacenar'a una lista de los empleados.
            String DEDUCCION_ORDEN_JUDICAL = "";//Deduccion que corresponde a la Deducción Retención por Orden Judicial.
            Double Cantidad_Retencion_Orden_Judicial = 0.0;//Cantidad que se le retendra al empleado.
            Double Cant_Porc_Retener_Prima_Vacacional = 0.0;//Cantidad o Porcentaje que se descontara al empleado por concepto de retención de orden judicial.
            String Tipo_Desc_Retencion_OJ_Prima_Vacacional = "";//Parámetro que nos indica si la retención será por un monto fijo, o por un porcentaje de retención.
            String OJ_Bruto_Neto_Prima_Vacacional = String.Empty;//Variable que almacena el valor si la retención se le hará al empleado sobre el NETO O EL BRUTO de su sueldo.
            Int32 Contador_Beneficiarios = 1;
            Double Cantidad_ISR_Prima_Vacacional = 0.0;

            try
            {
                Calculo_Deducciones.P_Nomina_ID = Nomina_ID;
                Calculo_Deducciones.P_No_Nomina = No_Nomina;
                Calculo_Deducciones.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Calculo_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;

                //Se estable las cantidades que que gravaron y exentaron las percepciones calculadas previamente.
                Calculo_Deducciones.Total_Percepciones = Total_Percepciones;//Total de percepciones.
                Calculo_Deducciones.Gravable_Aguinaldo = Total_Grava_Aguinaldo;//Total gravo el aguinaldo.
                Calculo_Deducciones.Gravable_Prima_Vacacional = Total_Grava_Prima_Vacacional;//Total grava la prima vacacional 
                Calculo_Deducciones.Gravable_Prima_Antiguedad = Total_Grava_Prima_Antiguedad;//Total grava la prima de antiguedad.
                Calculo_Deducciones.Gravable_Indemnizacion = Total_Grava_Indemnizacion;//Total grava el cálculo de indemnización.
                Calculo_Deducciones.Exenta_Prima_Antiguedad = Total_Exenta_Prima_Antiguedad;//Total exenta la prima de antiguedad.
                Calculo_Deducciones.Exenta_Indemnizacion = Total_Exenta_Indemnizacion;//Total exenta la indemnización.
                Calculo_Deducciones.Gravable_Dias_Festivos = Total_Grava_Dias_Festivos;//Total gravan los dias festivos.
                Calculo_Deducciones.Gravable_Tiempo_Extra = Total_Grava_Tiempo_Extra;//Total grava el tiempo extra.
                Calculo_Deducciones.Exenta_Tiempo_Extra = Total_Exenta_Tiempo_Extra;//Total exenta el tiempo extra.
                Calculo_Deducciones.Exenta_Dias_Festivos = Total_Exenta_Dias_Festivos;//Total exenta los dias festivos.
                Calculo_Deducciones.Gravable_Sueldo = Total_Grava_Sueldo;

                Calculo_Deducciones.Ingresos_Gravables_Empleado = Total_Ingresos_Gravables_Empleado;//Total de los ingresos gravables del empleado.
                Calculo_Deducciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;//Fecha de fin del periodo nominal actual.

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

                                //Consultamos la tabla de parámetros para obtener el identicador de la Orden judicial del concepto a retener al empleado.
                                Mi_SQL = "SELECT " +
                                                     Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional +
                                         " FROM " +
                                                     Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;
                                //Ejecutamos la consulta, para obtener el identificador de la deducción por concepto de Orden Judicial.
                                Dt_Retencion_Orden_Judicial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                                //Validamos que la consulta ó  búsqueda realizada tenga algún resultado.
                                if (Dt_Retencion_Orden_Judicial != null)
                                {
                                    if (Dt_Retencion_Orden_Judicial.Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(Dt_Retencion_Orden_Judicial.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional].ToString()))
                                        {
                                            //Obtenemos el identificador por concepto de Orden Judicial.
                                            DEDUCCION_ORDEN_JUDICAL = Dt_Retencion_Orden_Judicial.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional].ToString();

                                            //Recorremos la lista de Deducciones, para comparar y si existe alguna coincidencia con el parámetro consultado
                                            //se realiza el cálculo.
                                            foreach (Cls_Percepciones_Deducciones Deduccion_Calculada in Lista_Deducciones_Calculadas)
                                            {
                                                //Validamos que el objeto actual no sea null.
                                                if (Deduccion_Calculada != null)
                                                {
                                                    //Comparamos los identificadores, con el parámetro consultador obtener si el empleado
                                                    //tiene asignada dicha deducción.
                                                    if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_ORDEN_JUDICAL))
                                                    {
                                                        //Si aplica, validamos si dicha retención se hará por un monto fijo ó por un porcentaje
                                                        //sobre el total de sus percepciones.
                                                        if (Tipo_Desc_Retencion_OJ_Prima_Vacacional.Equals("CANTIDAD"))
                                                        {
                                                            //Obtenemos el monto a descontar al empleado por concepto de orden judicial.
                                                            Deduccion_Calculada.P_Monto += Cant_Porc_Retener_Prima_Vacacional;
                                                            //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                            //retención de orden judicial.
                                                            if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                            {
                                                                Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                Total_OJ_PV = Deduccion_Calculada.P_Monto;
                                                            }
                                                        }
                                                        else if (Tipo_Desc_Retencion_OJ_Prima_Vacacional.Equals("PORCENTAJE"))
                                                        {
                                                            if (OJ_Bruto_Neto_Prima_Vacacional.ToString().Trim().ToUpper().Equals("BRUTO"))
                                                            {
                                                                Double Bruto = Total_Prima_Vacacional;
                                                                Cant_Porc_Retener_Prima_Vacacional = Cant_Porc_Retener_Prima_Vacacional / 100;
                                                                Cantidad_Retencion_Orden_Judicial = (Bruto * Cant_Porc_Retener_Prima_Vacacional);
                                                                //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                                //retención de orden judicial.
                                                                Deduccion_Calculada.P_Monto += Cantidad_Retencion_Orden_Judicial;

                                                                if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                                {
                                                                    Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                    Total_OJ_PV = Deduccion_Calculada.P_Monto;
                                                                }
                                                            }
                                                            else if (OJ_Bruto_Neto_Prima_Vacacional.ToString().Trim().ToUpper().Equals("NETO"))
                                                            {
                                                                Cantidad_ISR_Prima_Vacacional = Obtener_Cantidad_ISR_Particular(Calculo_Deducciones.Calcular_ISPT_Prima_Vacacional(Empleado_ID));

                                                                Double Neto = Total_Prima_Vacacional - Cantidad_ISR_Prima_Vacacional;
                                                                if (Neto < 0) Neto = 0;
                                                                Cant_Porc_Retener_Prima_Vacacional = Cant_Porc_Retener_Prima_Vacacional / 100;
                                                                Cantidad_Retencion_Orden_Judicial = (Neto * Cant_Porc_Retener_Prima_Vacacional);
                                                                //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                                //retención de orden judicial.
                                                                Deduccion_Calculada.P_Monto += Cantidad_Retencion_Orden_Judicial;
                                                                if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                                {
                                                                    Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                    Total_OJ_PV = Deduccion_Calculada.P_Monto;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
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
            return Lista_Deducciones_Calculadas_Temporal;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial
        /// DESCRIPCIÓN: Obtiene el monto por concepto de Orden Judicial, si es que la misma aplicá para el empleado.
        /// 
        /// PARÁMETROS: Lista_Deducciones_Calculadas .- Lista de Percepciones Deducciones la cual se recorrera y se validara que 
        ///                                             corresponda a las deduccion de retencion de terceros..
        ///             Empleado_ID .- Identificador único que identifica al empleado en las operacione sinternas del sistema.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 2/Febrero/2011
        /// USUARIO MODIFICO:Juan Alberto Hernández Negrete.
        /// FECHA MODIFICO:23/Abril/2011
        /// CAUSA MODIFICACION: Ajustar Calculo de Orden Judicialcon los cambios que hubo en el calculo al momento de revizar el diseño
        ///                     con el personal de recursos humanos.
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Cantidad_Deducciones_Retencion_Orden_Judicial(List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas,
                                                                                          String Empleado_ID)
        {
            String Mi_SQL = "";//Variable que almacenrá las consultas.
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Cat_Empleados_Negocios Empleados_Negocio = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capá de negocios.
            List<Cls_Percepciones_Deducciones> Lista_Deducciones_Calculadas_Temporal = new List<Cls_Percepciones_Deducciones>();//Variable que almacenará una lista de percepciones calculadas.
            DataTable Dt_Retencion_Orden_Judicial = null;//Variable que almacenará el registro de retencion de Orden Judicial.
            DataTable Dt_Empleados = null;//Variable que almacenar'a una lista de los empleados.
            String DEDUCCION_ORDEN_JUDICAL = "";//Deduccion que corresponde a la Deducción Retención por Orden Judicial.
            Double Cantidad_Retencion_Orden_Judicial = 0.0;//Cantidad que se le retendra al empleado.
            Double Cant_Porc_Retener_Sueldo_Normal = 0.0;//Cantidad o Porcentaje que se descontara al empleado por concepto de retención de orden judicial.
            String Tipo_Desc_Retencion_OJ_Sueldo_Normal = "";//Parámetro que nos indica si la retención será por un monto fijo, o por un porcentaje de retención.
            String OJ_Bruto_Neto_Sueldo_Normal = String.Empty;//Variable que almacena el valor si la retención se le hará al empleado sobre el NETO O EL BRUTO de su sueldo.
            Double Total_Percepciones_Empleado = 0.0;//Total de percepciones del empleado, en su nómina actual.
            Double Total_Deducciones_Empleado = 0.0;//Total de deducciones del empleado, en su nómina actual.
            Int32 Contador_Beneficiarios = 1;

            try
            {
                Calculo_Deducciones.P_Nomina_ID = Nomina_ID;
                Calculo_Deducciones.P_No_Nomina = No_Nomina;
                Calculo_Deducciones.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Calculo_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;

                //Obtenemos los totales de percepciones y/o  deducciones así como los ingresos gravables y exentos del empleado.
                Calculo_Deducciones.Total_Percepciones = Total_Percepciones;
                Calculo_Deducciones.Total_Deducciones = Total_Deducciones;
                Calculo_Deducciones.Ingresos_Gravables_Empleado = Total_Ingresos_Gravables_Empleado;
                Calculo_Deducciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;

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
                                    Cant_Porc_Retener_Sueldo_Normal = Convert.ToDouble(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Sueldo].ToString()))
                                    Tipo_Desc_Retencion_OJ_Sueldo_Normal = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Sueldo].ToString().Trim();

                                if (!String.IsNullOrEmpty(PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Sueldo].ToString()))
                                    OJ_Bruto_Neto_Sueldo_Normal = PARAMTROS_OJ[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Sueldo].ToString();

                                //Consultamos la tabla de parámetros para obtener el identicador de la Orden judicial del concepto a retener al empleado.
                                Mi_SQL = "SELECT " +
                                                     Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial +
                                         " FROM " +
                                                     Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;
                                //Ejecutamos la consulta, para obtener el identificador de la deducción por concepto de Orden Judicial.
                                Dt_Retencion_Orden_Judicial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                                //Validamos que la consulta ó  búsqueda realizada tenga algún resultado.
                                if (Dt_Retencion_Orden_Judicial != null)
                                {
                                    if (Dt_Retencion_Orden_Judicial.Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(Dt_Retencion_Orden_Judicial.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial].ToString()))
                                        {
                                            //Obtenemos el identificador por concepto de Orden Judicial.
                                            DEDUCCION_ORDEN_JUDICAL = Dt_Retencion_Orden_Judicial.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial].ToString();

                                            //Recorremos la lista de Deducciones, para comparar y si existe alguna coincidencia con el parámetro consultado
                                            //se realiza el cálculo.
                                            foreach (Cls_Percepciones_Deducciones Deduccion_Calculada in Lista_Deducciones_Calculadas)
                                            {
                                                //Validamos que el objeto actual no sea null.
                                                if (Deduccion_Calculada != null)
                                                {
                                                    //Comparamos los identificadores, con el parámetro consultador obtener si el empleado
                                                    //tiene asignada dicha deducción.
                                                    if (Deduccion_Calculada.P_Clabe.Equals(DEDUCCION_ORDEN_JUDICAL))
                                                    {
                                                        //Si aplica, validamos si dicha retención se hará por un monto fijo ó por un porcentaje
                                                        //sobre el total de sus percepciones.
                                                        if (Tipo_Desc_Retencion_OJ_Sueldo_Normal.Equals("CANTIDAD"))
                                                        {
                                                            //Obtenemos el monto a descontar al empleado por concepto de orden judicial.
                                                            Deduccion_Calculada.P_Monto += Cant_Porc_Retener_Sueldo_Normal;
                                                            //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                            //retención de orden judicial.
                                                            if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                            {
                                                                Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                            }
                                                        }
                                                        else if (Tipo_Desc_Retencion_OJ_Sueldo_Normal.Equals("PORCENTAJE"))
                                                        {
                                                            if (OJ_Bruto_Neto_Sueldo_Normal.ToString().Trim().ToUpper().Equals("BRUTO"))
                                                            {
                                                                //Obtenemos el monto a descontar al empleado por concepto de orden judicial.
                                                                Total_Percepciones_Empleado = Total_Percepciones - (Total_Aguinaldo + Total_Prima_Vacacional + Total_Indemnizacion);
                                                                Double Bruto = Total_Percepciones_Empleado;
                                                                Cant_Porc_Retener_Sueldo_Normal = Cant_Porc_Retener_Sueldo_Normal / 100;
                                                                Cantidad_Retencion_Orden_Judicial = (Bruto * Cant_Porc_Retener_Sueldo_Normal);
                                                                //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                                //retención de orden judicial.
                                                                Deduccion_Calculada.P_Monto += Cantidad_Retencion_Orden_Judicial;

                                                                if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                                {
                                                                    Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                }
                                                            }
                                                            else if (OJ_Bruto_Neto_Sueldo_Normal.ToString().Trim().ToUpper().Equals("NETO"))
                                                            {
                                                                Total_Deducciones_Empleado = Total_Deducciones - (Total_OJ_Indemnizacion + Total_OJ_Aguinaldo + Total_OJ_PV);
                                                                Total_Percepciones_Empleado = Total_Percepciones - (Total_Aguinaldo + Total_Prima_Vacacional + Total_Indemnizacion);
                                                                Double Neto = (Total_Percepciones_Empleado - Total_Deducciones_Empleado);

                                                                if (Neto < 0) Neto = 0;

                                                                Cant_Porc_Retener_Sueldo_Normal = Cant_Porc_Retener_Sueldo_Normal / 100;
                                                                Cantidad_Retencion_Orden_Judicial = (Neto * Cant_Porc_Retener_Sueldo_Normal);
                                                                //Cargamos la cantidad ya cálculada como monto final a retener al empleado, por concepto de 
                                                                //retención de orden judicial.
                                                                Deduccion_Calculada.P_Monto += Cantidad_Retencion_Orden_Judicial;
                                                                if (Contador_Beneficiarios == (Dt_Empleados.Rows.Count))
                                                                {
                                                                    Cargar_Elemento_Lista_Deducciones_Calculadas(ref Lista_Deducciones_Calculadas_Temporal, Deduccion_Calculada);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
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
            return Lista_Deducciones_Calculadas_Temporal;
        }
        #endregion

        #endregion

        #region (Metodos Generales Operacion)
        ///********************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Leer_Percepciones_Deducciones_Empleados
        /// DESCRIPCION : Recorre la lista de percepciones y/o deducciones para realizar la sumatoria de percepciones y/o deducciones 
        ///               gravados y exentos que ha tenido el empleado en la generacion de la nómina.
        /// 
        /// PARAMETROS:  Lista_Percepciones_Deucciones.- Lista de percepciones y/o  deducciones que se aplicaron al empleado ya sea del
        ///                                              tipo [VARIABLE, FIJA o CALCULADA].
        ///                                              
        ///              Empleado_ID.- El empleado sobre el que se realizara la generación de la nómina.
        ///              Tipo.- Si es Percepcion y/o Deduccion.
        ///                            
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 21/Enero/2011 10:54 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        private void Leer_Percepciones_Deducciones_Empleados(List<Cls_Percepciones_Deducciones> Lista_Percepciones_Deucciones, String Empleado_ID,
                                                            String Tipo)
        {
            String Mi_SQL = "";//Variable que almacenará la consulta a ejecutar.
            DataTable Dt_Parametro = null;//Variable que almacenrá el parámetro de la nómina.
            DataTable Dt_Retencion_Terceros = null;//Variable que almacenará el registro de retencion a terceros.

            //----------- [ PERCEPCIONES ] ----------
            String PERCEPCION_QUINQUENIO = String.Empty;
            String PERCEPCION_PRIMA_VACACIONAL = String.Empty;
            String PERCEPCION_PRIMA_DOMINICAL = String.Empty;
            String PERCEPCION_AGUINALDO = String.Empty;
            String PERCEPCION_DIAS_FESTIVOS = String.Empty;
            String PERCEPCION_HORAS_EXTRA = String.Empty;
            String PERCEPCION_DIA_DOBLE = String.Empty;
            //String PERCEPCION_DIA_DOMINGO = String.Empty;
            String PERCEPCION_AJUSTE_ISR = String.Empty;
            String PERCEPCION_INCAPACIDADES = String.Empty;
            String PERCEPCION_SUBSIDIO = String.Empty;
            String PERCEPCION_PRIMA_ANTIGUEDAD = String.Empty;
            String PERCEPCION_INDEMNIZACION = String.Empty;
            String PERCEPCION_VACACIONES_PENDIENTES_PAGAR = String.Empty;
            String PERCEPCION_SUELDO_NORMAL = String.Empty;
            String PERCEPCION_VACACIONES = String.Empty;
            String PERCEPCION_FONDO_RETIRO = String.Empty;
            String PERCEPCION_PREVISION_SOCIAL_MULTIPLE = String.Empty;

            //----------- [ DEDUCCIONES ] ----------
            String DEDUCCIONES_FALTAS = String.Empty;
            String DEDUCCIONES_RETARDOS = String.Empty;
            String DEDUCCION_TERCERO = String.Empty;
            String DEDUCCION_FONDO_RETIRO = String.Empty;
            String DEDUCCION_ISR = String.Empty;
            String DEDUCCION_IMSS = String.Empty;
            String DEDUCCION_ISSEG = String.Empty;
            String DEDUCCION_VACACIONES_TOMADAS_MAS = String.Empty;
            String DEDUCCION_AGUINALDO_PAGADO_MAS = String.Empty;
            String DEDUCCION_PRIMA_VACACIONAL_PAGADA_MAS = String.Empty;
            String DEDUCCION_SUELDO_PAGADO_MAS = String.Empty;

            try
            {
                //Consulta de Parámetros de la nómina. 
                Mi_SQL = "SELECT " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ".* " +
                         " FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;
                //Ejecutamos la consulta.
                Dt_Parametro = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Cosnulta de el tipo de retencion que tiene el empleado, de acuerdo al partido al que pertenece-.
                Mi_SQL = "SELECT " + Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID +
                         " FROM " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros +
                         " WHERE " + Cat_Nom_Terceros.Campo_Tercero_ID +
                         " IN " +
                         " (SELECT " + Cat_Empleados.Campo_Terceros_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "')";
                //Ejecutamos la consulta.
                Dt_Retencion_Terceros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Retencion_Terceros != null)
                    if (Dt_Retencion_Terceros.Rows.Count > 0)
                        if (!string.IsNullOrEmpty(Dt_Retencion_Terceros.Rows[0][Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID].ToString()))
                            DEDUCCION_TERCERO = Dt_Retencion_Terceros.Rows[0][Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID].ToString();


                if (Dt_Parametro != null)
                {
                    if (Dt_Parametro.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString()))
                            PERCEPCION_QUINQUENIO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString()))
                            PERCEPCION_PRIMA_VACACIONAL = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString()))
                            PERCEPCION_PRIMA_DOMINICAL = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString()))
                            PERCEPCION_AGUINALDO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString()))
                            PERCEPCION_DIAS_FESTIVOS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString()))
                            PERCEPCION_HORAS_EXTRA = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString()))
                            PERCEPCION_DIA_DOBLE = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString();
                        //if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString()))
                        //    PERCEPCION_DIA_DOMINGO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString()))
                            PERCEPCION_AJUSTE_ISR = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString()))
                            PERCEPCION_INCAPACIDADES = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString()))
                            PERCEPCION_SUBSIDIO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString()))
                            PERCEPCION_PRIMA_ANTIGUEDAD = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString()))
                            PERCEPCION_INDEMNIZACION = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString()))
                            PERCEPCION_VACACIONES_PENDIENTES_PAGAR = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString()))
                            PERCEPCION_SUELDO_NORMAL = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString()))
                            PERCEPCION_VACACIONES = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString()))
                            PERCEPCION_FONDO_RETIRO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString()))
                            PERCEPCION_PREVISION_SOCIAL_MULTIPLE = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString();

                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString()))
                            DEDUCCIONES_FALTAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString()))
                            DEDUCCIONES_RETARDOS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString()))
                            DEDUCCION_FONDO_RETIRO = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISR].ToString()))
                            DEDUCCION_ISR = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISR].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString()))
                            DEDUCCION_IMSS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString()))
                            DEDUCCION_VACACIONES_TOMADAS_MAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString()))
                            DEDUCCION_AGUINALDO_PAGADO_MAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString()))
                            DEDUCCION_PRIMA_VACACIONAL_PAGADA_MAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString()))
                            DEDUCCION_SUELDO_PAGADO_MAS = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString();
                        if (!String.IsNullOrEmpty(Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString()))
                            DEDUCCION_ISSEG = Dt_Parametro.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString();

                    }//Fin de la validación de que existan algún registro del parámetro.
                }//Fin de la Validación de los parámetros consultados.


                foreach (Cls_Percepciones_Deducciones Percepcion_Deduccion in Lista_Percepciones_Deucciones)
                {
                    if (Percepcion_Deduccion != null)
                    {
                        if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_QUINQUENIO))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_PRIMA_VACACIONAL))
                        {
                            //Obtenemos el Total Grava la Prima Vacacional.
                            Total_Grava_Prima_Vacacional = Percepcion_Deduccion.P_Grava;
                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_PRIMA_DOMINICAL))
                        {
                            //Obtenemos el Total Grava la Prima Dominical.
                            Total_Grava_Prima_Dominical = Percepcion_Deduccion.P_Grava;
                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_AGUINALDO))
                        {
                            //Obtenemos el Total Grava el Aguinaldo.
                            Total_Grava_Aguinaldo = Percepcion_Deduccion.P_Grava;
                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_DIAS_FESTIVOS))
                        {
                            Total_Grava_Dias_Festivos = Percepcion_Deduccion.P_Grava;
                            Total_Exenta_Dias_Festivos = Percepcion_Deduccion.P_Exenta;
                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_HORAS_EXTRA))
                        {
                            Total_Grava_Tiempo_Extra = Percepcion_Deduccion.P_Grava;
                            Total_Exenta_Tiempo_Extra = Percepcion_Deduccion.P_Exenta;
                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_DIA_DOBLE))
                        {

                        }
                        //else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_DIA_DOMINGO))
                        //{

                        //}
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_AJUSTE_ISR))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_INCAPACIDADES))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_SUBSIDIO))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_SUELDO_NORMAL))
                        {
                            Total_Grava_Sueldo = Percepcion_Deduccion.P_Grava;
                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_VACACIONES))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_FONDO_RETIRO))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_PREVISION_SOCIAL_MULTIPLE))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(DEDUCCIONES_FALTAS))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(DEDUCCIONES_RETARDOS))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(DEDUCCION_TERCERO))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(DEDUCCION_FONDO_RETIRO))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(DEDUCCION_ISR))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(DEDUCCION_IMSS))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(DEDUCCION_VACACIONES_TOMADAS_MAS))
                        {

                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_PRIMA_ANTIGUEDAD))
                        {
                            Total_Grava_Prima_Antiguedad = Percepcion_Deduccion.P_Grava;
                            Total_Exenta_Prima_Antiguedad = Percepcion_Deduccion.P_Exenta;
                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(PERCEPCION_INDEMNIZACION))
                        {
                            Total_Grava_Indemnizacion = Percepcion_Deduccion.P_Grava;
                            Total_Exenta_Indemnizacion = Percepcion_Deduccion.P_Exenta;
                        }
                        else if (Percepcion_Deduccion.P_Clabe.Equals(DEDUCCION_ISSEG))
                        {

                        }

                        if (Tipo.Equals("PERCEPCION"))
                        {
                            if (string.IsNullOrEmpty(Percepcion_Deduccion.P_Es_ISR_Subsidio) || Percepcion_Deduccion.P_Es_ISR_Subsidio.Equals("PERCEPCION"))
                            {
                                Total_Exenta_Empleado += Percepcion_Deduccion.P_Exenta;
                                Total_Ingresos_Gravables_Empleado += ((Percepcion_Deduccion.P_Grava < 0) ? 0 : Percepcion_Deduccion.P_Grava);
                                Total_Percepciones += Percepcion_Deduccion.P_Monto;
                            }
                        }
                        else if (Tipo.Equals("DEDUCCION"))
                        {
                            if (string.IsNullOrEmpty(Percepcion_Deduccion.P_Es_ISR_Subsidio) || Percepcion_Deduccion.P_Es_ISR_Subsidio.Equals("DEDUCCION"))
                            {
                                Total_Deducciones += Percepcion_Deduccion.P_Monto;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al recorrer la lista de Percepciones Deducciones. Error: [" + Ex.Message + "]");
            }
        }
        ///********************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Limpiar_Cantidades
        /// DESCRIPCION : Limpia las Cantidades Totales de las variables declaradas globalmente en la clase de calculo de la nómina.
        ///               antes de generar la nómina de algún otro empleado.
        ///                            
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 21/Enero/2011 10:54 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        private void Limpiar_Cantidades()
        {
            Total_Grava_Prima_Dominical = 0.0;//Variable que almacenrá el total que gravará la prima dominical del empleado.
            Total_Grava_Prima_Vacacional = 0.0;//Variable que almacenrá el total que gravará la prima vacacional del empleado.
            Total_Grava_Aguinaldo = 0.0;//Variable que almacenrá el total que gravará el aguinaldo del empleado.
            Total_Ingresos_Gravables_Empleado = 0.0;//Variable que almacenrá el total de ingresos gravables del empleado.
            Total_Exenta_Empleado = 0.0;//Cantidad que exenta el empleado.
            Total_Percepciones = 0.0;//Variable que almacenrá el total de las percepciones recibidas por el empleado.
            Total_Deducciones = 0.0;//Variable que almacenrá el total de deducciones aplicadas al empleado.
            Total_Grava_Prima_Antiguedad = 0.0;//Se limpia la cantidad que gravo previamente el calculo de prima de antiguedad del empleado.
            Total_Grava_Indemnizacion = 0.0;//Se limpia la cantidad que gravo previamente el calculo de indemnización.
            Total_Exenta_Indemnizacion = 0.0;
            Total_Exenta_Prima_Antiguedad = 0.0;
            Total_Grava_Tiempo_Extra = 0.0;
            Total_Grava_Dias_Festivos = 0.0;
            Total_Exenta_Tiempo_Extra = 0.0;
            Total_Exenta_Dias_Festivos = 0.0;
            Total_Grava_Sueldo = 0.0;
        }
        ///********************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Crear_Tabla_Percepciones_Deducciones_Recibo_Nomina
        /// DESCRIPCION : Crea una tabla donde se almacenan todas las percepciones y/o deducciones que se aplicaron al empleado
        ///               para la generación de su nómina.
        /// 
        /// PARAMETROS:  Percepciones_Deducciones.- Lista de percepciones y/o  deducciones que se aplicaron al empleado ya sea del
        ///                                         tipo variable, fijo o calculada.
        ///              Dt_Percepciones_Deducciones_Recibo_Nomina.- Es la tabla donde se estaran almacenado los registros delas perce y/o  Dedu
        ///                                                          que se aplicaron al empelado para la generacion de su nómina.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 21/Enero/2011 10:54 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public void Crear_Tabla_Percepciones_Deducciones_Recibo_Nomina(List<Cls_Percepciones_Deducciones> Percepciones_Deducciones,
                                                                       ref DataTable Dt_Percepciones_Deducciones_Recibo_Nomina)
        {
            DataRow Renglon = null;//Variable que almacenara un nuevo registro de la tabal de percepciones y/o deducciones.
            try
            {
                foreach (Cls_Percepciones_Deducciones Percepcion_Deduccion in Percepciones_Deducciones)
                {
                    if (Percepcion_Deduccion != null)
                    {
                        Renglon = Dt_Percepciones_Deducciones_Recibo_Nomina.NewRow();//Creamos una registro.
                        Renglon["Percepcion_Deduccion"] = Percepcion_Deduccion.P_Clabe;
                        Renglon["Monto"] = Percepcion_Deduccion.P_Monto;
                        Renglon["Grava"] = Percepcion_Deduccion.P_Grava;
                        Renglon["Exenta"] = Percepcion_Deduccion.P_Exenta;
                        Dt_Percepciones_Deducciones_Recibo_Nomina.Rows.Add(Renglon);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Crear Tabla de Percepciones y/o Deducciones del Recibo de Nómina. Error: [" + Ex.Message + "]");
            }
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Consultar_Informacion_Empleado
        /// DESCRIPCIÓN: Consulta la información del empleado. Todos sus datos generales.
        /// 
        /// PARÁMETROS: Datos .- Estructura de la clase de negocios del empleado, que se utilizara para cargar la información.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 24/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        public void Consultar_Informacion_Empleado(ref Cls_Cat_Empleados_Negocios Datos)
        {
            Cls_Cat_Empleados_Negocios Empleados_Informacion = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
            DataTable Registro_Empleado = null;//Informacion del empleado registrado.

            try
            {
                Empleados_Informacion.P_No_Empleado = Datos.P_No_Empleado;
                Empleados_Informacion.P_Estatus = Datos.P_Estatus;
                Registro_Empleado = Empleados_Informacion.Consulta_Empleados_General();

                if (Registro_Empleado != null)
                {
                    if (Registro_Empleado.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Registro_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString()))
                            Datos.P_Dependencia_ID = Registro_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString();
                        if (!string.IsNullOrEmpty(Registro_Empleado.Rows[0][Cat_Empleados.Campo_Puesto_ID].ToString()))
                            Datos.P_Puesto_ID = Registro_Empleado.Rows[0][Cat_Empleados.Campo_Puesto_ID].ToString();
                        if (!string.IsNullOrEmpty(Registro_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString()))
                            Datos.P_Salario_Diario = Convert.ToDouble(Registro_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString());
                        if (!string.IsNullOrEmpty(Registro_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario_Integrado].ToString()))
                            Datos.P_Salario_Diario_Integrado = Convert.ToDouble(Registro_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario_Integrado].ToString());
                        if (!string.IsNullOrEmpty(Registro_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString()))
                            Datos.P_Tipo_Nomina_ID = Registro_Empleado.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString();
                        if (!string.IsNullOrEmpty(Registro_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString()))
                            Datos.P_Empleado_ID = Registro_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la informacion del Empleado al que se le esta generando la nómina. Error: [" + Ex.Message + "]");
            }
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Consultar_Dias_Trabajados
        /// DESCRIPCIÓN: Consulta los días que el empleado laboro en la última catorcenaa generar la nómina.
        /// 
        /// PARÁMETROS: Empleado_ID .- Clabe del empleado del cuál se está calculando la nómina.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 24/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        public Int32 Consultar_Dias_Trabajados(String Empleado_ID)
        {
            ///Variables de conexion con la capa de negocios.
            Cls_Ope_Nom_Deducciones_Negocio Informacion_Faltas_Empleado = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexion con la capo de negocios.
            Int32 Dias_Trabajados = 0;

            try
            {
                Informacion_Faltas_Empleado.P_Nomina_ID = Nomina_ID;
                Informacion_Faltas_Empleado.P_No_Nomina = No_Nomina;
                Dias_Trabajados = Informacion_Faltas_Empleado.Obtener_Dias_Laborados_Empleado(Empleado_ID);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias trabajados del empleado. Error: [" + Ex.Message + "]");
            }
            return Dias_Trabajados;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Quitar_Percepciones_Con_Montos_Cero
        /// DESCRIPCIÓN: 
        /// 
        /// PARÁMETROS: Lista_Percepciones_Deducciones .- Lista que se recorrera búscando percepciones y/o deducciones con montos igual 
        ///                                               a cero, y eliminarlas de la misma.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 3/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Quitar_Percepciones_Con_Montos_Cero(List<Cls_Percepciones_Deducciones> Lista_Percepciones_Deducciones)
        {
            List<Cls_Percepciones_Deducciones> Lista_Perc_Dedu_Sin_Montos_Cero = new List<Cls_Percepciones_Deducciones>();//Lista de Percepciones y/o Deduccion sin conceptos con montos igual a cero.

            try
            {
                ///Recorremos La Lista De Percepciones Y/O Deducciones En Búsqueda De Percepciones Y/O Deducciones 
                ///Con Montos Igual A Cero, Si Se Encuentra Un Concepto Con Un Monto Igual A Cero, Se Elimina De La 
                ///Lista De Percepiones Y/O Deducciones Que Se Este Recorriendo Actualmente.
                foreach (Cls_Percepciones_Deducciones Percepcion_IO_Deduccion in Lista_Percepciones_Deducciones)
                {
                    if (Percepcion_IO_Deduccion != null)
                    {
                        //Validamos si el monto de la percepcion y/o deducción es igual a cero.
                        if (Percepcion_IO_Deduccion.P_Monto > 0)
                        {
                            //Si es mayor a cero al agregamos a una nueva lista. Y continuamos haciendo el recorrido.
                            Lista_Perc_Dedu_Sin_Montos_Cero.Add(Percepcion_IO_Deduccion);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al hacer un barrido de las percepciones y/o deducciones y quitar las mismas con montos igual cero. Error: [" + Ex.Message + "]");
            }
            return Lista_Perc_Dedu_Sin_Montos_Cero;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Tablas_Percepciones_Deducciones
        /// 
        /// DESCRIPCIÓN: Recorre una Lista donde cada elemento es una sublista de Percepciones y/o Deducciones, Este método recorrera 
        ///              la lista y apartir de las mismas obtenedra un Arreglo de tablas una tabla de Percepciones y otra de Deducciones
        ///              con las cantidades que le corresponde a cada una de acuerdo al concepto.
        ///              
        /// PARÁMETROS: Lista_Percepciones_IO_Deducciones.- DataSet, que almacena todas las tablas de Percepciones y/o Deducciones.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 03/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        public DataSet Obtener_Tablas_Percepciones_Deducciones(List<List<Cls_Percepciones_Deducciones>> Lista_Percepciones_IO_Deducciones)
        {
            DataSet Ds_Tablas_Percepciones_Deducciones = new DataSet();//Estructura que almacenará las tablas de Percepciones y/o Deducciones
            DataTable Dt_Percepiones = new DataTable("PERCEPCION");//Estructura que almacenara la lista de Percepciones.
            DataTable Dt_Deducciones = new DataTable("DEDUCCION");//Estructura que almacenara la lista de Deducciones.
            DataRow Renglon = null;//Registro a insertar como concepto.
            Cls_Percepciones_Deducciones Percepcion_Deduccion_Aux;//Variable de tipo temporal que almacena una entidad del tipo 
            //[Cls_Percepciones_Deducciones] de forma temporal.

            try
            {
                //Generamos las columnas qu contendra nuestra tabla de percepciones.
                Dt_Percepiones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID, typeof(String));
                Dt_Percepiones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(String));
                Dt_Percepiones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion, typeof(String));
                Dt_Percepiones.Columns.Add("Cantidad", typeof(String));
                Dt_Percepiones.Columns.Add("Gravado", typeof(String));
                Dt_Percepiones.Columns.Add("Exento", typeof(String));

                //Generamos las columnas qu contendra nuestra tabla de deduc.
                Dt_Deducciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID, typeof(String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion, typeof(String));
                Dt_Deducciones.Columns.Add("Cantidad", typeof(String));
                Dt_Deducciones.Columns.Add("Gravado", typeof(String));
                Dt_Deducciones.Columns.Add("Exento", typeof(String));

                foreach (List<Cls_Percepciones_Deducciones> Percepciones_IO_Deducciones in Lista_Percepciones_IO_Deducciones)
                {
                    if (Percepciones_IO_Deducciones != null)
                    {
                        if (Percepciones_IO_Deducciones.Count > 0)
                        {
                            foreach (Cls_Percepciones_Deducciones Percepcion_Deduccion in Percepciones_IO_Deducciones)
                            {
                                if (Percepcion_Deduccion != null)
                                {

                                    Percepcion_Deduccion_Aux = Percepcion_Deduccion;


                                    if (string.IsNullOrEmpty(Percepcion_Deduccion.P_Tipo) ||
                                        string.IsNullOrEmpty(Percepcion_Deduccion.P_Tipo_Asignacion))
                                    {
                                        Percepcion_Deduccion_Aux = Percepcion_Deduccion;
                                        Consultar_Percepcion_Deduccion(ref Percepcion_Deduccion_Aux);
                                    }
                                    switch (Percepcion_Deduccion.P_Tipo)
                                    {
                                        case "PERCEPCION":
                                            Renglon = Dt_Percepiones.NewRow();
                                            Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Percepcion_Deduccion_Aux.P_Clabe;
                                            Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = Percepcion_Deduccion_Aux.P_Nombre;
                                            Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion] = Percepcion_Deduccion_Aux.P_Tipo_Asignacion;
                                            Renglon["Cantidad"] = Percepcion_Deduccion_Aux.P_Monto.ToString();
                                            Renglon["Gravado"] = Percepcion_Deduccion_Aux.P_Grava.ToString();
                                            Renglon["Exento"] = Percepcion_Deduccion_Aux.P_Exenta.ToString();
                                            Dt_Percepiones.Rows.Add(Renglon);
                                            break;
                                        case "DEDUCCION":
                                            Renglon = Dt_Deducciones.NewRow();
                                            Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Percepcion_Deduccion_Aux.P_Clabe;
                                            Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = Percepcion_Deduccion_Aux.P_Nombre;
                                            Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion] = Percepcion_Deduccion_Aux.P_Tipo_Asignacion;
                                            Renglon["Cantidad"] = Percepcion_Deduccion_Aux.P_Monto.ToString();
                                            Dt_Deducciones.Rows.Add(Renglon);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }

                Ds_Tablas_Percepciones_Deducciones.Tables.Add(Dt_Percepiones);
                Ds_Tablas_Percepciones_Deducciones.Tables.Add(Dt_Deducciones);
            }
            catch (Exception Ex)
            {
                throw new Exception(".Error: [" + Ex.Message + "]");
            }
            return Ds_Tablas_Percepciones_Deducciones;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Consultar_Percepcion_Deduccion
        /// 
        /// DESCRIPCIÓN:.  Recibe como parámetro una percepción y/o deducción, qua un no tiene asignado un tipo [PERCEPCIÓN] ó un
        ///                Tipo_Asignacion [Fijo, Variable ó Calculado]. Este método toma su Clave, lo consulta en la tabla de percepciones
        ///                y/o deducciones y le asigna un Tipo y un Tipo_Asignacion
        ///              
        /// PARÁMETROS:  Percepcion_Deduccion.- Entidad que almacena las datos de la percepción y/o deducción
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 03/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private void Consultar_Percepcion_Deduccion(ref Cls_Percepciones_Deducciones Percepcion_Deduccion)
        {
            Cls_Cat_Nom_Percepciones_Deducciones_Business Percepcion_Deduccion_Negocio = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocios.
            DataTable Dt_Percepciones_Deducciones = null;//Lista de Percepciones y/o Deducciones.

            try
            {
                Dt_Percepciones_Deducciones = Percepcion_Deduccion_Negocio.Busqueda_Percepcion_Deduccion_Por_ID(Percepcion_Deduccion.P_Clabe);

                if (Dt_Percepciones_Deducciones != null)
                {
                    if (Dt_Percepciones_Deducciones.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Percepciones_Deducciones.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString()))
                            Percepcion_Deduccion.P_Tipo = Dt_Percepciones_Deducciones.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Percepciones_Deducciones.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString()))
                            Percepcion_Deduccion.P_Tipo_Asignacion = Dt_Percepciones_Deducciones.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("error al consultar por el Identificador de la Percepcion Deduccion. Error: [" + Ex.Message + "]");
            }
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_Resultado
        /// DESCRIPCIÓN: Obtiene la cantidad de la tabla de resultados.
        /// 
        /// PARÁMETROS: Dt_Resultados_Calculo_Actual.- Resultados del calculo de la percepción.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 14/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private Double Obtener_Cantidad_Resultado(DataTable Dt_Resultados_Calculo_Actual)
        {
            Double Cantidad = 0.0;//Variable que almacenará la cantidad que le aplica según la percepción.
            Double Grava = 0.0;//Variable que almacenrá la cantidad que grava el quinquenio.
            Double Exenta = 0.0;//Variable que almacenará la cantidad que exenta el quinquenio.

            try
            {
                if (Dt_Resultados_Calculo_Actual is DataTable)
                {
                    if (Dt_Resultados_Calculo_Actual.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(Dt_Resultados_Calculo_Actual.Rows[0]["Calculo"].ToString().Trim()))
                            Cantidad = Convert.ToDouble(Dt_Resultados_Calculo_Actual.Rows[0]["Calculo"].ToString().Trim());
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Cargar la Lista de Percepciones Calculadas [FINIQUITO]. Error: [" + Ex.Message + "]");
            }
            return Cantidad;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Cantidad_ISR_Particular
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
            Double Grava = 0.0;//Variable que almacenrá la cantidad que grava el quinquenio.
            Double Exenta = 0.0;//Variable que almacenará la cantidad que exenta el quinquenio.

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
        #endregion

        #region (Metodos Generales Base Datos)
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Deducciones_Empleado
        /// DESCRIPCION : Consulta las percepciones y/o deducciones que le corresponden al empleado de acuerdo
        ///               al sindicato y  tipo de nomina al que pertence. 
        /// 
        /// PARAMETROS:  Empleado_Id.- Empleado a generar su nomina.
        ///              Tipo.- Percepcion o Deduccion
        ///              Tipo_Asignacion.- Fija, Variable o Calculada
        ///              Concepto.- SINDICATO o TIPO NOMINA
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 10/Enero/2011 10:54 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        private DataTable Consultar_Percepciones_Deducciones_Empleado(String Empleado_Id, String Tipo, String Tipo_Asignacion, String Concepto)
        {
            String Mi_SQL = "";//Variable que almacenrá la consulta.
            DataTable Dt_Percepciones_Deducciones = null;//Variable que almacena una lista de percepciones deducciones, que le aplican ala empleado.
            Cls_Cat_Nom_Percepciones_Deducciones_Business Cls_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocios.

            try
            {
                //Paso I.- Se construyé la consulta.
                Mi_SQL = "SELECT " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".*" +
                         ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto +
                         ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad +
                         ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe +
                         ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo +
                         ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida +
                         " FROM " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion +
                         ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                         " WHERE " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Tipo + "'" +
                         " AND " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + " ='" + Tipo_Asignacion + "'" +
                         " AND " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + " ='" + Concepto + "'" +
                         " AND " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Empleado_Id + "'" +
                         " AND " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                         "=" +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID;

                //Paso II.- Ejecutamos la consulta.
                Dt_Percepciones_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
            return Dt_Percepciones_Deducciones;
        }
        ///********************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Lista_Percepciones_Deducciones
        /// DESCRIPCION : Obtiene una lista de percepciones y/o deducciones de acuerdo  al concepto y tipo de
        ///               asignación al que pertencé. Esta búsqueda se realiza apartir de la tabla que es pasada
        ///               como parámetro.
        /// 
        /// PARAMETROS:  Dt_Percepciones_Deducciones.- Tabla a obtener las deducciones o percepciones.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 10/Enero/2011 10:54 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_Lista_Percepciones_Deducciones(DataTable Dt_Percepciones_Deducciones)
        {
            List<Cls_Percepciones_Deducciones> Percepciones_Deducciones = new List<Cls_Percepciones_Deducciones>();//Variable que almacenará una lista de Percepciones y/ó Deducciones [Fijas, Variables ó Calculadas].
            String Concepto = "";//Variable que almacenrá el concepto de la percepción y/ó deducción [Tipo_Nomina ó Sindicato]. 
            Double Cantidad = 0.0;//Variable que almacenrá la cantidad si la percepción y/ó deducción son fijas.
            Cls_Percepciones_Deducciones Objeto_Percepcion_Deduccion = null;

            //Paso I.- Recorremos la Tabla de Percepciones y/o Deducciones para separar las que corresponden 
            //         a un determinado concepto.
            foreach (DataRow Renglon in Dt_Percepciones_Deducciones.Rows)
            {
                Concepto = Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto].ToString();//Obtenemos el concepto de la percepción y/o deducción. 

                switch (Concepto)
                {
                    case "TIPO_NOMINA":
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()))
                        {
                            Cantidad = Convert.ToDouble(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString());
                        }
                        Objeto_Percepcion_Deduccion = new Cls_Percepciones_Deducciones();

                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString()))
                            Objeto_Percepcion_Deduccion.P_Clabe = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString();
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString()))
                            Objeto_Percepcion_Deduccion.P_Nombre = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString();
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()))
                            Objeto_Percepcion_Deduccion.P_Monto = Cantidad;
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString()))
                            Objeto_Percepcion_Deduccion.P_Aplica = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString();
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString()))
                            Objeto_Percepcion_Deduccion.P_Gravable = Convert.ToDouble(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString()))
                            Objeto_Percepcion_Deduccion.P_Porcentaje_Gravable = Convert.ToDouble(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString()))
                            Objeto_Percepcion_Deduccion.P_Tipo = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString();
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString()))
                            Objeto_Percepcion_Deduccion.P_Tipo_Asignacion = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString();

                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim()))
                        {
                            Objeto_Percepcion_Deduccion.P_Importe = Convert.ToDouble(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim());
                        }
                        else { Objeto_Percepcion_Deduccion.P_Importe = 0; }

                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim()))
                        {
                            Objeto_Percepcion_Deduccion.P_Saldo = Convert.ToDouble(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim());
                        }
                        else { Objeto_Percepcion_Deduccion.P_Saldo = 0; }

                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim()))
                        {
                            Objeto_Percepcion_Deduccion.P_Cantidad_Retenida = Convert.ToDouble(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim());
                        }
                        else { Objeto_Percepcion_Deduccion.P_Cantidad_Retenida = 0; }

                        Percepciones_Deducciones.Add(Objeto_Percepcion_Deduccion);
                        break;
                    case "SINDICATO":
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()))
                        {
                            Cantidad = Convert.ToDouble(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString());
                        }

                        Objeto_Percepcion_Deduccion = new Cls_Percepciones_Deducciones();

                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString()))
                            Objeto_Percepcion_Deduccion.P_Clabe = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString();
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString()))
                            Objeto_Percepcion_Deduccion.P_Nombre = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString();
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()))
                            Objeto_Percepcion_Deduccion.P_Monto = Cantidad;
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString()))
                            Objeto_Percepcion_Deduccion.P_Aplica = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString();
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString()))
                            Objeto_Percepcion_Deduccion.P_Gravable = Convert.ToDouble(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString()))
                            Objeto_Percepcion_Deduccion.P_Porcentaje_Gravable = Convert.ToDouble(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString()))
                            Objeto_Percepcion_Deduccion.P_Tipo = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString();
                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString()))
                            Objeto_Percepcion_Deduccion.P_Tipo_Asignacion = Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString();

                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim()))
                        {
                            Objeto_Percepcion_Deduccion.P_Importe = Convert.ToDouble(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim());
                        }
                        else { Objeto_Percepcion_Deduccion.P_Importe = 0;}

                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim()))
                        {
                            Objeto_Percepcion_Deduccion.P_Saldo = Convert.ToDouble(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim());
                        }
                        else { Objeto_Percepcion_Deduccion.P_Saldo = 0; }

                        if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim()))
                        {
                            Objeto_Percepcion_Deduccion.P_Cantidad_Retenida= Convert.ToDouble(Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim());
                        }
                        else { Objeto_Percepcion_Deduccion.P_Cantidad_Retenida = 0; }

                        Percepciones_Deducciones.Add(Objeto_Percepcion_Deduccion);
                        break;
                    default:
                        break;
                }
            }
            return Percepciones_Deducciones;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Aplica_Calculo_ISR
        /// DESCRIPCIÓN: Consulta a que nómina pertence el empleado y sis está aplicá calculo de ISR.
        /// 
        /// PARÁMETROS: Empleado_ID .- Identificador único que identifica al empleado en las operacione sinternas del sistema.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 19/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private Boolean Aplica_Calculo_ISR(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Cls_Empleados_Negocio = new Cls_Cat_Empleados_Negocios();//Variable de conexión conla capa de negocios.
            Cls_Cat_Tipos_Nominas_Negocio Cls_tipos_Nominas_Negocio = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Empleados = null;//Lista de empleados.
            DataTable Dt_Tipos_Nomina = null;//Lista de tipos de nóminas.
            String Tipo_Nomina_ID = "";//Variable que almacenara el identicador de la nómina a la que pertence el empleado,
            Boolean Aplica_ISR = false;

            try
            {
                Cls_Empleados_Negocio.P_Empleado_ID = Empleado_ID;
                Dt_Empleados = Cls_Empleados_Negocio.Consulta_Empleados_General();

                if (Dt_Empleados != null)
                    if (Dt_Empleados.Rows.Count > 0)
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()))
                            Tipo_Nomina_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString();

                Cls_tipos_Nominas_Negocio.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Tipos_Nomina = Cls_tipos_Nominas_Negocio.Consulta_Datos_Tipo_Nomina();

                if (Dt_Tipos_Nomina != null)
                    if (Dt_Tipos_Nomina.Rows.Count > 0)
                        if (!string.IsNullOrEmpty(Dt_Tipos_Nomina.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString()))
                            Aplica_ISR = (Dt_Tipos_Nomina.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString().Equals("SI")) ? true : false;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar si al empleado se aplicara el calculo de ISR. Error: [" + Ex.Message + "");
            }
            return Aplica_ISR;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_Monto_Despensa_Tipo_Nomina
        /// DESCRIPCIÓN: Consulta la cantidad  despensa que el empleado percibira de acuerdo al tipo de nómina al que pertenece.
        /// 
        /// PARÁMETROS: Empleado_ID .- Identificador único que identifica al empleado en las operación del sistema.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 19/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private Double Obtener_Monto_Despensa_Tipo_Nomina(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Cls_Empleados_Negocio = new Cls_Cat_Empleados_Negocios();//Variable de conexión conla capa de negocios.
            Cls_Cat_Tipos_Nominas_Negocio Cls_tipos_Nominas_Negocio = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Empleados = null;//Lista de empleados.
            DataTable Dt_Tipos_Nomina = null;//Lista de tipos de nóminas.
            String Tipo_Nomina_ID = "";//Variable que almacenara el identicador de la nómina a la que pertence el empleado,
            Double Cantidad = 0.0;

            try
            {
                Cls_Empleados_Negocio.P_Empleado_ID = Empleado_ID;
                Dt_Empleados = Cls_Empleados_Negocio.Consulta_Empleados_General();

                if (Dt_Empleados != null)
                    if (Dt_Empleados.Rows.Count > 0)
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()))
                            Tipo_Nomina_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString();

                Cls_tipos_Nominas_Negocio.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Tipos_Nomina = Cls_tipos_Nominas_Negocio.Consulta_Datos_Tipo_Nomina();

                if (Dt_Tipos_Nomina != null)
                    if (Dt_Tipos_Nomina.Rows.Count > 0)
                        if (!string.IsNullOrEmpty(Dt_Tipos_Nomina.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Despensa].ToString()))
                            Cantidad = Convert.ToDouble(Dt_Tipos_Nomina.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Despensa].ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar si al empleado se aplicara el calculo de ISR. Error: [" + Ex.Message + "");
            }
            return Cantidad;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Consultar_Columnas_Tabla_Ope_Nom_Totales_Nomina
        /// DESCRIPCIÓN: Consulta las columnas de la tabla de Ope_Nom_Totales_Nomina.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 21/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private DataTable Consultar_Columnas_Tabla_Ope_Nom_Totales_Nomina()
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Columnas_Tabla_Ope_Nom_Totales_Nomina = null;//Almacena una lista de los campos de la tabla de Ope_Nom_Totales_Nomina.

            try
            {
                Mi_SQL = "SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME='" + Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + "'";
                Mi_SQL += " AND SUBSTR(COLUMN_NAME, 6, 5) IN (SELECT " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Estatus + "='ACTIVO')";
                Mi_SQL += " UNION ";
                Mi_SQL += "(SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME ='OPE_NOM_TOTALES_NOMINA' AND COLUMN_NAME IN ('DETALLE_NOMINA_ID', 'NOMINA_ID', 'NO_NOMINA', 'NO_TOTAL_NOMINA', 'TIPO_NOMINA_ID'))";

                Dt_Columnas_Tabla_Ope_Nom_Totales_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las columnas de la tabla de Ope_Nom_Totales_Nomina. Error: [" + Ex.Message + "]");
            }
            return Dt_Columnas_Tabla_Ope_Nom_Totales_Nomina;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Guardar_Recibo_Nomina
        /// DESCRIPCIÓN: Guarda el registro del recibo generado al empleado de su nómina generada.
        ///             
        /// PARÁMETROS: Datos.- Entidad que trae los datos necesarios para el registro del recibo de nómina.
        ///             Colleccion_Listas_Perc_Dedu.- Colección de Listas de Percepciones y/o Deducciones que aplicaron al empleado.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 24/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        public void Guardar_Recibo_Nomina(Cls_Ope_Nom_Recibos_Empleados_Negocio Datos, DataTable Dt_Recibo_Empleado_Det)
        {
            Cls_Ope_Nom_Recibos_Empleados_Negocio Recibos_Empleados = new Cls_Ope_Nom_Recibos_Empleados_Negocio();//Variable de conexion a la capa de negocios.

            try
            {
                //Establecemos la informacion necesaria para generar el recibo de nómina.
                Recibos_Empleados.P_Nomina_ID = Datos.P_Nomina_ID;
                Recibos_Empleados.P_Detalle_Nomina_ID = Datos.P_Detalle_Nomina_ID;
                Recibos_Empleados.P_No_Nomina = Datos.P_No_Nomina;
                Recibos_Empleados.P_Tipo_Nomina_ID = Datos.P_Tipo_Nomina_ID;
                Recibos_Empleados.P_Empleado_ID = Datos.P_Empleado_ID;
                Recibos_Empleados.P_Dependencia_ID = Datos.P_Dependencia_ID;
                Recibos_Empleados.P_Puesto_ID = Datos.P_Puesto_ID;
                Recibos_Empleados.P_Dias_Trabajados = Datos.P_Dias_Trabajados;
                Recibos_Empleados.P_Total_Percepciones = Datos.P_Total_Percepciones;
                Recibos_Empleados.P_Total_Deducciones = Datos.P_Total_Deducciones;
                Recibos_Empleados.P_Total_Nomina = Datos.P_Total_Nomina;
                Recibos_Empleados.P_Gravado = Datos.P_Gravado;
                Recibos_Empleados.P_Exento = Datos.P_Exento;
                Recibos_Empleados.P_Salario_Diario = Datos.P_Salario_Diario;
                Recibos_Empleados.P_Salario_Diario_Integrado = Datos.P_Salario_Diario_Integrado;
                Recibos_Empleados.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                Recibos_Empleados.P_Nomina_Generada = Datos.P_Nomina_Generada;

                //Establecemos la tabla de Percepciones y/o Deducciones.
                Recibos_Empleados.P_Dt_Recibo_Empleado_Detalles = Dt_Recibo_Empleado_Det;
                //Registromos el recibo almacenandolo en la BD.
                Recibos_Empleados.Alta_Recibo_Empleado();
            }
            catch (Exception Ex)
            {
                throw new Exception("Generar y Guardar el recibo de nómina para el empleado. Error: [" + Ex.Message + "]");
            }
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Guardar_Totales_Nomina
        /// DESCRIPCIÓN: Guarda los Totales de la Nómina Generada en la Catorcena.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 25/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        public void Guardar_Totales_Nomina()
        {
            DataTable Dt_Totales_Nomina = Cls_Sessiones.Totales_Percepciones_Deducciones;//Obtenemos la tabla de totales de la nómina.
            DataTable Dt_Esquema_Ope_Nom_Totales_Nomina = null;
            String Mi_SQL = "";//Variable que almacenará la consulta.
            Object No_Total_Nomina = null;
            String Dato_Insertar = "";
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Nom_Totales_Nomina.Campo_No_Total_Nomina + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina;
                No_Total_Nomina = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Total_Nomina))
                {
                    No_Total_Nomina = "0000000001";
                }
                else
                {
                    No_Total_Nomina = String.Format("{0:0000000000}", Convert.ToInt32(No_Total_Nomina) + 1);
                }


                //Obtenemos el esquema de la tabla de [OPE_NOM_TOTALES_NOMINA].
                Dt_Esquema_Ope_Nom_Totales_Nomina = Consultar_Columnas_Tabla_Ope_Nom_Totales_Nomina();

                Mi_SQL = "INSERT INTO " + Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + " (";

                for (Int32 Contador = 0; Contador < Dt_Esquema_Ope_Nom_Totales_Nomina.Rows.Count; Contador++)
                {
                    if (Contador == (Dt_Esquema_Ope_Nom_Totales_Nomina.Rows.Count - 1)) Mi_SQL += Dt_Esquema_Ope_Nom_Totales_Nomina.Rows[Contador][0].ToString() + " ";
                    if (Contador < (Dt_Esquema_Ope_Nom_Totales_Nomina.Rows.Count - 1)) Mi_SQL += Dt_Esquema_Ope_Nom_Totales_Nomina.Rows[Contador][0].ToString() + ", ";
                }

                Mi_SQL += ") VALUES(";

                foreach (DataRow Renglon_Totales_Nomina in Dt_Totales_Nomina.Rows)
                {
                    for (Int32 Contador = 0; Contador < Dt_Esquema_Ope_Nom_Totales_Nomina.Rows.Count; Contador++)
                    {
                        if (Dt_Esquema_Ope_Nom_Totales_Nomina.Rows[Contador][0].ToString().Equals(Ope_Nom_Totales_Nomina.Campo_No_Nomina))
                        {
                            Dato_Insertar = Renglon_Totales_Nomina[Dt_Esquema_Ope_Nom_Totales_Nomina.Rows[Contador][0].ToString()].ToString();
                            if (Contador == (Dt_Esquema_Ope_Nom_Totales_Nomina.Rows.Count - 1)) Mi_SQL += ((string.IsNullOrEmpty(Dato_Insertar)) ? "0" : Dato_Insertar) + " ";
                            if (Contador < (Dt_Esquema_Ope_Nom_Totales_Nomina.Rows.Count - 1)) Mi_SQL += ((string.IsNullOrEmpty(Dato_Insertar)) ? "0" : Dato_Insertar) + ", ";
                        }
                        else if (Dt_Esquema_Ope_Nom_Totales_Nomina.Rows[Contador][0].ToString().Equals(Ope_Nom_Totales_Nomina.Campo_No_Total_Nomina))
                        {
                            Mi_SQL += "'" + No_Total_Nomina + "', ";
                        }
                        else
                        {
                            Dato_Insertar = Renglon_Totales_Nomina[Dt_Esquema_Ope_Nom_Totales_Nomina.Rows[Contador][0].ToString()].ToString();
                            if (Contador == (Dt_Esquema_Ope_Nom_Totales_Nomina.Rows.Count - 1)) Mi_SQL += "'" + ((string.IsNullOrEmpty(Dato_Insertar)) ? "0" : Dato_Insertar) + "' ";
                            if (Contador < (Dt_Esquema_Ope_Nom_Totales_Nomina.Rows.Count - 1)) Mi_SQL += "'" + ((string.IsNullOrEmpty(Dato_Insertar)) ? "0" : Dato_Insertar) + "', ";
                        }
                    }
                }
                Mi_SQL += ")";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos

                Cls_Sessiones.Historial_Nomina_Generada.Append("[TOTALES_NOMINA, " + No_Total_Nomina + "]\r\n");
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Consultar_Nomina_Generadas
        /// 
        /// DESCRIPCIÓN: Consulta si la nómina que se paso como parámetro al método, ya fue cálculada previamente. Esto lo realiza 
        ///              consultando la tabla de Ope_Nom_Recibos_Empleados, donde obtiene el registro que ya ha sido generado para
        ///              esa nómina en caso, de ya haberse generado previamente.
        ///              
        /// PARÁMETROS: Nomina_ID.- Identificador de la nómina que se desea consultar para validar qie no se encuntre ya en el sistema.
        ///             Detalle_Nomina_ID.- Identicador del Periodo a cálcular
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 25/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        public DataTable Consultar_Nomina_Generadas(String Nomina_ID, String Detalle_Nomina_ID)
        {
            String Mi_SQL = "";//Variable que almacena la consulta.
            DataTable Dt_Nominas_Generadas = null;//Lista de nóminas ya generadas.

            try
            {
                Mi_SQL = "SELECT " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ".* " +
                         " FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +
                         " WHERE " + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "' AND " +
                         Ope_Nom_Recibos_Empleados.Campo_Detalle_Nomina_ID + "='" + Detalle_Nomina_ID + "'";

                Dt_Nominas_Generadas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las nominas generadas. Error: [" + Ex.Message + "]");
            }
            return Dt_Nominas_Generadas;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: RollBack_Registros_Afectadoos_Generacion_Nomina
        /// 
        /// DESCRIPCIÓN: Método que ejecuta el Rollback de los registros que fueron afectados durante la Generación de la Nómina.
        ///              Este método tiene la funcion de realizar los UPDATE de los registros de las tablas que fueron afectadas.
        ///              [PRESTAMOS, AJUSTES ISR, RECIBOS DE NÓMINA, TOTALES DE LA NÓMINA].
        ///              
        /// PARÁMETROS: Tabla_Registros_Modificados.- DataSet, que almacena todas las tablas con los registros que fueron afectados
        ///             al generar la nómina.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 31/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        public void RollBack_Registros_Afectadoos_Generacion_Nomina(DataSet Tabla_Registros_Modificados)
        {
            ///********************************     PRESTAMOS   ****************************************
            DataTable Dt_Prestamos;//Tabla [PRESTAMOS] con los registros que fueron afectados al Generar la Nómina.
            String No_Solicitud = "";//Identicador de la solicitud de prestamo.
            Double Monto_Abonado = 0.0;//Cantidad abonada del prestamo.
            Double Saldo_Actual = 0.0;//Saldo Actual del Prestamo.
            Double No_Abono = 0;//No de Abono que se ha realizado.
            String Fecha_Finiquito_Prestamo = "";//Fecha de Finiquito del prestamo en caso de que ya se halla terminado de pagar.
            String Estado_Prestamo = "";//Estatus del prestamo actualmente. [PROCESO o PAGADO].
            ///********************************     AJUSTES ISR     ************************************
            DataTable Dt_Ajustes_ISR;//Tabla [AJUSTE ISR] con los registros que fueron afectados al Generar la Nómina.
            String No_Ajuste_ISR = "";//Identificador del Ajuste de ISR.
            Double Total_ISR_Ajustado = 0;//Cantidad que ya a sido ajusta de ISR.
            Double No_Pago = 0;//No de pago de Ajuste de ISR, que ya se ha realziado.
            String Estatus_Ajuste_ISR = "";//Estatus del Ajuste de ISR.
            ///********************************     RECIBOS DE NOMINA   ********************************
            DataTable Dt_Recibos_Generados;//Tabla [RECIBOS DE  LA NÓMINA] con los registros que fueron afectados al Generar la Nómina.
            String No_Recibo = "";//Identificador del número de recibo de nómina generado.
            ///********************************     TOTALES DE LA NOMINA    ****************************
            DataTable Dt_Totales_Nomina = null;//Tabla [TOTALES DE LA NÓMINA] con los registros que fueron afectados al Generar la Nómina.
            String No_Total_Nomina = "";//Identificador de la tabla que almacena los totales de la nómina.
            String Mi_SQL = "";//Variable que almacena la consulta.
            ///******************************  ACTUALIZAR DEDUCCIONES FIJAS APLICADAS POR PROVEEDOR  *****************************
            DataTable Dt_Deducciones_Fijas = null;//Variable que almacenara los registros de deducciones fijas aplicadas a los empleados.
            Double Cantidad = 0.0;                //Variable que almacena la cantidad a retener al empleado.
            Double Importe = 0.0;                 //Variable que almacena el importe que debe el empleado al proveedor.
            Double Saldo = 0.0;                   //Variable que almacena el saldo que aun tiene el empleado.
            Double Cantidad_Retenida = 0.0;       //Variable que almacena la cantidad que se le ha retenido al empleado.
            String Empleado_ID = String.Empty;    //Variable que almacena el identificador del empleado al que se le actualizara la información.
            String Deduccion_ID = String.Empty;   //Variable que almacena el identificador de la deduccion a actualizar.
            String Str_Nomina_ID = String.Empty;
            Int32 Int_No_Nomina = 0;
            String Referencia = String.Empty;

            try
            {
                //OBTENEMOS LAS TABLAS QUE FUERON AFECTADAS AL GENERAR LA NÓMINA.
                Dt_Prestamos = Tabla_Registros_Modificados.Tables["PRESTAMO"];
                Dt_Ajustes_ISR = Tabla_Registros_Modificados.Tables["AJUSTE_ISR"];
                Dt_Recibos_Generados = Tabla_Registros_Modificados.Tables["RECIBOS"];
                Dt_Totales_Nomina = Tabla_Registros_Modificados.Tables["TOTALES_NOMINA"];
                Dt_Deducciones_Fijas = Tabla_Registros_Modificados.Tables["EMPL_PERC_DEDU_DETA"];

                //Validamos si algún registros de PRESTAMOS fueron afectados, si es hay registros afectados,
                //se procede a realizar los respectivos update a la tabla de PRESTAMOS.
                if (Dt_Prestamos != null)
                {
                    foreach (DataRow Renglon in Dt_Prestamos.Rows)
                    {
                        if (Renglon != null)
                        {

                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString()))
                                No_Solicitud = Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString().Trim();

                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString()))
                                if (!Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString().Trim().Equals("NULL"))
                                    Monto_Abonado = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString().Trim());

                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString()))
                                if (!Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString().Trim().Equals("NULL"))
                                    Saldo_Actual = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString().Trim());

                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString()))
                                if (!Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString().Trim().Equals("NULL"))
                                    No_Abono = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString().Trim());

                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo].ToString()))
                                Fecha_Finiquito_Prestamo = Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo].ToString().Trim();

                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo].ToString()))
                                Estado_Prestamo = Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo].ToString().Trim();

                            //Generamos el Upadte de los registros afectados en la tabla de  Ope_Nom_Solicitudes_Prestamos. 
                            Mi_SQL = "UPDATE " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + " SET " +
                                     Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado + "=" + Monto_Abonado + ", " +
                                     Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual + "=" + Saldo_Actual + ", " +
                                     Ope_Nom_Solicitud_Prestamo.Campo_No_Abono + "=" + No_Abono + ", " +
                                     Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo + "=" + Fecha_Finiquito_Prestamo + ", " +
                                     Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + "='" + Estado_Prestamo + "' " +
                                     " WHERE " +
                                     Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + No_Solicitud + "'";
                            //Ejecutamos la consulta.
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                    }
                }

                //Validamos si algún registros de AJUSTES DE ISR fueron afectados, si es hay registros afectados,
                //se procede a realizar los respectivos update a la tabla de AJUSTES DE ISR.
                if (Dt_Ajustes_ISR != null)
                {
                    foreach (DataRow Renglon in Dt_Ajustes_ISR.Rows)
                    {
                        if (Renglon != null)
                        {
                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString()))
                                No_Ajuste_ISR = Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString().Trim();

                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString()))
                                if (!Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString().Trim().Equals("NULL"))
                                    Total_ISR_Ajustado = Convert.ToDouble(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString().Trim());

                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString()))
                                if (!Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString().Trim().Equals("NULL"))
                                    No_Pago = Convert.ToDouble(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString().Trim());

                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR].ToString()))
                                Estatus_Ajuste_ISR = Renglon[Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR].ToString().Trim();

                            //Generamos el Upadte de los registros afectados en la tabla de  Ope_Nom_Ajuste_ISR. 
                            Mi_SQL = "UPDATE " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + " SET " +
                                     Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado + "=" + Total_ISR_Ajustado + ", " +
                                     Ope_Nom_Ajuste_ISR.Campo_No_Pago + "=" + No_Pago + ", " +
                                     Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR + "='" + Estatus_Ajuste_ISR + "'" +
                                     " WHERE " +
                                     Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + "='" + No_Ajuste_ISR + "'";

                            //Ejecutamos la consulta.
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                    }
                }

                //Validamos si algún registros de RECIBOS DE LA NÓMINA fueron afectados, si es hay registros afectados,
                //se procede a realizar los respectivos update a la tabla de RECIBOS DE LA NÓMINA.
                if (Dt_Recibos_Generados != null)
                {
                    foreach (DataRow Renglon in Dt_Recibos_Generados.Rows)
                    {
                        if (Renglon != null)
                        {
                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Recibos_Empleados.Campo_No_Recibo].ToString()))
                                No_Recibo = Renglon[Ope_Nom_Recibos_Empleados.Campo_No_Recibo].ToString().Trim();

                            //Eliminamos todos los registros que se crearon en la tabla de OPE_NOM_RECIBO_EMPLEADO_DET
                            Mi_SQL = "DELETE FROM " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det +
                                " WHERE " + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + "='" + No_Recibo + "'";
                            //Ejecutamosla consulta.
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                            //Eliminamos todos lo registros que se crearon en la tabla de OPE_NOM_RECIBOS_EMPLEADOS.
                            Mi_SQL = "DELETE FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +
                                     " WHERE " +
                                     Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "='" + No_Recibo + "'";

                            //Ejecutamosla consulta.
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                    }
                }

                //Validamos si algún registros de OPE_NOM_TOTALES_NOMINA fueron afectados, si es hay registros afectados,
                //se procede a realizar los respectivos update a la tabla de OPE_NOM_TOTALES_NOMINA.
                if (Dt_Totales_Nomina != null)
                {
                    foreach (DataRow Renglon in Dt_Totales_Nomina.Rows)
                    {
                        if (Renglon != null)
                        {
                            if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Totales_Nomina.Campo_No_Total_Nomina].ToString()))
                                No_Total_Nomina = Renglon[Ope_Nom_Totales_Nomina.Campo_No_Total_Nomina].ToString().Trim();

                            //Eliminamos el registro creado de la tabla de OPE_NOM_TOTALES_NOMINA. 
                            Mi_SQL = "DELETE FROM " + Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina +
                                " WHERE " + Ope_Nom_Totales_Nomina.Campo_No_Total_Nomina + "='" + No_Total_Nomina + "'";

                            //Ejecutamos la consulta.
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                    }
                }

                //Se hace Rollback de la información afectada en la tabla de Cat_Nom_Empl_Perc_Dedu_Deta 
                //que corresponde a registros de deducciones fijas que le aplican al empleado como descuento
                //por prestamo de algun proveedor.
                if (Dt_Deducciones_Fijas is DataTable)
                {
                    if (Dt_Deducciones_Fijas.Rows.Count > 0)
                    {
                        foreach (DataRow DEDUCCION in Dt_Deducciones_Fijas.Rows)
                        {
                            if (DEDUCCION is DataRow)
                            {
                                if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID].ToString().Trim()))
                                    Empleado_ID = DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                    Deduccion_ID = DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim()))
                                    Cantidad = Convert.ToDouble(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim());

                                if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim()))
                                    Importe = Convert.ToDouble(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim());

                                if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim()))
                                    Saldo = Convert.ToDouble(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim()))
                                    Cantidad_Retenida = Convert.ToDouble(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim());

                                if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID].ToString().Trim()))
                                    Str_Nomina_ID = DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina].ToString().Trim()))
                                    Int_No_Nomina = Convert.ToInt32(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina].ToString().Trim());

                                if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia].ToString().Trim()))
                                    if (!DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia].ToString().Trim().ToUpper().Equals("NULL"))
                                        Referencia = DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia].ToString().Trim();

                                Mi_SQL = "UPDATE " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det;
                                Mi_SQL += " SET ";
                                Mi_SQL += Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + "=" + Cantidad + ", ";
                                Mi_SQL += Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe + "=" + Importe + ", ";
                                Mi_SQL += Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo + "=" + Saldo + ", ";
                                Mi_SQL += Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida + "=" + Cantidad_Retenida + ", ";
                                Mi_SQL += Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + "='" + Str_Nomina_ID + "', ";
                                Mi_SQL += Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina + "=" + Int_No_Nomina + ", ";
                                Mi_SQL += Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia + "='" + Referencia + "'";
                                Mi_SQL += " WHERE ";
                                Mi_SQL += Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Empleado_ID + "'";
                                Mi_SQL += " AND ";
                                Mi_SQL += Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" + Deduccion_ID + "'";

                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            }
                        }
                    }
                }


            }
            catch (Exception Ex)
            {
                throw new Exception("Error al eliminar los registros que se modificaron al generar la nomina. Error: [" + Ex.Message + "]");
            }
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Consultar_Bienes_Resguardos_Empleado
        /// 
        /// DESCRIPCIÓN: Consulta si el empleado aun tiene algún Bien Resguardo por entregar o  si ya esta libre.
        ///              
        /// PARÁMETROS: Empleado_ID.- Identificador del Empleado, el cuál será consultado para validar que ya no tenga nada pendiente 
        ///                           por entregar.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 03/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        public DataTable Consultar_Bienes_Resguardos_Empleado(String Empleado_ID, String Dependencia_ID, String Tipo)
        {
            String Mi_SQL = "";//Variable que almacena la consulta.
            DataTable Dt_Bienes_Resguardos = null;//Lista de Bienes reesguardos que aun tiene el empleado.

            try
            {
                Mi_SQL = "SELECT " +
                            Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ".*" +
                         " FROM " +
                            Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos +
                         " WHERE " +
                            Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "='" + Empleado_ID + "'" +
                         " AND " +
                            Ope_Pat_Bienes_Resguardos.Campo_Estatus + "='VIGENTE'";

                Dt_Bienes_Resguardos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los resguardos que tiene el empleado actualmente bajo su responsabilidad. Error: [" + Ex.Message + "]");
            }
            return Dt_Bienes_Resguardos;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Deducciones_Fijas_Empleado
        /// 
        /// DESCRIPCION : Consulta las percepciones y/o deducciones fijas que le corresponden al empleado de acuerdo
        ///               al sindicato y  tipo de nomina al que pertence. 
        /// 
        /// PARAMETROS:  Empleado_Id.- Empleado a generar su nomina.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 16/Julio/2011 10:54 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public DataTable Consultar_Deducciones_Fijas_Empleado(String Empleado_Id, String Deduccion_ID)
        {
            String Mi_SQL = "";//Variable que almacenrá la consulta.
            DataTable Dt_Percepciones_Deducciones = null;//Variable que almacena una lista de percepciones deducciones, que le aplican ala empleado.

            try
            {
                //Paso I.- Se construyé la consulta.
                Mi_SQL = "SELECT " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe + ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo + ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida + ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina + ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia +
                         " FROM " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                         " WHERE " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Empleado_Id + "'" +
                         " AND " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" + Deduccion_ID + "'";

                //Paso II.- Ejecutamos la consulta.
                Dt_Percepciones_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
            return Dt_Percepciones_Deducciones;
        }
        #endregion

        #region (Percepcion y/o Deducciones Totales)
        ///********************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Plantilla_Total_Percepciones_Deducciones
        /// DESCRIPCION : Crea la tabla de totales.
        /// 
        /// PARAMETROS:  Nomina_ID.- Identificador del calendario de nómina a calcular.
        ///              Detalle_Nomina_ID.- Catorcena a generar la nómina.
        ///              No_Nomina.- Catorcena a generar.
        ///              Tipo_Nomina_ID.- Tipo de Nomina al que pertencé el empleado. 
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 4/Enero/2011 10:54 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public void Plantilla_Total_Percepciones_Deducciones()
        {
            DataTable Dt_Totales = new DataTable();
            DataTable Dt_Percepciones = null;//Variable que almacenará una lista de percepciones del empleado.
            Cls_Cat_Nom_Percepciones_Deducciones_Business Cls_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocios.

            try
            {
                //Paso I.- Creamos las columnas de realcion de la tabla de [OPE_NOM_TOTALES_NOMINA]. 
                Dt_Totales.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Nomina_ID, typeof(String));
                Dt_Totales.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID, typeof(String));
                Dt_Totales.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_No_Nomina, typeof(Int32));
                Dt_Totales.Columns.Add(Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID, typeof(String));

                //Paso II.- Creamos una columna para cada percepcion.
                Cls_Percepciones_Deducciones.P_TIPO = "PERCEPCION";
                Dt_Percepciones = Cls_Percepciones_Deducciones.Consulta_Percepciones_Deducciones();
                if (Dt_Percepciones != null)
                {
                    foreach (DataRow Renglon in Dt_Percepciones.Rows)
                    {
                        Dt_Totales.Columns.Add("SUMA_" + Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString(), typeof(Double));
                    }
                }
                //Paso III.- Creamos una columna para almacenar el total de percepciones que hubo en la nomina actual.
                Dt_Totales.Columns.Add("TOTAL_PERCEPCIONES", typeof(Double));

                //Paso IV.- Creamos una columna para cada deduccion.
                Cls_Percepciones_Deducciones.P_TIPO = "DEDUCCION";
                Dt_Percepciones = Cls_Percepciones_Deducciones.Consulta_Percepciones_Deducciones();
                if (Dt_Percepciones != null)
                {
                    foreach (DataRow Renglon in Dt_Percepciones.Rows)
                    {
                        Dt_Totales.Columns.Add("SUMA_" + Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString(), typeof(Double));
                    }

                    //Paso V.- Creamos una columna para almacenar el total de deducciones que hubo en el calculo de la nomina actual.
                    Dt_Totales.Columns.Add("TOTAL_DEDUCCIONES", typeof(Double));
                }
                //Paso VI.- Agregamos los datos de Nomina_ID, Detalle_Nomina_ID, No_Nomina, Tipo_Nomina_ID 
                DataRow Renglon_Totales = Dt_Totales.NewRow();
                Renglon_Totales[Cat_Nom_Nominas_Detalles.Campo_Nomina_ID] = Nomina_ID;
                Renglon_Totales[Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID] = Detalle_Nomina_ID;
                Renglon_Totales[Cat_Nom_Nominas_Detalles.Campo_No_Nomina] = No_Nomina;
                Renglon_Totales[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID] = Tipo_Nomina_ID;
                Dt_Totales.Rows.Add(Renglon_Totales);
                //Paso VII.- Establecemos la variable de session que almacenará los totales de Percepciones y Deducciones.
                Cls_Sessiones.Totales_Percepciones_Deducciones = Dt_Totales;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las la tabla de [CAT_NOM_PERCEPCION_DEDUCCION]. Error: [" + Ex.Message + "]");
            }
        }
        ///********************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Totales_Nomina
        /// DESCRIPCION : Obtiene las cantidades de cada percepción y/o deducción de la catorcena de la cual se genero la nómina.
        /// 
        /// PARAMETROS:  Percepciones_Deducciones.- Lista de percepciones y/o deducciones que se han generado en la catorcena actual.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 21/Enero/2011 10:54 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public void Obtener_Totales_Nomina(DataTable Percepciones_Deducciones)
        {
            DataTable Dt_Columnas_Ope_Nom_Totales_Nomina = null;//Listara los campos de la tabla de Ope_Nom_Totales_Nomina.
            DataTable Dt_Totales_Nomina = Sessiones.Cls_Sessiones.Totales_Percepciones_Deducciones;//Obtenemos la table de totales.
            Boolean Salir = false;//Validara que la percepcion exista.
            String Percepcion_Deduccion_ID = "";//Variable que almacena el identificador del concepto.
            Double Monto = 0.0;

            try
            {
                Dt_Columnas_Ope_Nom_Totales_Nomina = Consultar_Columnas_Tabla_Ope_Nom_Totales_Nomina();

                if (Dt_Columnas_Ope_Nom_Totales_Nomina != null)
                {
                    foreach (DataRow Percepcion_Deduccion in Percepciones_Deducciones.Rows)
                    {
                        if (Percepcion_Deduccion != null)
                        {
                            foreach (DataRow Renglon in Dt_Columnas_Ope_Nom_Totales_Nomina.Rows)
                            {
                                if (!string.IsNullOrEmpty(Percepcion_Deduccion["Percepcion_Deduccion"].ToString()))
                                {
                                    Percepcion_Deduccion_ID = Percepcion_Deduccion["Percepcion_Deduccion"].ToString();

                                    Salir = (Renglon[0].ToString().Equals("SUMA_" + Percepcion_Deduccion_ID)) ? true : false;

                                    if (Salir)
                                    {
                                        Double Cantidad = Convert.ToDouble((!string.IsNullOrEmpty(Dt_Totales_Nomina.Rows[0]["SUMA_" + Percepcion_Deduccion_ID].ToString())) ? Dt_Totales_Nomina.Rows[0]["SUMA_" + Percepcion_Deduccion_ID].ToString() : "0");

                                        if (!string.IsNullOrEmpty(Percepcion_Deduccion["Monto"].ToString()))
                                            Monto = Convert.ToDouble(Percepcion_Deduccion["Monto"].ToString());

                                        Dt_Totales_Nomina.Rows[0]["SUMA_" + Percepcion_Deduccion_ID] = Cantidad + Monto;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                Sessiones.Cls_Sessiones.Totales_Percepciones_Deducciones = Dt_Totales_Nomina;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error obtener los Totales de la Nómina. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (ISR o Subsidio)
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Identificar_Percepcion_Dedeuccion
        /// DESCRIPCIÓN: Identifica si la Percepción y/o Deducción corresponde a una Retención de ISR o a un Subsidio para el
        ///              el Empleado.
        /// 
        /// PARÁMETROS: Lista_Percepciones_Deducciones .- Lista de Percepciones y/o Deducciones donde se realizara la búsqueda
        ///                                               de la Retención de ISR o Subsidio para el empleado.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 27/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private Cls_Percepciones_Deducciones Identificar_Percepcion_Dedeuccion(List<Cls_Percepciones_Deducciones> Lista_Percepciones_Deducciones)
        {
            Cls_Cat_Nom_Parametros_Negocio Informacion_Parametros = new Cls_Cat_Nom_Parametros_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Percepciones_Deducciones Percepcion_Deduccion_Identifacada = null;//Variable que almacenará la Percepción y/o Deducción que se desea identicar. 
            DataTable Dt_Parametros_Nomina = null;//Variable que almacenrá el parámetro de la nómina.
            String Percepcion_Subsidio = "";//Variable que almacenrá el identicador de la Percepcion que le corresponde al subsidio.
            String Deduccion_ISR = "";//Variable que almacenrá el identicador de la Deducción que le corresponde al ISR.
            try
            {
                //Paso II.- Consulta de los parámetros de la nómina.
                Dt_Parametros_Nomina = Informacion_Parametros.Consulta_Parametros();
                if (Dt_Parametros_Nomina != null)
                {
                    if (Dt_Parametros_Nomina.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Parametros_Nomina.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString()))
                            Percepcion_Subsidio = Dt_Parametros_Nomina.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString();
                        if (!string.IsNullOrEmpty(Dt_Parametros_Nomina.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISR].ToString()))
                            Deduccion_ISR = Dt_Parametros_Nomina.Rows[0][Cat_Nom_Parametros.Campo_Deduccion_ISR].ToString();
                    }
                }

                //Recorremos la lista, para buscar la que corresponde con el identicador en la tabla de [CAT_NOM_PARAMETROS], que le corresponde al subsidio. 
                foreach (Cls_Percepciones_Deducciones Percepcion_Dedeuccion in Lista_Percepciones_Deducciones)
                {
                    if (Percepcion_Dedeuccion != null)
                    {

                        if (Percepcion_Dedeuccion.P_Clabe.Equals(Percepcion_Subsidio))
                        {
                            Percepcion_Deduccion_Identifacada = Percepcion_Dedeuccion;
                        }
                        else if (Percepcion_Dedeuccion.P_Clabe.Equals(Deduccion_ISR))
                        {
                            Percepcion_Deduccion_Identifacada = Percepcion_Dedeuccion;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Identificar la percepcion y/o deduccion. Error: [" + Ex.Message + "]");
            }
            return Percepcion_Deduccion_Identifacada;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Obtener_ISR_Cas_Empleado
        /// DESCRIPCIÓN: En base al cálculo realizado de ISR de acuerdo al tipo de nómina al que pertenece el empleado.
        ///              se obtiene ya sea la Retención al Empleado de ISR o si la cantidad de ISPT es menor a cero se
        ///              convierte en un subsidio para el empleado.
        /// 
        /// PARÁMETROS: Deduccion_ISR_Empleado .- Deducción que corresponde a la deduccion de Retención de ISR.
        ///             Percepcion_Subsidio_Empleado.- Percepcion que corresponde a la Percepcion de Subsidio del empleado.
        ///             Empleado_ID.- Identificador del empleado sobre el que se aplicara la Retención de ISR o el Subsidio del empleado.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 27/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private List<Cls_Percepciones_Deducciones> Obtener_ISR_Cas_Empleado(Cls_Percepciones_Deducciones Deduccion_ISR_Empleado,
                                   Cls_Percepciones_Deducciones Percepcion_Subsidio_Empleado, String Empleado_ID)
        {
            List<Cls_Percepciones_Deducciones> Lista_Retencion_ISR_Subsidio = new List<Cls_Percepciones_Deducciones>();
            Cls_Percepciones_Deducciones Percepcion_Subsidio = Percepcion_Subsidio_Empleado;
            Cls_Percepciones_Deducciones Deduccion_ISR = Deduccion_ISR_Empleado;
            DataTable Dt_Resultado_Calculo_ISR = null;//Almacenará el resultado de la operación del cálculo de ISR.
            Double Cas_Empleado = 0.0;//Cantidad que se aportara al empleado como subsidio.
            Double ISR = 0.0;//Cantidad de ISR a retener al empleado.

            try
            {
                if ((Deduccion_ISR != null) && (Percepcion_Subsidio_Empleado != null))
                {
                    Percepcion_Subsidio.P_Es_ISR_Subsidio = "PERCEPCION";
                    Deduccion_ISR.P_Es_ISR_Subsidio = "DEDUCCION";

                    //Paso I.- Se realiza el cálculo de ISR. Por tipo de nómina al que pertence el empleado.
                    //Dt_Resultado_Calculo_ISR = Calcular_ISR_Tipo_Nomina(Empleado_ID);
                    Dt_Resultado_Calculo_ISR = Calcular_ISR_Finiquito(Empleado_ID);//Linea agregada para el calculo del nuevo finiquito.

                    //Paso II.- Se realiza la validación del resultado obtenido.
                    if (Dt_Resultado_Calculo_ISR != null)
                    {
                        if (Dt_Resultado_Calculo_ISR.Rows.Count > 0)
                        {
                            //Paso III.- Se valida si el resultado corresponde a una retención para el empleado o una percepción para el mismo.
                            if (((Boolean)Dt_Resultado_Calculo_ISR.Rows[0]["Percepcion_Deduccion"]))
                            {
                                //Se convierte en un subsidio para el empleado.
                                Cas_Empleado = Convert.ToDouble(Dt_Resultado_Calculo_ISR.Rows[0]["Cantidad"].ToString());
                                Percepcion_Subsidio.P_Monto = Cas_Empleado;
                            }
                            else
                            {
                                //La cantidad que se le rentendra al empleado como concepto de ISR.
                                ISR = Convert.ToDouble(Dt_Resultado_Calculo_ISR.Rows[0]["Cantidad"].ToString());
                                Deduccion_ISR.P_Monto = ISR;
                            }

                            Lista_Retencion_ISR_Subsidio.Add(Deduccion_ISR);
                            Lista_Retencion_ISR_Subsidio.Add(Percepcion_Subsidio);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al validar cual percepcion pertencé al subsidio para el empleado o auna retencion de ISR. Error:[" + Ex.Message + "]");
            }
            return Lista_Retencion_ISR_Subsidio;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Calcular_ISR_Tipo_Nomina
        /// DESCRIPCIÓN: Se consulta al empleado para obtener el tipo de nómina a la que pertenece el empleado y de acuerdo al Tipo de Nómina
        ///              se realiza el cálculo de ISR. Y retorno un resultado [true] si se trata de un subsidio para el empelado ó [false] si 
        ///              se trata de una retención al empleado.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del empleado sobre el que se aplicara la Retención de ISR o el Subsidio del empleado.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 27/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private DataTable Calcular_ISR_Tipo_Nomina(String Empleado_ID)
        {
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Cat_Empleados_Negocios Informacion_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
            DataTable Dt_ISR = null;//Estructura que almacena el resultado del cálculo de ISR que se obtuvo de llamar al método en la clase de [Ope_Nom_Deducciones_Negocios].  
            DataTable Dt_Resultado = new DataTable();//Variable que almacenará el resultado. Del cálculo realizado de ISR. 
            DataRow Renglon_Resultado = null;//Variable que guarda el resultado del cálculo de ISR y que será insertada al DataTable [Dt_Resultado].
            Double Cantidad = 0.0;//Cantidad de ISR a retener o Cas_Empleo_Causado
            Double Cas_Empleado = 0.0;//Variable que almacena la cantidad de Subsidio para el empleado.
            Double ISR = 0.0;//Variable que almacenará la cantidad de ISR a retener al empleado.
            Boolean Is_Subsidio = false;//Variable que válida si el resultado del cálculo es un subsidio para el empleado. 
            Boolean Is_ISR = false;//Variable que válida si el resultado del cálculo es una retención para el empleado. 

            try
            {
                //PASO I.- Consulta de la información del empleado.
                Informacion_Empleado.P_Empleado_ID = Empleado_ID;
                Consultar_Informacion_Empleado(ref Informacion_Empleado);

                //Establecemos las Cantidades Totales en Percepciones y/o Deducciones.
                Calculo_Deducciones.Total_Percepciones = Total_Percepciones;
                Calculo_Deducciones.Total_Deducciones = Total_Deducciones;
                Calculo_Deducciones.Ingresos_Gravables_Empleado = Total_Ingresos_Gravables_Empleado;
                Calculo_Deducciones.Gravable_Aguinaldo = Total_Grava_Aguinaldo;
                Calculo_Deducciones.Gravable_Prima_Vacacional = Total_Grava_Prima_Vacacional;
                Calculo_Deducciones.Gravable_Prima_Antiguedad = Total_Grava_Prima_Antiguedad;
                Calculo_Deducciones.Gravable_Indemnizacion = Total_Grava_Indemnizacion;
                Calculo_Deducciones.Exenta_Prima_Antiguedad = Total_Exenta_Prima_Antiguedad;
                Calculo_Deducciones.Exenta_Indemnizacion = Total_Exenta_Indemnizacion;
                Calculo_Deducciones.Gravable_Dias_Festivos = Total_Grava_Dias_Festivos;
                Calculo_Deducciones.Gravable_Tiempo_Extra = Total_Grava_Tiempo_Extra;
                Calculo_Deducciones.Exenta_Tiempo_Extra = Total_Exenta_Tiempo_Extra;
                Calculo_Deducciones.Exenta_Dias_Festivos = Total_Exenta_Dias_Festivos;
                Calculo_Deducciones.Gravable_Sueldo = Total_Grava_Sueldo;

                Calculo_Deducciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;
                //Generamos la estructura de la tabla que almacenrá los resultados de los cálculos realizados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));

                switch (Informacion_Empleado.P_Tipo_Nomina_ID)
                {
                    case "00001":
                        ///***********************************************************************************************************************
                        ///**************************************** ÁPLICA CÁLCULO ISR NÓMINA BASE ***********************************************
                        ///***********************************************************************************************************************
                        if (Aplica_Calculo_ISR(Empleado_ID))
                        {
                            Dt_ISR = Calculo_Deducciones.Calcular_ISR(Empleado_ID);
                            if (Dt_ISR != null)
                            {
                                if (Dt_ISR.Rows.Count > 0)
                                {
                                    if (((Boolean)Dt_ISR.Rows[0]["Percepcion_Deduccion"]))
                                    {
                                        Is_Subsidio = true;
                                        Is_ISR = false;
                                        Cas_Empleado = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                    else
                                    {
                                        Is_Subsidio = false;
                                        Is_ISR = true;
                                        ISR = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                }
                            }
                        }//Aplica ISR.
                        break;
                    case "00002":
                        ///****************************************************************************************************************************
                        ///**************************************** ÁPLICA CÁLCULO ISR NÓMINA EVENTUALES **********************************************
                        ///****************************************************************************************************************************
                        if (Aplica_Calculo_ISR(Empleado_ID))
                        {
                            Dt_ISR = Calculo_Deducciones.Calcular_ISR(Empleado_ID);
                            if (Dt_ISR != null)
                            {
                                if (Dt_ISR.Rows.Count > 0)
                                {
                                    if (((Boolean)Dt_ISR.Rows[0]["Percepcion_Deduccion"]))
                                    {
                                        Is_Subsidio = true;
                                        Is_ISR = false;
                                        Cas_Empleado = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                    else
                                    {
                                        Is_Subsidio = false;
                                        Is_ISR = true;
                                        ISR = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                }
                            }
                        }//Aplica ISR.
                        break;
                    case "00003":
                        ///*************************************************************************************************************************
                        ///************************************ NO ÁPLICA CÁLCULO ISR NÓMINA ASIMILABES ********************************************
                        ///*************************************************************************************************************************
                        if (Aplica_Calculo_ISR(Empleado_ID))
                        {
                            Dt_ISR = Calculo_Deducciones.Calcular_ISPT_Nomina_Asimilables(Empleado_ID);
                            if (Dt_ISR != null)
                            {
                                if (Dt_ISR.Rows.Count > 0)
                                {
                                    if (((Boolean)Dt_ISR.Rows[0]["Percepcion_Deduccion"]))
                                    {
                                        Is_Subsidio = true;
                                        Is_ISR = false;
                                        Cas_Empleado = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                    else
                                    {
                                        Is_Subsidio = false;
                                        Is_ISR = true;
                                        ISR = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                }
                            }
                        }//Aplica ISR.  
                        break;
                    case "00004":
                        ///*************************************************************************************************************************
                        ///************************************ ÁPLICA CÁLCULO ISR NÓMINA SUBROGADOS ***********************************************
                        ///*************************************************************************************************************************
                        if (Aplica_Calculo_ISR(Empleado_ID))
                        {
                            Dt_ISR = Calculo_Deducciones.Calcular_ISPT_Subrogados(Empleado_ID);
                            if (Dt_ISR != null)
                            {
                                if (Dt_ISR.Rows.Count > 0)
                                {
                                    if (((Boolean)Dt_ISR.Rows[0]["Percepcion_Deduccion"]))
                                    {
                                        Is_Subsidio = true;
                                        Is_ISR = false;
                                        Cas_Empleado = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                    else
                                    {
                                        Is_Subsidio = false;
                                        Is_ISR = true;
                                        ISR = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                }
                            }
                        }//Aplica ISR.                    
                        break;
                    case "00005":
                        ///******************************************************************************************************************
                        ///****************************** ÁPLICA CÁLCULO ISR NÓMINA JUBILADOS ***********************************************
                        ///******************************************************************************************************************
                        if (Aplica_Calculo_ISR(Empleado_ID))
                        {
                            Dt_ISR = Calculo_Deducciones.Calcular_ISPT_Pensionados(Empleado_ID);
                            if (Dt_ISR != null)
                            {
                                if (Dt_ISR.Rows.Count > 0)
                                {
                                    if (((Boolean)Dt_ISR.Rows[0]["Percepcion_Deduccion"]))
                                    {
                                        Is_Subsidio = true;
                                        Is_ISR = false;
                                        Cas_Empleado = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                    else
                                    {
                                        Is_Subsidio = false;
                                        Is_ISR = true;
                                        ISR = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                }
                            }
                        }//Aplica ISR.  
                        break;
                    case "00009":
                        ///******************************************************************************************************************
                        ///****************************** ÁPLICA CÁLCULO ISR NÓMINA SUBSEMUN ************************************************
                        ///******************************************************************************************************************
                        if (Aplica_Calculo_ISR(Empleado_ID))
                        {
                            Dt_ISR = Calculo_Deducciones.Calcular_ISR(Empleado_ID);
                            if (Dt_ISR != null)
                            {
                                if (Dt_ISR.Rows.Count > 0)
                                {
                                    if (((Boolean)Dt_ISR.Rows[0]["Percepcion_Deduccion"]))
                                    {
                                        Is_Subsidio = true;
                                        Is_ISR = false;
                                        Cas_Empleado = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                    else
                                    {
                                        Is_Subsidio = false;
                                        Is_ISR = true;
                                        ISR = Convert.ToDouble(Dt_ISR.Rows[0]["Cantidad"].ToString());
                                    }
                                }
                            }
                        }//Aplica ISR.  
                        break;
                    default:
                        break;
                }

                if (Is_Subsidio)
                {
                    Renglon_Resultado = Dt_Resultado.NewRow();
                    Renglon_Resultado["Percepcion_Deduccion"] = true;
                    Renglon_Resultado["Cantidad"] = Cas_Empleado;
                    Dt_Resultado.Rows.Add(Renglon_Resultado);
                }
                else if (Is_ISR)
                {
                    Renglon_Resultado = Dt_Resultado.NewRow();
                    Renglon_Resultado["Percepcion_Deduccion"] = false;
                    Renglon_Resultado["Cantidad"] = ISR;
                    Dt_Resultado.Rows.Add(Renglon_Resultado);
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el calculo de ISR. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*****************************************************************************************************************************
        /// NOMBRE MÉTODO: Calcular_ISR_Finiquito
        /// DESCRIPCIÓN: Se consulta al empleado para obtener el tipo de nómina a la que pertenece el empleado y de acuerdo al Tipo de Nómina
        ///              se realiza el cálculo de ISR. Y retorno un resultado [true] si se trata de un subsidio para el empelado ó [false] si 
        ///              se trata de una retención al empleado.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del empleado sobre el que se aplicara la Retención de ISR o el Subsidio del empleado.
        ///             
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 27/Enero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///*****************************************************************************************************************************
        private DataTable Calcular_ISR_Finiquito(String Empleado_ID)
        {
            Cls_Ope_Nom_Deducciones_Negocio Calculo_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Cat_Empleados_Negocios Informacion_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
            DataTable Dt_ISR = null;//Estructura que almacena el resultado del cálculo de ISR que se obtuvo de llamar al método en la clase de [Ope_Nom_Deducciones_Negocios].  
            DataTable Dt_Resultado = new DataTable();//Variable que almacenará el resultado. Del cálculo realizado de ISR. 
            DataRow Renglon_Resultado = null;//Variable que guarda el resultado del cálculo de ISR y que será insertada al DataTable [Dt_Resultado].
            Double Cantidad = 0.0;//Cantidad de ISR a retener o Cas_Empleo_Causado
            Double Cas_Empleado = 0.0;//Variable que almacena la cantidad de Subsidio para el empleado.
            Double ISR = 0.0;//Variable que almacenará la cantidad de ISR a retener al empleado.
            Boolean Is_Subsidio = false;//Variable que válida si el resultado del cálculo es un subsidio para el empleado. 
            Boolean Is_ISR = false;//Variable que válida si el resultado del cálculo es una retención para el empleado. 

            try
            {
                //PASO I.- Consulta de la información del empleado.
                Informacion_Empleado.P_Empleado_ID = Empleado_ID;
                Consultar_Informacion_Empleado(ref Informacion_Empleado);

                //Establecemos algunos valores para poder realizar los cálculos de las deducciones.
                Calculo_Deducciones.P_Nomina_ID = Nomina_ID;//Nomina actual.
                Calculo_Deducciones.P_No_Nomina = No_Nomina;//Periodo nominal actual.
                Calculo_Deducciones.P_Detalle_Nomina_ID = Detalle_Nomina_ID;//Identificador del periodo actual.
                Calculo_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;//Tipo de nómina al que pertenece el empleado.

                //Establecemos las Cantidades Totales en Percepciones y/o Deducciones.
                Calculo_Deducciones.Total_Percepciones = Total_Percepciones;
                Calculo_Deducciones.Total_Deducciones = Total_Deducciones;
                Calculo_Deducciones.Ingresos_Gravables_Empleado = Total_Ingresos_Gravables_Empleado;
                Calculo_Deducciones.Gravable_Aguinaldo = Total_Grava_Aguinaldo;
                Calculo_Deducciones.Gravable_Prima_Vacacional = Total_Grava_Prima_Vacacional;
                Calculo_Deducciones.Gravable_Prima_Antiguedad = Total_Grava_Prima_Antiguedad;
                Calculo_Deducciones.Gravable_Indemnizacion = Total_Grava_Indemnizacion;
                Calculo_Deducciones.Exenta_Prima_Antiguedad = Total_Exenta_Prima_Antiguedad;
                Calculo_Deducciones.Exenta_Indemnizacion = Total_Exenta_Indemnizacion;
                Calculo_Deducciones.Gravable_Dias_Festivos = Total_Grava_Dias_Festivos;
                Calculo_Deducciones.Gravable_Tiempo_Extra = Total_Grava_Tiempo_Extra;
                Calculo_Deducciones.Exenta_Tiempo_Extra = Total_Exenta_Tiempo_Extra;
                Calculo_Deducciones.Exenta_Dias_Festivos = Total_Exenta_Dias_Festivos;
                Calculo_Deducciones.Gravable_Sueldo = Total_Grava_Sueldo;

                Calculo_Deducciones.Fecha_Generar_Nomina = Fecha_Catorcena_Generar_Nomina;
                //Generamos la estructura de la tabla que almacenrá los resultados de los cálculos realizados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));

                DataTable Dt_Auxiliar = Calculo_Deducciones.Calcular_ISR_Total(Empleado_ID);

                if (Dt_Auxiliar is DataTable)
                {
                    if (Dt_Auxiliar.Rows.Count > 0)
                    {
                        foreach (DataRow FILA in Dt_Auxiliar.Rows)
                        {
                            if (!String.IsNullOrEmpty(FILA["Percepcion_Deduccion"].ToString().Trim()))
                            {
                                if (FILA["Percepcion_Deduccion"].ToString().Trim().ToUpper().Equals("true"))
                                {
                                    Is_Subsidio = true;
                                    if (!String.IsNullOrEmpty(FILA["Cantidad"].ToString().Trim()))
                                        Cas_Empleado = Convert.ToDouble(FILA["Cantidad"].ToString().Trim());
                                }
                                else
                                {
                                    Is_ISR = true;
                                    if (!String.IsNullOrEmpty(FILA["Cantidad"].ToString().Trim()))
                                        ISR = Convert.ToDouble(FILA["Cantidad"].ToString().Trim());
                                }

                                if (Is_Subsidio)
                                {
                                    Renglon_Resultado = Dt_Resultado.NewRow();
                                    Renglon_Resultado["Percepcion_Deduccion"] = true;
                                    Renglon_Resultado["Cantidad"] = Cas_Empleado;
                                    Dt_Resultado.Rows.Add(Renglon_Resultado);
                                }
                                else if (Is_ISR)
                                {
                                    Renglon_Resultado = Dt_Resultado.NewRow();
                                    Renglon_Resultado["Percepcion_Deduccion"] = false;
                                    Renglon_Resultado["Cantidad"] = ISR;
                                    Dt_Resultado.Rows.Add(Renglon_Resultado);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el calculo de ISR. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        #endregion


        #region (Actualizar Deducciones Proveedor)
        /// ************************************************************************************************************************
        /// Nombre: Actualizar_Saldos_Deducciones_Fijas_Proveedor
        /// 
        /// Descripción: Recibe como parámetro una lista de deducciones la cuál se recorre para identificar si la deducción
        ///              corresponde algun prestamo de algún proveedor y reaalizar la actualización de la información.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado.
        ///             Dt_Deducciones_Fijas_Proveedor.- Deduccciones fijas que le aplican al empleado por el sindicato al que
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 19/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        //// ************************************************************************************************************************
        public void Actualizar_Saldos_Deducciones_Fijas_Proveedor(String No_Empleado, DataTable Dt_Deducciones_Fijas_Proveedor)
        {
            Cls_Cat_Nom_Percepciones_Deducciones_Business Object_Filters = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
            Cls_Cat_Nom_Percepciones_Deducciones_Business INF_PERCEPCION_DEDUCCION = null;
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
            DataTable Dt_Deducciones_Fijas_Empleado = null;
            String Deduccion_ID = String.Empty;
            Double Cantidad = 0.0;
            Double Importe = 0.0;
            Double Saldo = 0.0;
            Double Cantidad_Retenida = 0.0;
            DataTable Dt_Reporte_Deducciones_Actualizadas = new DataTable();
            StringBuilder Historial_Nomina_Generada = new StringBuilder();

            try
            {
                INF_EMPLEADO = Cls_Ayudante_Nom_Informacion._Informacion_Empleado(No_Empleado);

                if (Dt_Deducciones_Fijas_Proveedor is DataTable)
                {
                    if (Dt_Deducciones_Fijas_Proveedor.Rows.Count > 0)
                    {
                        foreach (DataRow PERCEPCION_DEDUCCION in Dt_Deducciones_Fijas_Proveedor.Rows) {
                            if (PERCEPCION_DEDUCCION is DataRow) {
                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION["Percepcion_Deduccion"].ToString())) {

                                    Object_Filters.P_PERCEPCION_DEDUCCION_ID = PERCEPCION_DEDUCCION["Percepcion_Deduccion"].ToString();
                                    Object_Filters.P_TIPO = "DEDUCCION";
                                    Object_Filters.P_TIPO_ASIGNACION = "FIJA";
                                    INF_PERCEPCION_DEDUCCION = Cls_Ayudante_Nom_Informacion._Informacion_Percepcion_Deduccion(Object_Filters);

                                    Deduccion_ID = INF_PERCEPCION_DEDUCCION.P_PERCEPCION_DEDUCCION_ID;
                                    Dt_Deducciones_Fijas_Empleado = Consultar_Deducciones_Fijas_Empleado(INF_EMPLEADO.P_Empleado_ID, Deduccion_ID);

                                    if (Dt_Reporte_Deducciones_Actualizadas.Rows.Count <= 0)
                                        Dt_Reporte_Deducciones_Actualizadas = Dt_Deducciones_Fijas_Empleado.Clone();

                                    if (Dt_Deducciones_Fijas_Empleado is DataTable)
                                    {
                                        if (Dt_Deducciones_Fijas_Empleado.Rows.Count > 0)
                                        {
                                            foreach (DataRow Dr_DEDUCCION in Dt_Deducciones_Fijas_Empleado.Rows)
                                            {
                                                if (Dr_DEDUCCION is DataRow)
                                                {

                                                    Cantidad = 0.0;
                                                    Importe = 0.0;
                                                    Saldo = 0.0;
                                                    Cantidad_Retenida = 0.0;

                                                    if (!String.IsNullOrEmpty(Dr_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim()))
                                                        Cantidad = Convert.ToDouble(Dr_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim());

                                                    if (!String.IsNullOrEmpty(Dr_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim()))
                                                        Importe = Convert.ToDouble(Dr_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim());

                                                    if (!String.IsNullOrEmpty(Dr_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim()))
                                                        Saldo = Convert.ToDouble(Dr_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim());

                                                    if (!String.IsNullOrEmpty(Dr_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim()))
                                                        Cantidad_Retenida = Convert.ToDouble(Dr_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim());

                                                    if ((Saldo >= Cantidad) && (Cantidad != 0))
                                                    {
                                                        Dt_Reporte_Deducciones_Actualizadas.ImportRow(Dr_DEDUCCION);

                                                        Saldo -= Cantidad;
                                                        Cantidad_Retenida += Cantidad;

                                                        //if ((Saldo == 0) && (Cantidad_Retenida == Importe))
                                                        //{
                                                        Ejecutar_Actualizacion_Saldos_Finiquito(INF_EMPLEADO.P_Empleado_ID, Deduccion_ID);
                                                        //}
                                                        //else
                                                        //{
                                                        //    Ejecutar_Actualizacion_Saldos(INF_EMPLEADO.P_Empleado_ID, Deduccion_ID, Saldo, Cantidad_Retenida);
                                                        //}
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                Historial_Nomina_Generada = Cls_Sessiones.Historial_Nomina_Generada;
                Cls_Historial_Nomina_Generada.Crear_Registro_Insertar_Deducciones_Fijas(Dt_Reporte_Deducciones_Actualizadas, ref Historial_Nomina_Generada);
                Cls_Sessiones.Historial_Nomina_Generada = Historial_Nomina_Generada;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al actualizar los saldos de las deducciones por proveedor. Error: [" + Ex.Message + "]");
            }
        }
        /// ************************************************************************************************************************
        /// Nombre: Ejecutar_Actualizacion_Saldos
        /// 
        /// Descripción: Ejecuta la actualización de el saldo y de la cantidad retenida al empleado por concepto 
        ///              de alguna deducción que le aplica al empleado como pago algun proveedor.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado.
        ///             Deduccion_ID.- Indica la deduccion que se actualizara al empelado.
        ///             Nuevo_Saldo.- Indica el nuevo saldo que se le actualizara a la deduccion.
        ///             Nueva_Cantidad_Retenida. -Indica la nueva cantidad que se le a retenido al empleado.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 19/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        //// ************************************************************************************************************************
        protected void Ejecutar_Actualizacion_Saldos(String Empleado_ID, String Deduccion_ID,
            Double Nuevo_Saldo, Double Nueva_Cantidad_Retenida)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append("UPDATE ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det);
                Mi_SQL.Append(" SET ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo + "=" + Nuevo_Saldo + ", ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida + "=" + Nueva_Cantidad_Retenida);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" + Deduccion_ID + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la actualización de saldos. Error: [" + Ex.Message + "]");
            }
        }
        /// ************************************************************************************************************************
        /// Nombre: Ejecutar_Actualizacion_Saldos_Finiquito
        /// 
        /// Descripción: Ejecuta la actualización de la deducción cuando se realiza el finiquito de la misma.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado.
        ///             Deduccion_ID.- Deduccion la cual se le finiquitara al Empleado.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 19/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        //// ************************************************************************************************************************
        protected void Ejecutar_Actualizacion_Saldos_Finiquito(String Empleado_ID, String Deduccion_ID)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append("UPDATE ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det);
                Mi_SQL.Append(" SET ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + "=0, ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe + "=0, ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo + "=0, ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida + "=0, ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + "=NULL, ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina + "=NULL, ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia + "=NULL");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" + Deduccion_ID + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la actualización de saldos. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Consulta Información)
        /// ************************************************************************************************************************
        /// Nombre: _Informacion_Empleado
        /// 
        /// Descripción: Consulta la información del empleado.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 23/Septiembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ************************************************************************************************************************
        private Cls_Cat_Empleados_Negocios _Informacion_Empleado(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();
            Cls_Cat_Empleados_Negocios Obj_Emplados = new Cls_Cat_Empleados_Negocios();
            DataTable Dt_Empleados = null;

            try
            {
                Obj_Emplados.P_Empleado_ID = Empleado_ID;
                Dt_Empleados = Obj_Emplados.Consulta_Empleados_General();

                if (Dt_Empleados is DataTable)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleados.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString()))
                                    INF_EMPLEADO.P_Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString()))
                                    INF_EMPLEADO.P_Dependencia_ID = EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Contrato_ID].ToString()))
                                    INF_EMPLEADO.P_Tipo_Contrato_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Contrato_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Contrato_ID].ToString()))
                                    INF_EMPLEADO.P_Tipo_Contrato_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Contrato_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Puesto_ID].ToString()))
                                    INF_EMPLEADO.P_Puesto_ID = EMPLEADO[Cat_Empleados.Campo_Puesto_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Escolaridad_ID].ToString()))
                                    INF_EMPLEADO.P_Escolaridad_ID = EMPLEADO[Cat_Empleados.Campo_Escolaridad_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Sindicato_ID].ToString()))
                                    INF_EMPLEADO.P_Sindicado_ID = EMPLEADO[Cat_Empleados.Campo_Sindicato_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Turno_ID].ToString()))
                                    INF_EMPLEADO.P_Turno_ID = EMPLEADO[Cat_Empleados.Campo_Turno_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString()))
                                    INF_EMPLEADO.P_Zona_ID = EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Trabajador_ID].ToString()))
                                    INF_EMPLEADO.P_Tipo_Trabajador_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Trabajador_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Rol_ID].ToString()))
                                    INF_EMPLEADO.P_Rol_ID = EMPLEADO[Cat_Empleados.Campo_Rol_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString()))
                                    INF_EMPLEADO.P_No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Password].ToString()))
                                    INF_EMPLEADO.P_Password = EMPLEADO[Cat_Empleados.Campo_Password].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Apellido_Paterno].ToString()))
                                    INF_EMPLEADO.P_Apellido_Paterno = EMPLEADO[Cat_Empleados.Campo_Apellido_Paterno].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Apellido_Materno].ToString()))
                                    INF_EMPLEADO.P_Apelldo_Materno = EMPLEADO[Cat_Empleados.Campo_Apellido_Materno].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Nombre].ToString()))
                                    INF_EMPLEADO.P_Nombre = EMPLEADO[Cat_Empleados.Campo_Nombre].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Calle].ToString()))
                                    INF_EMPLEADO.P_Calle = EMPLEADO[Cat_Empleados.Campo_Calle].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Colonia].ToString()))
                                    INF_EMPLEADO.P_Colonia = EMPLEADO[Cat_Empleados.Campo_Colonia].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Codigo_Postal].ToString()))
                                    INF_EMPLEADO.P_Codigo_Postal = Convert.ToInt32(EMPLEADO[Cat_Empleados.Campo_Codigo_Postal].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Ciudad].ToString()))
                                    INF_EMPLEADO.P_Ciudad = EMPLEADO[Cat_Empleados.Campo_Ciudad].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Estado].ToString()))
                                    INF_EMPLEADO.P_Estado = EMPLEADO[Cat_Empleados.Campo_Estado].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Telefono_Casa].ToString()))
                                    INF_EMPLEADO.P_Telefono_Casa = EMPLEADO[Cat_Empleados.Campo_Telefono_Casa].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Telefono_Oficina].ToString()))
                                    INF_EMPLEADO.P_Telefono_Oficina = EMPLEADO[Cat_Empleados.Campo_Telefono_Oficina].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Extension].ToString()))
                                    INF_EMPLEADO.P_Extension = EMPLEADO[Cat_Empleados.Campo_Extension].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fax].ToString()))
                                    INF_EMPLEADO.P_Fax = EMPLEADO[Cat_Empleados.Campo_Fax].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Celular].ToString()))
                                    INF_EMPLEADO.P_Celular = EMPLEADO[Cat_Empleados.Campo_Celular].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Nextel].ToString()))
                                    INF_EMPLEADO.P_Nextel = EMPLEADO[Cat_Empleados.Campo_Nextel].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Correo_Electronico].ToString()))
                                    INF_EMPLEADO.P_Correo_Electronico = EMPLEADO[Cat_Empleados.Campo_Correo_Electronico].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Sexo].ToString()))
                                    INF_EMPLEADO.P_Sexo = EMPLEADO[Cat_Empleados.Campo_Sexo].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Nacimiento].ToString()))
                                    INF_EMPLEADO.P_Fecha_Nacimiento = Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Nacimiento].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_RFC].ToString()))
                                    INF_EMPLEADO.P_RFC = EMPLEADO[Cat_Empleados.Campo_RFC].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_CURP].ToString()))
                                    INF_EMPLEADO.P_CURP = EMPLEADO[Cat_Empleados.Campo_CURP].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Estatus].ToString()))
                                    INF_EMPLEADO.P_Estatus = EMPLEADO[Cat_Empleados.Campo_Estatus].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Ruta_Foto].ToString()))
                                    INF_EMPLEADO.P_Ruta_Foto = EMPLEADO[Cat_Empleados.Campo_Ruta_Foto].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Nombre_Foto].ToString()))
                                    INF_EMPLEADO.P_Nombre_Foto = EMPLEADO[Cat_Empleados.Campo_Nombre_Foto].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_IMSS].ToString()))
                                    INF_EMPLEADO.P_No_IMSS = EMPLEADO[Cat_Empleados.Campo_No_IMSS].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Forma_Pago].ToString()))
                                    INF_EMPLEADO.P_Forma_Pago = EMPLEADO[Cat_Empleados.Campo_Forma_Pago].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString()))
                                    INF_EMPLEADO.P_No_Cuenta_Bancaria = EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Inicio].ToString().Trim()))
                                    INF_EMPLEADO.P_Fecha_Inicio = Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Inicio].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Finiquito].ToString()))
                                    INF_EMPLEADO.P_Tipo_Finiquito = EMPLEADO[Cat_Empleados.Campo_Tipo_Finiquito].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Termino_Contrato].ToString().Trim()))
                                    INF_EMPLEADO.P_Fecha_Termino_Contrato = Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Termino_Contrato].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Baja].ToString().Trim()))
                                    INF_EMPLEADO.P_Fecha_Baja = Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Baja].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString()))
                                    INF_EMPLEADO.P_Salario_Diario = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario_Integrado].ToString()))
                                    INF_EMPLEADO.P_Salario_Diario_Integrado = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario_Integrado].ToString());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Lunes].ToString()))
                                    INF_EMPLEADO.P_Lunes = EMPLEADO[Cat_Empleados.Campo_Lunes].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Martes].ToString()))
                                    INF_EMPLEADO.P_Martes = EMPLEADO[Cat_Empleados.Campo_Martes].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Miercoles].ToString()))
                                    INF_EMPLEADO.P_Miercoles = EMPLEADO[Cat_Empleados.Campo_Miercoles].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Jueves].ToString()))
                                    INF_EMPLEADO.P_Jueves = EMPLEADO[Cat_Empleados.Campo_Jueves].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Viernes].ToString()))
                                    INF_EMPLEADO.P_Viernes = EMPLEADO[Cat_Empleados.Campo_Viernes].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Sabado].ToString()))
                                    INF_EMPLEADO.P_Sabado = EMPLEADO[Cat_Empleados.Campo_Sabado].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Domingo].ToString()))
                                    INF_EMPLEADO.P_Domingo = EMPLEADO[Cat_Empleados.Campo_Domingo].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Comentarios].ToString()))
                                    INF_EMPLEADO.P_Comentarios = EMPLEADO[Cat_Empleados.Campo_Comentarios].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()))
                                    INF_EMPLEADO.P_Tipo_Nomina_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString()))
                                    INF_EMPLEADO.P_Terceros_ID = EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Licencia_Manejo].ToString()))
                                    INF_EMPLEADO.P_No_Licencia = EMPLEADO[Cat_Empleados.Campo_No_Licencia_Manejo].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Vencimiento_Licencia].ToString()))
                                    INF_EMPLEADO.P_Fecha_Vigencia_Licencia = Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Vencimiento_Licencia].ToString());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Nombre_Beneficiario].ToString()))
                                    INF_EMPLEADO.P_Beneficiario_Seguro = EMPLEADO[Cat_Empleados.Campo_Nombre_Beneficiario].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Mensual_Actual].ToString()))
                                    INF_EMPLEADO.P_Salario_Mensual_Actual = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Mensual_Actual].ToString());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Banco_ID].ToString()))
                                    INF_EMPLEADO.P_Banco_ID = EMPLEADO[Cat_Empleados.Campo_Banco_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Reloj_Checador].ToString()))
                                    INF_EMPLEADO.P_Reloj_Checador = EMPLEADO[Cat_Empleados.Campo_Reloj_Checador].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Tarjeta].ToString()))
                                    INF_EMPLEADO.P_No_Tarjeta = EMPLEADO[Cat_Empleados.Campo_No_Tarjeta].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID].ToString()))
                                    INF_EMPLEADO.P_SAP_Fuente_Financiamiento = EMPLEADO[Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_SAP_Programa_ID].ToString()))
                                    INF_EMPLEADO.P_SAP_Programa_ID = EMPLEADO[Cat_Empleados.Campo_SAP_Programa_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_SAP_Area_Responsable_ID].ToString()))
                                    INF_EMPLEADO.P_SAP_Area_Responsable_ID = EMPLEADO[Cat_Empleados.Campo_SAP_Area_Responsable_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_SAP_Partida_ID].ToString()))
                                    INF_EMPLEADO.P_SAP_Partida_ID = EMPLEADO[Cat_Empleados.Campo_SAP_Partida_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_SAP_Codigo_Programatico].ToString()))
                                    INF_EMPLEADO.P_SAP_Codigo_Programatico = EMPLEADO[Cat_Empleados.Campo_SAP_Codigo_Programatico].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Area_ID].ToString()))
                                    INF_EMPLEADO.P_Area_ID = EMPLEADO[Cat_Empleados.Campo_Area_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Seguro].ToString()))
                                    INF_EMPLEADO.P_No_Seguro_Poliza = EMPLEADO[Cat_Empleados.Campo_No_Seguro].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Beneficiario].ToString()))
                                    INF_EMPLEADO.P_Beneficiario_Seguro = EMPLEADO[Cat_Empleados.Campo_Beneficiario].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Indemnizacion_ID].ToString()))
                                    INF_EMPLEADO.P_Tipo_Finiquito = EMPLEADO[Cat_Empleados.Campo_Indemnizacion_ID].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Empleado].ToString()))
                                    INF_EMPLEADO.P_Tipo_Empleado = EMPLEADO[Cat_Empleados.Campo_Tipo_Empleado].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Aplica_ISSEG].ToString()))
                                    INF_EMPLEADO.P_Aplica_ISSEG = EMPLEADO[Cat_Empleados.Campo_Aplica_ISSEG].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la información del empleado. Error: [" + Ex.Message + "]");
            }
            return INF_EMPLEADO;
        }
        /// ***********************************************************************************
        /// Nombre: _Informacion_Parametros_Nomina
        /// 
        /// Descripción: Consulta la información del parámetro de la nómina.
        /// 
        /// Parámetros: No Áplica.
        /// 
        /// Usuario Creo: Juan alberto Hernández Negrete.
        /// Fecha Creó: 23/Septiembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        private Cls_Cat_Nom_Parametros_Negocio _Informacion_Parametros_Nomina()
        {
            Cls_Cat_Nom_Parametros_Negocio Obj_Parametros = new Cls_Cat_Nom_Parametros_Negocio();//Variable de conexión a la capa de negocios.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = new Cls_Cat_Nom_Parametros_Negocio();//Variable que almacena la información del parámetro de nómina.
            DataTable Dt_Parametro = null;//Variable que almacena el registro del parámetro de la nómina.

            try
            {
                Dt_Parametro = Obj_Parametros.Consulta_Parametros();

                if (Dt_Parametro is DataTable)
                {
                    if (Dt_Parametro.Rows.Count > 0)
                    {
                        foreach (DataRow PARAMETRO in Dt_Parametro.Rows)
                        {
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Zona_ID].ToString().Trim()))
                                INF_PARAMETROS.P_Zona_ID = PARAMETRO[Cat_Nom_Parametros.Campo_Zona_ID].ToString();

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Parametro_ID].ToString().Trim()))
                                INF_PARAMETROS.P_Parametro_ID = PARAMETRO[Cat_Nom_Parametros.Campo_Parametro_ID].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Porcentaje_Prima_Vacacional].ToString().Trim()))
                                INF_PARAMETROS.P_Porcentaje_Prima_Vacacional = Convert.ToDouble(PARAMETRO[Cat_Nom_Parametros.Campo_Porcentaje_Prima_Vacacional].ToString().Trim()) / 100;
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Porcentaje_Fondo_Retiro].ToString().Trim()))
                                INF_PARAMETROS.P_Porcentaje_Fondo_Retiro = Convert.ToDouble(PARAMETRO[Cat_Nom_Parametros.Campo_Porcentaje_Fondo_Retiro].ToString().Trim()) / 100;
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Porcentaje_Prima_Dominical].ToString().Trim()))
                                INF_PARAMETROS.P_Porcentaje_Prima_Dominical = Convert.ToDouble(PARAMETRO[Cat_Nom_Parametros.Campo_Porcentaje_Prima_Dominical].ToString().Trim()) / 100;
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1].ToString().Trim()))
                                INF_PARAMETROS.P_Fecha_Prima_Vacacional_1 = PARAMETRO[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1].ToString().Trim();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2].ToString().Trim()))
                                INF_PARAMETROS.P_Fecha_Prima_Vacacional_2 = PARAMETRO[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2].ToString().Trim();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Salario_Limite_Prestamo].ToString().Trim()))
                                INF_PARAMETROS.P_Salario_Limite_Prestamo = Convert.ToDouble(PARAMETRO[Cat_Nom_Parametros.Campo_Salario_Limite_Prestamo].ToString().Trim());
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Salario_Mensual_Maximo].ToString().Trim()))
                                INF_PARAMETROS.P_Salario_Mensual_Maximo = Convert.ToDouble(PARAMETRO[Cat_Nom_Parametros.Campo_Salario_Mensual_Maximo].ToString().Trim());
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Salario_Diario_Int_Topado].ToString().Trim()))
                                INF_PARAMETROS.P_Salario_Diario_Integrado_Topado = Convert.ToDouble(PARAMETRO[Cat_Nom_Parametros.Campo_Salario_Diario_Int_Topado].ToString().Trim());
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Tipo_IMSS].ToString().Trim()))
                                INF_PARAMETROS.P_Tipo_IMSS = PARAMETRO[Cat_Nom_Parametros.Campo_Tipo_IMSS].ToString().Trim();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Minutos_Dia].ToString().Trim()))
                                INF_PARAMETROS.P_Minutos_Dia = PARAMETRO[Cat_Nom_Parametros.Campo_Minutos_Dia].ToString().Trim();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Minutos_Retardo].ToString().Trim()))
                                INF_PARAMETROS.P_Minutos_Retardo = PARAMETRO[Cat_Nom_Parametros.Campo_Minutos_Retardo].ToString().Trim();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple].ToString().Trim()))
                                INF_PARAMETROS.P_ISSEG_Porcentaje_Prevision_Social_Multiple = (Convert.ToDouble(PARAMETRO[Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple].ToString().Trim())).ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Aplicar_Empleado].ToString().Trim()))
                                INF_PARAMETROS.P_ISSEG_Porcentaje_Aplicar_Empleado = (Convert.ToDouble(PARAMETRO[Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Aplicar_Empleado].ToString().Trim()) / 100).ToString();

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString()))
                                INF_PARAMETROS.P_Percepcion_Quinquenio = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString()))
                                INF_PARAMETROS.P_Percepcion_Prima_Vacacional = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString()))
                                INF_PARAMETROS.P_Percepcion_Prima_Dominical = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString()))
                                INF_PARAMETROS.P_Percepcion_Aguinaldo = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString()))
                                INF_PARAMETROS.P_Percepcion_Dias_Festivos = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString()))
                                INF_PARAMETROS.P_Percepcion_Horas_Extra = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString()))
                                INF_PARAMETROS.P_Percepcion_Dia_Doble = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString()))
                                INF_PARAMETROS.P_Percepcion_Dia_Domingo = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString()))
                                INF_PARAMETROS.P_Percepcion_Ajuste_ISR = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString()))
                                INF_PARAMETROS.P_Percepcion_Incapacidades = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString()))
                                INF_PARAMETROS.P_Percepcion_Subsidio = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString()))
                                INF_PARAMETROS.P_Percepcion_Prima_Antiguedad = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString()))
                                INF_PARAMETROS.P_Percepcion_Indemnizacion = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString()))
                                INF_PARAMETROS.P_Percepcion_Vacaciones_Pendientes_Pagar = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString()))
                                INF_PARAMETROS.P_Percepcion_Vacaciones = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString()))
                                INF_PARAMETROS.P_Percepcion_Fondo_Retiro = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString()))
                                INF_PARAMETROS.P_Percepcion_Prevision_Social_Multiple = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString();

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString()))
                                INF_PARAMETROS.P_Deduccion_Faltas = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString()))
                                INF_PARAMETROS.P_Deduccion_Retardos = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString()))
                                INF_PARAMETROS.P_Deduccion_Fondo_Retiro = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_ISR].ToString()))
                                INF_PARAMETROS.P_Deduccion_ISR = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_ISR].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString()))
                                INF_PARAMETROS.P_Deduccion_IMSS = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString()))
                                INF_PARAMETROS.P_Deduccion_ISSEG = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString()))
                                INF_PARAMETROS.P_Deduccion_Vacaciones_Tomadas_Mas = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString()))
                                INF_PARAMETROS.P_Deduccion_Aguinaldo_Pagado_Mas = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString()))
                                INF_PARAMETROS.P_Deduccion_Prima_Vac_Pagada_Mas = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString()))
                                INF_PARAMETROS.P_Deduccion_Sueldo_Pagado_Mas = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo].ToString()))
                                INF_PARAMETROS.P_Deduccion_Orden_Judicial_Aguinaldo = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional].ToString()))
                                INF_PARAMETROS.P_Deduccion_Orden_Judicial_Prima_Vacacional = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion].ToString()))
                                INF_PARAMETROS.P_Deduccion_Orden_Judicial_Indemnizacion = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion].ToString();
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial].ToString()))
                                INF_PARAMETROS.P_Deduccion_Tipo_Desc_Orden_Judicial = PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial].ToString();
                        }
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
        /// Nombre: _Informacion_Terceros
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
        protected Cls_Cat_Nom_Terceros_Negocio _Informacion_Terceros(String Tercero_ID)
        {
            Cls_Cat_Nom_Terceros_Negocio Obj_Terceros = new Cls_Cat_Nom_Terceros_Negocio();//Variable de conexión a la capa de negocios.
            Cls_Cat_Nom_Terceros_Negocio INF_TERCERO = new Cls_Cat_Nom_Terceros_Negocio();//Variable que almacena la información del partido politico.
            DataTable Dt_Tercero = null;//Variable que almacena el registro del partido politico búscado.

            try
            {
                Obj_Terceros.P_Tercero_ID = Tercero_ID;
                Dt_Tercero = Obj_Terceros.Consultar_Terceros_Nombre();

                if (Dt_Tercero is DataTable)
                {
                    if (Dt_Tercero.Rows.Count > 0)
                    {
                        foreach (DataRow TERCERO in Dt_Tercero.Rows)
                        {
                            if (TERCERO is DataRow)
                            {
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
        /// Nombre: _Informacion_Zona_Economica
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
        protected Cls_Cat_Nom_Zona_Economica_Negocio _Informacion_Zona_Economica()
        {
            Cls_Cat_Nom_Zona_Economica_Negocio Obj_Zona_Economica = new Cls_Cat_Nom_Zona_Economica_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = new Cls_Cat_Nom_Zona_Economica_Negocio();//Variable que almacena la información de la zona económica.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacenara la información del parámetro de la nómina.
            DataTable Dt_Zona_Economica = null;//Variable que almacena la información del registro búscado.

            try
            {
                //CONSULTAMOS INFORMACIÓN DEL PARÁMETRO DE LA NÓMINA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();

                //CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONÓMICA.
                Obj_Zona_Economica.P_Zona_ID = INF_PARAMETRO.P_Zona_ID;
                Dt_Zona_Economica = Obj_Zona_Economica.Consulta_Datos_Zona_Economica();//consulta la información de la zona económica.

                if (Dt_Zona_Economica is DataTable)
                {
                    if (Dt_Zona_Economica.Rows.Count > 0)
                    {
                        foreach (DataRow ZONA_ECONOMICA in Dt_Zona_Economica.Rows)
                        {
                            if (ZONA_ECONOMICA is DataRow)
                            {
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
        /// Nombre: _Informacion_Tipo_Nomina
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
        protected Cls_Cat_Tipos_Nominas_Negocio _Informacion_Tipo_Nomina(String Tipo_Nomina_ID)
        {
            Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión a la capa de negocios.
            Cls_Cat_Tipos_Nominas_Negocio INF_TIPO_NOMINA = new Cls_Cat_Tipos_Nominas_Negocio();//Variable que almacena la información del tipo de nómina.
            DataTable Dt_Tipo_Nomina = null;//Variable que almacena el registro del tipo de nómina búscado.

            try
            {
                Obj_Tipos_Nominas.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Tipo_Nomina = Obj_Tipos_Nominas.Consulta_Datos_Tipo_Nomina();//Consultamos la información del tipo de nómina.

                if (Dt_Tipo_Nomina is DataTable)
                {
                    if (Dt_Tipo_Nomina.Rows.Count > 0)
                    {
                        foreach (DataRow TIPO_NOMINA in Dt_Tipo_Nomina.Rows)
                        {
                            if (TIPO_NOMINA is DataRow)
                            {
                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Tipo_Nomina_ID = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Exenta_Prima_Vacacional = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Exenta_Aguinaldo = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Aguinaldo = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Prima_Vacacional_1 = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Prima_Vacacional_2 = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Prima_Antiguedad = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim();

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Aplica_ISR = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString().Trim();

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Despensa].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Despensa = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Despensa].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Actualizar_Salario].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Actualizar_Salario = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Actualizar_Salario].ToString().Trim();

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
        /// Nombre: _Informacion_Puestos
        /// 
        /// Descripción: Consulta la información del puesto la que pertence el empleado.
        /// 
        /// Parámetros: Puesto_ID.- identificador del puesto.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        private Cls_Cat_Puestos_Negocio _Informacion_Puestos(String Puesto_ID)
        {
            Cls_Cat_Puestos_Negocio INF_PUESTO = new Cls_Cat_Puestos_Negocio();
            Cls_Cat_Puestos_Negocio Obj_Puestos = new Cls_Cat_Puestos_Negocio();
            DataTable Dt_Puestos = null;

            try
            {
                Obj_Puestos.P_Puesto_ID = Puesto_ID;
                Dt_Puestos = Obj_Puestos.Consultar_Puestos();

                if (Dt_Puestos is DataTable)
                {
                    if (Dt_Puestos.Rows.Count > 0)
                    {
                        foreach (DataRow PUESTO in Dt_Puestos.Rows)
                        {
                            if (PUESTO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim()))
                                    INF_PUESTO.P_Puesto_ID = PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim();
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Nombre].ToString().Trim()))
                                    INF_PUESTO.P_Nombre = PUESTO[Cat_Puestos.Campo_Nombre].ToString().Trim();
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Salario_Mensual].ToString().Trim()))
                                    INF_PUESTO.P_Salario_Mensual = Convert.ToDouble(PUESTO[Cat_Puestos.Campo_Salario_Mensual].ToString().Trim());
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Aplica_Fondo_Retiro].ToString().Trim()))
                                    INF_PUESTO.P_Aplica_Fondo_Retiro = PUESTO[Cat_Puestos.Campo_Aplica_Fondo_Retiro].ToString().Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la información del puesto del empleado. Error: [" + Ex.Message + "]");
            }
            return INF_PUESTO;
        }
        /// ***********************************************************************************
        /// Nombre: _Informacion_Sindicato
        /// 
        /// Descripción: Consulta la información del Sindicato al que pertence el empleado.
        /// 
        /// Parámetros: Sindicato_ID.- identificador del sindicato.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        private Cls_Cat_Nom_Sindicatos_Negocio _Informacion_Sindicato(String Sindicato_ID)
        {
            Cls_Cat_Nom_Sindicatos_Negocio INF_SINDICATO = new Cls_Cat_Nom_Sindicatos_Negocio();
            Cls_Cat_Nom_Sindicatos_Negocio Obj_Sindicato = new Cls_Cat_Nom_Sindicatos_Negocio();
            DataTable Dt_Sindicatos = null;

            try
            {
                Obj_Sindicato.P_Sindicato_ID = Sindicato_ID;
                Dt_Sindicatos = Obj_Sindicato.Consulta_Sindicato();

                if (Dt_Sindicatos is DataTable)
                {
                    if (Dt_Sindicatos.Rows.Count > 0)
                    {
                        foreach (DataRow SINDICATO in Dt_Sindicatos.Rows)
                        {
                            if (SINDICATO is DataRow)
                            {

                                if (!String.IsNullOrEmpty(SINDICATO[Cat_Nom_Sindicatos.Campo_Sindicato_ID].ToString().Trim()))
                                    INF_SINDICATO.P_Sindicato_ID = SINDICATO[Cat_Nom_Sindicatos.Campo_Sindicato_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(SINDICATO[Cat_Nom_Sindicatos.Campo_Nombre].ToString().Trim()))
                                    INF_SINDICATO.P_Nombre = SINDICATO[Cat_Nom_Sindicatos.Campo_Nombre].ToString().Trim();

                                if (!String.IsNullOrEmpty(SINDICATO[Cat_Nom_Sindicatos.Campo_Responsable].ToString().Trim()))
                                    INF_SINDICATO.P_Responsable = SINDICATO[Cat_Nom_Sindicatos.Campo_Responsable].ToString().Trim();

                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la información del sindicato. Error: [" + Ex.Message + "]");
            }
            return INF_SINDICATO;
        }
        /// ***********************************************************************************
        /// Nombre: _Informacion_IMSS
        /// 
        /// Descripción: Consulta la información de IMSS.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 30/Septiembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        private Cls_Tab_Nom_IMSS_Negocio _Informacion_IMSS()
        {
            Cls_Tab_Nom_IMSS_Negocio INF_IMSS = new Cls_Tab_Nom_IMSS_Negocio();
            Cls_Tab_Nom_IMSS_Negocio Obj_IMSS = new Cls_Tab_Nom_IMSS_Negocio();
            DataTable Dt_IMSS = null;

            try
            {
                Dt_IMSS = Obj_IMSS.Consulta_Datos_IMSS();

                if (Dt_IMSS is DataTable)
                {
                    if (Dt_IMSS.Rows.Count > 0)
                    {
                        foreach (DataRow IMSS in Dt_IMSS.Rows)
                        {
                            if (IMSS is DataRow)
                            {
                                if (!String.IsNullOrEmpty(IMSS[Tab_Nom_IMSS.Campo_IMSS_ID].ToString().Trim()))
                                    INF_IMSS.P_IMSS_ID = IMSS[Tab_Nom_IMSS.Campo_IMSS_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(IMSS[Tab_Nom_IMSS.Campo_Excendete_3_SMG_DF].ToString().Trim()))
                                    INF_IMSS.P_Excendente_SMG_DF = Convert.ToDouble(IMSS[Tab_Nom_IMSS.Campo_Excendete_3_SMG_DF].ToString().Trim());

                                if (!String.IsNullOrEmpty(IMSS[Tab_Nom_IMSS.Campo_Gastos_Medicos].ToString().Trim()))
                                    INF_IMSS.P_Gastos_Medicos = Convert.ToDouble(IMSS[Tab_Nom_IMSS.Campo_Gastos_Medicos].ToString().Trim());

                                if (!String.IsNullOrEmpty(IMSS[Tab_Nom_IMSS.Campo_Porcentaje_Cesantia_Vejez].ToString().Trim()))
                                    INF_IMSS.P_Porcentaje_Cesantia_Vejez = Convert.ToDouble(IMSS[Tab_Nom_IMSS.Campo_Porcentaje_Cesantia_Vejez].ToString().Trim());

                                if (!String.IsNullOrEmpty(IMSS[Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Esp].ToString().Trim()))
                                    INF_IMSS.P_Porcentaje_Enfermedad_Maternidad_Especie = Convert.ToDouble(IMSS[Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Esp].ToString().Trim());

                                if (!String.IsNullOrEmpty(IMSS[Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Pes].ToString().Trim()))
                                    INF_IMSS.P_Porcentaje_Enfermedad_Maternidad_Pesos = Convert.ToDouble(IMSS[Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Pes].ToString().Trim());

                                if (!String.IsNullOrEmpty(IMSS[Tab_Nom_IMSS.Campo_Porcentaje_Invalidez_Vida].ToString().Trim()))
                                    INF_IMSS.P_Porcentaje_Invalidez_Vida = Convert.ToDouble(IMSS[Tab_Nom_IMSS.Campo_Porcentaje_Invalidez_Vida].ToString().Trim());

                                if (!String.IsNullOrEmpty(IMSS[Tab_Nom_IMSS.Campo_Prestaciones_Dinero].ToString().Trim()))
                                    INF_IMSS.P_Prestaciones_Dinero = Convert.ToDouble(IMSS[Tab_Nom_IMSS.Campo_Prestaciones_Dinero].ToString().Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la información de IMSS. Error: [" + Ex.Message + "]");
            }
            return INF_IMSS;
        }
        #endregion

    }
}