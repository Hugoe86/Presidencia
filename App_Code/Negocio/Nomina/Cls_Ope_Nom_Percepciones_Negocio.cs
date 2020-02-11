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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Faltas_Empleado.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Tiempo_Extra.Negocio;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Domingos_Trabajados.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Ope_Dias_Festivos.Negocio;
using Presidencia.Calculo_Percepciones.Datos;
using Presidencia.DateDiff;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Ajuste_ISR.Negocio;
using Presidencia.Archivos_Historial_Nomina_Generada;
using System.Text;
using Presidencia.Indemnizacion.Negocio;
using Presidencia.Utilidades_Nomina;
using Presidencia.Cat_Terceros.Negocio;
using Presidencia.Puestos.Negocios;
using Presidencia.IMSS.Negocios;
using Presidencia.Calculo_Deducciones.Negocio;

namespace Presidencia.Calculo_Percepciones.Negocio
{
    public class Cls_Ope_Nom_Percepciones_Negocio
    {
        public DateTime Fecha_Generar_Nomina;

        #region (Calculo Percepciones)
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Sueldo_Nominal
        /// DESCRIPCION : 1.- Se consultan todas las faltas del empleado que tuvo de la catorcena anterior y se le restan alos dias 
        ///                   laborales para obtener los dias trabajados.
        ///                               
        ///               Calculo:
        ///                       Sueldo_Nominal = (Dias_Trabajados * Salario_Diario)
        ///                       Grava = 100%
        ///                       Exenta = 0%
        ///                       
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se efectuara el calculo del tiempo extra.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete. 
        /// FECHA_CREO  : 14/Diciembre/2010 4:39 pm.
        /// MODIFICO          : Juan Alberto Hernández Negrete.
        /// FECHA_MODIFICO    : 26/Septiembre/2011
        /// CAUSA_MODIFICACION: Ajustes para la integración del ISSEG.
        ///************************************************************************************************************************
        public DataTable Calcular_Sueldo_Normal(String Empleado_ID)
        {
            ///VARIABLES DE CONEXION CON LA CAPA DE NEGOCIOS
            Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexion con la capa de negocios
            Cls_Ope_Nom_Vacaciones_Empleado_Negocio Consulta_Vacaciones = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexion con la capa de negocios.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara toda la información del empleado.

            ///VARIABLES DE TIPO TABLA QUE ALMACENAN LA INFORMACION CONSULTADA DE FORMA ORDENADA.
            DataTable Dt_Resultado = null;//Variable que almacenara el resultado del calculo del SUELDO.

            ///VARIABLES DE TIPO CANTIDAD UTILIZADAS PARA REALIZAR EL CALCULO DEL SUELDO NOMINAL.
            Double Sueldo_Nominal = 0.0;//Variable que almacenara el total del SUELDO.
            Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado que le corresponde segun el puesto.
            Double Cantidad_Grava = 0.0;//Cantidad que grava el SUELDO.
            Double Cantidad_Exenta = 0.0;//Cantidad que exenta el SUELDO.

            ///VARIABLES QUE SE UTILIZARAN PARA ALMACENAR DATOS CON REFERENCIA A LOS DIAS QUE SE OCUPARAN PARA EL CALCULO DEL SUELDO NOMINAL
            Double Dias_Trabajados = 0;//Variable que almacenara la cantidad de dias trabajados del empleado en la catorcena.
            Double Dias_Vacaciones_Catorcena_Actual = 0;//Variable que almacenara lo dias que se tomaron de vacaciones y pertenecen a la catorcena actual.
            Double Dias_Incapacidad_Catorcena_Actual = 0;
            Double DIAS_LABORALES = 0;//Variable que almacenara los dias laborales del empleado.

            try
            {
                //PASO I.- CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

                //PASO II.- OBTENEMOS LOS DIAS QUE TIENE EL PERIODO NOMINAL A GENERAR.
                DIAS_LABORALES = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);//CONSULTAMOS LOS DIAS LABORALES DEL EMPLEADO EN LA CATORCENA.

                //PASO III.- OBTENEMOS LOS DIAS DE VACACIONES QUE HA TOMADO EL EMPLEADO EN EL PERIODO A GENERAR.
                Dias_Vacaciones_Catorcena_Actual = Obtener_Dias_Vacaciones_Periodo_Actual(Empleado_ID);

                //CONSULTAR LOS DÍAS DE INCAPACIDAD QUE TUVO EL EMPLEADO SIN EXCLUIR CUANDO APLICA PAGO 4 DÍA.
                Dias_Incapacidad_Catorcena_Actual = Obtener_Dias_Incapacidades_Sin_Excluir(Empleado_ID);
                
                //PASO IV.- OBTENEMOS LOS DIAS TRABAJADOS DEL EMPLEADO.
                Dias_Trabajados = (DIAS_LABORALES) - (Dias_Vacaciones_Catorcena_Actual + Dias_Incapacidad_Catorcena_Actual);

                //PASO V.- VALIDAMOS QUE EL CAMPO DE ISSEG NO SEA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //PASO 5.1.- VALIDAMOS SI EL EMPLEADO APLICA PARA EL CÁLCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //PASO 5.1.1.- OBTENEMOS LA CANTIDAD DIARIA DE SUELDO DEL NIVEL QUE TIENE ASIGNADO EL EMPLEADO [ISSEG].
                        //             ESTE DATO SE OBTIENE DEL CATALOGO DE PUESTOS.
                        Salario_Diario = Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //PASO 5.1.2.- OBTENEMOS EL SALARIO DIARIO DEL EMPLEADO [CAT_EMPLEADOS].
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //PASO 5.2.- OBTENEMOS EL SALARIO DIARIO DEL EMPLEADO [CAT_EMPLEADOS].
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //PASO VI.- CALCULAMOS EL SUELDO DEL EMPLEADO.
                Sueldo_Nominal = (Dias_Trabajados * Salario_Diario);
                //Obtenemos la cantidad que exenta el concepto del sueldo.
                Cantidad_Exenta = 0;
                //Obtenemos la cantidad que grava el concepto del sueldo.
                Cantidad_Grava = Obtener_Cantidad_Grava_Sueldo(Sueldo_Nominal, Empleado_ID);

                //PASO VII.- CREAMOS LA TABLA DE RESULTADOS DEL PUESTO.
                Dt_Resultado = Crear_Tabla_Resultados(Sueldo_Nominal, Cantidad_Grava, Cantidad_Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el SUELDO del empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Prevision_Social_Multiple
        /// DESCRIPCION : 1.- Se obtiene la PSM del empleado en base al Sueldo del Puesto y al Porcentaje de [%] Previsión Social.
        ///                   y se calcula lo que le corresponde de acuerdo a los dias contemplados en el periodo de nomina a generar.
        ///               Calculo:
        ///                       PSM_Nominal = (Dias_Trabajados * Cantidad_PSM_Diaria)
        ///                       Grava = 100%
        ///                       Exenta = 0%
        ///                       
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se efectuara el calculo de la PSM.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete. 
        /// FECHA_CREO  : 26/Septiembre/2011.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///************************************************************************************************************************
        public DataTable Calcular_Prevision_Social_Multiple(String Empleado_ID)
        {
            ///VARIABLES DE CONEXION CON LA CAPA DE NEGOCIOS
            Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexion con la capa de negocios
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Objeto que almacenara la información del empleado.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacenara la información del parámetro de la nómina.

            ///VARIABLES DE TIPO TABLA QUE ALMACENAN LA INFORMACION CONSULTADA DE FORMA ORDENADA.
            DataTable Dt_Resultado = null;//Variable que almacenara el resultado del calculo de PSM.

            ///VARIABLES DE TIPO CANTIDAD UTILIZADAS PARA REALIZAR EL CALCULO DE LA PSM..
            Double PSM = 0.0;//Variable que almacenara el total de PSM..
            Double Salario_Diario = 0.0;//Variable que almacenara la cantidad diaria de PSM del empleado que le corresponde segun el Nivel o Puesto.
            Double Cantidad_Grava = 0.0;//Cantidad que grava la PSM.
            Double Cantidad_Exenta = 0.0;//Cantidad que exenta la PSM.

            ///VARIABLES QUE SE UTILIZARAN PARA ALMACENAR DATOS CON REFERENCIA A LOS DIAS QUE SE OCUPARAN PARA EL CALCULO DE PSM.
            Int32 Dias_Trabajados = 0;//Variable que almacenara la cantidad de dias trabajados del empleado en la catorcena.
            Int32 DIAS_LABORALES = 0;//Variable que almacenara los dias laborales del empleado.
            Double NO_FALTAS_EMPLEADO = 0.0;//Variable que almacenara las faltas que ha tenido el empleado en la catorcena.
            Double DIAS_VACACIONES = 0.0;//Variable que almacenara los dias de vacaciones que ha tenido el empleado en la catorcena.
            Double DIAS_INCAPACIDAD = 0.0;//Variable que almacenara los dias de incapacidad que ha tenido el empleado en la catorcena.
            Double DIAS_TRABAJADOS = 0.0;//Variable que almacenara los dias trabajados del empleado.
            Double CANTIDAD_INCAPACIDAD = 0.0;//Variable que almacenara la cantidad por concepto de incapacidad.
            Double CANTIDAD_VACACIONES = 0.0;//Variable que almacenara la cantidad por concepto de vacaciones.
            Double SUELDO = 0.0;//Variable que almacenara la cantidad por concepto de sueldo.
            Double TOTAL_S_V_I = 0.0;//Variable que almacenara la cantidad total de [Sueldo - Vacaciones - Incapacidad]
            
            try
            {
                //CONSULTAMOS LA INFORMACION DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACIÓN DE LOS PARÁMETROS DE LA NÓMINA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();

                //OBTENEMOS LOS DIAS QUE TIENE LA CATORCENA, CONSIDERANDO SI LA FECHA DE INGRESO DEL EMPLEADO ES MAYOR A LA FECHA DE INICIO DE LA CATORCENA.
                DIAS_LABORALES = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //OBTENEMOS EL NÚMERO DE FALTAS QUE HA TENIDO EL EMPLEADO.
                //NO_FALTAS_EMPLEADO = Obtener_Faltas_Empleados(Empleado_ID);
                //OBTENEMOS LAS VACACIONES QUE HA TENIDO EL EMPLEADO.
                DIAS_VACACIONES = Obtener_Dias_Vacaciones_Periodo_Actual(Empleado_ID);
                //OBTENEMOS LAS INCAPACIDADES QUE HA TENIDO EL EMPLEADO.
                DIAS_INCAPACIDAD = Obtener_Dias_Incapacidades(Empleado_ID);

                //OBTENEMOS LOS DIAS TRABAJADOS DEL EMPLEADO.
                DIAS_TRABAJADOS = DIAS_LABORALES - (NO_FALTAS_EMPLEADO + DIAS_VACACIONES + DIAS_INCAPACIDAD);

                //VALIDAMOS QUE EL CAMPO DE ISSEG NO SEA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDAMOS SI EL EMPLEADO APLICA PARA EL CÁLCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DIARIA DE SUELDO DEL NIVEL QUE TIENE ASIGNADO EL EMPLEADO [ISSEG].
                        //             ESTE DATO SE OBTIENE DEL CATALOGO DE PUESTOS.
                        Salario_Diario = Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG(Empleado_ID);

                        //OBTENEMOS LA CANTIDAD DE INCAPACIDAD.
                        CANTIDAD_INCAPACIDAD = Obtener_Cantidad_Tabla_Resultados(Obtener_Incapacidades_Periodo(Empleado_ID));
                        //OBTENEMOS LA CANTIDAD DE VACACIONES.
                        CANTIDAD_VACACIONES = Obtener_Cantidad_Tabla_Resultados(Calcular_Vacaciones(Empleado_ID));
                    }
                }

                //OBTENEMOS EL SUELDO DE ACUERDO A LOS DÍAS LABORADOS DEL EMPLEADO.
                SUELDO = (DIAS_TRABAJADOS * Salario_Diario);
                //OBTENEMOS EL TOTAL CONSIDERANDO TOTAL COMO LA SUMA DE SUELDO - VACACIONES - INCAPACIDAD.
                TOTAL_S_V_I = (SUELDO + CANTIDAD_VACACIONES + CANTIDAD_INCAPACIDAD);

                //OBTENEMOS LA PREVISIÓN SOCIAL MÚLTIPLE.
                PSM = (TOTAL_S_V_I * Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple)) 
                    ? "0" : INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple));
                Cantidad_Exenta = PSM;
                Cantidad_Grava = 0;

                //PASO VII.- CREAMOS LA TABLA DE RESULTADO DEL CALCULO.
                Dt_Resultado = Crear_Tabla_Resultados(PSM, Cantidad_Grava, Cantidad_Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular la Previsión Social Múltiple. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Tiempo_Extra
        /// DESCRIPCION : 1.- Se consulta y se suman todas las horas extra que se le capturaron al empleado.
        /// 
        ///               Calculo: 
        ///                        Tiempo_Extra = (((Salario_Diario)/8) * 2) * Horas_Extra
        ///                        Grava = 100%
        ///                        Exenta= 0%
        ///                        
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se efectuara el calculo del tiempo extra.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete. 
        /// FECHA_CREO  : 16/Diciembre/2010 10:12 am.
        /// MODIFICO          : Juan Alberto Hernández Negrete.
        /// FECHA_MODIFICO    : 26/Septiembre/2011
        /// CAUSA_MODIFICACION: Cambio al nuevo esquema que ya considera la PSM del empleado.
        ///************************************************************************************************************************
        public DataTable Calcular_Tiempo_Extra(String Empleado_ID)
        {
            ///VARIABLES PARA ALMACENAR LOS DATOS DE LAS CONSULTAS.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacena la información del empleado.
            DataTable Dt_Tiempo_Extra_Empleado = null;  //Variable que almacenara una lista con informacion de las horas extra.

            ///VARIABLES PARA EL CALCULO DEL TIEMPO EXTRA
            Double Horas_Extra_Capturadas = 0.0;        //Variable que almacenara la cantidad de minutos que el empleado a trababajado de tiempo extra.
            Double Tiempo_Extra = 0.0;                  //Variable que almacenara el tiempo extra trabajado por el empleado.
            Double Salario_Diario = 0.0;                //Variable que almacenara el salario diario del empleado.
            Double Cantidad_Grava = 0.0;                //Cantidad que grava el tiempo extra.
            Double Cantidad_Exenta = 0.0;               //Cantidad que exenta el tiempo extra.

            ///VARIABLE QUE ALAMCENARA EL RESULTADO DE LOS CALCULOS REALIZADOS
            DataTable Dt_Resultado = null;              //Variable que almacenra el resultado de los calculos realizados.

            try
            {
                //PASO I.- CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

                //PASO II.- CONSULTAMOS LAS HORAS EXTRA LABORADAS EN EL PERIODO A GENERAR LA NOMINA.
                Dt_Tiempo_Extra_Empleado = Cls_Ope_Nom_Percepciones_Datos.Consultar_Tiempo_Extra_Empleado_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //PASO III.-VALIDAMOS PARA VERIFICAR QUE LA CONSULTA HALLA ENCONTRADO RESULTADOS.
                if (Dt_Tiempo_Extra_Empleado is DataTable)
                {
                    //PASO 3.1.- RECORREMOS LOS REGISTROS DE HORAS EXTRAS ENCONTRADOS EN EL SISTEMA EN LA CATORCENA ACTUAL.
                    foreach (DataRow Registro in Dt_Tiempo_Extra_Empleado.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Ope_Nom_Tiempo_Extra.Campo_Pago_Dia_Doble].ToString().Trim()))
                        {
                            if (Registro[Ope_Nom_Tiempo_Extra.Campo_Pago_Dia_Doble].ToString().Trim().ToUpper().Equals("NO"))
                            {
                                //PASO 3.1.1.- OBTENEMOS LA CANTIDAD DE TIEMPO EXTRA QUE A TRABAJADO EL EMPLEADO EN LA CATORCENA ACTUAL.
                                if (!String.IsNullOrEmpty(Registro[Ope_Nom_Tiempo_Extra.Campo_Horas].ToString().Trim()))
                                    Horas_Extra_Capturadas += Convert.ToDouble(Registro[Ope_Nom_Tiempo_Extra.Campo_Horas].ToString());
                            }
                        }
                    }
                }

                //PASO IV.- VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //PASO 4.1.- VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //PASO 4.1.1.- OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //PASO 4.1.2.- OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else {
                    //PASO 4.2.- OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //PASO IV.- EJECUTAMOS EL CALCULO DEL TIEMPO EXTRA.
                Tiempo_Extra = ((Salario_Diario / 8) * 2) * Horas_Extra_Capturadas;
                Cantidad_Grava = Tiempo_Extra;
                Cantidad_Exenta = 0;

                //PASO 5.- CREAR LA TABLA DE RESULTADOS DEL CALCULO DE TIEMPO EXTRA.
                Dt_Resultado = Crear_Tabla_Resultados(Tiempo_Extra, Cantidad_Grava, Cantidad_Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el tiempo extra del empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Prima_Vacacional
        /// DESCRIPCION : 1.- De acuerdo al TIPO_NOMINA que tiene el empleado se consultan los dias a considerar para este calculo
        /// 
        ///                   [Dias_Nominales] ó [Dias_Prima_Vacacional]                   
        ///                         DIAS_PRIMA_VACACIONAL_1   --> Periodo [ENERO - JUNIO] 
        ///                         DIAS_PRIMA_VACACIONAL_2   --> Periodo [JULIO - DICIEMBRE]
        ///                   
        ///               2.- Se obtienen los dias de los meses de [ENERO - JUNIO] ó [JULIO - DICIEMBRE] Dias Totales.
        ///               
        ///                 Diferencia_Dias = Fecha_Fin.Subtract(Fecha_Inicia)
        ///                 
        ///               3.- A los [Dias_Totales] se le restan las [Faltas_Empleado] y obtenemos los [Dias_Trabajados] del empleado.
        ///               
        ///                 Dias_Trabajados = (Dias_Totales - Faltas_Empleados)              
        /// 
        ///               4.- Se obtiene la parte proporcional de los dias a considerar para la prima vacacional.
        ///               
        ///               Parte proporcional de los dias a considerar: 
        ///               
        ///                        Dias_Nominales --> Dias_Totales
        ///                              X        --> Dias_Trabajados 
        ///                              
        ///               Nota: Donde [X] son los [Dias_Totales_Prima_Vacacional]. 
        ///                              
        ///               5.- Se consulta el parametro [%] [Porcentaje_Prima_Vacacional]  a considerar para este calculo. 
        ///                   [Salario_Zona_Economica] y [Dias_Exenta_Prima_Vacacional].
        ///               6.- Se consulta el salario diario del empleado [Salario_Diario].
        ///               7.- Se realiza el calculo de la prima vacacional.
        ///                 
        ///               Calculo:
        ///                        Prima_Vacacional = (Dias_Prima_Vacacional * Salario_Diario) * ([%] Porcentaje_Prima_Vacacional)
        ///                        Grava = Prima_Vacacional - Exenta
        ///                        Exenta = Dias_Exenta_Prima_Vacacional * Salario_Zona
        ///                        
        ///               Donde: [Dias_Exenta_Prima_Vacacional] son los dias que exenta.
        ///                      [Salario_Zona] Es el un parametro.
        ///                        
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se efectuara el calculo del tiempo extra.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete. 
        /// FECHA_CREO  : 16/Diciembre/2010 12:08 pm.
        /// MODIFICO          : Juan Alberto Hernández Negrete.
        /// FECHA_MODIFICO    : 26/Septiembre/2011
        /// CAUSA_MODIFICACION: Integrar la PSM al calculo de Prima Vacacional.
        ///************************************************************************************************************************
        public DataTable Calcular_Prima_Vacacional(String Empleado_ID, Int32 Periodo)
        {
            //VARIABLE DE NEGOCIO.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//VARIABLE QUE ALMACENARA LA INFORMACIÓN DEL EMPLEADO.
            Cls_Cat_Tipos_Nominas_Negocio INF_TIPO_NOMNA = null;//VARIABLE QUE ALMACENA LA INF. DEL TIPO DE NÓMINA.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//VARIABLE QUE ALMACENA LA INF. DE LA ZONA ECONÓMICA.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//VARIABLE QUE ALMACENA LA INF. DEL PARÁMETRO DE LA NÓMINA.

            ///VARIABLES QUE SE UTILIZARAN PARA EL CALCULO DE LA PRIMA VACACIONAL
            Int32 Dias_Totales = 0;//Variable que almacenara la cantidad de dias del periodo Enero - Junio
            Int32 Dias_Trabajados = 0;//Variable que almacenara los dias trabajados por el empleado una vez ya descontadas las faltas que a tenido el mismo.
            Int32 Faltas_Empleados = 0;//Variable que almacenara las falta que ha tenido el empleado en el periodo a evaluar.
            Double Dias_Prima_Vacacional = 0;//Variable que almacenara la cantidad de dias de vacaciones por periodo.
            Double Dias_Totales_Prima_Vacacional = 0.0;//Variable que almacenara los dias a considerar para el calculo de la prima vacacional.
            Double Dias_Exenta_Prima_Vacacional = 0;//Variable que almacena los dias que exenta la prima vacacional.

            ///VARIABLE DE TIPO CANTIDAD QUE SE OCUPARAN EN EL CALCULO DE LA PRIMA VACACIONAL.
            Double Prima_Vacacional = 0.0;//Cantidad que el empleado tendra derecho a percibir como prima vacacional.
            Double Cantidad_Grava = 0.0;//Cantidad que gravara la prima vaccaional.
            Double Cantidad_Exenta = 0.0;//Cantidad que exentara la prima vacacional.
            Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado.

            ///VARIABLE DE TIPO FECHA QUE SE OCUPARAN EN EL CALCULO DE LA PRIMA VACACIONAL.
            DateTime Fecha_Inicia = new DateTime();//Variable que almacenara la fecha de inicio del periodo para el calculo de la prima vacacional.
            DateTime Fecha_Fin = new DateTime();//Variable que almacenara la fecha de fin del periodo para el calculo de la prima vacacional.
            TimeSpan Diferencia_Dias = new TimeSpan();//Obtiene un objeto del tipo TimeSpan que almacena los valores de la diferencia de fechas.

            ///VARIABLE QUE ALMACENARA LOS RESULTADOS OBTENIDOS.
            DataTable Dt_Resultado = null;//Variable que alamcenara la Prima Vacacional, Cantidad Gravante y Cantidad Exenta.
            Int32 Dias_Descontar = 0;//Variable que almacena la cantidad de dias que se le descontaran si el año de ingreso del empleado. es el actual.

            try
            {
                //CONSULTAMOS INFORMACIÓN GENERAL NECESARIA PARA REALIZAR EL CÁLCULO DE PRIMA VACACIONAL.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_TIPO_NOMNA = _Informacion_Tipo_Nomina(INF_EMPLEADO.P_Tipo_Nomina_ID);//CONSULTAMOS LA INFORMACIÓN DEL TIPO DE NÓMINA.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();//CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONÓMICA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();//CONSULTAMOS LA INFORMACIÓN DEL PARÁMETRO DE LA NÓMINA.

                //PASO I.-VALIDAMOS Y OBTENEMOS LAS FECHAS DE ACUERDO AL PERIODO AL CUAL SE CALCULARA LA PRIMA VACACIONAL.
                if (Periodo == 1)
                {
                    //PERIODO VACACIONAL [ENERO - JUNIO]
                    Fecha_Inicia = new DateTime(Fecha_Generar_Nomina.Year, 1, 1, 0, 0, 0);
                    Fecha_Fin = new DateTime(Fecha_Generar_Nomina.Year, 6, 30, 23, 59, 59);
                }
                else if (Periodo == 2)
                {
                    //PERIODO VACACIONAL [JULIO - DICIEMBRE]
                    Fecha_Inicia = new DateTime(Fecha_Generar_Nomina.Year, 7, 1, 0, 0, 0);
                    Fecha_Fin = new DateTime(Fecha_Generar_Nomina.Year, 12, 31, 11, 59, 59);
                }

                if (INF_EMPLEADO.P_Fecha_Inicio.Year == Fecha_Inicia.Year)
                {
                    //SI EL EMPLEADO INGRESO EN EL AÑO ACTUAL SE OBTIENE LOS DIAS DEL INICIO DE AÑO Y SU FECHA DE 
                    //INGRESO PARA REALIZAR EL DESCUENTO DE ESO DIAS Y NO CONSIDERARLOS EN EL CÁLCULO.
                    Dias_Descontar = INF_EMPLEADO.P_Fecha_Inicio.Subtract(Fecha_Inicia).Days + 1;
                    if (Dias_Descontar < 0) Dias_Descontar = 0;
                }

                //PASO 2.- OBTENER LOS DIAS TOTALES EN EL PERIODO.
                Diferencia_Dias = Fecha_Fin.Subtract(Fecha_Inicia);
                Dias_Totales = Diferencia_Dias.Days + 1;

                //PASO 3.- CONSULTAMOS LAS FALTAS QUE A TENIDO EL EMPLEADO EN EL PERIODO
                Faltas_Empleados = Obtener_Faltas_Empleados(Empleado_ID, Fecha_Inicia, Fecha_Fin);

                //PASO 4.- OBTENEMOS DIAS TABAJADOS DEL EMPLEADO.
                Dias_Trabajados = (Dias_Totales - Faltas_Empleados - Dias_Descontar);


                /*
                 * CONSULTAMOS EL PARÁMETRO QUE INDICA LA CANTIDAD DE DÍAS QUE TIENE ASIGNADO EL 
                 * PERIODO VACACIONAL [PARÁMETRO POR TIPO DE NÓMINA].
                 */
                switch (Periodo)
                {
                    case 1:
                        Dias_Prima_Vacacional = INF_TIPO_NOMNA.P_Dias_Prima_Vacacional_1;
                        break;
                    case 2:
                        Dias_Prima_Vacacional = INF_TIPO_NOMNA.P_Dias_Prima_Vacacional_2;
                        break;
                    default:
                        break;
                }

                //VARIABLE QUE INDICA LOS DIAS QUE EXENTA LA PRIMA VACACIONAL [PARÁMETRO POR TIPO DE  NÓMINA].
                Dias_Exenta_Prima_Vacacional = INF_TIPO_NOMNA.P_Dias_Exenta_Prima_Vacacional / 2;

                //PASO 7.- OBTENEMOS LOS DIAS DE PRIMA VACACIONAL [FORMÚLA].
                Dias_Totales_Prima_Vacacional = (Dias_Prima_Vacacional * Dias_Trabajados) / Dias_Totales;

                /*
                 *CONSULTA DEL SUELDO CONSIDERANDO LA PREVISIÓN SOCIAL MÚLTIPLE 
                 *VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                */
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //PASO 11.- SE EJECUTA EL CALCULO DE LA PRIMA VACACIONAL.
                Prima_Vacacional = (Dias_Totales_Prima_Vacacional * Salario_Diario) * INF_PARAMETRO.P_Porcentaje_Prima_Vacacional;

                //PASO 12.- OBTENEMOS LA CANTIDAD EXENTA LA PRIMA VACACIONAL. 
                Cantidad_Exenta = (Dias_Exenta_Prima_Vacacional) * INF_ZONA_ECONOMICA.P_Salario_Diario;

                //PASO 13.- OBTENEMOS LA CANTIDAD GRAVANTE DE LA PRIMA VACACIONAL.
                Cantidad_Grava = Prima_Vacacional - Cantidad_Exenta;

                //ESTE PASO SE AGREGO PARA EVITAR QUE EXISTA UNA CANTIDAD QUE GRAVE DE FORMA NEGATIVA.
                if (Cantidad_Exenta > Prima_Vacacional)
                {
                    Cantidad_Grava = 0;
                    Cantidad_Exenta = Prima_Vacacional;
                }

                //PASO 14.- OBTENEMOS LA TABLA DE RESULTADOS.
                Dt_Resultado = Crear_Tabla_Resultados(Prima_Vacacional, Cantidad_Grava, Cantidad_Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular la prima vacacional. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Aguinaldo
        /// DESCRIPCION : 1.- Se obtiene de acuerdo al tipo de nomina los dias de aguinaldo a considerar [Dias_Aguinaldo].
        ///               2.- Se restan 365 dias las faltas que tuvo el empleado durante todo el año si
        ///                   es que el empleado tiene una fecha de ingreso menor al año actual.
        ///               
        ///                     Fecha_Ingreso menor o igual 01/Enero/Año
        ///                     Dias_Trabajados = 365 - Faltas_Empleado
        ///                
        ///                3.- Si el empleado comenzo a trabajar despues del 01/Enero/Año  se debe restar a 365
        ///                    dias, los dias 01/Enero/Año actual menos fecha de ingreso, para poder determinar cuales
        ///                    seran los dias que deberan ser descontados a 365 dias, restando tabien las faltas que tuvo
        ///                    el empleado.
        ///                
        ///                     Dias_Descontar = Fecha_Ingreso_Empleado.Subtract(Fecha_Inicio_Año_Actual).Days
        ///                     Dias_Trabajados = 365 - (Dias_Descontar + Faltas_Empleado) 
        ///                    
        ///                4.- Se obtiene la parte proporcional de los dias a considerar para el aguinaldo.
        ///               
        ///                     Dias_Aguinaldo  --> 365
        ///                            X        --> Dias_Trabajados
        ///                            
        ///                     Nota: En donde (X) son los Dias_Totales_Aguinaldo.
        ///                     
        ///                 5.- Se consulta como parametro el salario de la zona (Salario_Zona).
        ///                 6.- Se consulta el salario diario del empleado.
        ///                 7.- Se realiza el calculo del aguinaldo.
        ///                     
        ///                 Calculo:
        ///                 
        ///                     Aguinaldo = (Dias_Totales_Aguinaldo * Salario_Diario)
        ///                     Exenta = 30 * Salario_Zona
        ///                     Grava = Aguinaldo - Exenta
        ///                     
        ///                     Si Exenta >= Aguinaldo entonces Aguinaldo Grava $0.00 y Exenta 100% 
        ///                     
        ///        IMPORTANTE: 1.- Si el empleado cambio de tipo de nómina se deberá considerar como fecha de 
        ///                        ingreso cuando ocurrio este cambio y se calcula su aguinaldo proporcional.
        ///                    2.- Lo correspodiente a su tipo de nómina anterior queda cubierto con el finiquito
        ///                        que se le dio al empleado al darlo de baja.
        ///                        
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se efectuara el calculo del tiempo extra.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete. 
        /// FECHA_CREO  : 16/Diciembre/2010 10:12 am.
        /// MODIFICO          : Juan Alberto Hernandez Negrete.
        /// FECHA_MODIFICO    : 26/Septiembre/2011
        /// CAUSA_MODIFICACION: Integración de la PSM al calculo del aguinaldo.
        ///************************************************************************************************************************
        public DataTable Calcular_Aguinaldo(String Empleado_ID)
        {
            //VARIABLES QUE ALMACENAN LA INFORMACIÓN DE QUE SE USARA EN EL CALCULO DE AGUINALDO.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//VARIABLE QUE ALMACENA LA INFORMACIÓN DEL EMPLEADO.
            Cls_Cat_Tipos_Nominas_Negocio INF_TIPO_NOMINA = null;//VARIABLE QUE ALMACENA LA INF. DEL TIPO DE NÓMINA.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//VARIABLE QUE ALMACENA LA INF. DE LA ZONA ECONÓMICA.

            ///VARIABLE QUE ALMACENARA LOS RESULTADOS OBTENIDOS DEL CALCULO DEL AGUINALDO.
            DataTable Dt_Resultado = null;//Variable que alamcenara la Prima Vacacional, Cantidad Gravante y Cantidad Exenta.

            ///VARIABLES DE TIPO FECHA QUE SE UTILIZARAN EL EL CALCULO DEL AGUINALDO.
            DateTime Fecha_Inicio_Anyo_Actual = new DateTime(Fecha_Generar_Nomina.Year, 1, 1, 0, 0, 0);//Variable que almacenara la fecha de inicio que se considera para el calculo del aguinaldo del año actual.            
            DateTime Fecha_Fin_Anyo_Actual = new DateTime(Fecha_Generar_Nomina.Year, 12, 31, 23, 59, 59);//Variable que se usara como fecha de inicio para la busqueda de faltas del empleado.

            ///VARIABLES DE TIPO INT32 QUE ALMCENARAN CANTIDADES DE DIAS QUE SE OCUPARAN PARA EL CALCULO DEL AGUINALDO.
            Double Dias_Trabajados = 0;//Variable que almacenar la cantidad de dias trabajados del empleado en año actual.
            Double Faltas_Empleado = 0;//Variable que almacenara los dias que el empleado a faltado en el año actual.
            Double Dias_Descontar = 0;//Variable que almacenara los dias que se le descontaran al empleado si entro en una fecha posterios al 01/01/Año.
            Double Dias_Totales_Aguinaldo = 0;//Variable que almacenara los dias de aguinaldo que le corresponden al empleado.

            ///VARIABLES DE TIPO DOUBLE QUE ALMACENARÁN CANTIDADES QUE SE OCUPARÁN PARA EL CALCULO DEL AGUINALDO.
            Double Salario_Diario = 0.0;//Variable que almacena el salario diario del empleado.
            Double Aguinaldo = 0.0;//Variable que almacenara la cantidad de aguinaldo que el empleado recibira.
            Double Grava = 0.0;//Variable que almacenra la cantidad gravante.
            Double Exenta = 0.0;//Variable que almacenara la cantidad exenta.
            Int32 DIAS_ANIO = 0;//Variable que almacena los dias totales del año.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LOS PARAMETROS DEL TIPO DE NOMINA.
                INF_TIPO_NOMINA = _Informacion_Tipo_Nomina(INF_EMPLEADO.P_Tipo_Nomina_ID);
                //CONSULTAMOS LA ZONA ECONOMICA A LA QUE PERTENECE EL EMPLEADO.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();

                //OBTENEMOS LOS DIAS DEL AÑO.
                DIAS_ANIO = (Int32)(Fecha_Fin_Anyo_Actual.Subtract(Fecha_Inicio_Anyo_Actual).Days + 1);

                //PASO 2.- OBTENEMOS LAS FALTAS QUE A TENIDO EL EMPLEADO EN AÑO ACTUAL [FALTAS_EMPLEADO].
                Faltas_Empleado = Obtener_Faltas_Empleados(Empleado_ID, Fecha_Inicio_Anyo_Actual, Fecha_Fin_Anyo_Actual);

                //PASO 3.- OBTENEMOS LOS DIAS A DESCONTAR SI EL EMPLEADO ENTRO A LABORAR DESPUES DEL INICIO DEL AÑO ACTUAL.
                if (INF_EMPLEADO.P_Fecha_Inicio > Fecha_Inicio_Anyo_Actual)
                {
                    //OBTENEMOS LOS DIAS A DESCONTAR [DIAS_DESCONTAR], AL CALCULO DEL AGUINALDO SI EL EMPLEADO COMENZO A LABORAR 
                    //DESPUES DEL [01/01/AÑO_ACTUAL]
                    Dias_Descontar = INF_EMPLEADO.P_Fecha_Inicio.Subtract(Fecha_Inicio_Anyo_Actual).Days + 1;
                    if (Dias_Descontar < 0) Dias_Descontar = 0;
                }

                //PASO 4.- OBTENEMOS LOS [DIAS_TRABAJADOS] DEL EMPLEADO.
                Dias_Trabajados = DIAS_ANIO - (Dias_Descontar + Faltas_Empleado);

                //PASO 5.- OBTENEMOS LOS [DIAS_TOTALES_AGUINALDO] DE AGUINALDO.
                Dias_Totales_Aguinaldo = (INF_TIPO_NOMINA.P_Dias_Aguinaldo * Dias_Trabajados) / DIAS_ANIO;

                //***************** CONSULTA DEL SUELDO CONSIDERANDO LA PREVISIÓN SOCIAL MÚLTIPLE *****************
                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //PASO 8.- SE REALIZA EL CALCULO DEL AGUINALDO.
                Aguinaldo = (Dias_Totales_Aguinaldo) * (Salario_Diario);

                //PASO 9.- OBTENEMOS LA CANTIDAD QUE EXENTA EL AGUINALDO.
                Exenta = (30) * INF_ZONA_ECONOMICA.P_Salario_Diario;

                //PASO 10.- OBTENEMOS LA CANTIDAD QUE GRAVA EL AGUINALDO.
                Grava = Aguinaldo - Exenta;

                /**
                 *  NOTA: SI (EXENTO >= AGUINALDO)
                 *  GRAVA = $ 0.00
                 *  EXENTO = AGUINALDO.
                **/
                if (Exenta >= Aguinaldo)
                {
                    Grava = 0;
                    Exenta = Aguinaldo;
                }

                //PASO 11.- GENERAMOS LA TABLA DE RESULTADOS DEL AGUILADO [AGUINALDO, EXENTA Y GRAVA].
                Dt_Resultado = Crear_Tabla_Resultados(Aguinaldo, Grava, Exenta);

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el aguinaldo. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Fondo_Retiro
        /// DESCRIPCION : 1.- Se consulta el salario diario del empleado.
        ///               2.- Se consulta el [%] PORCENTAJE_FONDO_RETIRO.
        ///               3.- Se calcula el fondo de retiro.
        ///               
        ///         Calculo:
        ///                 Fondo_Retiro = (Salario_Diario * [Dias Periodo Nominal]) * [%] PORCENTAJE_FONDO_RETIRO
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Calculo: 
        ///                      Salario_Diario = (Sueldo_Mensual_Puesto/(30.42))
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Diciembre/2010
        /// MODIFICO          : Juan Alberto Hernandez Negrete.
        /// FECHA_MODIFICO    : 26/Septiembre/2011
        /// CAUSA_MODIFICACION: Integración de PSM al calculo de fondo de retiro.
        ///***********************************************************************************************************************
        public DataTable Calcular_Fondo_Retiro(String Empleado_ID)
        {
            //VARIABLES QUE ALMACENAN INFORMACIÓN QUE SE USARA AL REALIZAR LOS CALCULOS.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable  que almacena la información del empleado.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacena la información del parámetro de nómina.

            ///VARIABLES DE TIPO TABLA QUE ALMACENAN  LA INFORMACION CONSULTADA.
            DataTable Dt_Resultados = null;//Variable que almacenara los resultados de los calculos.

            ///VARIABLES DE TIPO CANTIDAD UTILIZADAS EN EL CALCULO DEL FONDO DE RETIRO DEL EMPLEADO.
            Double Fondo_Retiro = 0.0;//Variable que almacenara la cantidad destina al fondo de retiro del empleado.
            Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado.
            Double Cantidad_Grava = 0.0;//Variable que almacena la cantidad que grava el fondo de retiro
            Double Cantidad_Exenta = 0.0;//Variable que alamcena la cantidad que exenta el fondo de retiro.
            Int32 DIAS_LABORADOS = 0;//Dias laborados en la catorcena.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACIÓN DEL PARÁMETRO DE LA NÓMINA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();

                //Obtener los dias laborados en la catorcena por el empleado.
                DIAS_LABORADOS = Obtener_Dias_Laborados_Empleado(Empleado_ID);

                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO O NIVEL]
                        Salario_Diario = Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //SE CALCULA EL FONDO DE RETIRO.
                Fondo_Retiro = (Salario_Diario * DIAS_LABORADOS) * INF_PARAMETRO.P_Porcentaje_Fondo_Retiro;
                Cantidad_Grava = Fondo_Retiro;
                Cantidad_Exenta = 0;
                Dt_Resultados = Crear_Tabla_Resultados(Fondo_Retiro, Cantidad_Grava, Cantidad_Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el fondo de retiro del empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultados;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Fondo_Retiro
        /// DESCRIPCION : 1.- Se consulta el salario diario del empleado [Salario_Diario].
        ///               2.- Se consulta las faltas que a tenido el empleado por incapacidad [Faltas_Empleado_Incapacidad].
        ///               3.- Se ejecuta el calculo de las incapacidades.
        ///               
        ///         Calculo:
        ///                 Incapacidades = Salario_Diario * Faltas_Empleado_Incapacidad
        ///                 
        ///                 Grava: $0.00
        ///                 Exenta: Incapacidades
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public DataTable Calcular_Incapacidades(String Empleado_ID)
        {
            DataTable Dt_Resultado = null;//Variable que almacenara el resultado del calculo de las incapacidades

            try
            {
                Dt_Resultado = Obtener_Incapacidades_Periodo(Empleado_ID);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Resultado;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Prima_Dominical
        /// DESCRIPCION : 1.- Se consulta y se suman los domingos trabajados del empleado.
        ///               2.- Se consulta el parámetro [%] de Prima Dominical a considerar.
        ///               3.- Se calcula la prima vacacional.
        ///               
        ///         Calculo:
        ///                 Prima_Dominical = Domingos * [%] de Prima_Dominical 
        ///                 
        ///                 Grava: Prima_Dominical
        ///                 Exenta: $0.00
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public DataTable Calcular_Prima_Dominical(String Empleado_ID)
        {
            ///VARIABLES DE CONEXION CON LA CAPA DE NEGOCIOS.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacenara información del parámetro de la nómina.

            ///VARIABLE DE TIPO TABLA QUE ALMACENAN LA INFORMACION CONSULTADA.
            DataTable Dt_Domingos_Trabajados = null;//Variable que almacenara los resultados de la consulta de los domingos trabajados.
            DataTable Dt_Resultados = null;//Variable que almacenra el resultado del calculo de prima dominical

            ///VARIABLES DE TIPO CANTIDAD.
            Double Prima_Dominical = 0.0;//Variable que almacenara la cantidad de prima dominical que le corresponde al empleado.
            Double Domingos_Trabajados = 0;//Variable que almacenara en numero de domingo que el empleado trabajo durante el año.
            Double Cantidad_Grava = 0.0;//Variable que almacena la cantidad que gravaran los domingos.
            Double Cantidad_Exenta = 0.0;//Variable que almacenara la cantidad que exenta los domingos.
            Double Salario_Diario_Sueldo_Mas_PSM = 0.0;//Variable que almacenara el salario diario del empleado.

            try
            {
                //CONSULTAMOS INFORMACIÓN GENERAL PARA CÁLCULO DE LA PRIMA VACACIONAL.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();//CONSULTAMOS LA INFORMACIÓN DEL PARÁMETRO.

                /*
                 * OBTENEMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO
                 * APLICA PARA ISSEG SE CONSIDERE LA PREVISIÓN SOCIAL JUNTO CON EL SUELDO.
                 */
                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario_Sueldo_Mas_PSM = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario_Sueldo_Mas_PSM = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario_Sueldo_Mas_PSM = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //CONSULTAMOS LOS DIAS DOMINGOS LABORADOS POR EL EMPLEADO.
                Dt_Domingos_Trabajados = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Domingos_Empleado_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                if (Dt_Domingos_Trabajados is DataTable)
                {
                    if (Dt_Domingos_Trabajados.Rows.Count > 0)
                        Domingos_Trabajados = Dt_Domingos_Trabajados.Rows.Count;
                    else Domingos_Trabajados = 0;
                }

                //CALCULO DE LA PRIMA DOMINICAL
                Prima_Dominical = (Domingos_Trabajados * Salario_Diario_Sueldo_Mas_PSM) * INF_PARAMETRO.P_Porcentaje_Prima_Dominical;
                Cantidad_Grava = Prima_Dominical;
                Cantidad_Exenta = 0;

                //CREAR TABLA DE RESULTADOS.
                Dt_Resultados = Crear_Tabla_Resultados(Prima_Dominical, Cantidad_Grava, Cantidad_Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular la Prima Dominical. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultados;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Quinquenios
        /// DESCRIPCION : 1.- Este aplica a partir  de que el empleado cumple 5 o más años trabajando para presidencia, esta aplica 
        ///                   solo una vez al mes.
        ///               2.- Se verifica si el empleado pertence algún sindicato y la fecha de ingreso del mismo.
        ///                   a).- Si pertencé algún sindicato y la fecha de ingreso es mayor o igual a 5 años entoncés:
        ///                   
        ///                         * Consulta de acuerdo al sindicato y a los años trabajados la cantidad a otorgar al
        ///                           empleado.
        ///                 
        ///                 Grava: 100%
        ///                 Exenta:  0%
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 20/Diciembre/2010 10:27 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public DataTable Calcular_Quinquenios(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Consulta_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
            Cls_Cat_Nom_Sindicatos_Negocio Consulta_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexion con la capa de negocios.
            Cls_Ope_Nom_Faltas_Empleado_Negocio Consulta_Detalles_Faltas_Empleado = new Cls_Ope_Nom_Faltas_Empleado_Negocio();//Variable de conexion con la capa de negocios.            
            DataTable Dt_Datos_Empleado = null;//Variable que guardara la informacion del empleado.
            DataTable Dt_Antiguedad_Sindicato = null;//Variable que almacenara datos de los detalles de losm sindicatos y las percepciones deducciones.
            DataTable Dt_Resultados = null;//Variable que almacenara las cantidades finales de Quinquenio, Gravante y Exento.
            DateTime Fecha_Ingreso_Empleado = new DateTime();//Variable que almacenara kla fecha de ingreso del empleado.
            DateTime Fecha_Auxiliar = new DateTime();//Variable que almacenara la fecha que tendria el empleado 5 años despues de su ingreso.
            DateTime Fecha_Actual = Fecha_Generar_Nomina;//Variable que almacenara la fecha actual.
            Int32 Anyos_Trabajados = 0;//Variable que almacenara la cantidad de año que tiene el empleado laborando para presidencia.
            Double Grava = 0.0;//Variable que almacenará la cantidad que grava el calculo del quinquenio.
            Double Exenta = 0.0;//Variable que almacenará la cantidad que exenta el calculo del quinquenio.
            Double Cantidad_Quinquenio = 0.0;//Variable que almacenra el valor a entregar al empleado.
            Double Cantidad_Por_Antiguedad = 0.0;//Variable que almacenara la Cantidad Por Antiguedad que el empleado recibira por estar en algun determinado sindicato. 
            String Sindicato_ID = "";//Variable que almacenara el sindicato al que pertenece el empleado.
            String Anios_Antiguedad_Sindicato = String.Empty;

            try
            {
                //Paso 1.- Consultamos la informacion del empleado para obtener su fecha de ingreso.
                Consulta_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Datos_Empleado = Consulta_Empleados.Consulta_Empleados_General();//Ejecutamos la consulta.
                //Validamos que la busqueda encontro resultados.
                if (Dt_Datos_Empleado != null)
                {
                    if (Dt_Datos_Empleado.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString())) 
                            Fecha_Ingreso_Empleado = Convert.ToDateTime(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString());
                        if (!string.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Sindicato_ID].ToString())) 
                            Sindicato_ID = Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Sindicato_ID].ToString();
                    }
                }

                //Paso 2.- Incrementamos 5 años la fecha de ingreso del empleado para saber si el empleado lleva 5 años o mas laborando.
                Fecha_Auxiliar = Fecha_Ingreso_Empleado.AddYears(5);

                //Paso 3.- Validamos que la fecha 5 años depues de la fecha de ingreso del empleado sea menor o igual a la fecha actual.
                if (Fecha_Auxiliar <= Fecha_Actual)
                {
                    //Paso 4.- Obtenemos los años laborados en presidencia del empleado.
                    //Anyos_Trabajados =(Int32) (DateTime.Today.Subtract(Fecha_Ingreso_Empleado).TotalDays/365);
                    Anyos_Trabajados = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Year, Fecha_Ingreso_Empleado, Fecha_Generar_Nomina));
                    //Paso 5.- Consultamos las antiguedades registradas para el sindicato.
                    Consulta_Sindicatos.P_Sindicato_ID = Sindicato_ID;
                    Dt_Antiguedad_Sindicato = Consulta_Sindicatos.Consultar_Antiguedades_Sindicales();

                    if (Dt_Antiguedad_Sindicato is DataTable)
                    {
                        if (Dt_Antiguedad_Sindicato.Rows.Count > 0)
                        {
                            Anios_Antiguedad_Sindicato = Dt_Antiguedad_Sindicato.Compute("MAX(ANIOS)", "ANIOS <=  " + Anyos_Trabajados).ToString();
                            DataRow[] Filas = Dt_Antiguedad_Sindicato.Select("ANIOS = " + Anios_Antiguedad_Sindicato);

                           foreach (DataRow FILA in Filas)
                           {
                               if (FILA is DataRow)
                               {
                                   if (!String.IsNullOrEmpty(FILA["MONTO"].ToString()))
                                   {
                                       Cantidad_Por_Antiguedad = Convert.ToDouble(FILA["MONTO"].ToString().Trim());
                                   }
                               }
                           }
                        }
                    }
                }

                //Validamos el periodo en el que nos encontramos actualmente.
                if ((No_Nomina % 2) != 0)
                {
                    //Paso 8.- Obtenemos la cantidad Cantidad Quinquenio
                    Cantidad_Quinquenio = Cantidad_Por_Antiguedad;
                    Grava = Cantidad_Quinquenio;
                    Exenta = 0;
                }

                //Paso 9.- Creamos la tabla de resultados
                Dt_Resultados = Crear_Tabla_Resultados(Cantidad_Quinquenio, Grava, Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el monto del quinquenio. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultados;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Pago_Dias_Festivos
        /// DESCRIPCION : 1.- Consulta los dias festivos trabajados por el empleado y estos se suman [Dias Festivos].
        ///               2.- Se consulta el salario diario del empleado.
        ///               3.- se realiza el calculo:
        ///                 
        ///         Calculo:
        ///                 Pago_Dias_Festivos = (Dias_Festivos * Salario_Diario) * [2]
        ///                   
        ///                 Grava: 100%
        ///                 Exenta:  0%
        ///                 
        ///         Nota: Solo se aplica una vez al mes y es aplicada a la última catorcena.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 20/Diciembre/2010 10:27 am.
        /// MODIFICO          : Juan alberto Hernandez Negrete.
        /// FECHA_MODIFICO    : 26/Septiembre/2011
        /// CAUSA_MODIFICACION: Integrar PSM al cálculo de días festivos.
        ///***********************************************************************************************************************
        public DataTable Calcular_Pago_Dias_Festivos(String Empleado_ID)
        {
            //VARIABLES QUE ALMACENAN INFORMACION COMO OBJETO.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

            ///VARIABLES QUE ALMACENARAN LOS DATOS DE LA CONSULTA.
            DataTable Dt_Resultado = null;//variable que almacena el resultado del calculo de vacaciones.

            ///VARIABLES DE TIPO CANTIDAD
            Int32 Dias_Festivos = 0;//Variable que almacenara la cantidad de dias de vacaciones que a tomado el empleado.
            Double Salario_Diario_Empleado = 0.0;//Variable que almacenara el salario diario  del empleado de acuerdo al puesto.
            Double Pago_Dias_Festivo = 0.0;//Variable que almacenara la cantidad resultante de las vacaciones.
            Double Grava = 0.0;//Variable que almacenara la cantidad que gravan las vacaciones
            Double Exenta = 0.0;//Variable que almacenara la cantidad que exenta las vacaciones.

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.

                //Obtenemos los dias festivos que a tenido el empleado en la catorcena actual.
                Dias_Festivos = Obtener_Dias_Dias_Festivos_Empleado(Empleado_ID);

                /*
                 * OBTENEMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO
                 * APLICA PARA ISSEG SE CONSIDERE LA PREVISIÓN SOCIAL JUNTO CON EL SUELDO.
                 */
                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario_Empleado = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario_Empleado = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario_Empleado = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //REALIZAMOS EL CALCULO DE PAGO DIAS FESTIVOS.
                Pago_Dias_Festivo = (Dias_Festivos * Salario_Diario_Empleado) * 2;
                Grava = Pago_Dias_Festivo;
                Exenta = 0;

                //CREAMOS LA TABLA DE RESULTADOS.
                Dt_Resultado = Crear_Tabla_Resultados(Pago_Dias_Festivo, Grava, Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el Pago Dias Festivos. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Vacaciones
        /// DESCRIPCION : 1.- se consulta si el empleado tomo vacaciones en la catorcena aplicar, si fue asi cuantos dias tomo.
        ///                   [Dias_Vacaciones]
        ///               2.- Se consulta el salario diario del empleado.
        ///               3.- Se realiza el calculo:
        ///                 
        ///         Calculo:
        ///                 Vacaciones = (Dias_Vacaciones * Salario_Diario)             
        ///                   
        ///                 Grava: 100%
        ///                 Exenta:  0%
        ///                 
        ///         Nota: Solo se aplica una vez al mes y es aplicada a la última catorcena.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 20/Diciembre/2010 10:27 am.
        /// MODIFICO          : Juan Alberto Hernández Negrete.
        /// FECHA_MODIFICO    : 26/Septiembre/2011
        /// CAUSA_MODIFICACION: Integrar PSM al cálculo de vacaciones.
        ///***********************************************************************************************************************
        public DataTable Calcular_Vacaciones(String Empleado_ID)
        {
            //VARIABLE TIPO OBJETO QUE ALMACENA INFORMACION.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

            ///VARIABLES QUE ALMACENARAN LOS DATOS DE LA CONSULTA.
            DataTable Dt_Resultado = null;//variable que almacena el resultado del calculo de vacaciones.

            ///VARIABLES DE TIPO CANTIDAD
            Int32 Dias_Vacaciones = 0;//Variable que almacenara la cantidad de dias de vacaciones que a tomado el empleado.
            Double Salario_Diario_Empleado = 0.0;//Variable que almacenara el salario diario  del empleado de acuerdo al puesto.
            Double Cantidad_Vacaciones = 0.0;//Variable que almacenara la cantidad resultante de las vacaciones.
            Double Grava = 0.0;//Variable que almacenara la cantidad que gravan las vacaciones
            Double Exenta = 0.0;//Variable que almacenara la cantidad que exenta las vacaciones.
            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.

                //OBTENER LOS DIAS DE VACACIONES QUE A TOMADO EL EMPLEADO EN LA CATORCENA ACTUAL
                Dias_Vacaciones = Obtener_Dias_Vacaciones_Periodo_Actual(Empleado_ID);
                //CAMBIAR EL ESTADO DE LAS VACACIONES DEL EMPLEADO EN EL PERIODO
                Cls_Ope_Nom_Percepciones_Datos.Descomprometer_Vacaciones_Empleados(Empleado_ID, Nomina_ID, No_Nomina);

                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO O NIVEL]
                        Salario_Diario_Empleado = Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario_Empleado = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario_Empleado = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //SE REALIZA EL CALCULO DE LAS VACACIONES.
                Cantidad_Vacaciones = (Dias_Vacaciones * Salario_Diario_Empleado);
                Grava = Cantidad_Vacaciones;
                Exenta = 0;
                //OBTENEMOS TABLA DE RESULTADOS DEL CALCULO DE CANTIDAD
                Dt_Resultado = Crear_Tabla_Resultados(Cantidad_Vacaciones, Grava, Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el Vacaciones. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calculo_Dia_Doble
        /// DESCRIPCION : 1.- Se consulta y se suman todas los Dias Doble que se le capturaron al empleado.
        /// 
        ///               Calculo: 
        ///                        Dia_Doble = (Salario_Diario * 2) * Dias_Doble
        ///                        Grava = 100%
        ///                        Exenta= 0%
        ///                        
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se efectuara el calculo del tiempo extra.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete. 
        /// FECHA_CREO  : 12/Enero/2011 10:12 am.
        /// MODIFICO          : Juan Alberto Hernández Negrete.
        /// FECHA_MODIFICO    : 26/Septiembre/2011
        /// CAUSA_MODIFICACION: Integrar PSM al calculo de Día Doble.
        ///************************************************************************************************************************
        public DataTable Calculo_Dia_Doble(String Empleado_ID)
        {
            ///VARIABLES DE CONEXION CON LA CAPA DE NEGOCIOS.
            Cls_Ope_Nom_Tiempo_Extra_Negocio Consulta_Tiempo_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio();//Variables de conexion con la capa de negocios.
            Cls_Ope_Nom_Faltas_Empleado_Negocio Consulta_Calendario_Nomina_Empleados = new Cls_Ope_Nom_Faltas_Empleado_Negocio();//Variable de conexion con la capa de negocios. 
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.

            ///VARIABLES PARA ALMACENAR LOS DATOS DE LAS CONSULTAS.
            DataTable Dt_Tiempo_Extra_Empleado = null;//Variable que almacenara una lista con informacion de las horas extra.

            ///VARIABLES PARA EL CALCULO DEL TIEMPO EXTRA
            Double Cantidad_Pago_Dias_Dobles = 0.0;//Variable que almacenara el tiempo extra trabajado por el empleado.
            Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado.
            Double Cantidad_Grava = 0.0;//Cantidad que grava el tiempo extra.
            Double Cantidad_Exenta = 0.0;//Cantidad que exenta el tiempo extra.

            ///VARIABLE QUE ALAMCENARA EL RESULTADO DE LOS CALCULOS REALIZADOS
            DataTable Dt_Resultado = null;//Variable que almacenra el resultado de los calculos realizados.
            Int32 Contador_Dias_Dobles = 0;//Variable que almacenara el núemro de dias dobles laborados.

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAR LA INFORMACION DEL EMPLEADO.

                //CONSULTAMOS LOS DIAS DOBLES CAPTURADOS AL EMPLEADO.
                Dt_Tiempo_Extra_Empleado = Cls_Ope_Nom_Percepciones_Datos.Consultar_Tiempo_Extra_Empleado_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //EJECUTAMOS VALIDACIÓN PARA VERIFICAR QUE LA CONSULTA HALLA ENCONTRADO RESULTADOS.
                if (Dt_Tiempo_Extra_Empleado is DataTable)
                {
                    //RECORREMOS LOS REGISTROS DE HORAS EXTRAS ENCONTRADOS EN EL SISTEMA EN LA CATORCENA ACTUAL.
                    foreach (DataRow Registro in Dt_Tiempo_Extra_Empleado.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Ope_Nom_Tiempo_Extra.Campo_Pago_Dia_Doble].ToString().Trim()))
                        {
                            if (Registro[Ope_Nom_Tiempo_Extra.Campo_Pago_Dia_Doble].ToString().Trim().ToUpper().Equals("SI"))
                            {
                                Contador_Dias_Dobles = Contador_Dias_Dobles + 1;
                            }
                        }
                    }
                }

                /*
                 * OBTENEMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO
                 * APLICA PARA ISSEG SE CONSIDERE LA PREVISIÓN SOCIAL JUNTO CON EL SUELDO.
                 */
                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //EJECUTAMOS EL CALCULO DEL TIEMPO EXTRA.
                Cantidad_Pago_Dias_Dobles = (Salario_Diario * 2) * Contador_Dias_Dobles;
                Cantidad_Grava = Cantidad_Pago_Dias_Dobles;
                Cantidad_Exenta = 0;

                //Paso 5.- Crear la tabla de resultados del calculo de tiempo extra.
                Dt_Resultado = Crear_Tabla_Resultados(Cantidad_Pago_Dias_Dobles, Cantidad_Grava, Cantidad_Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular los dias dobles laborados por el empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Pago_Ajuste_ISR
        /// DESCRIPCION : 1.- Se Consulta y se suman todas los Dias Doble que se le capturaron al empleado.
        /// 
        ///               Calculo: 
        ///                        Total_Resta_Pagar_Ajuste_ISR = (Total_ISR_Ajustar - Total_ISR_Ajustado)
        ///                        
        ///                        a).- Total_Resta_Pagar_Ajuste_ISR >= Pago_Catorcenal_ISR
        ///                        
        ///                             Total_ISR_Ajustado = ( Total_ISR_Ajustado + Pago_Catorcenal_ISR )
        ///                             
        ///                        b).- Total_Resta_Pagar_Ajuste_ISR = Pago_Catorcenal_ISR
        ///                        
        ///                             Total_ISR_Ajustado = Total_ISR_Ajustar
        ///                        
        ///                        Grava = 0%
        ///                        Exenta= 100%
        ///                        
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se efectuara el calculo del tiempo extra.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete. 
        /// FECHA_CREO  : 12/Enero/2011 10:12 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///************************************************************************************************************************
        public DataTable Pago_Ajuste_ISR(String Empleado_Id)
        {
            Cls_Ope_Nom_Ajuste_ISR_Negocio Modificar_Ajustes_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//variable de conexión con la capa de negocios.
            DataTable Dt_Ajustes_ISR_Empelado = null;//Variable que alamcenrá los registros de Ajuste de ISR para el empleado.
            Double Total_ISR_Ajustar = 0.0;//Variable que almacena la cantidad de ISR que se Ajustara.
            Double Pago_Catorcenal_ISR = 0.0;//El pago Catorcenal que el empleado percibirá. 
            Double Total_ISR_Ajustado = 0.0;//Variable que almacena la cantidad total de ISR Ajustado.
            Int32 No_Pago = 0;//Almacena el número de pagos ya realizados.
            Double Total_Resta_Pagar_Ajuste_ISR = 0.0;//Total de ISR que falta por Ajustar.
            Double Numero_Catorcenas =0.0;//Número de catorcenas en las que se realizara el ajuste de ISR.
            String Estatus = "";//Estatus  Actual del Ajuste de ISR.
            Double Cantidad_Percibira_Empleado_Ajuste_ISR = 0.0;//Cantidad que percibira el empleado catorcenalmente.
            String No_Ajuste_ISR = "";//Identificador del ajuste de ISR.
            DataTable Dt_Resultado = null;//Variable que almacenra el resultado de los calculos realizados.
            StringBuilder Historial_Nomina_Generada = null;//Variable que almacena los registros afectado en la nómina generada.

            try
            {
                //Consultamos los Ajustes de ISR que tiene el empleado.
                Dt_Ajustes_ISR_Empelado = Cls_Ope_Nom_Percepciones_Datos.Consultar_Ajustes_ISR_Empleado(Empleado_Id, Fecha_Generar_Nomina);

                if (Dt_Ajustes_ISR_Empelado != null)
                {
                    foreach (DataRow Renglon in Dt_Ajustes_ISR_Empelado.Rows)
                    {
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString()))
                            No_Ajuste_ISR = Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString();

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString()))
                            Total_ISR_Ajustar = Convert.ToDouble(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString());

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString()))
                            Total_ISR_Ajustado = Convert.ToDouble(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString());

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR].ToString()))
                            Pago_Catorcenal_ISR = Convert.ToDouble(Renglon[Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR].ToString());

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString()))
                            No_Pago = Convert.ToInt32(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString());

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas].ToString()))
                            Numero_Catorcenas = Convert.ToInt32(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas].ToString());

                        //Obtenemos la cantidad de ISR que falta por ajustar.
                        Total_Resta_Pagar_Ajuste_ISR = (Total_ISR_Ajustar - Total_ISR_Ajustado);

                        //Validamos si aún falta pagos por realizarse.
                        if (No_Pago <= Numero_Catorcenas)
                        {
                            No_Pago = No_Pago + 1;

                            if (No_Pago < Numero_Catorcenas)
                            {
                                //Actualizamos el Total de ISR Ajustado.
                                Total_ISR_Ajustado = Total_ISR_Ajustado + Pago_Catorcenal_ISR;
                            }
                            else if (No_Pago == Numero_Catorcenas)
                            {
                                Total_ISR_Ajustado = Total_ISR_Ajustar;
                            }

                            //Validamos el estatus del Ajuste de ISR.
                            if (Total_ISR_Ajustado == Total_ISR_Ajustar)
                            {
                                Estatus = "Pagado";
                            }
                            else
                            {
                                Estatus = "Proceso";
                            }

                            //Establecer los valores para actualiza el Ajuste ISR.
                            Modificar_Ajustes_ISR.P_No_Ajuste_ISR = No_Ajuste_ISR;
                            Modificar_Ajustes_ISR.P_Estatus_Ajuste_ISR = Estatus;
                            Modificar_Ajustes_ISR.P_No_Pago = No_Pago;
                            Modificar_Ajustes_ISR.P_Total_ISR_Ajustado = Total_ISR_Ajustado;
                            //Se ejecuta la actualización del Ajuste de ISR.
                            Cls_Ope_Nom_Percepciones_Datos.Pago_Ajustes_ISR_Empleado(Modificar_Ajustes_ISR);

                            Cantidad_Percibira_Empleado_Ajuste_ISR = Pago_Catorcenal_ISR;
                        }
                    }
                    if (Dt_Ajustes_ISR_Empelado.Rows.Count > 0)
                    {
                        Historial_Nomina_Generada = Cls_Sessiones.Historial_Nomina_Generada;
                        Cls_Historial_Nomina_Generada.Crear_Registro_Insertar_Ajuste_ISR(Dt_Ajustes_ISR_Empelado, ref Historial_Nomina_Generada);
                        Cls_Sessiones.Historial_Nomina_Generada = Historial_Nomina_Generada;
                    }
                }

                //Crear la tabla de resultados del calculo de tiempo extra.
                Dt_Resultado = Crear_Tabla_Resultados(Cantidad_Percibira_Empleado_Ajuste_ISR, 0, Cantidad_Percibira_Empleado_Ajuste_ISR);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al realizar el pago de ISR. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Pago_Ajuste_ISR_Finiquito
        /// DESCRIPCION : 1.- Se Consulta y se suman todas los Dias Doble que se le capturaron al empleado.
        /// 
        ///               Calculo: 
        ///                        Total_Resta_Pagar_Ajuste_ISR = (Total_ISR_Ajustar - Total_ISR_Ajustado)
        ///                        
        ///                        a).- Total_Resta_Pagar_Ajuste_ISR >= Pago_Catorcenal_ISR
        ///                        
        ///                             Total_ISR_Ajustado = ( Total_ISR_Ajustado + Pago_Catorcenal_ISR )
        ///                             
        ///                        b).- Total_Resta_Pagar_Ajuste_ISR = Pago_Catorcenal_ISR
        ///                        
        ///                             Total_ISR_Ajustado = Total_ISR_Ajustar
        ///                        
        ///                        Grava = 0%
        ///                        Exenta= 100%
        ///                        
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se efectuara el calculo del tiempo extra.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete. 
        /// FECHA_CREO  : 12/Enero/2011 10:12 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///************************************************************************************************************************
        public DataTable Pago_Ajuste_ISR_Finiquito(String Empleado_Id)
        {
            Cls_Ope_Nom_Ajuste_ISR_Negocio Modificar_Ajustes_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//variable de conexión con la capa de negocios.
            DataTable Dt_Ajustes_ISR_Empelado = null;//Variable que alamcenrá los registros de Ajuste de ISR para el empleado.
            Double Total_ISR_Ajustar = 0.0;//Variable que almacena la cantidad de ISR que se Ajustara.
            Double Pago_Catorcenal_ISR = 0.0;//El pago Catorcenal que el empleado percibirá. 
            Double Total_ISR_Ajustado = 0.0;//Variable que almacena la cantidad total de ISR Ajustado.
            Int32 No_Pago = 0;//Almacena el número de pagos ya realizados.
            Double Total_Resta_Pagar_Ajuste_ISR = 0.0;//Total de ISR que falta por Ajustar.
            Double Numero_Catorcenas = 0.0;//Número de catorcenas en las que se realizara el ajuste de ISR.
            String No_Ajuste_ISR = "";//Identificador del ajuste de ISR.
            DataTable Dt_Resultado = null;//Variable que almacenra el resultado de los calculos realizados.
            StringBuilder Historial_Nomina_Generada = null;//Variable que almacena los registros afectado en la nómina generada.

            try
            {
                //Consultamos los Ajustes de ISR que tiene el empleado.
                Dt_Ajustes_ISR_Empelado = Cls_Ope_Nom_Percepciones_Datos.Consultar_Ajustes_ISR_Empleado_Finiquito(Empleado_Id, Fecha_Generar_Nomina);

                if (Dt_Ajustes_ISR_Empelado != null)
                {
                    foreach (DataRow Renglon in Dt_Ajustes_ISR_Empelado.Rows)
                    {
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString()))
                            No_Ajuste_ISR = Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString();

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString()))
                            Total_ISR_Ajustar = Convert.ToDouble(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString());

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString()))
                            Total_ISR_Ajustado = Convert.ToDouble(Renglon[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString());

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR].ToString()))
                            Pago_Catorcenal_ISR = Convert.ToDouble(Renglon[Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR].ToString());

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString()))
                            No_Pago = Convert.ToInt32(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString());

                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas].ToString()))
                            Numero_Catorcenas = Convert.ToInt32(Renglon[Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas].ToString());

                        //Obtenemos la cantidad de ISR que falta por ajustar.
                        Total_Resta_Pagar_Ajuste_ISR = (Total_ISR_Ajustar - Total_ISR_Ajustado);
                    }
                    if (Dt_Ajustes_ISR_Empelado.Rows.Count > 0)
                    {
                        Historial_Nomina_Generada = Cls_Sessiones.Historial_Nomina_Generada;
                        Cls_Historial_Nomina_Generada.Crear_Registro_Insertar_Ajuste_ISR(Dt_Ajustes_ISR_Empelado, ref Historial_Nomina_Generada);
                        Cls_Sessiones.Historial_Nomina_Generada = Historial_Nomina_Generada;
                    }
                }

                //Crear la tabla de resultados del calculo de tiempo extra.
                Dt_Resultado = Crear_Tabla_Resultados(Total_Resta_Pagar_Ajuste_ISR, 0, Total_Resta_Pagar_Ajuste_ISR);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al realizar el pago de ISR. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        #endregion

        #region (Metodos Generales)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Salario_Diario_Empleado
        /// DESCRIPCION : Obtenemos el salario diario del empleado segun su puesto actual.
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Calculo: 
        ///                      Salario_Diario = (Sueldo_Mensual_Puesto/(365/12))
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 16/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///******************************************************************************* 
        public Double Obtener_Salario_Diario_Empleado(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Consulta_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
            DataTable Dt_Empleados = null;//Variable que almacenara los datos del empleado consultado.
            DataTable Dt_Datos_Puesto = null;//Variable que almacenara la informacion del puesto del e            
            String Salario_Mensual_Puesto = "";//Variable que almacenara el salario del puesto del empleado.
            String Puesto_Empleado = "";//Variable que almacenara el Puesto_ID
            Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado que le corresponde segun el puesto.
            Double DIAS_MES = Cls_Utlidades_Nomina.Dias_Mes_Fijo;//Variable que almacenara los dias del mes que se tomaran para obtener el salario diario mensual.
            Cls_Cat_Empleados_Negocios INF_EMPLEADOS = null;

            try
            {
                //Consultamos el puesto del empleado que tiene actualmente.
                Consulta_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Empleados = Consulta_Empleados.Consulta_Empleados_General();

                //Validamos que exista algun registro que corresponda con el id del empleado buscado.
                if (Dt_Empleados is DataTable)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleados.Rows)
                        {
                            //Obtenemos el puesto del empleado, que nos servira para obtener el salario mensual que le corresponde al puesto.
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Puesto_ID].ToString().Trim()))
                                Puesto_Empleado = EMPLEADO[Cat_Empleados.Campo_Puesto_ID].ToString().Trim();
                        }
                    }
                }

                //Consultar el salario mensual del puesto, para obtener el salario diario del empleado.
                Consulta_Empleados.P_Puesto_ID = Puesto_Empleado;
                Dt_Datos_Puesto = Consulta_Empleados.Consulta_Puestos_Empleados();//Consultamos la informacion del puesto

                //Validamos que exista algun resultado que corresponda con el id del puesto buscado.
                if (Dt_Datos_Puesto is DataTable)
                {
                    if (Dt_Datos_Puesto.Rows.Count > 0)
                    {
                        foreach (DataRow PUESTO in Dt_Datos_Puesto.Rows)
                        {
                            if (PUESTO is DataRow)
                            {
                                //Obtenemos el salario diario del empleado, esto de acuerdo al salario mensual que le corresponde al puesto.
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Salario_Mensual].ToString().Trim()))
                                    Salario_Mensual_Puesto = HttpUtility.HtmlDecode(PUESTO[Cat_Puestos.Campo_Salario_Mensual].ToString().Trim());

                                if (!String.IsNullOrEmpty(Salario_Mensual_Puesto))
                                {
                                    Salario_Diario = (Convert.ToDouble(Salario_Mensual_Puesto) / DIAS_MES);
                                }
                                else
                                {
                                    INF_EMPLEADOS = Consultar_Inf_Empleado(Empleado_ID);
                                    Salario_Diario = INF_EMPLEADOS.P_Salario_Diario;
                                }
                            }
                        }
                    }
                    else
                    {
                        Salario_Diario = 0.0;
                    }
                }
                else
                {
                    Salario_Diario = 0.0;
                }

                if (Salario_Diario == 0)
                {
                    INF_EMPLEADOS = Consultar_Inf_Empleado(Empleado_ID);
                    Salario_Diario = Convert.ToDouble((String.IsNullOrEmpty(INF_EMPLEADOS.P_Salario_Diario.ToString()) ? "0" : INF_EMPLEADOS.P_Salario_Diario.ToString()));
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el salario diario del empleado. Error: [" + Ex.Message + "]");
            }
            return Salario_Diario;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Verifica_Dia_Laboral_Empleado
        /// DESCRIPCION : Verifica si el dia de la semana es un dia laboral para el empleado.
        /// PARAMETROS:  DayOfWeek Dia.- Dia de la semana a evaluar.
        ///              Dias_Laborales_Semana.- Programacion de dias laborales del empleado.
        ///              
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 15/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///******************************************************************************* 
        private Boolean Verifica_Dia_Laboral_Empleado(DayOfWeek Dia, Boolean[] Dias_Laborales_Semana)
        {
            Boolean Resultado = false;//Variable que almacenara el valor si es o no un dia laboral para el empleado.
            //Se realiza la validacion del dia laboral del empleado.
            switch (Dia)
            {
                case DayOfWeek.Monday:
                    if (Dias_Laborales_Semana[0]) Resultado = true;
                    break;
                case DayOfWeek.Tuesday:
                    if (Dias_Laborales_Semana[1]) Resultado = true;
                    break;
                case DayOfWeek.Wednesday:
                    if (Dias_Laborales_Semana[2]) Resultado = true;
                    break;
                case DayOfWeek.Thursday:
                    if (Dias_Laborales_Semana[3]) Resultado = true;
                    break;
                case DayOfWeek.Friday:
                    if (Dias_Laborales_Semana[4]) Resultado = true;
                    break;
                case DayOfWeek.Saturday:
                    if (Dias_Laborales_Semana[5]) Resultado = true;
                    break;
                case DayOfWeek.Sunday:
                    if (Dias_Laborales_Semana[6]) Resultado = true;
                    break;
                default:
                    Resultado = false;
                    break;
            }
            return Resultado;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Es_Laboral
        /// DESCRIPCION : Valida si el dia pasado como parametro del empleado es un dia laboral
        /// para el mismo.
        /// PARAMETROS: Empleado_ID.- Empleado al que se consultara el dia laboral.
        ///             Dia_Semana.- Dia a evaluar si es un dia laboral o no.
        ///             
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 15/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************  
        private Boolean Es_Laboral(String Empleado_ID, DayOfWeek Dia_Semana)
        {
            Cls_Cat_Empleados_Negocios Cat_Empleados_Consulta = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
            Boolean[] Dias_Laborales_Semana = new Boolean[7];//Varuable que almacenara la configuracion laboral de los dias de la semana para el empleado.
            DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados consultados.
            Boolean LUNES = false;//Dia Lunes
            Boolean MARTES = false;//Dia Martes
            Boolean MIERCOLES = false;//Dia Miercoles
            Boolean JUEVES = false;//Dia Jueves
            Boolean VIERNES = false;//Dia Viernes
            Boolean SABADO = false;//Dia Sabado
            Boolean DOMINGO = false;//Dia Domingo
            Boolean Dia_Laboral = false;//Variable que almacenara si el dia de la semana es un dia laboral o no.

            try
            {
                Cat_Empleados_Consulta.P_Empleado_ID = Empleado_ID;//Establecemos el no de empleado a consultar.
                Dt_Empleados = Cat_Empleados_Consulta.Consulta_Datos_Empleado();//Ejecutamos la consulta de los empleados.
                //Validamos que la consulta halla encontrado resultados.
                if (Dt_Empleados != null)
                {
                    //Vallidamos que por lo menos exista un registro.
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        //Obtenemos la configuracion laboral de los dias que si labora el empleado por semana.
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Lunes].ToString())) LUNES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Lunes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Martes].ToString())) MARTES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Martes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Miercoles].ToString())) MIERCOLES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Miercoles].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Jueves].ToString())) JUEVES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Jueves].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Viernes].ToString())) VIERNES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Viernes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sabado].ToString())) SABADO = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sabado].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Domingo].ToString())) DOMINGO = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Domingo].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    }
                }
                //Pasamos la configuracion laboral a la variable que los almacenara.
                Dias_Laborales_Semana[0] = LUNES;
                Dias_Laborales_Semana[1] = MARTES;
                Dias_Laborales_Semana[2] = MIERCOLES;
                Dias_Laborales_Semana[3] = JUEVES;
                Dias_Laborales_Semana[4] = VIERNES;
                Dias_Laborales_Semana[5] = SABADO;
                Dias_Laborales_Semana[6] = DOMINGO;

                Dia_Laboral = Verifica_Dia_Laboral_Empleado(Dia_Semana, Dias_Laborales_Semana);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dia_Laboral;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Crear_Tabla_Resultados
        /// DESCRIPCION : Crea la tabla de resultados de operacion calculada y su 
        ///               respectiva cantidad de gravante y exento.
        ///               
        /// PARAMETROS: Resultado.- Es el dato calculado.
        ///             Grava.- Cantidad que grava.
        ///             Exenta.- Cantidad que exenta.
        ///             
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 16/Diciembre/2010 4:26 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************  
        private DataTable Crear_Tabla_Resultados(Double Resultado, Double Grava, Double Exenta)
        {
            DataTable Dt_Resultado = new DataTable();//Variable que alamcenara la Prima Vacacional, Cantidad Gravante y Cantidad Exenta.
            DataRow Renglon = null;//Variable que almacenara los datos de los calculos y que se agregara a la tabla de resultados.
            try
            {
                Dt_Resultado.Columns.Add("Calculo", typeof(System.Double));
                Dt_Resultado.Columns.Add("Grava", typeof(System.Double));
                Dt_Resultado.Columns.Add("Exenta", typeof(System.Double));

                //Llenamos el Dt_Resultado con las cantidades calculadas.
                Renglon = Dt_Resultado.NewRow();
                Renglon["Calculo"] = Resultado;
                Renglon["Grava"] = Grava;
                Renglon["Exenta"] = Exenta;
                Dt_Resultado.Rows.Add(Renglon);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al crear la tabla de resultados. Errot: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Faltas_Empleados
        /// DESCRIPCION : Obtiene la cantidad de faltas que ha tenido el empleado
        ///               entre la fecha inicio y final de la busqueda.
        ///               
        /// PARAMETROS: Empleado_ID.- Empleado sobre el cial se hará la busqueda de faltas.
        ///             Fecha_Inicia.- Fecha donde comenzara la busqueda de falta del empleado.
        ///             Fecha_Termina.- Fecha donde terminara la busqueda de faltas del empleado.
        ///             
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 16/Diciembre/2010 4:26 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************  
        public Int32 Obtener_Faltas_Empleados(String Empleado_ID, DateTime Fecha_Inicio, DateTime Fecha_Fin)
        {
            ///Variables de conexion con la capa de negocios.
            Cls_Ope_Nom_Faltas_Empleado_Negocio Consulta_Detalles_Faltas_Empleado = new Cls_Ope_Nom_Faltas_Empleado_Negocio();//Variable de conexion con la capa de negocios.           
            ///Variables de tipo tabla para almacenra las consultas.
            DataTable Dt_Faltas_Empelado = null;//Variable que almacenara la lista de faltas del empleado.
            ///Variables utilizadas para obtener las faltas del empleado.                        
            Int32 Faltas_Empleado = 0;//Variable que almacenara la cantidad de faltas que a tenido el empleado en la catorcena anterior.
            Double Minutos_Retardo = 0.0;//Almacenar la cantidad de minutos de retardo.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacena la informacion del parametro de nómina.
            Double PARAMETRO_MINUTOS_RETARDO = 0.0;//Variable que almacena el numero de minutos que se tomaran por considerar los retardos como faltas.

            try
            {
                //CONSULTAR INFORMACIÓN DE LOS PARÁMETROS DE NÓMINA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();
                PARAMETRO_MINUTOS_RETARDO = Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_Minutos_Retardo)) ? "0" : INF_PARAMETRO.P_Minutos_Retardo);

                //Consultamos las faltas que a tenido el empleado en la catorcena anterior..
                Consulta_Detalles_Faltas_Empleado.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Fecha_Inicio);
                Consulta_Detalles_Faltas_Empleado.P_Fecha_Fin = string.Format("{0:dd/MM/yyyy}", Fecha_Fin);
                Consulta_Detalles_Faltas_Empleado.P_Empleado_ID = Empleado_ID;
                Consulta_Detalles_Faltas_Empleado.P_Estatus = "Autorizado";
                Dt_Faltas_Empelado = Consulta_Detalles_Faltas_Empleado.Consultar_Faltas_Empelado();

                //Validamos que la busqueda halla tenido resultados
                if (Dt_Faltas_Empelado is DataTable)
                {
                    foreach (DataRow Renglon in Dt_Faltas_Empelado.Rows)
                    {
                        if (Renglon is DataRow)
                        {
                            if (Renglon[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString().Equals("INASISTENCIA") ||
                                Renglon[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString().Equals("JUSTIFICADA"))
                            {
                                Faltas_Empleado += 1;
                            }
                            else if (Renglon[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString().Equals("RETARDO"))
                            {
                                if (Renglon[Ope_Nom_Faltas_Empleado.Campo_Retardo].ToString().Equals("SI"))
                                {

                                    Minutos_Retardo = Convert.ToDouble(Renglon[Ope_Nom_Faltas_Empleado.Campo_Cantidad].ToString());

                                    if (Minutos_Retardo > PARAMETRO_MINUTOS_RETARDO)
                                    {
                                        Faltas_Empleado += 1;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener las faltas del empleado en la catorcena anterior. Error: [" + Ex.Message + "]");
            }
            return Faltas_Empleado;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Faltas_Empleados
        /// DESCRIPCION : Obtiene la cantidad de faltas que ha tenido el empleado
        ///               entre la fecha inicio y final de la busqueda.
        ///               
        /// PARAMETROS: Empleado_ID.- Empleado sobre el cial se hará la busqueda de faltas.
        ///             Fecha_Inicia.- Fecha donde comenzara la busqueda de falta del empleado.
        ///             Fecha_Termina.- Fecha donde terminara la busqueda de faltas del empleado.
        ///             Tipo_Falta.- Tipo de Falta por lo que se hara la busqueda de las falatas del empleado. 
        ///             
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 16/Diciembre/2010 4:26 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///******************************************************************************* 
        private Int32 Obtener_Faltas_Empleados(String Empleado_ID, String Tipo_Falta)
        {
            ///Variables de tipo tabla para almacenra las consultas.
            DataTable Dt_Faltas_Empleado = null;//Variable que almacenara la lista de faltas del empleado.
            ///Variables utilizadas para obtener las faltas del empleado.                        
            Int32 Faltas_Empleado = 0;//Variable que almacenara la cantidad de faltas que a tenido el empleado en la catorcena anterior.

            try
            {
                Dt_Faltas_Empleado = Cls_Ope_Nom_Percepciones_Datos.Consultar_Faltas_Empleado_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //Validamos que la busqueda halla tenido resultados
                if (Dt_Faltas_Empleado is DataTable)
                {
                    foreach (DataRow Falta in Dt_Faltas_Empleado.Rows)
                    {
                        if (Falta is DataRow)
                        {
                            if (Falta[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString().Equals(Tipo_Falta))
                            {
                                Faltas_Empleado += 1;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener las faltas del empleado en la catorcena anterior. Error: [" + Ex.Message + "]");
            }
            return Faltas_Empleado;
        }
        ///***************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Faltas_Empleados
        /// DESCRIPCION : Obtiene las faltas que el empleado tuvo en la catorcena anterior al periodo actual.
        ///               
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cial se hará la busqueda de faltas.
        ///             
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 16/Diciembre/2010 4:26 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///**************************************************************************************************************************** 
        private Int32 Obtener_Faltas_Empleados(String Empleado_ID)
        {
            ///Variables de tipo tabla para almacenra las consultas.
            DataTable Dt_Faltas_Empleado = null;//Variable que almacenara la lista de faltas del empleado.
            ///Variables utilizadas para obtener las faltas del empleado.                        
            Int32 Faltas_Empleado = 0;//Variable que almacenara la cantidad de faltas que a tenido el empleado en la catorcena anterior.
            Double Minutos_Retardo = 0.0;//Almacenar la cantidad de minutos de retardo.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacena la informacion del parametro de nómina.
            Double PARAMETRO_MINUTOS_RETARDO = 0.0;//Variable que almacena el numero de minutos que se tomaran por considerar los retardos como faltas.

            try
            {
                //CONSULTAR INFORMACIÓN DE LOS PARÁMETROS DE NÓMINA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();
                PARAMETRO_MINUTOS_RETARDO = Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_Minutos_Retardo)) ? "0" : INF_PARAMETRO.P_Minutos_Retardo);

                //Consultamos las faltas que ha tenido el empleado en el periodo a generar la nómina.
                Dt_Faltas_Empleado = Cls_Ope_Nom_Percepciones_Datos.Consultar_Faltas_Empleado_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //Validamos que la busqueda halla tenido resultados
                if (Dt_Faltas_Empleado is DataTable)
                {
                    foreach (DataRow Renglon in Dt_Faltas_Empleado.Rows)
                    {
                        if (Renglon is DataRow)
                        {
                            if (Renglon[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString().Equals("INASISTENCIA") ||
                                Renglon[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString().Equals("JUSTIFICADA"))
                            {
                                Faltas_Empleado += 1;
                            }
                            else if (Renglon[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString().Equals("RETARDO"))
                            {
                                if (Renglon[Ope_Nom_Faltas_Empleado.Campo_Retardo].ToString().Equals("SI"))
                                {

                                    Minutos_Retardo = Convert.ToDouble(Renglon[Ope_Nom_Faltas_Empleado.Campo_Cantidad].ToString());

                                    if (Minutos_Retardo > PARAMETRO_MINUTOS_RETARDO)
                                    {
                                        Faltas_Empleado += 1;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener las faltas del empleado en la catorcena anterior. Error: [" + Ex.Message + "]");
            }
            return Faltas_Empleado;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Dias_Vacaciones_Periodo_Actual
        /// DESCRIPCION : Consulta los dias de vacaciones que a tomado el empleado en la catorcena actual.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 20/Diciembre/2010 10:27 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public Int32 Obtener_Dias_Vacaciones_Periodo_Actual(String Empleado_ID)
        {       
            DataTable Dt_Vacaciones = null;//Variable que alamcenara las vacaciones que ha tomado el empleado en la catorcena.
            Int32 Dias_Vacaciones = 0;//Variable que alamacenara los dias de vacaciones que ha tomado el empleado en esta catorcena.

            try
            {
                //Consulta de los dias de vacaciones que tomo el empleado y aplican para el periodo a generar la nomina.
                Dt_Vacaciones = Cls_Ope_Nom_Percepciones_Datos.Consultar_Vacaciones_Empleado_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina, Fecha_Generar_Nomina);

                if (Dt_Vacaciones is DataTable) {
                    if (Dt_Vacaciones.Rows.Count > 0) {
                        Dias_Vacaciones = Dt_Vacaciones.Rows.Count;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al consultar los dias de vacaciones del empleado en la catorcena actual. Error: [" + Ex.Message + "]");
            }
            return Dias_Vacaciones;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Dias_Vacaciones_Periodo_Actual
        /// DESCRIPCION : Consulta los dias de vacaciones que a tomado el empleado en la catorcena actual.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 20/Diciembre/2010 10:27 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public Int32 Obtener_Dias_Vacaciones_Periodo_Actual(String Empleado_ID, Boolean Actualizar_Estatus_Dias_Vacaciones)
        {
            DataTable Dt_Vacaciones = null;//Variable que alamcenara las vacaciones que ha tomado el empleado en la catorcena.
            Int32 Dias_Vacaciones = 0;//Variable que alamacenara los dias de vacaciones que ha tomado el empleado en esta catorcena.

            try
            {
                //Consulta de los dias de vacaciones que tomo el empleado y aplican para el periodo a generar la nomina.
                Dt_Vacaciones = Cls_Ope_Nom_Percepciones_Datos.Consultar_Vacaciones_Empleado_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina, Fecha_Generar_Nomina);

                if (Dt_Vacaciones is DataTable)
                {
                    foreach (DataRow Vacacion in Dt_Vacaciones.Rows)
                    {
                        if (Vacacion is DataRow)
                        {
                            if (Actualizar_Estatus_Dias_Vacaciones)
                            {
                                if (!string.IsNullOrEmpty(Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_No_Dia_Vacacion].ToString()))
                                {
                                    Cls_Ope_Nom_Percepciones_Datos.Cambiar_Estatus_Vacaciones(Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_No_Dia_Vacacion].ToString());
                                    Dias_Vacaciones += 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al consultar los dias de vacaciones del empleado en la catorcena actual. Error: [" + Ex.Message + "]");
            }
            return Dias_Vacaciones;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Dias_Dias_Festivos_Empleado
        /// DESCRIPCION : Consulta los dias festivos que a tomado el empleado en la catorcena actual.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 20/Diciembre/2010 10:27 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        private Int32 Obtener_Dias_Dias_Festivos_Empleado(String Empleado_ID)
        {           
            DataTable Dt_Dias_Festivos = null;//Variable que almacena los datos de los dias festivos consultados.
            Int32 Dias_Festivos = 0;//Variable que almacenara los dias festivos que el empleado a tomado en el periodo actual.

            try
            {
                Dt_Dias_Festivos = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Festivos_Empleado_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //Validamos que la busqueda halla encontrado resultados.
                if (Dt_Dias_Festivos is DataTable)
                    if (Dt_Dias_Festivos.Rows.Count > 0)
                        Dias_Festivos = Dt_Dias_Festivos.Rows.Count;                
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias festivos del empleado. Error: [" + Ex.Message + "]");
            }
            return Dias_Festivos;
        }
        /// **********************************************************************************************************************
        /// Nombre: Obtener_Dias_Incapacidades
        /// 
        /// Descripción: Obtiene los dias de incapacidad que ha tomado el empleado descontando cuando aplica pago cuarto día.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado para uso interno del sistema.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 28/Septiembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// **********************************************************************************************************************
        private Double Obtener_Dias_Incapacidades(String Empleado_ID)
        {
            ///VARIABLE QUE ALMACENARA EL RESULTADO DEL CALCULO.
            DataTable Dt_Incapacidades = null;//Variable que almacenara las incapacidades de los empleados.
            Double Cantidad_Dias = 0;//Variable que almacena la cantidad de dias de incapacidad del empleado.
            Double Contador_Dias = 0.0;//Variable que almacenara la cantidad de dias que el empleado estuvo de incapacidad.

            try
            {
                //CONSULTAMOS LA INCAPACIDADES QUE HA TENIDO EL EMPLEADO EN LA NOMINA A GENERAR.
                Dt_Incapacidades = Cls_Ope_Nom_Percepciones_Datos.Consultar_Incapacidades(Nomina_ID, No_Nomina, Empleado_ID);

                if (Dt_Incapacidades is DataTable)
                {
                    if (Dt_Incapacidades.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon in Dt_Incapacidades.Rows)
                        {
                            if (Renglon is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Aplica_Pago_Cuarto_Dia].ToString().Trim()))
                                {
                                    if (Renglon[Ope_Nom_Incapacidades.Campo_Aplica_Pago_Cuarto_Dia].ToString().Trim().ToUpper().Equals("SI") &&
                                        Renglon[Ope_Nom_Incapacidades.Campo_Extencion_Incapacidad].ToString().Trim().ToUpper().Equals("NO"))
                                    {
                                        if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString().Trim()))
                                        {
                                            Cantidad_Dias = Convert.ToInt32(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString());
                                            Cantidad_Dias = Cantidad_Dias - 3;
                                            Contador_Dias += ((Cantidad_Dias < 0) ? 0 : Cantidad_Dias);
                                        }
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString().Trim()))
                                        {
                                            Cantidad_Dias = Convert.ToInt32(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString());
                                            Contador_Dias += Cantidad_Dias;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las incapacidades que a tenido el empleado en el periodo. Error: [" + Ex.Message + "]");
            }
            return Contador_Dias;
        }
        /// **********************************************************************************************************************
        /// Nombre: Obtener_Dias_Incapacidades
        /// 
        /// Descripción: Obtiene los dias de incapacidad que ha tomado el empleado descontando cuando aplica pago cuarto día.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado para uso interno del sistema.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 28/Septiembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// **********************************************************************************************************************
        private Double Obtener_Dias_Incapacidades_Sin_Excluir(String Empleado_ID)
        {
            ///VARIABLE QUE ALMACENARA EL RESULTADO DEL CALCULO.
            DataTable Dt_Incapacidades = null;//Variable que almacenara las incapacidades de los empleados.
            Double Cantidad_Dias = 0;//Variable que almacena la cantidad de dias de incapacidad del empleado.
            Double Contador_Dias = 0.0;//Variable que almacenara la cantidad de dias que el empleado estuvo de incapacidad.

            try
            {
                //CONSULTAMOS LA INCAPACIDADES QUE HA TENIDO EL EMPLEADO EN LA NOMINA A GENERAR.
                Dt_Incapacidades = Cls_Ope_Nom_Percepciones_Datos.Consultar_Incapacidades(Nomina_ID, No_Nomina, Empleado_ID);

                if (Dt_Incapacidades is DataTable)
                {
                    if (Dt_Incapacidades.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon in Dt_Incapacidades.Rows)
                        {
                            if (Renglon is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Aplica_Pago_Cuarto_Dia].ToString().Trim()))
                                {
                                    if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString().Trim()))
                                    {
                                        Contador_Dias += Convert.ToInt32(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString().Trim());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las incapacidades que a tenido el empleado en el periodo. Error: [" + Ex.Message + "]");
            }
            return Contador_Dias;
        }
        /// **********************************************************************************************************************
        /// Nombre: Obtener_Cantidad_Tabla_Resultados
        /// 
        /// Descripción: Obtiene de la tabla de resutados la cantidad.
        /// 
        /// Parámetros: Dt_Resultado.- Tabla con la información de resultado de algun cálculo.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 28/Septiembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// **********************************************************************************************************************
        private Double Obtener_Cantidad_Tabla_Resultados(DataTable Dt_Resultado)
        {
            Double Cantidad = 0.0;//Variable que almacenara la cantidad.

            try
            {
                if (Dt_Resultado is DataTable)
                {
                    if (Dt_Resultado.Rows.Count > 0)
                    {
                        foreach (DataRow RESULTADO in Dt_Resultado.Rows)
                        {
                            if (RESULTADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(RESULTADO["Calculo"].ToString().Trim()))
                                    Cantidad = Convert.ToDouble(RESULTADO["Calculo"].ToString().Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad de la tabla de resultado. Error: [" + Ex.Message + "]");
            }
            return Cantidad;
        }
        ///****************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Incapacidades_Periodo
        /// 
        /// DESCRIPCION : Consulta los dias de incapacidad que existen autorizados para el periodo de generacion de nomina
        ///               actual.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificador del empleado.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***************************************************************************************************************************************
        private DataTable Obtener_Incapacidades_Periodo(String Empleado_ID)
        {
            ///VARIABLES DE TIPO CANTIDAD UTILIZADAS EN EL CALCULO.
            Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado segun su puesto.
            Double Incapacidades = 0.0;//Variable que almacenara el calculo de las incapacidades.
            Double Cantidad_Grava = 0.0;//Variable que almacena la cantidad que grava las incapacidades
            Double Cantidad_Exenta = 0.0;//Variable que almacena la cantidad que exenta las incapacidades

            ///VARIABLE QUE ALMACENARA EL RESULTADO DEL CALCULO.
            DataTable Dt_Resultado = null;//Variable que almacenara el resultado del calculo de las incapacidades
            DataTable Dt_Incapacidades = null;//Variable que almacenara las incapacidades de los empleados.
            long Cantidad_Dias = 0;//Variable que almacena la cantidad de dias de incapacidad del empleado.
            Double Porcentaje_Incapacidad = 0.0;//Variable que almacenara el procentaje de la incapacidad.
            Cls_Cat_Empleados_Negocios INF_EMPLEADOS = null;//Variable que almacena la información de los empleados.
            
            try
            {
                INF_EMPLEADOS = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.

                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADOS.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADOS.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        // OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario = Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //CONSULTAMOS LA INCAPACIDADES QUE HA TENIDO EL EMPLEADO EN LA NOMINA A GENERAR.
                Dt_Incapacidades = Cls_Ope_Nom_Percepciones_Datos.Consultar_Incapacidades(Nomina_ID, No_Nomina, Empleado_ID);

                if (Dt_Incapacidades is DataTable) {
                    if (Dt_Incapacidades.Rows.Count > 0) {
                        foreach (DataRow Renglon in Dt_Incapacidades.Rows) {
                            if (Renglon is DataRow) {
                                if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Aplica_Pago_Cuarto_Dia].ToString().Trim())) {
                                    if (Renglon[Ope_Nom_Incapacidades.Campo_Aplica_Pago_Cuarto_Dia].ToString().Trim().ToUpper().Equals("SI") &&
                                        Renglon[Ope_Nom_Incapacidades.Campo_Extencion_Incapacidad].ToString().Trim().ToUpper().Equals("NO"))
                                    {
                                        if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString().Trim()))
                                        {
                                            Cantidad_Dias = Convert.ToInt32(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString());
                                            Cantidad_Dias = Cantidad_Dias - 3;

                                            if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Porcentaje_Incapacidad].ToString().Trim()))
                                            {
                                                Porcentaje_Incapacidad = Convert.ToDouble(Renglon[Ope_Nom_Incapacidades.Campo_Porcentaje_Incapacidad].ToString().Trim());

                                                //CALCULAMOS LAS INCAPACIDADES.
                                                Incapacidades += ((Salario_Diario * Cantidad_Dias) * (Porcentaje_Incapacidad / 100));
                                            }
                                        }
                                    }
                                    else {
                                        if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString().Trim()))
                                        {
                                            Cantidad_Dias = Convert.ToInt32(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString());

                                            if (!String.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Porcentaje_Incapacidad].ToString().Trim()))
                                            {
                                                Porcentaje_Incapacidad = Convert.ToDouble(Renglon[Ope_Nom_Incapacidades.Campo_Porcentaje_Incapacidad].ToString().Trim());

                                                //CALCULAMOS LAS INCAPACIDADES.
                                                Incapacidades += ((Salario_Diario * Cantidad_Dias) * (Porcentaje_Incapacidad / 100));
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Cantidad_Grava = 0;
                        Cantidad_Exenta = Incapacidades;
                        //Paso 4.- Crear la tabla de resultados del calculo realizado.
                        Dt_Resultado = Crear_Tabla_Resultados(Incapacidades, Cantidad_Grava, Cantidad_Exenta);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las incapacidades que a tenido el empleado en el periodo. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// *****************************************************************************************************************
        /// NOMBRE: Obtener_Dias_Laborados_Empleado
        /// 
        /// DESCRIPCIÓN: obtiene los dias laborados del empleado descontando las faltas que ha tenido el mismo durante
        ///              el periodo a generar.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del empleado para realizar las operaciones del sistema.
        /// 
        /// USUARIO CREO: Juan Alberto Hernandez Negrete.
        /// FECHA CROE: 26/Septiembre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// *****************************************************************************************************************
        private Int32 Obtener_Dias_Laborados_Empleado(String Empleado_ID)
        {
            Int32 Numero_Faltas_Empleado = 0;//Variable que guarda el número de faltas que ha tenido el empleado.
            Int32 DIAS_TOTALES = 0;//Número de dias que tiene la catorcena.
            Int32 Dias_Laborados = 0;//Número de dias laborados por el empleado.

            try
            {
                DIAS_TOTALES = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);
                //Consultamos el número de faltas que ha tenido el empleado.
                Numero_Faltas_Empleado = Obtener_Faltas_Empleados(Empleado_ID);
                //Obtenemos el número de dias laborados por el empleado.
                Dias_Laborados = (DIAS_TOTALES - Numero_Faltas_Empleado);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias laborados del empleado. Error: [" + Ex.Message + "]");
            }
            return Dias_Laborados;
        }
        /// **********************************************************************************************
        ///  Nombre: Obtener_Cantidad_Grava_Sueldo
        ///  
        ///  Descripción: Método que devuelve la cantidad que gravara el empleado por concepto del sueldo.
        ///  
        ///  Parámetros: Cantidad_Sueldo.- Cantidad que se le pagara al empleado por concepto de sueldo. 
        ///              Empleado_ID.- Identificador del empleado.
        /// 
        ///  Usuario Creo: Juan Alberto Hernández Negrete.
        ///  Fecha Creo: 24/Noviembre/2011.
        ///  Usuario Modifico:
        ///  Fecha Modifico:
        /// **********************************************************************************************
        private Double Obtener_Cantidad_Grava_Sueldo(Double Cantidad_Sueldo, String Empleado_ID)
        {
            Cls_Ope_Nom_Deducciones_Negocio Obj_Negocio_Deducciones = new Cls_Ope_Nom_Deducciones_Negocio();//Variable de conexión a la capa de negocios.
            Double Cantidad_Grava_Sueldo = 0.0;//Variable que almacenara la cantidad total que gravara el sueldo.
            Double Cantidad_Inasistencias = 0.0;//Variable que almacenara la cantidad total que se le descontara al empleado por concepto de faltas.

            try
            {
                //Obtenemos la cantidad que se le retuvo al empleado por concepto de inasistencias.
                Cantidad_Inasistencias = Obj_Negocio_Deducciones.Calcular_Inasistencias(Empleado_ID);
                //Obtenemos la cantidad que gravara el sueldo, ya  considerando el total del sueldo menos las faltas que tuvo el empleado.
                Cantidad_Grava_Sueldo = Cantidad_Sueldo - Cantidad_Inasistencias;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad que grava el sueldo. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Grava_Sueldo;
        }
        #endregion

        #region (Variables Privadas)
        private DateTime Fecha_Inicia_Periodo_Nominal;
        private DateTime Fecha_Final_Periodo_Nominal;
        private String Nomina_ID = "";                  //Variable que almacenará el Identificador de la nómina.
        private Int32 No_Nomina = 0;                    //Variable que almacena el numero de catorcena de la cual se desea generar la nómina.
        private String Detalle_Nomina_ID = "";          //Variable que identifica el perido seleccionado para generar la nómina.
        private String Tipo_Nomina_ID = "";             //Variable que almacena el tipo de nómina de la cual se desea generar la nómina.
        #endregion

        #region (Variables Públicas)
        public DateTime P_Fecha_Inicia_Periodo_Nominal
        {
            get { return Fecha_Inicia_Periodo_Nominal; }
            set { Fecha_Inicia_Periodo_Nominal = value; }
        }

        public DateTime P_Fecha_Final_Periodo_Nominal
        {
            get { return Fecha_Final_Periodo_Nominal; }
            set { Fecha_Final_Periodo_Nominal = value; }
        }

        public DateTime P_Fecha_Generar_Nomina
        {
            get { return Fecha_Generar_Nomina; }
            set { Fecha_Generar_Nomina = value; }
        }

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
        #endregion

        #region (Finiquitos)
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Aguinaldo
        /// DESCRIPCION : 1.- Se obtiene de acuerdo al tipo de nomina los dias de aguinaldo a considerar [Dias_Aguinaldo].
        ///               2.- Se restan 365 dias las faltas que tuvo el empleado durante todo el año si
        ///                   es que el empleado tiene una fecha de ingreso menor al año actual.
        ///               
        ///                     Fecha_Ingreso menor o igual 01/Enero/Año
        ///                     Dias_Trabajados = 365 - Faltas_Empleado
        ///                
        ///                3.- Si el empleado comenzo a trabajar despues del 01/Enero/Año  se debe restar a 365
        ///                    dias, los dias 01/Enero/Año actual menos fecha de ingreso, para poder determinar cuales
        ///                    seran los dias que deberan ser descontados a 365 dias, restando tabien las faltas que tuvo
        ///                    el empleado.
        ///                
        ///                     Dias_Descontar = Fecha_Ingreso_Empleado.Subtract(Fecha_Inicio_Año_Actual).Days
        ///                     Dias_Trabajados = (DIAS) - (Dias_Descontar + Faltas_Empleado) 
        ///                    
        ///                4.- Se obtiene la parte proporcional de los dias a considerar para el aguinaldo.
        ///               
        ///                     Dias_Aguinaldo  --> 365
        ///                            X        --> Dias_Trabajados
        ///                            
        ///                     Nota: En donde (X) son los Dias_Totales_Aguinaldo.
        ///                     
        ///                 5.- Se consulta como parametro el salario de la zona (Salario_Zona).
        ///                 6.- Se consulta el salario diario del empleado.
        ///                 7.- Se realiza el calculo del aguinaldo.
        ///                     
        ///                 Calculo:
        ///                 
        ///                     Aguinaldo = (Dias_Totales_Aguinaldo * Salario_Diario)
        ///                     Exenta = 30 * Salario_Zona
        ///                     Grava = Aguinaldo - Exenta
        ///                     
        ///                     Si Exenta >= Aguinaldo entonces Aguinaldo Grava $0.00 y Exenta 100% 
        ///                     
        ///        IMPORTANTE: 1.- Si el empleado cambio de tipo de nómina se deberá considerar como fecha de 
        ///                        ingreso cuando ocurrio este cambio y se calcula su aguinaldo proporcional.
        ///                    2.- Lo correspodiente a su tipo de nómina anterior queda cubierto con el finiquito
        ///                        que se le dio al empleado al darlo de baja.
        ///                        
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se efectuara el calculo del tiempo extra.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete. 
        /// FECHA_CREO  : 16/Diciembre/2010 10:12 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///************************************************************************************************************************
        public DataTable Calcular_Aguinaldo_Finiquito(String Empleado_ID)
        {
            ///VARIABLE QUE ALMACENARA LOS RESULTADOS OBTENIDOS DEL CALCULO DEL AGUINALDO.
            DataTable Dt_Resultado = null;//Variable que alamcenara la Prima Vacacional, Cantidad Gravante y Cantidad Exenta.

            ///VARIABLES DE TIPO FECHA QUE SE UTILIZARAN EL EL CALCULO DEL AGUINALDO.
            DateTime Fecha_Inicio_Anyo_Actual = new DateTime(Fecha_Generar_Nomina.Year, 1, 1, 0, 0, 0);//Variable que almacenara la fecha de inicio que se considera para el calculo del aguinaldo del año actual.            
            DateTime Fecha_Fin_Anyo_Actual = new DateTime(Fecha_Generar_Nomina.Year, Fecha_Generar_Nomina.Month, Fecha_Generar_Nomina.Day, 23, 59, 59);//Variable que se usara como fecha de inicio para la busqueda de faltas del empleado.

            ///VARIABLES DE TIPO INT32 QUE ALMCENARAN CANTIDADES DE DIAS QUE SE OCUPARAN PARA EL CALCULO DEL AGUINALDO.
            Double Dias_Trabajados = 0;//Variable que almacenar la cantidad de dias trabajados del empleado en año actual.
            Double Faltas_Empleado = 0;//Variable que almacenara los dias que el empleado a faltado en el año actual.
            Double Dias_Descontar = 0;//Variable que almacenara los dias que se le descontaran al empleado si entro en una fecha posterios al 01/01/Año.
            Double Dias_Totales_Aguinaldo = 0;//Variable que almacenara los dias de aguinaldo que le corresponden al empleado.

            ///VARIABLES DE TIPO DOUBLE QUE ALMACENARÁN CANTIDADES QUE SE OCUPARÁN PARA EL CALCULO DEL AGUINALDO.
            Double Salario_Diario = 0.0;//Variable que almacena el salario diario del empleado.
            Double Aguinaldo = 0.0;//Variable que almacenara la cantidad de aguinaldo que el empleado recibira.
            Double Grava = 0.0;//Variable que almacenra la cantidad gravante.
            Double Exenta = 0.0;//Variable que almacenara la cantidad exenta. 
            Int32 DIAS_VAN_ANIO = 0;//Variable que almacena los dias que van del año actual.
            Int32 DIAS_ANIO = 0;//Variable que almacena los dias totales del año.

            //VARIABLES DE NEGOCIO 
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            Cls_Cat_Tipos_Nominas_Negocio INF_TIPO_NOMINA = null;//Variable que almacenara la información del tipo de nómina.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null; //Variable que almacena la infomacion del parámetro de nómina.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//Variable que almacena la información de la zona economica.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACIÓN DEL TIPO DE NÓMINA.
                INF_TIPO_NOMINA = _Informacion_Tipo_Nomina(INF_EMPLEADO.P_Tipo_Nomina_ID);
                //CONSULTAMOS LA INFORMACIÓN DELA ZONA ECONÓMICA.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();
                //CONSULTAMOS LA INFORMACIÓN DEL PARÁMETRO DE LA NÓMINA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();

                //DIAS TOTALES DEL AÑO ACTUAL A GENERAR EL FINIQUITO.
                DIAS_ANIO = (Int32)(new DateTime(Fecha_Generar_Nomina.Year, 12, 31, 23, 59, 59).Subtract(Fecha_Inicio_Anyo_Actual).Days + 1);//aJUSTE

                //OBTENEMOS LOS DIAS QUE VAN DEL AÑO A GENERAR EL FINIQUITO.
                DIAS_VAN_ANIO = (Int32)(Fecha_Fin_Anyo_Actual.Subtract(Fecha_Inicio_Anyo_Actual).Days);

                //PASO 2.- OBTENEMOS LAS FALTAS QUE A TENIDO EL EMPLEADO EN AÑO ACTUAL [FALTAS_EMPLEADO].
                Faltas_Empleado = Obtener_Faltas_Empleados(Empleado_ID, Fecha_Inicio_Anyo_Actual, Fecha_Fin_Anyo_Actual);

                //PASO 3.- OBTENEMOS LOS DIAS A DESCONTAR SI EL EMPLEADO ENTRO A LABORAR DESPUES DEL INICIO DEL AÑO ACTUAL.
                if (INF_EMPLEADO.P_Fecha_Inicio > Fecha_Inicio_Anyo_Actual)
                {
                    //OBTENEMOS LOS DIAS A DESCONTAR [DIAS_DESCONTAR], AL CALCULO DEL AGUINALDO SI EL EMPLEADO COMENZO A LABORAR 
                    //DESPUES DEL [01/01/AÑO_ACTUAL]
                    Dias_Descontar = INF_EMPLEADO.P_Fecha_Inicio.Subtract(Fecha_Inicio_Anyo_Actual).Days + 1;
                }

                //PASO 4.- OBTENEMOS LOS [DIAS_TRABAJADOS] DEL EMPLEADO.
                Dias_Trabajados = DIAS_VAN_ANIO - (Dias_Descontar + Faltas_Empleado);

                //PASO 5.- OBTENEMOS LOS [DIAS_TOTALES_AGUINALDO] DE AGUINALDO.
                Dias_Totales_Aguinaldo = (INF_TIPO_NOMINA.P_Dias_Aguinaldo * Dias_Trabajados) / DIAS_ANIO;

                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    // OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //PASO 8.- SE REALIZA EL CALCULO DEL AGUINALDO.
                Aguinaldo = (Dias_Totales_Aguinaldo) * (Salario_Diario);

                //PASO 9.- OBTENEMOS LA CANTIDAD QUE EXENTA EL AGUINALDO.
                Exenta = (30) * INF_ZONA_ECONOMICA.P_Salario_Diario;

                //PASO 10.- OBTENEMOS LA CANTIDAD QUE GRAVA EL AGUINALDO.
                Grava = Aguinaldo - Exenta;

                /**
                 *  NOTA: SI (EXENTO >= AGUINALDO)
                 *  GRAVA = $ 0.00
                 *  EXENTO = AGUINALDO.
                **/
                if (Exenta >= Aguinaldo)
                {
                    Grava = 0;
                    Exenta = Aguinaldo;
                }

                //PASO 11.- GENERAMOS LA TABLA DE RESULTADOS DEL AGUILADO [AGUINALDO, EXENTA Y GRAVA].
                Dt_Resultado = Crear_Tabla_Resultados(Aguinaldo, Grava, Exenta);

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el aguinaldo finiquito. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// **********************************************************************************************************************
        /// Nombre: Calcular_Prima_Vacacional
        /// 
        /// Descripción: Aquí se realzia el cálculo de la prima vacacional 1ra y 2da parte.
        /// 
        /// Parámetros: Empleado_ID.- Identificador interno del empleado.
        /// 
        /// Usuario Creó: Juan Alberto Hernandez Negrete.
        /// Fecha Creó: 21/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// **********************************************************************************************************************
        public DataTable Calcular_Prima_Vacacional_Finiquito(String Empleado_ID)
        {
            //VARIABLE DE NEGOCIO.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//VARIABLE QUE ALMACENARA LA INF. DEL EMPELADO.
            Cls_Cat_Tipos_Nominas_Negocio INF_TIPO_NOMINA = null;//VARIABLE QUE ALMACENARA LA INF. DEL TIPO DE NÓMINA.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//VARIABLE QUE ALMACENARA LA INF. DEL PARÁMETRO DE LA NÓMINA.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//VARIABLE QUE ALMACENARA LA INF.DE LA ZONA ECONÓMICA.

            ///VARIABLES QUE SE UTILIZARAN PARA EL CALCULO DE LA PRIMA VACACIONAL
            Int32 Dias_Totales = 0;//Variable que almacenara la cantidad de dias del periodo Enero - Junio
            Int32 Dias_Trabajados = 0;//Variable que almacenara los dias trabajados por el empleado una vez ya descontadas las faltas que a tenido el mismo.
            Int32 Faltas_Empleados = 0;//Variable que almacenara las falta que ha tenido el empleado en el periodo a evaluar.
            Double Dias_Prima_Vacacional = 0;//Variable que almacenara la cantidad de dias de vacaciones por periodo.
            Double Dias_Totales_Prima_Vacacional = 0.0;//Variable que almacenara los dias a considerar para el calculo de la prima vacacional.
            Double Dias_Exenta_Prima_Vacacional = 0;//Variable que almacena los dias que exenta la prima vacacional.

            ///VARIABLE DE TIPO CANTIDAD QUE SE OCUPARAN EN EL CALCULO DE LA PRIMA VACACIONAL.
            Double Prima_Vacacional = 0.0;//Cantidad que el empleado tendra derecho a percibir como prima vacacional.
            Double Cantidad_Grava = 0.0;//Cantidad que gravara la prima vaccaional.
            Double Cantidad_Exenta = 0.0;//Cantidad que exentara la prima vacacional.
            Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado.

            ///VARIABLE DE TIPO FECHA QUE SE OCUPARAN EN EL CALCULO DE LA PRIMA VACACIONAL.
            DateTime Fecha_Inicia = new DateTime();//Variable que almacenara la fecha de inicio del periodo para el calculo de la prima vacacional.
            DateTime Fecha_Fin = new DateTime();//Variable que almacenara la fecha de fin del periodo para el calculo de la prima vacacional.
            TimeSpan Diferencia_Dias = new TimeSpan();//Obtiene un objeto del tipo TimeSpan que almacena los valores de la diferencia de fechas.

            ///VARIABLE QUE ALMACENARA LOS RESULTADOS OBTENIDOS.
            DataTable Dt_Resultado = null;//Variable que alamcenara la Prima Vacacional, Cantidad Gravante y Cantidad Exenta.
            Int32 Dias_Descontar = 0;//Variable que almacena la cantidad de dias que se le descontaran si el año de ingreso del empleado. es el actual.
            Int32 Periodo = 0;//Variable que almacenara la información acerca de cuál es el periodo vacacional actual.
            Int32 Dias_Van_Periodo_Actual = 0;//Variable que almacenara los días que van del periodo actual.

            try
            {
                //CONSULTAR INFORMACION REQUERIDA PARA EL CALCULO DE PRIMA VACACIONAL.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INF. DEL EMPLEADO.
                INF_TIPO_NOMINA = _Informacion_Tipo_Nomina(INF_EMPLEADO.P_Tipo_Nomina_ID);//CONSULTAMOS LA INF.DEL TIPO DE NÓMINA.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();//CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONÓMICA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();//CONSULTAMOS LA INF. DEL PARÁMETRO DE LA NÓMINA.

                //OBTENEMOS EL PERIODO VACACIONAL ACTUAL.
                Periodo = Obtener_Periodo_Vacacional(Fecha_Generar_Nomina);

                //PASO I.-VALIDAMOS Y OBTENEMOS LAS FECHAS DE ACUERDO AL PERIODO AL CUAL SE CALCULARA LA PRIMA VACACIONAL.
                if (Periodo == 1)
                {
                    //PERIODO VACACIONAL [ENERO - JUNIO]
                    Fecha_Inicia = new DateTime(Fecha_Generar_Nomina.Year, 1, 1, 0, 0, 0);
                    Fecha_Fin = new DateTime(Fecha_Generar_Nomina.Year, 6, 30, 23, 59, 59);
                }
                else if (Periodo == 2)
                {
                    //PERIODO VACACIONAL [JULIO - DICIEMBRE]
                    Fecha_Inicia = new DateTime(Fecha_Generar_Nomina.Year, 7, 1, 0, 0, 0);
                    Fecha_Fin = new DateTime(Fecha_Generar_Nomina.Year, 12, 31, 11, 59, 59);
                }

                if (INF_EMPLEADO.P_Fecha_Inicio.Year == Fecha_Inicia.Year)
                {
                    if (INF_EMPLEADO.P_Fecha_Inicio.Month >= Fecha_Inicia.Month)
                    {
                        if (INF_EMPLEADO.P_Fecha_Inicio.Day >= Fecha_Inicia.Day)
                        {
                            //SI EL EMPLEADO INGRESO EN EL AÑO ACTUAL SE OBTIENE LOS DIAS DEL INICIO DE AÑO Y SU FECHA DE 
                            //INGRESO PARA REALIZAR EL DESCUENTO DE ESO DIAS Y NO CONSIDERARLOS EN EL CÁLCULO.
                            Dias_Descontar = INF_EMPLEADO.P_Fecha_Inicio.Subtract(Fecha_Inicia).Days + 1;
                        }
                    }
                }

                //OBTENEMOS LOS DÍAS QUE HAN TRANCURRIDO DEL PERIODO VACACIONAL ACTUAL.
                Dias_Van_Periodo_Actual = INF_EMPLEADO.P_Fecha_Baja.Subtract(Fecha_Inicia).Days + 1;
                if (Dias_Van_Periodo_Actual < 0) Dias_Van_Periodo_Actual = 0;

                //PASO 2.- OBTENER LOS DIAS TOTALES EN EL PERIODO.
                Diferencia_Dias = Fecha_Fin.Subtract(Fecha_Inicia);
                Dias_Totales = Diferencia_Dias.Days + 1;

                //PASO 3.- CONSULTAMOS LAS FALTAS QUE A TENIDO EL EMPLEADO EN EL PERIODO
                Faltas_Empleados = Obtener_Faltas_Empleados(Empleado_ID, Fecha_Inicia, Fecha_Fin);

                //PASO 4.- OBTENEMOS DIAS TABAJADOS DEL EMPLEADO.
                Dias_Trabajados = (Dias_Van_Periodo_Actual - (Faltas_Empleados + Dias_Descontar));

                //SE OBTIENEN LOS DIAS A CONSIDERAR PARA EL CALCULO DE LA PRIMA VACACIONAl PRIMERA O SEGUNDA PARTE
                //[PARÁMETRO POR TIPO DE NÓMINA].
                switch (Periodo)
                {
                    case 1:
                        Dias_Prima_Vacacional = INF_TIPO_NOMINA.P_Dias_Prima_Vacacional_1;
                        break;
                    case 2:
                        Dias_Prima_Vacacional = INF_TIPO_NOMINA.P_Dias_Prima_Vacacional_2;
                        break;
                    default:
                        break;
                }

                //OBTENEMOS LOS DÍAS QUE EXENTA LA PRIMA VACACIONAL [PARÁMETRO POR TIPO DE NÓMINA].
                Dias_Exenta_Prima_Vacacional = INF_TIPO_NOMINA.P_Dias_Exenta_Prima_Vacacional;

                //PASO 7.- OBTENEMOS LOS DIAS DE PRIMA VACACIONAL.
                Dias_Totales_Prima_Vacacional = (Dias_Prima_Vacacional * Dias_Trabajados) / Dias_Totales;

                /*
                 * OBTENEMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO APLICA PARA [ISSEG]
                 * SE CONSIDERARA LA PREVISIÓN MÚLTIPLE AL OBTENER EL SALARIO DIARIO DEL EMPLEADO.
                */
                
                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //PASO 11.- SE EJECUTA EL CALCULO DE LA PRIMA VACACIONAL.
                Prima_Vacacional = (Dias_Totales_Prima_Vacacional * Salario_Diario) * INF_PARAMETRO.P_Porcentaje_Prima_Vacacional;

                //PASO 12.- OBTENEMOS LA CANTIDAD EXENTA LA PRIMA VACACIONAL. 
                Cantidad_Exenta = (Dias_Exenta_Prima_Vacacional) * INF_ZONA_ECONOMICA.P_Salario_Diario;

                //PASO 13.- OBTENEMOS LA CANTIDAD GRAVANTE DE LA PRIMA VACACIONAL.
                Cantidad_Grava = Prima_Vacacional - Cantidad_Exenta;

                //ESTE PASO SE AGREGO PARA EVITAR QUE EXISTA UNA CANTIDAD QUE GRAVE DE FORMA NEGATIVA.
                if (Cantidad_Exenta > Prima_Vacacional) {
                    Cantidad_Grava = 0;
                    Cantidad_Exenta = Prima_Vacacional;
                }

                //PASO 14.- OBTENEMOS LA TABLA DE RESULTADOS.
                Dt_Resultado = Crear_Tabla_Resultados(Prima_Vacacional, Cantidad_Grava, Cantidad_Exenta);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular la prima vacacional finiquito. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// **********************************************************************************************************************
        /// Nombre: Calculo_Indemnizacion
        /// 
        /// Descripción: Aquí se realzia el cálculo de la indemnización de los empleados.
        /// 
        /// Parámetros: Empleado_ID.- Identificador interno del empleado.
        /// 
        /// Usuario Creó: Juan Alberto Hernandez Negrete.
        /// Fecha Creó: 21/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// **********************************************************************************************************************
        public DataTable Calculo_Indemnizacion(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();//Variable de conexión a la capa de negocios.
            DataTable Dt_Resultado = null;//Variable que almacenra el resultado de los calculos realizados.
            String INDEMNIZACION_ID = String.Empty;//Variable que almacenara el identificador del tipo de indemnizacion.
            Int32 Dias_Indemnizacion = 0;//Variable que almacenara los dias de indemnizacion que le corresponderan al empleado.
            Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado.
            Double INDEMNIZACION = 0.0;//Variable que almacenara la cantidad deindemnización que le correspondera al empleado.
            Double GRAVA = 0.0;//Cantidad que grava la indemnizacion al empleado.
            Double EXENTA = 0.0;//Cantidad que exenta la indemnización del empleado.

            try
            {
                //PASO I.- Consultamos la información del empleado.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

                //PASO II.- Obtenemos los dias de indemnizacion que le corresponde al empleado 
                //          de acuerdo al tipo de finiquito seleccionado.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Tipo_Finiquito))
                    Dias_Indemnizacion = Obtener_Dias_Indemnizacion(INF_EMPLEADO.P_Tipo_Finiquito.Trim());

                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //PASO IV.- ejecutamos la formúla para el cálculo de indemnización.
                INDEMNIZACION = (Salario_Diario * Dias_Indemnizacion);
                GRAVA = INDEMNIZACION;
                EXENTA = 0;

                //Paso 5.- Crear la tabla de resultados del calculo de indemnización.
                Dt_Resultado = Crear_Tabla_Resultados(INDEMNIZACION, GRAVA, EXENTA);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar el calculo de indemnización. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// **********************************************************************************************************************
        /// Nombre: Calculo_Prima_Antiguedad
        /// 
        /// Descripción: Se realiza el cálculo de la prima de antiguedad.
        /// 
        /// Parámetros: Empleado_ID.- Identificador interno del empleado.
        ///             Fecha_Finiquito.- Fecha en que se realizara el finiquito del empleado.
        ///             Tipo_Salario_Aplica.- Indica si el calculo se realizara sobre el salario diario SMG o 
        ///                                   sobre el SMG2.
        /// 
        /// Usuario Creó: Juan Alberto Hernandez Negrete.
        /// Fecha Creó: 21/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// **********************************************************************************************************************
        public DataTable Calculo_Prima_Antiguedad(String Empleado_ID, DateTime Fecha_Finiquito, String Tipo_Salario_Aplica)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//Variable que almacena la información de zona económica a la que pertence el empleado.

            Double Salario_Diario_Empleado = 0.0;//Variable que almacenara el salario diario del empleado.
            Double SMG = 0.0;//Variable que almacenara el salario de la zona.
            Double SMG2 = 0.0;//Variable que almacenara el salario minimo de la zona por 2.
            Double PRIMA_ANTIGUEDAD = 0.0;//Cantidad calculada por concepto de prima de antiguedad.
            Double GRAVA = 0.0;//Cantidad que grava la prima de antiguedad.
            Double EXENTA = 0.0;//Cantidad que exenta la prima de antiguedad.
            Int32 Dias_Antiguedad_Empleado = 0;//Variable que almacena los dias de antiguedad del empleado.
            DateTime? Fecha_Finiquito_Empleado = Fecha_Finiquito;//Variable que almacena la fecha en se realiza el finiquito del empleado.
            DataTable Dt_Resultado = null;//Variable que almacenra el resultado de los calculos realizados.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACION DE LA ZONA ECONÓMICA DEL EMPLEADO.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();

                //PASO II.- OBTENEMOS LOS DIAS QUE LE CORRESPONDEN AL EMPLEADO COMO PAGO DE PRIMA DE ANTIGUEDAD.
                Dias_Antiguedad_Empleado = Obtener_Dias_Prima_Antiguedad(INF_EMPLEADO.P_Empleado_ID, INF_EMPLEADO.P_Tipo_Nomina_ID,
                    INF_EMPLEADO.P_Fecha_Inicio, ((DateTime)Fecha_Finiquito_Empleado));


                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario_Empleado = Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario_Empleado = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario_Empleado = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                //PASO IV.- Obtenemos el salario diario de la zona economica.
                SMG = INF_ZONA_ECONOMICA.P_Salario_Diario;//Obtenemos el SMG
                SMG2 = (SMG * 2);//Calculamos el Tope 2 SMGZ

                //PASO V.- Se ejecuta el calculo de la prima de antiguedad del empleado.
                switch (Tipo_Salario_Aplica)
                {
                    case "SMG2":
                        PRIMA_ANTIGUEDAD = (Dias_Antiguedad_Empleado * SMG2);
                        GRAVA = PRIMA_ANTIGUEDAD;
                        EXENTA = 0;
                        break;
                    case "Salario Diario":
                        PRIMA_ANTIGUEDAD = (Dias_Antiguedad_Empleado * Salario_Diario_Empleado);
                        GRAVA = PRIMA_ANTIGUEDAD;
                        EXENTA = 0;
                        break;
                    default:
                        break;
                }

                //Paso 5.- Crear la tabla de resultados del calculo de prima de antiguedad.
                Dt_Resultado = Crear_Tabla_Resultados(PRIMA_ANTIGUEDAD, GRAVA, EXENTA)   ;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el cálculo de la PRIMA DE ANTIGUEDAD. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// **********************************************************************************************************************
        /// Nombre: Consultar_Inf_Empleado
        /// 
        /// Descripción: Método en el que se realiza la consulta de la información del empleado.
        /// 
        /// Parámetros: Empleado_ID.- Identificador interno del sistema para manipular al empleado.
        /// 
        /// Usuario Creó: Juan Alberto Hernandez Negrete.
        /// Fecha Creó: 21/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// **********************************************************************************************************************
        public Cls_Cat_Empleados_Negocios Consultar_Inf_Empleado(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion a la capa de negocios.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();//Variable que almacenara la información del empleado.
            DataTable Dt_Empleado = null;//Variable que almacenara la información del empleado.

            try
            {
                Obj_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();

                if (Dt_Empleado is DataTable) {
                    if (Dt_Empleado.Rows.Count > 0) {
                        foreach (DataRow EMPLEADO in Dt_Empleado.Rows) {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim()))
                                    INF_EMPLEADO.P_No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Inicio].ToString().Trim()))
                                    INF_EMPLEADO.P_Fecha_Inicio = Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Inicio].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Tipo_Nomina_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Zona_ID = EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Finiquito].ToString().Trim()))
                                    INF_EMPLEADO.P_Tipo_Finiquito = EMPLEADO[Cat_Empleados.Campo_Tipo_Finiquito].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Baja].ToString().Trim()))
                                    INF_EMPLEADO.P_Fecha_Baja = Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Baja].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()))
                                    INF_EMPLEADO.P_Salario_Diario = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim());
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
        /// **********************************************************************************************************************
        /// Nombre: Obtener_Dias_Prima_Antiguedad
        /// 
        /// Descripción: Se consulta los dias de prima de antiguedad que le corresponden al empelado de acuerdo al tipo de
        ///              nómina al que pertenece el empleado.
        /// 
        /// Parámetros: Empleado_ID.- Identificador interno del empleado.
        ///             Tipo_Nomina_ID.- Tipo de nómina al que pertence el empleado.
        ///             Fecha_Inicio.- Fecha en que el empleado comenzo a laborar.
        ///             Fecha_Finiquito.- Fecha en que se dará de baja al empleado.
        /// 
        /// Usuario Creó: Juan Alberto Hernandez Negrete.
        /// Fecha Creó: 21/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// **********************************************************************************************************************
        public Int32 Obtener_Dias_Prima_Antiguedad(String Empleado_ID, String Tipo_Nomina_ID,
            DateTime Fecha_Inicio, DateTime Fecha_Finiquito)
        {
            Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Tipos_Nominas = null;//Variable que almacenara la información del tipo de nómina consultada.
            Int32 DIAS_PRIMA_ANTIGUEDAD = 0;//Variable que almacenara los dias de prima de antiguedad que le corresponden al empleado de acuerdo al TN que pertence.
            Int32 Anio_Ingreso = 0;//Variable que almacena el año de ingreso del empleado.
            Int32 Anio_Finiquito = 0;//Variable que almacenara la fecha en que se aplicara el finiquito.
            Int32 Dias_Acumulados = 0;//Variable que almacena los dias a considerar para el finiquito.

            try
            {
                Obj_Tipos_Nominas.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Tipos_Nominas = Obj_Tipos_Nominas.Consulta_Datos_Tipo_Nomina();

                if (Dt_Tipos_Nominas is DataTable) {
                    if (Dt_Tipos_Nominas.Rows.Count > 0) {
                        foreach (DataRow TIPO_NOMINA in Dt_Tipos_Nominas.Rows) {
                            if (TIPO_NOMINA is DataRow) {
                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim()))
                                    DIAS_PRIMA_ANTIGUEDAD = Convert.ToInt32(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim());
                            }
                        }
                    }
                }

                Anio_Ingreso = Fecha_Inicio.Year;//Año de ingreso del empleado.
                Anio_Finiquito = Fecha_Finiquito.Year;//Año de finiquito del empleado.

                for (Int32 Contador_Anios = Anio_Ingreso; Contador_Anios <= Anio_Finiquito; Contador_Anios++) {
                    if (Contador_Anios < Anio_Finiquito) {
                        if (Contador_Anios > Anio_Ingreso) {
                            Dias_Acumulados += DIAS_PRIMA_ANTIGUEDAD;
                        }
                        else if (Contador_Anios == Anio_Ingreso) {
                            DateTime _Fecha_Fin = new DateTime(Contador_Anios, 12, 31, 23, 59, 59);
                            Int32 Dias_Diferencia = (Int32)(DateDiff.Cls_DateAndTime.DateDiff(DateInterval.Day, Fecha_Inicio, _Fecha_Fin) + 1);

                            Dias_Acumulados += ((DIAS_PRIMA_ANTIGUEDAD * Dias_Diferencia) / 365);
                        }
                    }
                    else if (Contador_Anios == Anio_Finiquito) {
                        DateTime _Fecha_Inicio = new DateTime(Contador_Anios, 1, 1, 0, 0, 0);
                        Int32 Dias_Diferencia = (Int32)(DateDiff.Cls_DateAndTime.DateDiff(DateInterval.Day, _Fecha_Inicio, Fecha_Finiquito) + 1);

                        Dias_Acumulados += ((DIAS_PRIMA_ANTIGUEDAD * Dias_Diferencia) / 365);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener los dias que le corresponden al empleado por concepto de prima de antiguedad. Error: [" + Ex.Message + "]");
            }
            return Dias_Acumulados;
        }
        /// **********************************************************************************************************************
        /// Nombre: Obtener_Salario_Zona_Economica
        /// 
        /// Descripción: Método que obtiene el salario de la zona económica  a la que pertenece.
        /// 
        /// Parámetros: Zona_ID.- Identificador de la zona a la que pertenece el empleado.
        /// 
        /// Usuario Creó: Juan Alberto Hernandez Negrete.
        /// Fecha Creó: 21/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// **********************************************************************************************************************
        public Double Obtener_Salario_Zona_Economica(String Zona_ID)
        {
            Cls_Cat_Nom_Zona_Economica_Negocio Obj_Zona_Economica = 
                new Cls_Cat_Nom_Zona_Economica_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Zona_Economica = null;//Variable que almacena el registro de la zona economica consultada.
            Double Salario_Diario_Zona = 0.0;//Variable que almacenara el salario diario del empleado.

            try
            {
                Obj_Zona_Economica.P_Zona_ID = Zona_ID;
                Dt_Zona_Economica = Obj_Zona_Economica.Consulta_Datos_Zona_Economica();

                if (Dt_Zona_Economica is DataTable) {
                    if (Dt_Zona_Economica.Rows.Count > 0) { 
                        foreach(DataRow SMGZ in Dt_Zona_Economica.Rows){
                            if (SMGZ is DataRow) {
                                if (!String.IsNullOrEmpty(SMGZ[Cat_Nom_Zona_Economica.Campo_Salario_Diario].ToString().Trim()))
                                    Salario_Diario_Zona = Convert.ToDouble(SMGZ[Cat_Nom_Zona_Economica.Campo_Salario_Diario].ToString().Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el salario diario de la zona economica. Error: [" + Ex.Message + "]");
            }
            return Salario_Diario_Zona;
        }
        /// **********************************************************************************************************************
        /// Nombre: Obtener_Dias_Indemnizacion
        /// 
        /// Descripción: Método utilizado para obtener los dias de indemnización que le corresponden al tipo de indemnización
        ///              que tiene el empleado.
        /// 
        /// Parámetros: Indemnización_ID.- Identificador del tipo de indemnización al que pertenece el empleado.
        /// 
        /// Usuario Creó: Juan Alberto Hernandez Negrete.
        /// Fecha Creó: 21/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// **********************************************************************************************************************
        public Int32 Obtener_Dias_Indemnizacion(String Indemnizacion_ID)
        {
            Cls_Cat_Nom_Indemnizacion_Negocio Obj_Indemnizacion = 
                new Cls_Cat_Nom_Indemnizacion_Negocio();//Variable de conexión hacia la cap de negocios.
            DataTable Dt_Indemnizacion = null;//Variable que almacena el registro de indemnización consultado.
            Int32 Dias_Prima_Antiguedad = 0;//Variable que almacena los dias de prima de antiguedad que le corresponden al empleado.

            try
            {
                Obj_Indemnizacion.P_Indemnizacion_ID = Indemnizacion_ID;
                Dt_Indemnizacion = Obj_Indemnizacion.Consultar_Indemnizaciones();

                if (Dt_Indemnizacion is DataTable)
                {
                    if (Dt_Indemnizacion.Rows.Count > 0)
                    {
                        foreach (DataRow INDEMNIZACION in Dt_Indemnizacion.Rows)
                        {
                            if (INDEMNIZACION is DataRow)
                            {
                                if (!String.IsNullOrEmpty(INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Dias].ToString().Trim()))
                                    Dias_Prima_Antiguedad = Convert.ToInt32(INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Dias].ToString().Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener los dias de indemnización del empleado. Error: [" + Ex.Message + "]");
            }
            return Dias_Prima_Antiguedad;
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Periodo_Vacacional
        ///
        ///DESCRIPCIÓN: Consulta y obtiene el periodo vacacional en el que se encuentra el empleado. PERIODO I [ENERO - JUNIO] Ó PERIODO II 
        ///             [JULIO - DICIEMBRE].
        ///             
        ///CREO: Juan Alberto Hernández Negrete
        ///FECHA_CREO: 04/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        public Int32 Obtener_Periodo_Vacacional(DateTime Fecha_Actual)
        {
            Int32 Periodo_Vacacional = 0;           //Variable que almacenara el periodo vacacional actual.    
            Int32 Anio_Calendario_Nomina = 0;       //Variable que almacena el año del calendario de la nomina.

            try
            {
                //Consulta el año actual del periodo nóminal.
                Anio_Calendario_Nomina = Obtener_Anio_Calendario_Nomina();

                if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 1, 1)) >= 0) &&
                    (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 6, 30)) <= 0))
                {
                    //PERIODO VACACIONAL I
                    Periodo_Vacacional = 1;
                }
                else if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 7, 1)) >= 0) &&
                    (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 12, 31)) <= 0))
                {
                    //PERIODO VACACIONAL II
                    Periodo_Vacacional = 2;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el periodo vacacional. Error: [" + Ex.Message + "]");
            }
            return Periodo_Vacacional;
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Anio_Calendario_Nomina
        ///
        ///DESCRIPCIÓN: Consulta y obtiene el año de la nómina actual. Está búsqueda se realiza en los calendarios de nómina que se encuentran
        ///             registrados actualmente en el sistema.
        ///             
        ///CREO: Juan Alberto Hernández Negrete
        ///FECHA_CREO: 04/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        public Int32 Obtener_Anio_Calendario_Nomina()
        {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la clase de negocios.
            DataTable Dt_Calendario_Nomina = null;                                                                      //Variable que guardara la información del calendario de nómina consultado.
            Int32 Anio_Calendario_Nomina = 0;                                                                          //Variable que almacena el año del calendario de nomina.         

            try
            {
                //Consultamos la el calendario de nómina que esta activo actualmente. 
                Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consultar_Calendario_Nomina_Fecha_Actual();

                if (Dt_Calendario_Nomina is DataTable)
                {
                    foreach (DataRow Informacion_Calendario_Nomina in Dt_Calendario_Nomina.Rows)
                    {
                        if (Informacion_Calendario_Nomina is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Informacion_Calendario_Nomina[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString()))
                            {
                                Anio_Calendario_Nomina = Convert.ToInt32(Informacion_Calendario_Nomina[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el año del calendario de nómina del año actual. Error: [" + Ex.Message + "]");
            }
            return Anio_Calendario_Nomina;
        }
        #endregion

        #region (Finiquitos Vacaciones)

        #region (Vacaciones Pendientes Por Pagar)
        /// *************************************************************************************************************************
        /// Nombre: Vacaciones_Pendientes_Pagar
        /// 
        /// Descripción: Método que realiza el cálculo de la cantidad que le aplica al empleado por lo dias de vacaciones
        ///              que tiene el empleado y que no ha tomado.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado para control interno del sistema.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 22/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public DataTable Vacaciones_Pendientes_Pagar(String Empleado_ID)
        {
            Int32 Dias_Vacaciones = 0;
            Double Cantidad_Descontar_Vacaciones = 0.0;
            Double Salario_Diario = 0.0;
            DataTable Dt_Resultado = null;//Variable que almacenra el resultado de los calculos realizados.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.

                //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                {
                    //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                    if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                    {
                        //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO + PSM]
                        Salario_Diario = Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG(Empleado_ID);
                    }
                    else
                    {
                        //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                        Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                    }
                }
                else
                {
                    //OBTENEMOS SALARIO DIARIO [SALARIO DIARIO CAT_EMPLEADOS]
                    Salario_Diario = Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(Empleado_ID);
                }

                Dias_Vacaciones = Dias_Vacaciones_Base_Formula(Empleado_ID);

                if (Dias_Vacaciones > 0)
                {
                    Cantidad_Descontar_Vacaciones = (Dias_Vacaciones * Salario_Diario);
                    Dt_Resultado = Crear_Tabla_Resultados(Cantidad_Descontar_Vacaciones, 0, 0);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad que se le debe al empleado por dias de vacaciones " +
                    "que aun tiene pendiente por tomar. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        #endregion

        #region (Métodos Generales)
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Insertar_Actualizar_Detalle_Periodo_Vacacional
        ///
        ///DESCRIPCIÓN: Obtiene en base a formúla los dias de vacaciones que le corresponden al empelado según su antiguedad en la empresa.
        ///
        ///             Si Antiguedad Laboral Menor a 1 entoncés:
        ///             
        ///                 Dias_Año [365 ó 366] ---> 20 Dias de Vacaciones al año.
        ///                 N Dias Laborados     --->  X Dias de Vacaciones al año.
        ///             
        /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
        ///             
        ///CREO: Juan Alberto Hernández Negrete
        ///FECHA_CREO: 04/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private Int32 Dias_Vacaciones_Base_Formula(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
            DataTable Dt_Empleados = null;                                              //Variable que almacena la informacion del empleado consultado.     
            Int32 Cantidad_Dias_Vacaciones = 0;                                                    //Cantidad de dias que le corresponden al empleado segun su fecha de ingreso a presidencia.
            Int32 Dias_Aplican_Calculo = 0;                                                             //Cantidad de dias que lleva el empleado laborando en presidencia.
            DateTime? Fecha_Ingreso_Empleado = null;                                    //Variable que alamaceba la fecha de ingreso del empleado a presidencia.
            DateTime Fecha_Actual = DateTime.Now;                                       //Variable que almacena la fecha actual.
            String Tipo_Nomina_ID = "";                                                 //Variable que almacenara el tipo de nómina al que pertence el empleado.
            Int32 Anio_Nomina = 0;                                                             //Variable que almacenara el año de calendario de nómina vigente actualmente.         
            Int32 Periodo_Actual = 0;                                                   //Variable que almacenara el periodo vacacional actual.
            Int32 Auxiliar = 0;                                                         //Variable auxiliar que almacenara los dias entre la fecha de ingreso y el 30 de Junio del año actual.
            Int32 Dias_Totales_Primer_Periodo_Vacacional = 0;                           //Variable que almacenara los dias totales del primer periodo [Enero - Junio].
            Int32 Dias_Totales_Segundo_Periodo_Vacacional = 0;                          //Variable que almacenara los dias totales del segundo periodo [Julio - Diciembre].
            Int32 Dias_Periodo_II_Anio_Anterior = 0;
            Int32 Dias_VP_I = 0;
            Int32 Dias_VP_II = 0;

            try
            {
                Anio_Nomina = Obtener_Anio_Calendario_Nomina();//Obtenemos el anio del calendario de nomina vigente actualmente.
                Periodo_Actual = Obtener_Periodo_Vacacional();//Obtenemos el periodo vacacional en el que nos encontramos actualmente.

                //Obtenemos el total de dias del periodo de [Enero - Junio] y del periodo [Julio - Diciembre] del anio del calendario de nomina vigente.
                Dias_Totales_Primer_Periodo_Vacacional = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 1, 1), new DateTime(Anio_Nomina, 6, 30)) + 1;
                Dias_Totales_Segundo_Periodo_Vacacional = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 7, 1), new DateTime(Anio_Nomina, 12, 31)) + 1;

                //Consultamos la información general del empleado.
                Obj_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

                //Validamos que la búsqueda.
                if (Dt_Empleados is DataTable)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow Empleado in Dt_Empleados.Rows)
                        {
                            if (Empleado is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString()))
                                {
                                    //Obtenemos la fecha de ingreso del empleado.
                                    Fecha_Ingreso_Empleado = Convert.ToDateTime(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString());

                                    //Consultamos el tipo de nomina al que pertence el empleado.
                                    Tipo_Nomina_ID = Empleado[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString();
                                    //Obtenemos los dias de vacaciones que le corresponden al periodo vacacional de acuerdo al tipo de nómina.
                                    Dias_VP_I = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, 1);
                                    Dias_VP_II = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, 2);

                                    //Si el año de ingreso del empleado a presidencia es igual a l año actual entoncés:
                                    if (((DateTime)Fecha_Ingreso_Empleado).Year == Anio_Nomina)
                                    {
                                        //Identificamos el periodo, si el periodo actual es el primero entonces:
                                        if (Periodo_Actual == 1)
                                        {
                                            //Obtenemos los dias que el empleado lleva laborando en presidencia.
                                            Dias_Aplican_Calculo = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), Fecha_Actual) + 1);
                                            Cantidad_Dias_Vacaciones = (Int32)(Dias_Aplican_Calculo * Dias_VP_I) / Dias_Totales_Primer_Periodo_Vacacional;
                                        }
                                        else if (Periodo_Actual == 2)
                                        {
                                            if (((DateTime)Fecha_Ingreso_Empleado) <= new DateTime(Anio_Nomina, 06, 30, 23, 59, 59))
                                            {
                                                //Consultamos los dias que lleva laborando el empleado en presidencia.
                                                Dias_Aplican_Calculo = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), new DateTime(Anio_Nomina, 06, 30, 23, 59, 59)) + 1);
                                                Cantidad_Dias_Vacaciones += (Int32)(Dias_Aplican_Calculo * Dias_VP_I) / Dias_Totales_Primer_Periodo_Vacacional;
                                            }

                                            //Consultamos los dias que lleva laborando el empleado en presidencia.
                                            Dias_Aplican_Calculo = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 07, 1, 0, 0, 0), Fecha_Actual) + 1);
                                            Auxiliar = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 07, 1, 0, 0, 0), ((DateTime)Fecha_Ingreso_Empleado)) + 1);
                                            if (Auxiliar < 0) Auxiliar = 0;
                                            Dias_Aplican_Calculo = Dias_Aplican_Calculo - Auxiliar;
                                            Cantidad_Dias_Vacaciones += (Int32)(Dias_Aplican_Calculo * Dias_VP_II) / Dias_Totales_Segundo_Periodo_Vacacional;
                                        }
                                    }
                                    else if (((DateTime)Fecha_Ingreso_Empleado).Year < Anio_Nomina)
                                    {
                                        //Identificamos el periodo, si el periodo actual es el primero entonces:
                                        if (Periodo_Actual == 1)
                                        {
                                            //Obtenemos los dias del periodo anterior.
                                            Dias_Periodo_II_Anio_Anterior = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, (DateTime)Fecha_Ingreso_Empleado, new DateTime((Anio_Nomina - 1), 12, 31)) + 1);
                                            Auxiliar = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, (DateTime)Fecha_Ingreso_Empleado, new DateTime((Anio_Nomina - 1), 06, 30)) + 1);
                                            if (Auxiliar < 0) Auxiliar = 0;
                                            Dias_Periodo_II_Anio_Anterior = Dias_Periodo_II_Anio_Anterior - Auxiliar;
                                            Cantidad_Dias_Vacaciones += (Int32)((Dias_Periodo_II_Anio_Anterior * Dias_VP_II) / Dias_Totales_Segundo_Periodo_Vacacional);

                                            //Obtenemos dias periodo actual.
                                            Dias_VP_I = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, 1);
                                            Cantidad_Dias_Vacaciones += Dias_VP_I;
                                        }
                                        else if (Periodo_Actual == 2)
                                        {
                                            Cantidad_Dias_Vacaciones += Dias_VP_I;

                                            Dias_Aplican_Calculo = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 06, 30), Fecha_Actual) + 1);
                                            Cantidad_Dias_Vacaciones += (Int32)((Dias_Aplican_Calculo * Dias_VP_II) / Dias_Totales_Segundo_Periodo_Vacacional);
                                        }
                                    }

                                    if (Cantidad_Dias_Vacaciones > 0)
                                        Cantidad_Dias_Vacaciones = Cantidad_Dias_Vacaciones - Obtener_Dias_Tomados_Vacaciones(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString().Trim());
                                    else
                                        Cantidad_Dias_Vacaciones = Obtener_Dias_Tomados_Vacaciones(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()) * (-1);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el porcentaje [%] de los dias que le corresponden al" +
                                    "empleado en base a la fecha de ingreso que tiene. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Dias_Vacaciones;
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dias_Vacaciones_Tipo_Nomina
        ///
        ///DESCRIPCIÓN: Consulta los dias de vacaciones que le corresponde al empleado por el tipo de nómina al que pertence. Y de acuerdo al 
        ///             periodo vacacional consultado.
        ///             
        /// PARÁMETROS: Tipo_Nomina_ID.- Identificador o clave única para identificar a los tipos de nomina que se encuentran dadas de 
        ///                              alta en el sistema.
        ///             Periodo.- Periodo Vacacional a consultar los dias de vacaciones del empleado.                  
        ///             
        ///CREO: Juan Alberto Hernández Negrete 
        ///FECHA_CREO: 15/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private Int32 Consultar_Dias_Vacaciones_Tipo_Nomina(String Tipo_Nomina_ID, Int32 Periodo)
        {
            Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Tipos_Nomina = null;                                                    //Variable que guarda la información del tipo de nomina.
            Int32 Dias_Vacaciones_PVI = 0;                                                       //Variable que almacena las dias de vacaciones del primer periodo vacacional.
            Int32 Dias_Vacaciones_PVII = 0;                                                      //Variable que almacena las dias de vacaciones del segundo periodo vacacional.
            Int32 Dias = 0;

            try
            {
                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Tipos_Nomina = Obj_Tipos_Nomina.Consulta_Datos_Tipo_Nomina();

                if (Dt_Tipos_Nomina is DataTable)
                {
                    if (Dt_Tipos_Nomina.Rows.Count > 0)
                    {
                        foreach (DataRow Tipo_Nomina in Dt_Tipos_Nomina.Rows)
                        {
                            if (Tipo_Nomina is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString()))
                                {
                                    Dias_Vacaciones_PVI = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString());
                                    if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString()))
                                    {
                                        Dias_Vacaciones_PVII = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString());

                                        if (Periodo == 1) Dias = Dias_Vacaciones_PVI;
                                        else if (Periodo == 2) Dias = Dias_Vacaciones_PVII;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias de vacaciones del empleado de acuerdo a su tipo de nómina. Error: [" + Ex.Message + "]");
            }
            return Dias;
        }
        ///***********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Dias_Disponibles_Vacaciones
        ///
        ///DESCRIPCIÓN: Obtiene los dias de vacaciones que tiene disponibles el empleado.
        ///
        ///PARAMETROS: Empleado_ID.- Identificador único del empleado.
        ///
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 7/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***********************************************************************************************************************************
        private Int32 Obtener_Dias_Tomados_Vacaciones(String Empleado_ID)
        {
            Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexion con la capa de negocios.
            DataTable Dt_Detalle_Vacacion_Empleado = null;                                                                   //Variable que almacena la lista de vacaciones que a tomado el empleado.
            Int32 Contador_Dias_Vacaciones_Empleados = 0;                                                                    //Variable que almacena la cantidad de dias de vacaciones que tiene asignados el empleado.

            try
            {
                //Consultamos los dias que tiene el empleado como dias disponibles de vacaciones en el periodo actual, el periodo
                //anterior al actual y el periodo siguiente al actual.
                Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Detalle_Vacacion_Empleado = Obj_Vacaciones_Empleados.Consultar_Cantidad_Dias_Disponiubles_Por_Periodo_Empleado();

                if (Dt_Detalle_Vacacion_Empleado is DataTable)
                {
                    foreach (DataRow Detalle_Vacacion in Dt_Detalle_Vacacion_Empleado.Rows)
                    {
                        if (Detalle_Vacacion is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Detalle_Vacacion[Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles].ToString()))
                            {
                                Contador_Dias_Vacaciones_Empleados += Convert.ToInt32(Detalle_Vacacion[Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados].ToString());
                            }
                        }
                    }
                }

                //A los dias que tiene empleado como dias disponibles de vacaciones de acuerdos al periodo vacacional
                //se le suman los dias que tomo el empleado de vacaciones anteriormente y se encuntran en un estatus 
                //de PAGADOS y PENDIENTES por tomar.
                // Contador_Dias_Vacaciones_Empleados += Consultar_Dias_Pagados_Perndientes_Tomar(Empleado_ID);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias de vacaciones que na tenido el empelado. Error: [" + Ex.Message + "]");
            }
            return Contador_Dias_Vacaciones_Empleados;
        }
        ///***********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Periodo_Vacacional
        ///
        ///DESCRIPCIÓN: Obtiene periodo vacacional en el que nos encontramos actualmente.
        ///
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 9/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***********************************************************************************************************************************
        private Int32 Obtener_Periodo_Vacacional()
        {
            Int32 Periodo_Vacacional = 0;           //Variable que almacenara el periodo vacacional actual.    
            Int32 Anio_Calendario_Nomina = 0;       //Variable que almacena el año del calendario de la nomina.
            DateTime Fecha_Actual = DateTime.Now;   //Variable que almacena la fecha actual.

            try
            {
                //Consulta el año actual del periodo nóminal.
                Anio_Calendario_Nomina = Obtener_Anio_Calendario_Nomina();

                if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 1, 1)) >= 0) &&
                    (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 6, 30, 23, 59, 59)) <= 0))
                {
                    //PERIODO VACACIONAL I
                    Periodo_Vacacional = 1;
                }
                else if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 7, 1)) >= 0) &&
                    (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 12, 31, 23, 59, 59)) <= 0))
                {
                    //PERIODO VACACIONAL II
                    Periodo_Vacacional = 2;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el periodo vacacional. Error: [" + Ex.Message + "]");
            }
            return Periodo_Vacacional;
        }
        #endregion

        #endregion

        #region (Calculo Salarios Empleado)
        /// *******************************************************************************************************************
        /// NOMBRE: Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG
        /// 
        /// DESCRIPCIÓN: Obtenemos la cantidad diaria seguún el monto mensual que tiene asignado el NIVEL al que pertenece 
        ///              el empleado. Este método se creó para considerar el nuevo calculo de ISSEG.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del empleado que usa el sistema para realizar los operaciones sobre el mismo.
        /// 
        /// USUARIO CREO: Juan Alberto Hernández Negrete.
        /// FECHA CREO: 26/Septiembre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODOFICO:
        /// *******************************************************************************************************************
        private Double Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            Cls_Cat_Puestos_Negocio INF_PUESTO = null;//Variable que almacenara la información del puesto.
            Double Salario_Diario = 0.0;//Variable que almacenara la cantidad diaria que le corresponde al empleado según su puesto.

            try
            {
                //PASI I.- CONSULTAR LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

                //PASO II.- VALIDAMOS QUE EL EMPLEADO TENGA ALGÚN PUESTO.
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Puesto_ID))
                {
                    //PASO 2.1.- CONSULTAMOS LA INFORMACIÓN DEL PUESTO.
                    INF_PUESTO = _Informacion_Puestos(INF_EMPLEADO.P_Puesto_ID);
                    //PASO 2.2.- OBTENEMOS EL SALARIO DIARIO DEL PUESTO.
                    Salario_Diario = (INF_PUESTO.P_Salario_Mensual / Cls_Utlidades_Nomina.Dias_Mes_Fijo);
                    //Validamos que el salario mensual del puesto no sea cero.
                    if (INF_PUESTO.P_Salario_Mensual <= 0) Salario_Diario = INF_EMPLEADO.P_Salario_Diario;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el salario diario del empleado ISSEG. Error: [" + Ex.Message + "]");
            }
            return Salario_Diario;
        }
        /// *******************************************************************************************************************
        /// NOMBRE: Obtener_Cantidad_Diaria_PSM_ISSEG
        /// 
        /// DESCRIPCIÓN: Obtenemos la cantidad diaria de Previsión Social Múltiple que le corresponde al empleado según el nivel
        ///              al que pertenecé.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del empleado que usa el sistema para realizar los operaciones sobre el mismo.
        /// 
        /// USUARIO CREO: Juan Alberto Hernández Negrete.
        /// FECHA CREO: 26/Septiembre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODOFICO:
        /// *******************************************************************************************************************
        private Double Obtener_Cantidad_Diaria_PSM_ISSEG(String Empleado_ID)
        {
            //VARIABLES NEGOCIO.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empelado.
            Cls_Cat_Puestos_Negocio INF_PUESTO = null;//Variable que almacenara la informacion del puesto.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacenara la información de los parámetros de nómina.

            //VARIABLES PARA ALMACENARA LA INFORMACION DE LOS PARAMETROS DE LA NOMINA.
            Double CANTIDAD_DIARIA_PSM = 0.0;
            Double PREVISION_SOCIAL_MULTIPLE = 0.0;

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_PUESTO = _Informacion_Puestos(INF_EMPLEADO.P_Puesto_ID);//CONSULTAMOS LA INFORMACIÓN DEL PUESTO.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();//CONSULTAMOS LA INFORMACIÓN DE LOS PARÁMETRO PARÁMETROS DE NÓMINA.

                //PASO I.- OBTENEMOS LA PREVISION SOCIAL MULTIPLE DEL EMPLEADO.
                PREVISION_SOCIAL_MULTIPLE = (INF_PUESTO.P_Salario_Mensual * Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple)) ? "0" : INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple));

                //VALIDAMOS QUE EL PUESTO NO VENGA CON SALARIO MENSUAL CERO.
                if (INF_PUESTO.P_Salario_Mensual <= 0)
                    PREVISION_SOCIAL_MULTIPLE = ((INF_EMPLEADO.P_Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo) * 
                        Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple)) ? "0" : INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple));

                //PASO II.- OBTENEMOS LA CANTIDAD DIARIA DE PREVISIÓN SOCIAL MÚLTIPLE [DIAS MES 30.42].
                CANTIDAD_DIARIA_PSM = (PREVISION_SOCIAL_MULTIPLE / Cls_Utlidades_Nomina.Dias_Mes_Fijo);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el salario del empleado (ISSEG). Error: [" + Ex.Message + "]");
            }
            return CANTIDAD_DIARIA_PSM;
        }
        /// *******************************************************************************************************************
        /// NOMBRE: Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG
        /// 
        /// DESCRIPCIÓN: Obtenemos la cantidad diaria integrando el sueldo del puesto o nivel más la PSM.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del empleado que usa el sistema para realizar los operaciones sobre el mismo.
        /// 
        /// USUARIO CREO: Juan Alberto Hernández Negrete.
        /// FECHA CREO: 26/Septiembre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODOFICO:
        /// *******************************************************************************************************************
        private Double Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(String Empleado_ID){
            //VARIABLES NEGOCIO
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//VARIABLE QUE ALMACENARA  LA INFORMACION DEL EMPLEADO.
            Cls_Cat_Puestos_Negocio INF_PUESTO = null;//VARIABLE QUE ALMACENARA LA INFORMACIÓN DEL PUESTO.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//VARIABLE QUE ALMACENARA LA INFORMACIÓN DEL PARÁMETRO DE LA NÓMINA.

            //VARIABLES PARA ALMACENARA LA INFORMACION DE LOS PARAMETROS DE LA NOMINA.
            Double SALARIO_DIARIO_INTEGRADO_SUELDO_MAS_PSM = 0.0;//VARIABLE QUE ALMACENA LA CANTIDAD DIARIO INTEGRANDO EL SUELDO DEL NIVEL MAS LA PSM.
            Double PREVISION_SOCIAL_MULTIPLE = 0.0;//CANTIDAD QUE LE CORRESPONDE AL EMPLEADO DE PREVISIÓN SOCIAL MÚLTIPLE.
            Double SUELDO_TOTAL = 0.0;//VARIABLE QUE ALMACENA EL SUELDO MENSUAL QUE TIENE EL NIVEL [PUESTO].

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAR INFORMACION DEL EMPLEADO.
                INF_PUESTO = _Informacion_Puestos(INF_EMPLEADO.P_Puesto_ID);//CONSULTAR INFORMACION DEL PUESTO
                INF_PARAMETRO= _Informacion_Parametros_Nomina();//CONSULTA LA INFORMACION DE LOS PARAMETROS.

                if (INF_PUESTO.P_Salario_Mensual > 0)
                {
                    //PASO I.- OBTENEMOS LA PSM.
                    PREVISION_SOCIAL_MULTIPLE = (INF_PUESTO.P_Salario_Mensual *
                        Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple)) ?
                        "0" : INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple));

                    //PASO II.- OBTENER EL SUELDO TOTAL [INTEGRANDO SUELDO DEL NIVEL MAS LA PSM]
                    SUELDO_TOTAL = (INF_PUESTO.P_Salario_Mensual + PREVISION_SOCIAL_MULTIPLE);
                }
                else {
                    //PASO I.- OBTENEMOS LA PSM.
                    PREVISION_SOCIAL_MULTIPLE = ((INF_EMPLEADO.P_Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo) *
                        Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple)) ?
                        "0" : INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple));

                    //PASO II.- OBTENER EL SUELDO TOTAL [INTEGRANDO SUELDO DEL NIVEL MAS LA PSM]
                    SUELDO_TOTAL = ((INF_EMPLEADO.P_Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo) + PREVISION_SOCIAL_MULTIPLE);
                }

                //PASO III.- OBTENER EL SALARIO DIARIO [SE INTEGRA EL SUELDO DEL PUESTO O NIVEL Y SE SUMA A LA PSM.]
                SALARIO_DIARIO_INTEGRADO_SUELDO_MAS_PSM = (SUELDO_TOTAL / Cls_Utlidades_Nomina.Dias_Mes_Fijo);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el salario del empleado integrando EL SUELDO MAS PSM (ISSEG). Error: [" + Ex.Message + "]");
            }
            return SALARIO_DIARIO_INTEGRADO_SUELDO_MAS_PSM;
        }
        /// *******************************************************************************************************************
        /// NOMBRE: Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG
        /// 
        /// DESCRIPCIÓN: Obtenemos el salario diario del catalogo de empleados.
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador del empleado que usa el sistema para realizar los operaciones sobre el mismo.
        /// 
        /// USUARIO CREO: Juan Alberto Hernández Negrete.
        /// FECHA CREO: 26/Septiembre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODOFICO:
        /// *******************************************************************************************************************
        private Double Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//SE CONSULTA LA INFORMACIÓN DEL EMPLEADO.
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el salario del empleado (Sin ISSEG). Error: [" + Ex.Message + "]");
            }
            return INF_EMPLEADO.P_Salario_Diario;
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