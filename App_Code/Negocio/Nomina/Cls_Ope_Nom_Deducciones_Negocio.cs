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
using System.Globalization;
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
using Presidencia.Calculo_Deducciones.Datos;
using System.Collections.Generic;
using Presidencia.Prestamos.Negocio;
using Presidencia.Archivos_Historial_Nomina_Generada;
using System.Text;
using Presidencia.IMSS.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Calculo_Percepciones.Negocio;
using Presidencia.Utilidades_Nomina;
using Presidencia.Puestos.Negocios;
using Presidencia.Cat_Terceros.Negocio;
using Presidencia.ISR.Negocios;
using Presidencia.Subsidio.Negocios;
using Presidencia.Ayudante_Informacion;

namespace Presidencia.Calculo_Deducciones.Negocio
{
    public class Cls_Ope_Nom_Deducciones_Negocio
    {
        ///**************************************************************************************
        ///       CANTIDADES TOTALES DE LAS PERCEPCIONES OBTENIDAS POR EL EMPLEADO EN SU
        ///                                   RECIBO DE NÓMINA
        ///**************************************************************************************
        public Double Ingresos_Gravables_Empleado;
        public Double Gravable_Prima_Vacacional;
        public Double Gravable_Aguinaldo;
        public Double Gravable_Prima_Antiguedad;
        public Double Gravable_Indemnizacion;
        public Double Exenta_Prima_Antiguedad;
        public Double Exenta_Indemnizacion;
        public Double Gravable_Tiempo_Extra;
        public Double Gravable_Dias_Festivos;
        public Double Exenta_Tiempo_Extra;
        public Double Exenta_Dias_Festivos;
        public Double Total_Percepciones;
        public Double Total_Deducciones;
        public Double Gravable_Sueldo;
        public DateTime Fecha_Generar_Nomina;

        #region (Calculos ISR Catorcenal)
        ///********************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_ISR
        /// DESCRIPCION : 
        ///             1.-	Se consulta el salario diario de la zona y este se multiplica por los días laborados. 
        ///                 para obtener el Salario de la Zona.
        ///                 
        ///                 Salario_Zona= Salario_Diario_Zona* Días laborados
        ///                 
        ///             2.- Se obtienen todos los Ingresos Gravables del Empleado y la suma de 
        ///                 estos se considera como Total de Ingresos Gravables.
        ///             3.- Si el Total de Ingresos Gravables es mayor a cero entonces:
        ///             
        ///                 a) .- Se consulta la tabla de ISR  para obtener el límite inferior (LI),
        ///                       Procentaje (%) y Cuota Fija (CF). En base al Total de Ingreso Gravable
        ///                       del empleado.
        ///                 b).- Se consulta la tabla de subsidio para obtener el subsidio causado
        ///                      en base al Total de Ingreso Gravable del empleado.
        ///                 c).- Calcular el ISPT del Empleado en base a la formula: 
        ///                     
        ///                     Si Salario_Zona [MENOR] Ingresos_Gravables Entonces
        ///                         ISPT = ((( Ingresos_Gravables - LI ) * % ) + CF ) - Subsidio
        ///                 
        ///                 d).- Si el ISPT > 0 entonces:
        ///                     - Se convierte en ISR a retener
        ///                       ISR = ISPT
        ///                 
        ///                 e).- Si ISPT menor a 0 entonces:
        ///                     - Se convierte en un subsidio para el empleado por lo tanto se
        ///                       convierte en una percepción para el empleado
        ///                       
        ///                       Subsidio_Empleado = ISPT * (-1)
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador único del empleado. Para manejar las operaciones internas del sistema.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 29/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///********************************************************************************************************************************************************
        public DataTable Calcular_ISR(String Empleado_ID)
        {
            ///Variables para mantener en memoria los registros consultados.
            DataTable Dt_Resultado = new DataTable();   //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;           //Almacena el registro que contiene el resultado de los cálculos de ISR.
            DataTable Dt_Tabulador_ISR;                 //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR
            DataTable Dt_Tabulador_Subsidio;            //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_SUBSIDIO         
            ///Variables de la Tabla de TAB_NOM_ISR
            Double Limite_Inferior_Tab_ISR = 0;         //Limite_Inferior correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Cuota_Fija_Tab_ISR = 0;              //Couta_Fija correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Porcentaje_Tab_ISR = 0;              //Porcentaje correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            ///Variables de la Tabla TAB_NOM_SUBSIDIO
            Double Subsidio = 0;                        //Subsidio correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_SUBSIDIO
            ///Variables para la generaciòn de la nomina
            Double ISPT = 0;                            //Obtiene el impuesto a retener al Empleado
            Double Cas_ISR = 0;                         //Obtiene el cas a entregar al Empleado para el pago del ISPT
            Double ISR_Retener_Empleado = 0;            //ISR a retener al Empleado 
            Double Cas_Empleado = 0;                    //Subsidio para el Empleado. La deduccion pasa a ser percepcion para el empleado.
            Double Salario_Diario_Zona = 0.0;           //Variable que almacenará el salario diario de la zona.
            Double Salario_Zona = 0.0;                  //Salario de la Zona.
            Int32 Dias_Laborados = 0;                   //Variable que almacena la cantidad de dias laborados por el empleado.
            Int32 DIAS_PERIODO_NOMINAL = 0;             //Variable que almacena la cantidad de dias del periodo nomina.
            //VARIABLES DE NEGOCIO.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//Variable que almacena la información de la zona económica.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONOMICA.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();

                //Obtenemos los días que tiene el periodo nominal.
                DIAS_PERIODO_NOMINAL = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //Obtenemos los días laborados por el empleado.
                Dias_Laborados = Obtener_Dias_Laborados_Empleado(Empleado_ID);
                //Aquí se obtiene el salario diario de la zona económica que se establecio como parámetro..
                Salario_Diario_Zona = INF_ZONA_ECONOMICA.P_Salario_Diario;
                //Obtenemos el salario de la zona de acuerdo los días del periodo.
                Salario_Zona = (Salario_Diario_Zona * Dias_Laborados);

                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                if (Ingresos_Gravables_Empleado > 0)
                {
                    //Validamos que el Total de Ingresos Gravables sea mayor a cero.
                    //if (Salario_Zona < Ingresos_Gravables_Empleado)
                    //{
                        //Paso II.- Se consulta la tabla de ISR [TAB_NOM_ISR]  para obtener el límite inferior (LI),
                        //          Procentaje (%) y Cuota Fija (CF). En base al Total de Ingreso Gravable
                        //          del empleado.
                        Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Ingresos_Gravables_Empleado, DIAS_PERIODO_NOMINAL);

                        if (Dt_Tabulador_ISR is DataTable)
                        {
                            if (Dt_Tabulador_ISR.Rows.Count > 0)
                            {
                                //Obtenemos los valores de Limite Inferior, Couta Fija y Porcentaje de Impuesto Gravable
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                    Limite_Inferior_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString());
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                                    Cuota_Fija_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString());
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim()))
                                    Porcentaje_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                            }
                        }

                        //Paso II.- Se consulta la tabla de subsidio [TAB_NOM_SUBSIDIO] para obtener el subsidio causado
                        //          en base al Total de Ingreso Gravable del empleado.
                        Dt_Tabulador_Subsidio = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_Subsidio_Empleado(Ingresos_Gravables_Empleado, DIAS_PERIODO_NOMINAL);

                        if (Dt_Tabulador_Subsidio is DataTable)
                        {
                            if (Dt_Tabulador_Subsidio.Rows.Count > 0)
                            {
                                //Obtenemos la cantidad de subsidio para el empleado.
                                if (!String.IsNullOrEmpty(Dt_Tabulador_Subsidio.Rows[0][2].ToString().Trim()))
                                    Subsidio = Convert.ToDouble(Dt_Tabulador_Subsidio.Rows[0][2].ToString());
                            }
                        }

                        //Paso IV.- Se ejecuta el calculo del ISPT
                        ISPT = (((Ingresos_Gravables_Empleado - Limite_Inferior_Tab_ISR) * Porcentaje_Tab_ISR) + Cuota_Fija_Tab_ISR) - Subsidio;

                        //Paso V.- Si ISPT es mayor a cero entonces es impuesto a retener por parte del trabajador
                        if (ISPT > 0)
                        {
                            //se convierete en una deducción para el empleado.
                            ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", ISPT));
                            Cas_Empleado = 0;

                            Renglon_Resultado = Dt_Resultado.NewRow();
                            Renglon_Resultado["Percepcion_Deduccion"] = false;
                            Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                            Dt_Resultado.Rows.Add(Renglon_Resultado);
                        }
                        else
                        {
                            //Se convierte en una percepción para el empleado.
                            ISR_Retener_Empleado = 0;
                            Cas_Empleado = ISPT * (-1);

                            Renglon_Resultado = Dt_Resultado.NewRow();
                            Renglon_Resultado["Percepcion_Deduccion"] = true;
                            Renglon_Resultado["Cantidad"] = Cas_Empleado;
                            Dt_Resultado.Rows.Add(Renglon_Resultado);
                        }
                    //}
                }//Validacion  para calcular el ISPT solo si el ingreso gravable es mayor a cero.
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al Calcular_ISR. ERROR [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_ISPT_Nomina_Asimilables
        /// DESCRIPCIÓN:
        ///             1.- Se Suman Todos los Ingresos Gravantes del Empleado.
        ///             
        ///             2.- Si el Total de Ingresos Gravables es mayor a cero entonces:
        ///             
        ///                 a) .- Se consulta la tabla de ISR  para obtener el límite inferior (LI),
        ///                       Procentaje (%) y Cuota Fija (CF). En base al Total de Ingreso Gravante
        ///                       del Empleado.
        ///                     
        ///                 c).- Calcular el ISPT del Empleado en base a la formula:
        ///                     
        ///                     ISPT = ((( Ingresos_Gravables - LI ) * % ) + CF )
        ///                 
        ///                 d).- ISR a retener
        ///                       ISR = ISPT
        ///                       
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 29/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public DataTable Calcular_ISPT_Nomina_Asimilables(String Empleado_ID)
        {
            ///Variables para mantener en memoria los registros consultados.
            DataTable Dt_Resultado = new DataTable();    //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;            //Variable que almacenara el resultado de la operación de calculo de ISR para el tipo de nomina Asimilable
            DataTable Dt_Tabulador_ISR;                  //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR
            ///Variables de la Tabla de TAB_NOM_ISR
            Double Limite_Inferior_Tab_ISR = 0;          //Limite_Inferior correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Cuota_Fija_Tab_ISR = 0;               //Couta_Fija correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Porcentaje_Tab_ISR = 0;               //Porcentaje correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR           
            ///Variables para la generaciòn de la nomina
            Double ISPT = 0;                             //Obtiene el impuesto a retener al Empleado
            Double ISR_Retener_Empleado = 0;             //ISR a retener al Empleado 
            ///Total de Ingresos Gravables del Empleado. Que Pertenecen a la Nomina 03 [Asimilables]
            Double Total_Ingresos_Gravables_Empleado = 0;//Variable que almacenará el total de ingresos gravables del empleado.
            Double Salario_Diario_Zona = 0.0;            //Variable que almacenará el salario diario de la zona.
            Double Salario_Zona = 0.0;                   //Salario de la Zona.
            Int32 Dias_Laborados = 0;                    //Variable que almacena la cantidad de dias laborados por el empleado.
            Int32 DIAS_PERIODO_NOMINAL = 0;             //Variable que almacena la cantidad de dias del periodo nomina.
            //VARIABLES DE NEGOCIO.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//Variable que almacena la información de la zona económica.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONOMICA.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();

                //Obtenemos los días que tiene el periodo nóminal.
                DIAS_PERIODO_NOMINAL = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //Obtenemos los días laborados por el empleado.
                Dias_Laborados = Obtener_Dias_Laborados_Empleado(Empleado_ID);
                //Aquí se obtiene el salario diario de la zona económica establecida como parámetro..
                Salario_Diario_Zona = INF_ZONA_ECONOMICA.P_Salario_Diario;
                //Obtenemos el salario de la zona de acuerdo a los días de catorcena.
                Salario_Zona = (Salario_Diario_Zona * Dias_Laborados);

                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado

                //Paso I.- Obtenemos el Total de Ingresos Gravantes del Empleado.
                Total_Ingresos_Gravables_Empleado = Ingresos_Gravables_Empleado;
                //Si el Total de Ingresos Gravante del Empleado es mayor a cero entoncés:
                //if (Salario_Zona < Total_Ingresos_Gravables_Empleado)
                //{
                    if (Ingresos_Gravables_Empleado > 0)
                    {
                        //Paso II.- Se consulta la tabla de ISR  para obtener el límite inferior (LI),
                        //          Procentaje (%) y Cuota Fija (CF). En base al Total de Ingreso Gravante
                        //          del Empleado.
                        Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Total_Ingresos_Gravables_Empleado, DIAS_PERIODO_NOMINAL);

                        //Validamos que la consulta halla encontrado resultados.
                        if (Dt_Tabulador_ISR is DataTable)
                        {
                            if (Dt_Tabulador_ISR.Rows.Count > 0)
                            {
                                //Obtenemos las cantidades de Limite Inferior, Cuota Fija y Porcentaje de ISR.
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                    Limite_Inferior_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString());
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                                    Cuota_Fija_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString());
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim()))
                                    Porcentaje_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                            }
                        }

                        //Paso III.- Se ejecuta el calculo del ISPT Nomina Asimilable. En este tipo de nómina no aplica el subsidio para el empleado.
                        ISPT = (((Total_Ingresos_Gravables_Empleado - Limite_Inferior_Tab_ISR) * Porcentaje_Tab_ISR) + Cuota_Fija_Tab_ISR);

                        //Paso IV.- El ISR a retener al empleado es igual al ISPT calculado.
                        ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", ISPT));

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = false;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                //}
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al Calcular_ISPT_Nomina_Asimilables. ERROR [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_ISPT_Subrogados
        /// DESCRIPCIÓN:
        ///             1.- Se Suma Todas las Percepciones del Empleado.
        ///             
        ///             2.- Si el Total de Percepciones del Empleado es mayor a cero entoncés:
        ///             
        ///                 a) .- Se consulta la tabla de ISR  para obtener el límite inferior (LI),
        ///                       Procentaje (%) y Cuota Fija (CF). En base al Total de las Percepciones.
        ///                     
        ///                 b).- Se consulta la tabla de subsidio para obtener el subsidio causado
        ///                      en base al Total de las Percepciones.
        ///                     
        ///                 c).- Calcular el ISPT del Empleado en base a la formula:
        ///                     
        ///                     ISPT = ((( Ingresos_Gravables - LI ) * % ) + CF ) - Subsidio
        ///                 
        ///                 d).- Si el ISPT > 0 entonces:
        ///                     - Se convierte en ISR a retener
        ///                       ISR = ISPT
        ///                 
        ///                 e).- Si ISPT menor a 0 entonces:
        ///                     - Se convierte en un subsidio para el empleado por lo tanto se
        ///                       convierte en una percepción para el empleado
        ///                       
        ///                       Subsidio_Empleado = ISPT * (-1) 
        ///                       
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 29/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public DataTable Calcular_ISPT_Subrogados(String Empleado_ID)
        {
            ///Variables para mantener en memoria los registros consultados.
            DataTable Dt_Resultado = new DataTable();   //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;           //Variable que almacenara el registro del resultado del calculo de ISR.
            DataTable Dt_Tabulador_ISR;                 //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR
            DataTable Dt_Tabulador_Subsidio;            //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_SUBSIDIO        
            ///Variables de la Tabla de TAB_NOM_ISR
            Double Limite_Inferior_Tab_ISR = 0;         //Limite_Inferior correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Cuota_Fija_Tab_ISR = 0;              //Couta_Fija correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Porcentaje_Tab_ISR = 0;              //Porcentaje correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            ///Variables de la Tabla TAB_NOM_SUBSIDIO
            Double Subsidio = 0;                        //Subsidio correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_SUBSIDIO
            ///Variables para la generaciòn de la nomina
            Double ISPT = 0;                            //Obtiene el impuesto a retener al Empleado
            Double ISR_Retener_Empleado = 0;            //ISR a retener al Empleado 
            Double Cas_Empleado = 0;                    //Subsidio para el Empleado. La deduccion pasa a ser percepcion para el empleado.
            ///Total Percepciones del Empleado
            Double Total_Percepciones_Empleado = 0;     //Variable que almacenara el total de percepciones del empleado.
            Double Salario_Diario_Zona = 0.0;           //Variable que almacenará el salario diario de la zona.
            Double Salario_Zona = 0.0;                  //Salario de la Zona.
            Int32 Dias_Laborados = 0;                   //Variable que almacena la cantidad de dias laborados por el empleado.
            Int32 DIAS_PERIODO_NOMINAL = 0;             //Variable que almacena la cantidad de dias del periodo nomina.
            //VARIABLES DE NEGOCIO.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//Variable que almacena la información de la zona económica.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONOMICA.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();

                //Obtenemos los días que tiene el periodo nominal.
                DIAS_PERIODO_NOMINAL = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //Obtenemos los días laborados por el empleado.
                Dias_Laborados = Obtener_Dias_Laborados_Empleado(Empleado_ID);
                //Aquí se obtiene el salario diario de la zona económica establecida como parámetro.
                Salario_Diario_Zona = INF_ZONA_ECONOMICA.P_Salario_Diario;
                //Obtenemos el salario de la zona en base a los días de catorcena.
                Salario_Zona = (Salario_Diario_Zona * Dias_Laborados);

                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                //Paso I.- Se suman todas las Percepciones del Empleado. Si estas son mayores a cero entoncés:
                Total_Percepciones_Empleado = Total_Percepciones;
                //Si el total de percepciones del empleado es mayor a cero entonces:
                //if (Salario_Zona < Total_Percepciones_Empleado)
                //{
                    if (Total_Percepciones_Empleado > 0)
                    {
                        //Paso 2.- Consulta los valores de la tabla de TAB_NOM_ISR de acuerdo al ingreso gravable
                        //         del empleado para el cálculo del ISPT.
                        Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Total_Percepciones_Empleado, DIAS_PERIODO_NOMINAL);

                        if (Dt_Tabulador_ISR is DataTable)
                        {
                            if (Dt_Tabulador_ISR.Rows.Count > 0)
                            {
                                //Obtenemos las cantidades de Limite Inferior, Cuota Fija y Porcentaje de ISR.
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                    Limite_Inferior_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString());
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                                    Cuota_Fija_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString());
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim()))
                                    Porcentaje_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                            }
                        }

                        //Paso 3.- Se consulta la tabla de subsidio TAB_NOM_SUBSIDIO para obtener el subsidio causado
                        //         en base al Total de Percepciones del Empleado.
                        Dt_Tabulador_Subsidio = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_Subsidio_Empleado(Total_Percepciones_Empleado, DIAS_PERIODO_NOMINAL);

                        if (Dt_Tabulador_Subsidio is DataTable)
                        {
                            if (Dt_Tabulador_Subsidio.Rows.Count > 0)
                            {
                                //Obtenemos la cantidad de subsidio causado para el empleado.
                                if (!String.IsNullOrEmpty(Dt_Tabulador_Subsidio.Rows[0][2].ToString().Trim()))
                                    Subsidio = Convert.ToDouble(Dt_Tabulador_Subsidio.Rows[0][2].ToString());
                            }
                        }

                        //Paso IV.- Se ejecuta el calculo del ISPT Subrogados.
                        ISPT = (((Total_Percepciones_Empleado - Limite_Inferior_Tab_ISR) * Porcentaje_Tab_ISR) + Cuota_Fija_Tab_ISR) - Subsidio;

                        //Paso V.- Si ISPT es mayor a cero entonces es impuesto a retener por parte del trabajador.
                        if (ISPT > 0)
                        {
                            //Se convierte en ISR a retener al empleado.
                            ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", ISPT));
                            Cas_Empleado = 0;

                            Renglon_Resultado = Dt_Resultado.NewRow();
                            Renglon_Resultado["Percepcion_Deduccion"] = false;
                            Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                            Dt_Resultado.Rows.Add(Renglon_Resultado);
                        }
                        else
                        {
                            //Se convierte en un subsidio para el empleado y esta será una percepción  a otorgar
                            //al empleado.
                            ISR_Retener_Empleado = 0;
                            Cas_Empleado = ISPT * (-1);

                            Renglon_Resultado = Dt_Resultado.NewRow();
                            Renglon_Resultado["Percepcion_Deduccion"] = true;
                            Renglon_Resultado["Cantidad"] = Cas_Empleado;
                            Dt_Resultado.Rows.Add(Renglon_Resultado);
                        }
                    }
                //}//Validacion  para calcular el ISPT solo si el ingreso gravable es mayor a cero
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al Calcular_ISPT_Subrogados. ERROR [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// ******************************************************************************************************************************************
        /// NOMBRE: Calcular_ISPT_Pensionados
        /// 
        /// DESCRIPCIÓ DEL PROCESO:
        ///            1.	En coordinación de nomina se concentra la información de las percepciones que corresponden a 
        ///                 cada uno de los empleados por nomina de la catorcena a calcular. 
        ///            2.	De acuerdo a estas cantidades y conceptos se  realizan los cálculos par determinar los totales 
        ///                 gravables.
        ///            3.	Se consulta el salario diario de la zona y el salario diario del empleado
        ///            4.	El salario diario de la zona se multiplica por 9: 
        ///            
        ///                    Salario_Zona_Gravable=Salario Diario * 9
        ///                    
        ///            5.	Si el Salario Diario Empleado es mayor al Salario_Zona_Gravable Entonces.
        ///                 a).	El Salario Diario de la Zona se multiplica por los días 
        ///                 b).	Se  consulta la tabla de ISR y se obtiene el Limite Inferior (LI), Porcentaje (%) y Cuota Fija (CF)
        ///                     En base al total de Ingreso Gravable del empleado.
        ///                 c).	Se calcula el ISPT en base a la formula:
        ///                         ISR=0
        ///                     
        ///                         Salario_Zona_Gravable=Salario Diario * 9
        ///                        
        ///                         Si Salario_Zona_Gravable > Salario_Diario_Empleado Entonces
        ///                        
        ///                         Salario_Zona=Salario_Diario_Zona*Dias_Laborados
        ///                        
        ///                         SI Salario_Zona [MENOR] Ingresos_Gravables Entonces
        ///                        
        ///                         ISPT= ((Ingresos_Gravables-LI)* %) + CF
        /// 
        /// PARÁMETROS: Empleado_ID.- Identificador único del empleado.
        /// 
        /// CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ:15/Febrero/2011 17:26 pm.
        /// MODIFICO:
        /// FECHA MODIFICO:
        /// ******************************************************************************************************************************************
        public DataTable Calcular_ISPT_Pensionados(String Empleado_ID)
        {
            ///Variables para mantener en memoria los registros consultados.
            DataTable Dt_Resultado = new DataTable();//Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;       //Variable que almacena el registro con el resultado del cálculo de ISR. 
            DataTable Dt_Tabulador_ISR;             //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR  
            ///Variables de la Tabla de TAB_NOM_ISR
            Double Limite_Inferior_Tab_ISR = 0;     //Limite_Inferior correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Cuota_Fija_Tab_ISR = 0;          //Couta_Fija correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Porcentaje_Tab_ISR = 0;          //Porcentaje correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double ISPT = 0.0;                      //Variable que almacenara el ISPT del pensionado a generar su nomina.
            Double ISR_Retener_Empleado = 0.0;      //Variable que almacena el ISR a retener al empleado.
            Double Ingresos_Gravables = 0;          //Variable que almacena el total de ingresos gravables del empleado.
            Double Salario_Diario_Empleado = 0.0;   //Variable que almacenara el salario diario del empleado.
            Double Salario_Diario_Zona = 0.0;       //Variable que almacena el salario diario de la zona.
            Double Salario_Zona_Gravable = 0.0;     //Variable que almacena Salario Gravable de la zona.
            Double Salario_Zona = 0.0;              //Variable que almacena el salario de la zona.
            Int32 Dias_Laborados = 0;               //Variable que almacena los dias laborados del empleado.
            Int32 DIAS_PERIODO_NOMINAL = 0;         //Variable que almacena la cantidad de dias del periodo nomina.
            //VARIABLES DE NEGOCIO.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//Variable que almacena la información de la zona económica.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONOMICA.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();

                //Obtenemos los dias que tiene el periodo nominal.
                DIAS_PERIODO_NOMINAL = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                /*
                 * CONSULTAMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO APLICA 
                 * PARA ISSEG, SE CONSIDERAR PARA OBTENER EL SALARIO DIARIO LA SUMA DEL SUELDO MÁS LA PSM
                 * DEL EMPLEADO.
                 */
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

                //Obtenemos los días laborados por el empleado.
                Dias_Laborados = Obtener_Dias_Laborados_Empleado(Empleado_ID);
                //Aquí se obtiene el salario diario de la zona económica que se establecio como parámetro.
                Salario_Diario_Zona = INF_ZONA_ECONOMICA.P_Salario_Diario;
                //Obtenemos el salario de la zona económica.
                Salario_Zona_Gravable = (Salario_Diario_Zona * 9);

                //Si Salario_Zona_Gravable > Salario_Mensual_Empleado Entonces
                if (Salario_Zona_Gravable > Salario_Diario_Empleado)
                {
                    //Salario_Zona = Salario_Diario_Zona * Dias_Laborados
                    Salario_Zona = (Salario_Diario_Zona * Dias_Laborados);
                    //SI Salario_Zona [MENOR] Ingresos_Gravables Entonces
                    if (Salario_Zona < Ingresos_Gravables)
                    {
                        Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Ingresos_Gravables, DIAS_PERIODO_NOMINAL);

                        if (Dt_Tabulador_ISR is DataTable)
                        {
                            if (Dt_Tabulador_ISR.Rows.Count > 0)
                            {
                                //Obtenemos la cantidad de Limite Inferior, Cuota Fija y Procentaje de ISR.
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                    Limite_Inferior_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString());
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                                    Cuota_Fija_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString());
                                if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim()))
                                    Porcentaje_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());

                                //ISPT= ((Ingresos_Gravables-LI)* %) + CF
                                ISPT = ((Ingresos_Gravables - Limite_Inferior_Tab_ISR) * Porcentaje_Tab_ISR) + Cuota_Fija_Tab_ISR;
                                ISR_Retener_Empleado = ISPT;//El ISPT se convierte en el ISR a retener al empleado.

                                //Creamos la tabla de resultados del Cálculo de ISR de los Pensionados.
                                Renglon_Resultado = Dt_Resultado.NewRow();//Fila clón de la tabla de resultados.
                                Renglon_Resultado["Percepcion_Deduccion"] = false;//false significa que el concepto es una deducción aplicar al empleado.
                                Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;//Monto a descontar al empleado.
                                Dt_Resultado.Rows.Add(Renglon_Resultado);//agregamos la fila a la tabla de resultados.
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar el cálculo de ISPT Pensionados. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_ISPT_Prima_Vacacional_Aguinaldo
        /// DESCRIPCIÓN:
        ///             1.- Se suma el Gravante de la Prima Vacacional y el Gravante del Aguinaldo
        ///                 para tener el Total Ingreso Gravante.
        ///                 
        ///             2.- Si el Total de Ingresos Gravables es mayor a cero entonces:
        ///             
        ///                 a) .- Se consulta la tabla de ISR  para obtener el límite inferior (LI),
        ///                       Procentaje (%) y Cuota Fija (CF). En base al Total de Ingreso Gravable
        ///                       del Empleado.
        ///                     
        ///                 b).- Se consulta la tabla de subsidio para obtener el subsidio causado
        ///                      en base al Total de Ingreso Gravable del Empleado.
        ///                     
        ///                 c).- Calcular el ISPT en base a la formula:
        ///                 
        ///                     Si Salario_Zona [MENOR] Ingresos_Gravables Entonces
        ///                     
        ///                     Ingresos_Gravables = Grava_Aguinaldo + Grava_PV + Sueldo_Mensual
        ///                     
        ///                     ISPT_Aguinaldo_PV_Sueldo_Mensual = ((( Ingresos_Gravables - LI ) * % ) + CF )
        ///                     
        ///                     ISPT_Sueldo_Mensual = ((Sueldo_Mensual-LI)*%) + CF
        ///                     
        ///                     ISPT = ISPT_Aguinaldo_PV_Sueldo_Mensual - ISPT_Sueldo_Mensual
        ///                 
        ///                 d).- Si el ISPT > 0 entonces:
        ///                     - Se convierte en ISR a retener
        ///                       ISR = ISPT
        ///                 
        ///                 e).- Si ISPT menor a 0 entonces:
        ///                     - Se convierte en un subsidio para el empleado por lo tanto se
        ///                       convierte en una percepción para el empleado
        ///                       
        ///                       Subsidio_Empleado = ISPT * (-1)
        ///                       
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 29/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public DataTable Calcular_ISPT_Prima_Vacacional_Aguinaldo(String Empleado_ID)
        {
            //VARIABLES DE NEGOCIO
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.

            ///VARIABLES PARA MANTENER EN MEMORIA LOS REGISTROS CONSULTADOS.
            DataTable Dt_Tabulador_ISR;                          //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR        
            DataTable Dt_Resultado = new DataTable();            //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;                    //Variable que almacena el registro que almacena el resultado de la operación actual.

            ///VARIABLES DE LA TABLA DE TAB_NOM_ISR
            Double Limite_Inferior_Tab_ISR = 0;                  //Limite_Inferior correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Cuota_Fija_Tab_ISR = 0;                       //Couta_Fija correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Porcentaje_Tab_ISR = 0;                       //Porcentaje correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR

            ///VARIABLES PARA LA GENERACIÒN DE LA NOMINA
            Double ISPT = 0;                                     //Obtiene el impuesto a retener al Empleado
            Double ISR_Retener_Empleado = 0;                     //ISR a retener al Empleado 
            Double Cas_Empleado = 0;                             //Subsidio para el Empleado. La deduccion pasa a ser percepcion para el empleado.

            ///TOTAL DE GRAVA DE LA PRIMA VACACIONAL Y EL AGUINALDO.
            Double Ingreso_Gravable = 0;                         //Variable que almacenará la Cantidad Total que Grava la Prima Vacacional y el Aguinaldo Juntos.
            Double Sueldo_Mensual = 0.0;                         //Variable que almacenara el sueldo mensual del empleado.
            Double Salario_Diario = 0.0;                         //Variable que almacenara el salario diario del empleado.   
            Double DIAS = Cls_Utlidades_Nomina.Dias_Mes_Fijo;    //Variable que almacenara el  dias a considerara para realizar esta operación.
            Double ISPT_Aguinaldo_PV_Sueldo_Mensual = 0.0;       //Variable que almacenara el ISPT Aguinaldo PV y Sueldo Mensual.
            Double ISPT_Sueldo_Mensual = 0.0;                    //Variable que almacenara el ISPT Sueldo Mensual.
            Double DIAS_PERIODO_NOMINAL = 0;                     //Variable que almacena la cantidad de dias del periodo nomina.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

                //OBTENEMOS LOS DIAS QUE TIENE EL PERIODO NOMINAL.
                DIAS_PERIODO_NOMINAL = Cls_Utlidades_Nomina.Dias_Mes_Fijo;

                /*
                 * CONSULTAMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO APLICA 
                 * PARA ISSEG, SE CONSIDERAR PARA OBTENER EL SALARIO DIARIO LA SUMA DEL SUELDO MÁS LA PSM
                 * DEL EMPLEADO.
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

                //PASO II.- OBTENEMOS EL SUELDO MENSUAL DEL EMPLEADO.
                Sueldo_Mensual = (Salario_Diario * DIAS);

                //PASO III.- CREAMOS LA ESTRUCTURA DE LA TABLA DE RESULTADOS.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                //Paso IV.- Se Suma el Gravante de la Prima Vacacional y el Gravante del Aguinaldo.
                Ingreso_Gravable = (Gravable_Prima_Vacacional + Gravable_Aguinaldo + Sueldo_Mensual);

                //PASO V.- Si el total gravante de prima vacacional y aguinaldo es mayor a cero entonces.
                if (Ingreso_Gravable > 0)
                {
                    //PASO VI.- Se consulta la tabla de ISR [TAB_NOM_ISR]  para obtener el límite inferior (LI),
                    //          Procentaje (%) y Cuota Fija (CF). En base al Total de Ingreso Gravable
                    //          del Empleado.
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Ingreso_Gravable, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR is DataTable)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //Obtenemos las cantidades de Limite_Inferior, Cuota Fija y Porcentaje de ISR.
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                Limite_Inferior_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                                Cuota_Fija_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim()))
                                Porcentaje_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                        }
                    }

                    //PASO VII.- Se ejecuta el calculo del ISPT_Aguinaldo_PV_Sueldo_Mensual
                    ISPT_Aguinaldo_PV_Sueldo_Mensual = (((Ingreso_Gravable - Limite_Inferior_Tab_ISR) * Porcentaje_Tab_ISR) + Cuota_Fija_Tab_ISR);

                    //PASO VIII.- Se consulta la tabla de ISR [TAB_NOM_ISR]  para obtener el límite inferior (LI),
                    //          Procentaje (%) y Cuota Fija (CF). En base al sueldo Mensual del Empleado.
                    //          del Empleado.
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Sueldo_Mensual, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR is DataTable)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //Obtenemos las cantidades de Limite_Inferior, Cuota Fija y Porcentaje de ISR.
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString()))
                                Limite_Inferior_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString()))
                                Cuota_Fija_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString()))
                                Porcentaje_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                        }
                    }

                    //PASO IX.- Obtenemos el ISPT_Sueldo_Mensual.
                    ISPT_Sueldo_Mensual = (((Sueldo_Mensual - Limite_Inferior_Tab_ISR) * Porcentaje_Tab_ISR) + Cuota_Fija_Tab_ISR);

                    //PASO X.- Obtenemos el ISPT.
                    ISPT = (ISPT_Aguinaldo_PV_Sueldo_Mensual - ISPT_Sueldo_Mensual);

                    //Paso XI.- Si ISPT es mayor a cero entonces es impuesto a retener por parte del trabajador
                    if (ISPT > 0)
                    {
                        //Obtenemos la cantidad que se le duducirá al empleado.
                        //Se convierte en ISR a retener al empleado.
                        ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", ISPT));
                        Cas_Empleado = 0;

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = false;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                    else
                    {
                        //si ISPT es menor a cero entonces, se convierte en eun subsidio para el empleado.
                        //y este será una percepción a entregar al empleado.
                        ISR_Retener_Empleado = 0;

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = false;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al Calcular_ISPT_Prima_Vacacional_Aguinaldo. ERROR [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_ISPT_Prima_Vacacional
        /// DESCRIPCIÓN:
        ///             1.- Se obtiene el grava de la prima vacacional. 
        ///             2.- Si es mayor a cero entonces:
        ///             
        ///                 a) .- Se consulta la tabla de ISR  para obtener el límite inferior (LI),
        ///                       Procentaje (%) y Cuota Fija (CF). En base a la Grava de la Prima Vacacional.
        ///                     
        ///                 b).- Se consulta la tabla de subsidio para obtener el subsidio causado
        ///                      en base a la Grava de la Prima Vacacional.
        ///                     
        ///                 c).- Calcular el ISPT del Empleado en base a la formula: 
        ///                     
        ///                     ISPT = ((( Ingresos_Gravables - LI ) * % ) + CF ) - Subsidio
        ///                 
        ///                 d).- Si el ISPT > 0 entonces:
        ///                     - Se convierte en ISR a retener
        ///                       ISR = ISPT
        ///                 
        ///                 e).- Si ISPT menor a 0 entonces:
        ///                     - Se convierte en un subsidio para el empleado por lo tanto se
        ///                       convierte en una percepción para el empleado
        ///                       
        ///                       Subsidio_Empleado = ISPT * (-1) 
        ///                       
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 29/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public DataTable Calcular_ISPT_Prima_Vacacional(String Empleado_ID)
        {
            //VARIABLES DE NEGOCIO
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.

            ///VARIABLES PARA MANTENER EN MEMORIA LOS REGISTROS CONSULTADOS.
            DataTable Dt_Resultado = new DataTable();            //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;                    //Variable que almacena el registro con el resultado del cálculo de ISR. 
            DataTable Dt_Tabulador_ISR;                          //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR      

            ///VARIABLES DE LA TABLA DE TAB_NOM_ISR
            Double Limite_Inferior_Tab_ISR = 0;                  //Limite_Inferior correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Cuota_Fija_Tab_ISR = 0;                       //Couta_Fija correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR
            Double Porcentaje_Tab_ISR = 0;                       //Porcentaje correspondiente al salario gravable del empleado. Consultado en la tabla de TAB_NOM_ISR

            ///VARIABLES PARA LA GENERACIÒN DE LA NOMINA
            Double ISPT = 0;                                     //Obtiene el impuesto a retener al Empleado
            Double ISR_Retener_Empleado = 0;                     //ISR a retener al Empleado 
            Double Cas_Empleado = 0;                             //Subsidio para el Empleado. La deduccion pasa a ser percepcion para el empleado.

            ///TOTAL GRAVA DE PRIMA VACIONAL
            Double Ingreso_Gravable = 0;                         //Variable que almacenará el valor de la cantidad que Grava la Prima Vacacional.
            Double Sueldo_Mensual = 0.0;                         //Variable que almacenara el sueldo mensual del empleado.
            Double Salario_Diario = 0.0;                         //Variable que almacenara el salario diario del empleado.   
            Double DIAS = Cls_Utlidades_Nomina.Dias_Mes_Fijo;    //Variable que almacenara el  dias a considerara para realizar esta operación.
            Double ISPT_Prima_Vacacional_Sueldo_Mensual = 0.0;   //Variable que almacenara el ISPT Aguinaldo PV y Sueldo Mensual.
            Double ISPT_Sueldo_Mensual = 0.0;                    //Variable que almacenara el ISPT Sueldo Mensual.
            Double DIAS_PERIODO_NOMINAL = 0;                      //Variable que almacena la cantidad de dias del periodo nomina.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

                //OBTENEMOS LOS DIAS QUE TIENE EL PERIODO NOMINAL.
                DIAS_PERIODO_NOMINAL = Cls_Utlidades_Nomina.Dias_Mes_Fijo;

                /*
                 * CONSULTAMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO APLICA 
                 * PARA ISSEG, SE CONSIDERAR PARA OBTENER EL SALARIO DIARIO LA SUMA DEL SUELDO MÁS LA PSM
                 * DEL EMPLEADO.
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

                //PASO II.- OBTENEMOS EL SUELDO MENSUAL DEL EMPLEADO.
                Sueldo_Mensual = (Salario_Diario * DIAS);

                //PASO III.- CREAMOS LA ESTRUCTURA DE LA TABLA DE RESULTADOS.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                //PASO I.- SE OBTIENE EL GRAVA DE LA PRIMA VACACIONAL. 
                Ingreso_Gravable = (Gravable_Prima_Vacacional + Sueldo_Mensual);

                //SI ES MAYOR A CERO ENTONCES:
                if (Ingreso_Gravable > 0)
                {
                    //PASO II.- SE CONSULTA LA TABLA DE ISR [TAB_NOM_ISR] PARA OBTENER EL LÍMITE INFERIOR (LI),
                    //          PROCENTAJE (%) Y CUOTA FIJA (CF). EN BASE A LA GRAVA DE LA PRIMA VACACIONAL.
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Ingreso_Gravable, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR is DataTable)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //OBTENEMOS LA CANTIDAD DE LIMITE INFERIOR, CUOTA FIJA Y PROCENTAJE DE ISR.
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString()))
                                Limite_Inferior_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString()))
                                Cuota_Fija_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString()))
                                Porcentaje_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                        }
                    }

                    //PASO IV.- SE EJECUTA EL CALCULO DEL ISPT VACACIONES.
                    ISPT_Prima_Vacacional_Sueldo_Mensual = (((Ingreso_Gravable - Limite_Inferior_Tab_ISR) * Porcentaje_Tab_ISR) + Cuota_Fija_Tab_ISR);

                    //PASO II.- SE CONSULTA LA TABLA DE ISR [TAB_NOM_ISR] PARA OBTENER EL LÍMITE INFERIOR (LI),
                    //          PROCENTAJE (%) Y CUOTA FIJA (CF). EN BASE AL SUELDO MENSUAL.
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Sueldo_Mensual, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR is DataTable)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //OBTENEMOS LA CANTIDAD DE LIMITE INFERIOR, CUOTA FIJA Y PROCENTAJE DE ISR.
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString()))
                                Limite_Inferior_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString()))
                                Cuota_Fija_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString()))
                                Porcentaje_Tab_ISR = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                        }
                    }

                    //PASO IV.- SE EJECUTA EL CALCULO DEL ISPT VACACIONES.
                    ISPT_Sueldo_Mensual = (((Sueldo_Mensual - Limite_Inferior_Tab_ISR) * Porcentaje_Tab_ISR) + Cuota_Fija_Tab_ISR);

                    //OBTENEMOS EL ISPT.
                    ISPT = (ISPT_Prima_Vacacional_Sueldo_Mensual - ISPT_Sueldo_Mensual);

                    //PASO V.- SI ISPT ES MAYOR A CERO ENTONCES ES IMPUESTO A RETENER POR PARTE DEL TRABAJADOR.
                    if (ISPT > 0)
                    {
                        //SE CONVIERTE EN ISR A RETENER AL EMPLEADO.
                        ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", ISPT));
                        Cas_Empleado = 0;

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = false;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                    else
                    {
                        //SE CONVIERTE EN UN SUBSIDIO PARA EL EMPLEADO Y ESTE SERÁ UNA UNA PERCEPCIÓN 
                        //A OTORGAR AL EMPLEADO.
                        ISR_Retener_Empleado = 0;

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = true;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                }//VALIDACION  PARA CALCULAR EL ISPT SOLO SI EL INGRESO GRAVABLE ES MAYOR A CER0
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al Calcular_ISPT_Vacaciones. ERROR [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        #endregion

        #region (Deducciones Calculadas)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Retardos
        /// DESCRIPCION : 
        ///               1.- Se consulta y se suma la cantidad de minutos que el empleado
        ///                   llego tarde.
        ///               2.- Se obtiene la cantidad a descontar.
        ///               
        ///     Formula:  Descuento_Retardos = (Salario_Diario / 480 min) * MINUTOS_RETARDO
        /// 
        /// PARÁMETROS: Empleado_ID.- Clabe del empleado que utilizaramos para obtener toda
        ///                           la información referente al empleado que se requiera para
        ///                           realizar el calculo actual.
        ///                           
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 28/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public Double Calcular_Retardos(String Empleado_ID)
        {
            ///VARIABLES DE TIPO CANTIDAD [$ Ó DIAS].
            Double Cantidad_Descontar_Retardos = 0.0;//Variable que almacenrá la cantidad que se le descontará al empleado por los retardos que ha tenido en la catorcena anterior a la actual.
            Double Minutos_Retardo = 0.0;//Variable que almacenará la cantidad de minutos que el empleado a acumulado como retardos durante la catorcena anterior a la actual. 
            Double Salario_Diario_Empleado = 0.0;//Variable que almacenará el salario diario del empleado, de acuerdo al puesto que tiene actualmente.
            Double MINUTOS_DIA = 0.0;//Variable que almacena la cantidad de minutos que tiene un dia de 8 Hrs. laborales.[Este debe de ser un Parámetro]

            //VARIABLES DE NEGOCIO
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacena la información del parámetro de la nómina.

            try
            {
                //CONSULTAMOS LA INFORMACION DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACIÓN DEL PARÁMETRO DE LA NÓMINA
                INF_PARAMETRO = _Informacion_Parametros_Nomina();

                //OBTENEMOS EL PARAMETRO DE LOS MINUTOS QUE TENDRÁ EL DÍA.
                MINUTOS_DIA = Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_Minutos_Dia)) ? "0" : INF_PARAMETRO.P_Minutos_Dia);

                /*
                 * CONSULTAMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO APLICA 
                 * PARA ISSEG, SE CONSIDERAR PARA OBTENER EL SALARIO DIARIO LA SUMA DEL SUELDO MÁS LA PSM
                 * DEL EMPLEADO.
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

                //OBTENEMOS LA CANTIDAD DE MINUTOS DE RETARDO QUE EL EMPLEADO A TENIDO EN EL PERIODO DE PAGO ANTERIOR AL ACTUAL.
                Minutos_Retardo = Obtener_Minutos_Retardo_Empleado(Empleado_ID);

                //VALIDAR QUE LOS MINUTOS DE RETARDO SEAN MAYORES A CERO.
                if (Minutos_Retardo > 0)
                {
                    //REALIZAMOS EL CALCULO DE LA CANTIDAD QUE SE LE DESCONTARÁ AL EMPLEADO DE ACUERDO A LA 
                    //CANTIDAD DE MINUTOS QUE TUVO EN LA CATORCENA ANTERIOR A LA ACTUAL.
                    Cantidad_Descontar_Retardos = (Salario_Diario_Empleado / MINUTOS_DIA) * Minutos_Retardo;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar el calculo de retardos del empleado. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Descontar_Retardos;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Retenciones_Terceros
        /// DESCRIPCION : 
        ///                 1.- Se obtienen el neto a entregar al empleado.
        ///                     NETO = Total_Percepciones - Total_Deducciones
        ///                 2.- Se consulta el parámetro a considerar para está
        ///                     deducciónel cual es un porcentaje [%].
        ///                 3.- Se realiza el calculo de la Deduccion.
        ///                 
        ///                 Formula:  Retencion_Terceros = NETO * [%]
        ///                 
        /// PARÁMETROS: Empleado_ID.- Clabe del empleado que utilizaramos para obtener toda
        ///                           la información referente al empleado que se requiera para
        ///                           realizar el calculo actual.
        ///                           
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 28/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public Double Calcular_Retenciones_Terceros(String Empleado_ID)
        {
            ///VARIABLES DE TIPO TABLA QUE ALMACENARAN EL REGISTRO CONSULTADO DE RETENCION A TERCEROS.
            DataTable Dt_Terceros = null;       //Variable que almacenara el registro de retencion a terceros consultado.

            ///VARIABLE DE TIPO CANTIDAD [$ Ó DIAS], QUE SE OCUPARAN EN EL CALCULO DE RETENCIAN A TERCEROS.
            Double Neto = 0.0;                  //Variable que almacenará la cantidad neta a entregar al empleado.
            Double Porcentaje_Retencion = 0.0;  //Variable que almacenará el porcentaje de retención.
            Double Retencion_Terceros = 0.0;    //Variable que almacenará la cantidad a retener al empleado.

            try
            {
                //PASO I.- SE OBTIENE EL NETO A ENTREGAR AL EMPLEADO.
                Neto = Total_Percepciones - Total_Deducciones;

                //PASO II.- SE CONSULTA EL PARÁMETRO A CONSIDERAR PARA ESTE CALCULO.
                Dt_Terceros = Cls_Ope_Nom_Deducciones_Datos.Consultar_Parametro_Retencion_Terceros(Empleado_ID);

                if (Dt_Terceros is DataTable)
                    if (Dt_Terceros.Rows.Count > 0)
                        if (!string.IsNullOrEmpty(Dt_Terceros.Rows[0][Cat_Nom_Terceros.Campo_Porcentaje_Retencion].ToString()))
                            Porcentaje_Retencion = Convert.ToDouble(Dt_Terceros.Rows[0][Cat_Nom_Terceros.Campo_Porcentaje_Retencion].ToString()) / 100;

                //PASO III.- SE REALIZA EL CALCULO DE LA RETENCION A TERCEROS.
                Retencion_Terceros = (Neto * Porcentaje_Retencion);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular las retenciones a terceros que tiene el empleado. Error: [" + Ex.Message + "]");
            }
            return Retencion_Terceros;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Retencion_Fondo_Retiro
        /// DESCRIPCION : 
        ///               1.- Se consulta el parámetro a considerar para esta deducción
        ///                   el cual es un porcentaje [%].
        ///               2.- Se calcula la Retención
        ///               
        ///               Formula:   Retencion = ( ( Salario_Diario * 14 [dias] )* [%] ) * 2 
        ///               
        /// PARÁMETROS: Empleado_ID.- Clabe del empleado que utilizaramos para obtener toda
        ///                           la información referente al empleado que se requiera para
        ///                           realizar el calculo actual.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 28/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public Double Retencion_Fondo_Retiro(String Empleado_ID)
        {
            //VARIABLES DE NEGOCIO
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//VARIABLE QUE ALMACENARA LA INFORMACIÓN DEL EMPLEADO.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//VARIABLE QUE ALMACENARA LA INF. DEL PARÁMETRO DE LA NÓMINA.

            ///VARIABLE DE TIPO CANTIDAD [$ Ó DIAS], QUE SE OCUPARAN EN EL CALCULO DE RETENCIAN A TERCEROS.
            Double Cantidad_Retener_Fondo_Retiro = 0.0;//Variable que almacenrá la cantidad a retener al empleado y que se ira al fondo de retiro del empleado.
            Double Salario_Diario = 0.0;//Variable que almacenará el salario diario del empleado.
            Double DIAS_CONSIDERAR = 0;//Dias que se consideraran para este calculo.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACIÓN DEL PARÁMETRO DE LA NÓMINA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();

                //Obtener los dias laborados por el empleado.
                DIAS_CONSIDERAR = Obtener_Dias_Laborados_Empleado(Empleado_ID);

                /*
                 * CONSULTAMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO APLICA 
                 * PARA ISSEG, SE CONSIDERAR PARA OBTENER EL SALARIO DIARIO LA SUMA DEL SUELDO MÁS LA PSM
                 * DEL EMPLEADO.
                 */
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

                //PASO III.- REALIZAMOS EL CÁLCULO DE RETENCIÓN DEL FONDO DE RETIRO.
                Cantidad_Retener_Fondo_Retiro = ((Salario_Diario * DIAS_CONSIDERAR) * INF_PARAMETRO.P_Porcentaje_Fondo_Retiro) * 2;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar el calculo de Retencion Fondo Retiro del empleado. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Retener_Fondo_Retiro;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_Inasistencias
        /// DESCRIPCION : 
        ///               1.- Se considera el número de faltas que tuvo el empleado durante
        ///                   la catorcena anterior y estas se suman.
        ///               2.- El número de faltas se multiplican por el salario diario del 
        ///                   empleado.
        ///               
        ///               Formula:   Inasistencias = ( Número_Faltas * Salario_Diario ) 
        /// 
        /// PARÁMETROS: Empleado_ID.- Clabe del empleado que utilizaramos para obtener toda
        ///                           la información referente al empleado que se requiera para
        ///                           realizar el calculo actual.
        ///                           
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 28/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public Double Calcular_Inasistencias(String Empleado_ID)
        {
            //VARIABLES DE NEGOCIO
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

            //VARIABLES GENERALES
            Int32 No_Faltas_Empleado = 0;//Variable que almacenará el número de faltas que a tenido el empleado en la catorcena anterior.
            Double Salario_Diario_Empleado = 0.0;//Variable que almacenrá el salario deiario del empleado.
            Double Cantidad_Descontar_Inasistencias = 0.0;//Variable que almacenará la cantidad a descontar por las inasistencias que a tenido el empleado en la catorcena anterior.  

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

                /*
                 * CONSULTAMOS EL SALARIO DIARIO DEL EMPLEADO CONSIDERANDO QUE SI EL EMPLEADO APLICA 
                 * PARA ISSEG, SE CONSIDERAR PARA OBTENER EL SALARIO DIARIO LA SUMA DEL SUELDO MÁS LA PSM
                 * DEL EMPLEADO.
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

                //PASO I.- OBTENEMOS LA CANTIDAD DE FALTAS QUE A TENIDO EL EMPLEADO EN LA CATORCENA ANTERIOR.
                No_Faltas_Empleado = Obtener_Faltas_Empleados(Empleado_ID);

                //PASO III.- REALIZAR EL CALCULO DE EL MONTO A DESCONTYAR POR LAS INASISTENCIAS QUE A TENIDO EL 
                //EMPLEADO EN LA CATORCENA ANTERIOR.
                Cantidad_Descontar_Inasistencias = (No_Faltas_Empleado * Salario_Diario_Empleado);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular las inasistencias. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Descontar_Inasistencias;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Pago_Abono_Prestamo
        /// DESCRIPCION : Ejecuta el pago del abono del prestamo.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificador del empleado.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Enero/2010 10:27 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public DataTable Pago_Abono_Prestamo(String Empleado_ID)
        {
            Cls_Ope_Nom_Pestamos_Negocio Cls_Prestamos_Negocio = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
            DataTable Dt_Abonos_Prestamos = new DataTable();//Estructura que almacena el registro del abono al prestamo realizado.
            DataTable Dt_Prestamos_Autorizados_Activos_Empleado = null;//Lista de prestamos que debe actualmente el empleado.
            String No_Solicitud = "";//Varaible que almacenará el número de solicitud del prestamo.
            DataRow Renglon_Abono = null;//Variable que almacenara el registro del abono realizado.
            String Deduccion_ID = "";//Variable que almacena el identificador de la deducción aplicar.
            StringBuilder Historial_Nomina_Generada = null;//Variable que almacena el Historial de los registros afectados al generar la nómina.
            Double Importe_Prestamo = 0.0;//Cantidad o Monto solicitado como prestamo.
            Double Importe_Interes = 0.0;//Cantidad, Monto o % Extra por autorizar el del prestamo.  
            Double Total_Prestamo = 0.0;//Monto total del prestamo.
            Double Monto_Abonado = 0.0;//Cantidad abonada al prestamo.
            Double Saldo_Actual = 0.0;//Saldo actual del prestamo.
            Double Abono = 0.0;//Cantidad a descontar catorcenalmente como concepto del prestamo otorgado.
            Double No_Abono = 0.0;//Núemro de abono realizado al préstamo.
            Double No_Pagos = 0.0;//Número de pagos que el empleado debera realizar para cobrar el total del préstamo.

            try
            {
                Dt_Abonos_Prestamos.Columns.Add("Deduccion", typeof(String));
                Dt_Abonos_Prestamos.Columns.Add("Cantidad", typeof(Double));

                Dt_Prestamos_Autorizados_Activos_Empleado = Cls_Ope_Nom_Deducciones_Datos.Consultar_Prestamos_Empleado(Empleado_ID, Fecha_Generar_Nomina);

                if (Dt_Prestamos_Autorizados_Activos_Empleado != null)
                {
                    foreach (DataRow Renglon in Dt_Prestamos_Autorizados_Activos_Empleado.Rows)
                    {
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString().Trim()))
                            No_Solicitud = Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString().Trim();
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString().Trim()))
                            Importe_Prestamo = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString().Trim()))
                            Importe_Interes = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString().Trim()))
                            Total_Prestamo = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString().Trim()))
                            Monto_Abonado = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString().Trim()))
                            Saldo_Actual = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString().Trim()))
                            Abono = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString().Trim()))
                            No_Abono = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString().Trim());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString().Trim()))
                            No_Pagos = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString().Trim());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                            Deduccion_ID = Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                        if (Saldo_Actual >= Abono)
                        {
                            No_Abono = No_Abono + 1;

                            if (No_Abono < No_Pagos)
                            {
                                Saldo_Actual = (Saldo_Actual - Abono);
                                Monto_Abonado += Abono;

                                Cls_Prestamos_Negocio.P_Monto_Abonado = Monto_Abonado;
                                Cls_Prestamos_Negocio.P_Saldo_Actual = Saldo_Actual;
                                Cls_Prestamos_Negocio.P_No_Abono = (Int32)No_Abono;
                                Cls_Prestamos_Negocio.P_No_Solicitud = No_Solicitud;
                            }
                            else if (No_Abono == No_Pagos)
                            {
                                Saldo_Actual = 0;
                                Monto_Abonado = Total_Prestamo;

                                Cls_Prestamos_Negocio.P_Monto_Abonado = Monto_Abonado;
                                Cls_Prestamos_Negocio.P_Saldo_Actual = Saldo_Actual;
                                Cls_Prestamos_Negocio.P_No_Abono = (Int32)No_Abono;
                                Cls_Prestamos_Negocio.P_No_Solicitud = No_Solicitud;
                            }

                            Cls_Ope_Nom_Deducciones_Datos.Capturar_Pago_Abono_Prestamo_Catorcenal(Cls_Prestamos_Negocio);

                            if (Total_Prestamo == Monto_Abonado)
                            {
                                Cls_Ope_Nom_Deducciones_Datos.Cambiar_Estado_Prestamo(No_Solicitud, Fecha_Generar_Nomina);
                            }

                            Renglon_Abono = Dt_Abonos_Prestamos.NewRow();
                            Renglon_Abono["Deduccion"] = Deduccion_ID;
                            Renglon_Abono["Cantidad"] = Abono;
                            Dt_Abonos_Prestamos.Rows.Add(Renglon_Abono);
                        }
                    }
                    if (Dt_Prestamos_Autorizados_Activos_Empleado.Rows.Count > 0)
                    {
                        Historial_Nomina_Generada = Cls_Sessiones.Historial_Nomina_Generada;
                        Cls_Historial_Nomina_Generada.Crear_Registro_Insertar_Prestamo(Dt_Prestamos_Autorizados_Activos_Empleado, ref Historial_Nomina_Generada);
                        Cls_Sessiones.Historial_Nomina_Generada = Historial_Nomina_Generada;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar el Pago del prestamo. Error: [" + Ex.Message + "]");
            }
            return Dt_Abonos_Prestamos;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Pago_Prestamo_Finiquito
        /// DESCRIPCION : Ejecuta el pago del abono del prestamo.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificador del empleado.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Enero/2010 10:27 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public DataTable Pago_Prestamo_Finiquito(String Empleado_ID)
        {
            Cls_Ope_Nom_Pestamos_Negocio Cls_Prestamos_Negocio = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
            DataTable Dt_Abonos_Prestamos = new DataTable();//Estructura que almacena el registro del abono al prestamo realizado.
            DataTable Dt_Prestamos_Autorizados_Activos_Empleado = null;//Lista de prestamos que debe actualmente el empleado.
            String No_Solicitud = "";//Varaible que almacenará el número de solicitud del prestamo.
            DataRow Renglon_Abono = null;//Variable que almacenara el registro del abono realizado.
            String Deduccion_ID = "";//Variable que almacena el identificador de la deducción aplicar.
            StringBuilder Historial_Nomina_Generada = null;//Variable que almacena el Historial de los registros afectados al generar la nómina.
            Double Importe_Prestamo = 0.0;//Cantidad o Monto solicitado como prestamo.
            Double Importe_Interes = 0.0;//Cantidad, Monto o % Extra por autorizar el del prestamo.  
            Double Total_Prestamo = 0.0;//Monto total del prestamo.
            Double Monto_Abonado = 0.0;//Cantidad abonada al prestamo.
            Double Saldo_Actual = 0.0;//Saldo actual del prestamo.
            Double Abono = 0.0;//Cantidad a descontar catorcenalmente como concepto del prestamo otorgado.
            Double No_Abono = 0.0;//Núemro de abono realizado al préstamo.
            Double No_Pagos = 0.0;//Número de pagos que el empleado debera realizar para cobrar el total del préstamo.

            try
            {
                Dt_Abonos_Prestamos.Columns.Add("Deduccion", typeof(String));
                Dt_Abonos_Prestamos.Columns.Add("Cantidad", typeof(Double));

                Dt_Prestamos_Autorizados_Activos_Empleado = Cls_Ope_Nom_Deducciones_Datos.Consultar_Prestamos_Empleado_Finiquito(Empleado_ID, Fecha_Generar_Nomina);

                if (Dt_Prestamos_Autorizados_Activos_Empleado != null)
                {
                    foreach (DataRow Renglon in Dt_Prestamos_Autorizados_Activos_Empleado.Rows)
                    {
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString().Trim()))
                            No_Solicitud = Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString().Trim();
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString().Trim()))
                            Importe_Prestamo = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString().Trim()))
                            Importe_Interes = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString().Trim()))
                            Total_Prestamo = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString().Trim()))
                            Monto_Abonado = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString().Trim()))
                            Saldo_Actual = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString().Trim()))
                            Abono = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString().Trim()))
                            No_Abono = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString().Trim());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString().Trim()))
                            No_Pagos = Convert.ToDouble(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString().Trim());
                        if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                            Deduccion_ID = Renglon[Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                        if (Saldo_Actual != 0)
                        {
                            Renglon_Abono = Dt_Abonos_Prestamos.NewRow();
                            Renglon_Abono["Deduccion"] = Deduccion_ID;
                            Renglon_Abono["Cantidad"] = Saldo_Actual;
                            Dt_Abonos_Prestamos.Rows.Add(Renglon_Abono);
                        }
                    }
                    if (Dt_Prestamos_Autorizados_Activos_Empleado.Rows.Count > 0)
                    {
                        Historial_Nomina_Generada = Cls_Sessiones.Historial_Nomina_Generada;
                        Cls_Historial_Nomina_Generada.Crear_Registro_Insertar_Prestamo(Dt_Prestamos_Autorizados_Activos_Empleado, ref Historial_Nomina_Generada);
                        Cls_Sessiones.Historial_Nomina_Generada = Historial_Nomina_Generada;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar el Pago del prestamo. Error: [" + Ex.Message + "]");
            }
            return Dt_Abonos_Prestamos;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_ISSEG
        /// 
        /// DESCRIPCION : Método que realiza el cálculo de ISSEG.
        /// 
        ///         TOPE = (Salario Diario Zona Economica C * 10) * (30 días)
        ///         
        ///             Actualmente:
        ///                 Salario Diario Zona C = $59.08
        ///                 Entoncés:
        ///                     Tope = ((59.08 * 10) * 30)
        ///                     Tope = $17,724.00
        /// 
        ///         Si Salario_Mensual es menor a Tope Entonces:
        ///         
        ///             Calculo cuando el Salario Mensual no sobrepasa el TOPE ISSEG
        ///             ISSEG = (SALARIO_MENSUAL * (% Factor Social)) / 2
        ///             
        ///         Si Salario_Mensual es mayor a Tope Entonces:
        ///             ISSEG = 1347.00
        /// 
        /// PARAMETROS:  Empleado_ID: El identificador del empleado.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Septiembre/2011 10:27 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public Double Calcular_ISSEG(String Empleado_ID)
        {
            //VARIABLES DE NEGOCIO
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la informacion del empleado.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacenara la información del parámetro de nómina.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;

            //VARIABLES GENERALES PARA LOS CÁLCULOS.
            Double Deduccion_ISSEG = 0.0;//Variable que almacenara el monto de la deducción de ISSEG
            Double Salario_Diario_Empleado = 0.0;//Variable que almacenara el salario diario del empleado.
            Double SALARIO_MENSUAL_NIVEL = 0.0;//variable que almacenara el salario mensual del puesto.
            Double DIAS_NOMINA = 0.0;//Variable que almacena los días que tiene el periodo nóminal.
            Double Cantidad_Retener_Tope_ISSEG = 0.0;//Variable que almacena la cantidad que se le retendrá al empleado cuando se sobrepase el tope de ISSEG.
            Double Tope_ISSEG = 0.0;//Tope ISSEG en donde ya no aplicara el calculo si no una retencion por cantidad fija.

            try
            {
                //OBTENEMOS LOS DÍAS QUE TIENE EL PERIODO NOMINAL A GENERAR, CONSIDERANDO SI LA FECHA DE INGRESO DEL EMPLEADO ES MAYOR A LA FECHA DE INICIO DE LA CATORCENA.
                DIAS_NOMINA = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //CONSULTAMOS LA INFORMACION DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULATAMOS INFORMACIÓN DEL PARÁMETRO DE NÓMINA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();
                //Consultamos la zona económica.
                INF_ZONA_ECONOMICA = Cls_Ayudante_Nom_Informacion._Informacion_Zona_Economica(INF_EMPLEADO.P_Zona_ID);

                if (!INF_EMPLEADO.P_Tipo_Nomina_ID.Equals("00005"))
                {
                    /*
                     * CONSULTAMOS EL SALARIO DIARIO DEL EMPLEADO SI EL EMPLEADO APLICA 
                     * PARA ISSEG.
                     */
                    //VALIDAMOS QUE EL CAMPO DE ISSEG NO VENGA NULL.
                    if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Aplica_ISSEG))
                    {
                        //VALIDACIÓN PARA SABER SI APLICA O NO PARA EL CALCULO DE ISSEG.
                        if (INF_EMPLEADO.P_Aplica_ISSEG.Trim().ToUpper().Equals("SI"))
                        {
                            //OBTENEMOS LA CANTIDAD DE SALARIO DIARIO [SUELDO DEL NUEVO TABULADOR]
                            Salario_Diario_Empleado = Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG(Empleado_ID);
                            //OBTENEMOS EL SALARIO MENSUAL DEL TABULADOR
                            SALARIO_MENSUAL_NIVEL = (Salario_Diario_Empleado * Cls_Utlidades_Nomina.Dias_Mes_Fijo);
                        }
                    }

                    //El salario de la zona C 10 veces por 30 días.
                    Tope_ISSEG = ((INF_ZONA_ECONOMICA.P_Salario_Diario * 10) * 30);

                    //Si Salario_Mensual es mayor a Tope_ISSEG Entonces:
                    if (SALARIO_MENSUAL_NIVEL > Tope_ISSEG)
                    {
                        Cantidad_Retener_Tope_ISSEG = Convert.ToDouble(String.IsNullOrEmpty(INF_PARAMETRO.P_Tope_ISSEG) ? "0" : INF_PARAMETRO.P_Tope_ISSEG);
                        Deduccion_ISSEG = Cantidad_Retener_Tope_ISSEG;
                    }
                    else
                    {
                        //Si Salario_Mensual es menor a Tope_ISSEG Entonces:
                        //Deduccion_ISSEG = ((SALARIO_MENSUAL_NIVEL * Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_ISSEG_Porcentaje_Aplicar_Empleado)) ? "0" : INF_PARAMETRO.P_ISSEG_Porcentaje_Aplicar_Empleado)) / Cls_Utlidades_Nomina.Dias_Mes_Fijo) * DIAS_NOMINA;
                        Deduccion_ISSEG = (SALARIO_MENSUAL_NIVEL * Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_ISSEG_Porcentaje_Aplicar_Empleado)) ? "0" : INF_PARAMETRO.P_ISSEG_Porcentaje_Aplicar_Empleado)) / 2;
                    }
                }
                else Deduccion_ISSEG = 0.0;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar el cálculo de ISSEG. Error: [" + Ex.Message + "]");
            }
            return Deduccion_ISSEG;
        }
        #endregion

        #region (Cálculo de IMSS)
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calculo_IMSS_Actual
        /// 
        /// DESCRIPCION : Método que realiza el cálculo de IMSS actual.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificador del empleado al que se le calculará la dedución.
        ///                      
        /// CREO        : Francisco Antonio Gallardo Castañeda.
        /// FECHA_CREO  : 14/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public Double Calculo_IMSS_Actual(String Empleado_ID)
        {
            //VARIABLES DE NEGOCIO.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//VARIABLE QUE ALMACENARA LA INFORMACIÓN DEL EMPLEADO.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;//VARIABLE QUE ALMACENARA LA INFORMACION DE LA ZONA ECONÓMICA.
            Cls_Tab_Nom_IMSS_Negocio INF_IMSS = null;//VARIABLE QUE ALMACENARA LA INFORMACIÓN DEL TABULADOR DEL IMSS.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;//VARIABLE QUE ALMACENA LA INFORMACIÓN DE LOS PARÁMETROS DE LA NÓMINA.

            //VARIABLES GENERALES.
            Double Cantidad_IMSS = 0.0;//VARIABLE QUE ALMACENARA LA CANTIDAD DE IMSS.
            Double Salario_Diario = 0.0;//VARIABLE QUE ALMACENARA LA CANTIDAD DIARIA DE PREVISIÓN SOCIAL.
            Double Salario_Diario_Integrado_IMSS = 0.0;//VARIABLE QUE ALMACENARA EL SALARIO DIARIO INTEGRADO PARA IMSS.
            Double Salario_Base_Cotizacion = 0.0;//VARIABLE QUE ALMACENARA EL SALARIO BASE COTIZACIÓN.
            Double Porcentaje_Retencion_IMSS = 0.0;//VARIABLE QUE ALMACENARA EL TOTAL DE PORCENTAJE DE RETENCIÓN DE IMSS.
            Double SDDF_Tope_3_Salarios_Minimos = 0.0;//VARIABLE QUE ALMACENARA EL TOTAL DE 3 SALARIOS MINIMOS DE LA ZONA ECONÓMICA DEL EMPLEADO.
            Double Diferencia_Excedente = 0.0;//VARIABLE QUE ALMACENARA EL SALARIO DIARIO INTEGRADO MENOS 3 VECES EL SALARIO MINIMO.
            Double Salario_Base_Cotizacion_Excedente = 0.0;//VARIABLE QUE ALMACENARA EL SALARIO BASE COTIZACIÓN EXCEDENTE.
            Double Dias_Catorcena = 0;//VARIABLE QUE ALMACENARA LOS DIAS DEL PERIODO NOMINAL.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONÓMICA.
                INF_ZONA_ECONOMICA = _Informacion_Zona_Economica();
                //CONSULTAMOS LA INFORMACIÓN DE IMSS.
                INF_IMSS = _Informacion_IMSS();
                //CONSULTAMOS LA INFORMACIÓN DE LOS PARÁMETROS,
                INF_PARAMETROS = _Informacion_Parametros_Nomina();

                //OBTENEMOS LA CANTIDAD DE 3 VECES EL SALARIO DE ZONA.
                SDDF_Tope_3_Salarios_Minimos = (INF_ZONA_ECONOMICA.P_Salario_Diario * 3);

                //OBETENEMOS LOS DIAS DEL PERIODO NOMINAL.
                //Dias_Catorcena = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);
                Dias_Catorcena = Convert.ToDouble(String.IsNullOrEmpty(INF_PARAMETROS.P_Dias_IMSS) ? "0" : INF_PARAMETROS.P_Dias_IMSS);

                /*
                 * OBTENEMOS LA CANTIDAD DIARIA POR CONCEPTO DE PREVISIÓN SOCIAL MÚLTIPLE SI ES QUE EL EMPLEADO
                 * APLICA PARA EL NUEVO CÁLCULO DE ISSEG O LA CANTIDAD DIARIA DE ACUERDO AL SALARIO DE SU PUESTO.
                 */
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

                //VALIDAMOS QUE EL SALARIO DIARIO DEL EMPLEADO NO SEA MAYOR QUE EL SALARIO MENSUAL MÁXIMO ESTABLECIDO COMO PARÁMETRO.
                if ((Salario_Diario * 30.42) <= INF_PARAMETROS.P_Salario_Mensual_Maximo)
                    //OBTENEMOS EL SALARIO DIARIO INTEGRADO IMSS [CANTIDAD DIARIA * (1.126)]
                    Salario_Diario_Integrado_IMSS = (Salario_Diario * 1.126);
                else
                    Salario_Diario_Integrado_IMSS = INF_PARAMETROS.P_Salario_Diario_Integrado_Topado;

                //OBTENEMOS EL PORCENTAJE DE RETENCIÓN DE IMSS.
                Porcentaje_Retencion_IMSS = (INF_IMSS.P_Porcentaje_Invalidez_Vida + INF_IMSS.P_Porcentaje_Cesantia_Vejez + INF_IMSS.P_Prestaciones_Dinero +
                    INF_IMSS.P_Gastos_Medicos);

                //OBTENEMOS EL SALARIO BASE CON EL QUE COTIZARA EL EMPLEADO.
                Salario_Base_Cotizacion = (Salario_Diario_Integrado_IMSS * Dias_Catorcena);

                //SE CÁLCULA LA CANTIDAD DE IMSS A RETENER AL EMPLEADO.
                Cantidad_IMSS = (Salario_Base_Cotizacion * Porcentaje_Retencion_IMSS);

                //VALIDAMOS SI EL SALARIO DIARIO INTEGRADO IMSS ES MAYOR A 3 VECES EL SALARIO DE LA ZONA ECONÓMICA DEL DISTRITO FEDERAL [3_SDDF].
                if (Salario_Diario_Integrado_IMSS > SDDF_Tope_3_Salarios_Minimos)
                {
                    //RESTAMOS AL SDI_IMSS 3 VECES EL SALARIO DE LA ZONA ECONÓMICA.
                    Diferencia_Excedente = (Salario_Diario_Integrado_IMSS - SDDF_Tope_3_Salarios_Minimos) * INF_IMSS.P_Excendente_SMG_DF;
                    Cantidad_IMSS = ((Salario_Diario_Integrado_IMSS * Porcentaje_Retencion_IMSS) + Diferencia_Excedente) * Dias_Catorcena;
                }

                //Validamos que el empleado no tenga un tipo de nomina ASIMILABLE O PENSIONADOS ya que estas nomina no se les aplica retencion de IMSS.
                //Por lo tanto si el empleado pertence a estas nominas la ret. de imss es de cero.
                if (INF_EMPLEADO != null)
                {
                    if (INF_EMPLEADO.P_Tipo_Nomina_ID.Equals("00003") ||
                        INF_EMPLEADO.P_Tipo_Nomina_ID.Equals("00005"))
                    {
                        Cantidad_IMSS = 0.00;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular la DEDUCCIÓN de IMSS actual. Error: [" + Ex.Message + "]");
            }
            return Cantidad_IMSS;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Calcular_IMSS
        /// DESCRIPCION : Obtiene el monto de la deducción del IMSS.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificador del empleado al que se le calculará la dedución.
        ///                      
        /// CREO        : Francisco Antonio Gallardo Castañeda.
        /// FECHA_CREO  : 14/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public Double Calcular_IMSS(String Empleado_ID)
        {
            DataTable Dt_Calculo_IMSS = new DataTable();
            Double IMSS_Deduccion = 0;
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;
            Double Salario_Diario = 0.0;

            try
            {
                //CONSULTAMOS LA INF. EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);
                //CONSULTAMOS LA INF. PARAMETRO.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();

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

                Dt_Calculo_IMSS.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Calculo_IMSS.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                //SE CONSULTAN LOS DATOS DE LA TABLA DE IMSS
                Cls_Tab_Nom_IMSS_Negocio IMSS_Negocio = new Cls_Tab_Nom_IMSS_Negocio();
                DataTable Dt_Datos_IMSS = IMSS_Negocio.Consulta_Datos_IMSS();

                //CONSULTAMOS LOS DIAS DEL PERIODO NOMINAL
                Int32 Dias_Catorcena = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //OBTENEMOS LOS PARÁMETROS QUE SE OCUPARAN PARA REALIZAR EL CÁLCULO.
                Double Salario_Mensual = (Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo);
                Double Salario_Mensual_Maximo = INF_PARAMETRO.P_Salario_Mensual_Maximo;
                Double Salario_Diario_Integrado_Topado = INF_PARAMETRO.P_Salario_Diario_Integrado_Topado;
                Double Excecente_SMG_DF = Convert.ToDouble(Dt_Datos_IMSS.Rows[0][Tab_Nom_IMSS.Campo_Excendete_3_SMG_DF]);
                Double Prestamos_Dinero = Convert.ToDouble(Dt_Datos_IMSS.Rows[0][Tab_Nom_IMSS.Campo_Prestaciones_Dinero]);
                Double Gastos_Medicos = Convert.ToDouble(Dt_Datos_IMSS.Rows[0][Tab_Nom_IMSS.Campo_Gastos_Medicos]);
                Double Invalidez_Vida = Convert.ToDouble(Dt_Datos_IMSS.Rows[0][Tab_Nom_IMSS.Campo_Porcentaje_Invalidez_Vida]);

                //OBTENER LAS FALTAS DEL EMPLEADO
                Int32 Faltas = Obtener_Faltas_Empleados(Empleado_ID);

                //Se Obtienen los dias trabajados
                Int32 Dias_Trabajados = Dias_Catorcena - Faltas;

                Double Porcentaje_IMSS = Prestamos_Dinero + Gastos_Medicos + Invalidez_Vida;
                Double Cantidad_IMSS = 0;
                Double Salario_Diario_Integrado = 0;
                Double Salario_Diario_DF = 0;
                Double Diferencia_Excedente = 0;

                //Se valida el salario del empleado para 
                if (Salario_Mensual <= Salario_Mensual_Maximo)
                {
                    DataTable Dt_Percepciones_Fijas_IMSS = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "PERCEPCION", "FIJA");
                    DataTable Dt_Percepciones_Variables_IMSS = Consultar_Percepciones_Deducciones_Empleado(Empleado_ID, "PERCEPCION", "VARIABLE");

                    for (Int32 Contador = 0; Contador < Dt_Percepciones_Fijas_IMSS.Rows.Count; Contador++)
                    {
                        Cantidad_IMSS = Cantidad_IMSS + (Convert.ToDouble(Dt_Percepciones_Fijas_IMSS.Rows[Contador][Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()) / Cls_Utlidades_Nomina.Dias_Mes_Fijo);
                    }
                    for (Int32 Contador = 0; Contador < Dt_Percepciones_Variables_IMSS.Rows.Count; Contador++)
                    {
                        Cantidad_IMSS = Cantidad_IMSS + Obtener_Cantidad_Deduccion_Variable_Aplica(Empleado_ID, Dt_Percepciones_Variables_IMSS.Rows[Contador][Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString());
                    }
                    Salario_Diario_Integrado = (Salario_Diario * 1.126) + Cantidad_IMSS;
                }
                else
                {
                    Salario_Diario_Integrado = Salario_Diario_Integrado_Topado;
                }

                Salario_Diario_DF = Excecente_SMG_DF * 3;

                //Se valida si excede para sacar esa diferencia
                if (Salario_Diario_Integrado > Salario_Diario_DF)
                {
                    Diferencia_Excedente = (Salario_Diario_Integrado - Salario_Diario_DF) * Excecente_SMG_DF;
                }

                //Se calcula la deducción de IMSS
                IMSS_Deduccion = ((Salario_Diario_Integrado * Porcentaje_IMSS) + Diferencia_Excedente) * Dias_Trabajados;

                DataRow Renglon_Resultado = Dt_Calculo_IMSS.NewRow();
                Renglon_Resultado["Percepcion_Deduccion"] = true;
                Renglon_Resultado["Cantidad"] = IMSS_Deduccion;
                Dt_Calculo_IMSS.Rows.Add(Renglon_Resultado);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el Monto de Calculo de IMSS. Error: [" + Ex.Message + "]");
            }
            return IMSS_Deduccion;
        }
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
        /// MODIFICO          : Francisco Antonio Gallardo Castañeda
        /// FECHA_MODIFICO    : 18/Abril/2011
        /// CAUSA_MODIFICACION: Se le agrego el filtro que sea aplicable de IMSS solamente
        ///***********************************************************************************************************************
        private DataTable Consultar_Percepciones_Deducciones_Empleado(String Empleado_Id, String Tipo, String Tipo_Asignacion)
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
                         " FROM " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion +
                         ", " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                         " WHERE " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Tipo + "'" +
                         " AND " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + " ='" + Tipo_Asignacion + "'" +
                         " AND " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Empleado_Id + "'" +
                         " AND " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                         "=" +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID +
                         " AND " +
                         Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Aplica_IMSS + " = " + "'SI'";

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
                if (Dt_Deduccion_Var_Empl_Det != null)
                {
                    foreach (DataRow Deduccion_Variable in Dt_Deduccion_Var_Empl_Det.Rows)
                    {
                        if (!string.IsNullOrEmpty(Deduccion_Variable[Ope_Nom_Deduc_Var_Emp_Det.Campo_Cantidad].ToString()))
                        {
                            Cantidad = Convert.ToDouble(Deduccion_Variable[Ope_Nom_Deduc_Var_Emp_Det.Campo_Cantidad].ToString());
                        }
                    }//Fin foreach 
                }//Fin validación DataTable 
            }//Fin del try
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
            return Cantidad;
        }
        #endregion

        #region (Finiquitos Vacaciones)

        #region (Vacaciones Tomadas Mas)
        /// *************************************************************************************************************************
        /// Nombre: Vacaciones_Tomadas_Mas
        /// 
        /// Descripción: Método que realiza el cálculo de las vacaciones que a tomado el empelado por adelantado.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado para control interno del sistema.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 22/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public Double Vacaciones_Tomadas_Mas(String Empleado_ID)
        {
            Int32 Dias_Vacaciones = 0;
            Int32 Dias_Tomados_Mas = 0;
            Double Cantidad_Descontar_Vacaciones = 0.0;
            Double Salario_Diario = 0.0;
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

                if (Dias_Vacaciones < 0)
                {
                    Dias_Tomados_Mas = Math.Abs(Dias_Vacaciones);
                    Cantidad_Descontar_Vacaciones = (Dias_Tomados_Mas * Salario_Diario);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad que se ha excedido de vacaciones tomadas. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Descontar_Vacaciones;
        }
        #endregion

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
        public Double Vacaciones_Pendientes_Pagar(String Empleado_ID)
        {
            Int32 Dias_Vacaciones = 0;
            Double Cantidad_Descontar_Vacaciones = 0.0;
            Double Salario_Diario = 0.0;
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.

                Dias_Vacaciones = Dias_Vacaciones_Base_Formula(Empleado_ID);

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

                if (Dias_Vacaciones > 0)
                {
                    Cantidad_Descontar_Vacaciones = (Dias_Vacaciones * Salario_Diario);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la cantidad que se le debe al empleado por dias de vacaciones " +
                    "que aun tiene pendiente por tomar. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Descontar_Vacaciones;
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
        ///***********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Anio_Calendario_Nomina
        ///
        ///DESCRIPCIÓN: Obtiene el año del calendario de nomina que se encuentra vigente actualmente. 
        ///
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 9/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***********************************************************************************************************************************
        private Int32 Obtener_Anio_Calendario_Nomina()
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
        protected Cls_Cat_Empleados_Negocios Consultar_Inf_Empleado(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion a la capa de negocios.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();//Variable que almacenara la información del empleado.
            DataTable Dt_Empleado = null;//Variable que almacenara la información del empleado.

            try
            {
                Obj_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();

                if (Dt_Empleado is DataTable)
                {
                    if (Dt_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                        {
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
        #endregion

        #endregion

        #region (Finiquitos ISR)
        public DataTable Calcular_ISR_Prima_Vacaciones_Finiquito(String Empleado_ID)
        {
            ///Variables para mantener en memoria los registros consultados.
            DataTable Dt_Resultado = new DataTable();   //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;           //Almacena el registro que contiene el resultado de los cálculos de ISR.
            DataTable Dt_Tabulador_ISR;                 //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR
            Double DIAS_PERIODO_NOMINAL = 0;             //Variable que almacena la cantidad de dias del periodo nomina.
            Double Base_Gravable = 0.0;
            Double Limite_Inferior = 0.0;
            Double Exedente = 0.0;
            Double Porcentaje_Exedente = 0;
            Double IMPUESTO_MARGINAL = 0.0;
            Double Cuota_Fija = 0.0;
            Double Impuesto = 0.0;
            Double Impuesto_Retener = 0.0;
            Double ISR_Retener_Empleado = 0;            //ISR a retener al Empleado 
            Double Cas_Empleado = 0;                    //Subsidio para el Empleado. La deduccion pasa a ser percepcion para el empleado.

            try
            {
                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                //Obtenemos los dias que tiene el periodo nominal.
                DIAS_PERIODO_NOMINAL = Cls_Utlidades_Nomina.Dias_Mes_Fijo;

                Base_Gravable = Gravable_Prima_Vacacional;

                if (Base_Gravable > 0) {

                    //Paso II.- Se consulta la tabla de ISR [TAB_NOM_ISR]  para obtener el límite inferior (LI),
                    //          Procentaje (%) y Cuota Fija (CF). En base al Ingreso Gravable de la PV.
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Base_Gravable, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR is DataTable)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //Obtenemos los valores de Limite Inferior, Couta Fija y Porcentaje de Impuesto Gravable
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                Limite_Inferior = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                            {
                                Cuota_Fija = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim());
                            }
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim()))
                                Porcentaje_Exedente = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim());
                        }
                    }

                    Exedente = (Base_Gravable - Limite_Inferior);
                    IMPUESTO_MARGINAL = (Exedente * Porcentaje_Exedente);
                    Impuesto = IMPUESTO_MARGINAL + Cuota_Fija;
                    Impuesto_Retener = Impuesto;

                    //Paso V.- Si Impuesto_Retener es mayor a cero entonces es impuesto a retener por parte del trabajador
                    if (Impuesto_Retener > 0)
                    {
                        //se convierete en una deducción para el empleado.
                        ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", Impuesto_Retener));
                        Cas_Empleado = 0;

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = false;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                    else
                    {
                        //Se convierte en una percepción para el empleado.
                        ISR_Retener_Empleado = 0;
                        Cas_Empleado = Impuesto_Retener * (-1);

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = true;
                        Renglon_Resultado["Cantidad"] = Cas_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el ISR de vacaciones. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }

        public DataTable Calcular_ISR_Sueldo_Finiquito(String Empleado_ID)
        {
            ///Variables para mantener en memoria los registros consultados.
            DataTable Dt_Resultado = new DataTable();   //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;           //Almacena el registro que contiene el resultado de los cálculos de ISR.
            DataTable Dt_Tabulador_ISR;                 //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR
            Int32 DIAS_PERIODO_NOMINAL = 0;             //Variable que almacena la cantidad de dias del periodo nomina.
            Double Base_Gravable = 0.0;
            Double Limite_Inferior = 0.0;
            Double Exedente = 0.0;
            Double Porcentaje_Exedente = 0;
            Double IMPUESTO_MARGINAL = 0.0;
            Double Cuota_Fija = 0.0;
            Double Impuesto = 0.0;
            Double Subsidio_Entregado_Efectivo = 0.0;
            Double Impuesto_Retener = 0.0;
            Double Salario_Diario = 0.0;
            Double Salario_Periodo = 0.0;
            Double ISR_Retener_Empleado = 0;            //ISR a retener al Empleado 
            Double Cas_Empleado = 0;                    //Subsidio para el Empleado. La deduccion pasa a ser percepcion para el empleado.
            Double Dias_Laborados = 0.0;

            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

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

                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                Dias_Laborados = Obtener_Dias_Laborados_Empleado(Empleado_ID);

                //Obtenemos los dias que tiene el periodo nominal.
                DIAS_PERIODO_NOMINAL = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                Salario_Periodo = (Salario_Diario * DIAS_PERIODO_NOMINAL);

                Double Total_Sin_Gravables_Finiquito = Ingresos_Gravables_Empleado - (Gravable_Indemnizacion + Gravable_Prima_Antiguedad + Gravable_Dias_Festivos + Gravable_Aguinaldo + Gravable_Prima_Vacacional + Gravable_Tiempo_Extra);
                Base_Gravable = Total_Sin_Gravables_Finiquito;

                if (Base_Gravable > 0)
                {

                    //Paso II.- Se consulta la tabla de ISR [TAB_NOM_ISR]  para obtener el límite inferior (LI),
                    //          Procentaje (%) y Cuota Fija (CF). En base al Ingreso Gravable de la PV.
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Base_Gravable, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR is DataTable)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //Obtenemos los valores de Limite Inferior, Couta Fija y Porcentaje de Impuesto Gravable
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                Limite_Inferior = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                            {
                                Cuota_Fija = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim());
                            }
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString()))
                                Porcentaje_Exedente = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                        }
                    }

                    Exedente = (Base_Gravable - Limite_Inferior);
                    IMPUESTO_MARGINAL = (Exedente * Porcentaje_Exedente);
                    Impuesto = IMPUESTO_MARGINAL + Cuota_Fija;
                    Impuesto_Retener = Impuesto;

                    if (Impuesto_Retener > 0)
                        Impuesto_Retener = (Impuesto_Retener / DIAS_PERIODO_NOMINAL) * Dias_Laborados;

                    //Paso V.- Si Impuesto_Retener es mayor a cero entonces es impuesto a retener por parte del trabajador
                    if (Impuesto_Retener > 0)
                    {
                        //se convierete en una deducción para el empleado.
                        ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", Impuesto_Retener));
                        Cas_Empleado = 0;

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = false;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                    else
                    {
                        //Se convierte en una percepción para el empleado.
                        ISR_Retener_Empleado = 0;
                        Cas_Empleado = Impuesto_Retener * (-1);

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = true;
                        Renglon_Resultado["Cantidad"] = Cas_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el ISR de vacaciones. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }

        public DataTable Calcular_ISR_Sueldo_Pagado_Mas_Finiquito(String Empleado_ID)
        {
            ///Variables para mantener en memoria los registros consultados.
            DataTable Dt_Resultado = new DataTable();   //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;           //Almacena el registro que contiene el resultado de los cálculos de ISR.
            DataTable Dt_Tabulador_ISR;                 //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR
            DataTable Dt_Tabulador_Subsidio;            //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_SUBSIDIO         
            Int32 DIAS_PERIODO_NOMINAL = 0;             //Variable que almacena la cantidad de dias del periodo nomina.
            Double Base_Gravable = 0.0;
            Double Limite_Inferior = 0.0;
            Double Exedente = 0.0;
            Double Porcentaje_Exedente = 0;
            Double IMPUESTO_MARGINAL = 0.0;
            Double Cuota_Fija = 0.0;
            Double Impuesto = 0.0;
            Double Subsidio_Entregado_Efectivo = 0.0;
            Double Impuesto_Retener = 0.0;
            Double Salario_Diario = 0.0;
            Double Salario_Periodo = 0.0;
            Double ISR_Retener_Empleado = 0;            //ISR a retener al Empleado 
            Double Cas_Empleado = 0;                    //Subsidio para el Empleado. La deduccion pasa a ser percepcion para el empleado.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);//CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.

                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

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

                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                //Obtenemos los dias que tiene el periodo nominal.
                DIAS_PERIODO_NOMINAL = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                Salario_Periodo = (Salario_Periodo * DIAS_PERIODO_NOMINAL);

                Base_Gravable = Gravable_Sueldo;

                if (Base_Gravable > 0)
                {

                    //Paso II.- Se consulta la tabla de ISR [TAB_NOM_ISR]  para obtener el límite inferior (LI),
                    //          Procentaje (%) y Cuota Fija (CF). En base al Ingreso Gravable de la PV.
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Base_Gravable, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR != null)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //Obtenemos los valores de Limite Inferior, Couta Fija y Porcentaje de Impuesto Gravable
                            if (!string.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString()))
                                Limite_Inferior = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString());
                            if (!string.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString()))
                            {
                                Cuota_Fija = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString());
                            }
                            if (!string.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString()))
                                Porcentaje_Exedente = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                        }
                    }

                    //Paso II.- Se consulta la tabla de subsidio [TAB_NOM_SUBSIDIO] para obtener el subsidio causado
                    //          en base al Total de Ingreso Gravable del empleado.
                    Dt_Tabulador_Subsidio = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_Subsidio_Empleado(Base_Gravable, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_Subsidio != null)
                    {
                        if (Dt_Tabulador_Subsidio.Rows.Count > 0)
                        {
                            //Obtenemos la cantidad de subsidio para el empleado.
                            if (!string.IsNullOrEmpty(Dt_Tabulador_Subsidio.Rows[0][2].ToString()))
                            {
                                Subsidio_Entregado_Efectivo = Convert.ToDouble(Dt_Tabulador_Subsidio.Rows[0][2].ToString());
                            }
                        }
                    }

                    Exedente = (Base_Gravable - Limite_Inferior);
                    IMPUESTO_MARGINAL = (Exedente * Porcentaje_Exedente);
                    Impuesto = IMPUESTO_MARGINAL + Cuota_Fija;
                    Impuesto_Retener = Impuesto - Subsidio_Entregado_Efectivo;

                    //Paso V.- Si Impuesto_Retener es mayor a cero entonces es impuesto a retener por parte del trabajador
                    if (Impuesto_Retener > 0)
                    {
                        //se convierete en una deducción para el empleado.
                        ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", Impuesto_Retener));
                        Cas_Empleado = 0;

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = false;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                    else
                    {
                        //Se convierte en una percepción para el empleado.
                        ISR_Retener_Empleado = 0;
                        Cas_Empleado = Impuesto_Retener * (-1);

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = true;
                        Renglon_Resultado["Cantidad"] = Cas_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el ISR de vacaciones. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }

        public DataTable Calcular_ISPT_Percepciones_Por_Retiro(String Empleado_ID)
        {
            Double Prima_Antiguedad = 0.0;
            Double Indemnizacion = 0.0;
            Double Base_Gravable = 0.0;
            Double Impuesto_SMO = 0.0;
            Double SMO = 0.0;
            Double Tasa = 0.0;
            Double Impuesto_Retener_1 = 0.0;
            Double Impuesto_Retener_2 = 0.0;
            Double Limite_Inferior = 0.0;
            Double Exedente = 0.0;
            Double Porcentaje = 0.0;
            Double IMPUESTO_MARGINAL = 0.0;
            Double Cuota_Fija = 0.0;
            Double Impuesto = 0.0;
            Double Subsidio = 0.0;
            Double Salario_Diario = 0.0;
            Double Salario_Mensual = 0.0;
            Double ISPT = 0.0;
            DataTable Dt_Tabulador_ISR;                 //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR
            DataTable Dt_Tabulador_Subsidio;            //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_SUBSIDIO
            DataTable Dt_Resultado = new DataTable();   //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;           //Almacena el registro que contiene el resultado de los cálculos de ISR.
            Double DIAS_PERIODO_NOMINAL = 0;             //Variable que almacena la cantidad de dias del periodo nomina.
            Double ISR_Retener_Empleado = 0;            //ISR a retener al Empleado 
            Double Cas_Empleado = 0;                    //Subsidio para el Empleado. La deduccion pasa a ser percepcion para el empleado.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

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

                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                //Obtenemos los dias que tiene el periodo nominal.
                DIAS_PERIODO_NOMINAL = Cls_Utlidades_Nomina.Dias_Mes_Fijo;

                //Salario_Diario = Obtener_Salario_Diario_Empleado(Empleado_ID);
                Salario_Mensual = (Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo);

                //Obtenemos la cantidad que grava tanto la prima de antiguedad y la indemnización.
                Prima_Antiguedad = Gravable_Prima_Antiguedad;
                Indemnizacion = Gravable_Indemnizacion;

                //Obtenemos la base gravable que es la suma de la cantidad gravable del prima de antiguedad y de la indemnización.
                Base_Gravable = (Prima_Antiguedad + Indemnizacion);

                if (Base_Gravable > 0) {
                    Impuesto_SMO = Calcular_Impuesto_SMO(Empleado_ID);
                    SMO = Salario_Mensual;

                    Tasa = ((Impuesto_SMO / SMO) * 100);

                    //////////////////  [ Calculo I ]  ///////////////////////////
                    Impuesto_Retener_1 = (Base_Gravable * (Tasa / 100));


                    //////////////////  [ Calculo II ]  ///////////////////////////
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Base_Gravable, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR is DataTable)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //Obtenemos los valores de Limite Inferior, Couta Fija y Porcentaje de Impuesto Gravable
                            if (!string.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                Limite_Inferior = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim());
                            if (!string.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                            {
                                Cuota_Fija = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim());
                            }
                            if (!string.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim()))
                                Porcentaje = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString());
                        }
                    }

                    Dt_Tabulador_Subsidio = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_Subsidio_Empleado(Base_Gravable, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_Subsidio is DataTable)
                    {
                        if (Dt_Tabulador_Subsidio.Rows.Count > 0)
                        {
                            //Obtenemos la cantidad de subsidio para el empleado.
                            if (!string.IsNullOrEmpty(Dt_Tabulador_Subsidio.Rows[0][2].ToString()))
                            {
                                Subsidio = Convert.ToDouble(Dt_Tabulador_Subsidio.Rows[0][2].ToString());
                            }
                        }
                    }

                    Exedente = (Base_Gravable - Limite_Inferior);
                    IMPUESTO_MARGINAL = (Exedente * Porcentaje);
                    Impuesto = (IMPUESTO_MARGINAL + Cuota_Fija);
                    Impuesto_Retener_2 = (Impuesto - Subsidio);

                    if (Base_Gravable < Salario_Mensual)
                        ISPT = Impuesto_Retener_1;
                    else
                        ISPT = Impuesto_Retener_2;

                    //Paso V.- Si Impuesto_Retener es mayor a cero entonces es impuesto a retener por parte del trabajador
                    if (ISPT > 0)
                    {
                        //se convierete en una deducción para el empleado.
                        ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", ISPT));
                        Cas_Empleado = 0;

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = false;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                    else
                    {
                        //Se convierte en una percepción para el empleado.
                        ISR_Retener_Empleado = 0;
                        Cas_Empleado = ISPT * (-1);

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = true;
                        Renglon_Resultado["Cantidad"] = Cas_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el ISR de percepciones por pagos de separación. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }

        public Double Calcular_Impuesto_SMO(String Empleado_ID)
        {
            ///Variables para mantener en memoria los registros consultados.
            DataTable Dt_Tabulador_ISR;                 //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR
            Double DIAS_PERIODO_NOMINAL = 0;             //Variable que almacena la cantidad de dias del periodo nomina.
            Double Base_Gravable = 0.0;
            Double Limite_Inferior = 0.0;
            Double Exedente = 0.0;
            Double Porcentaje_Exedente = 0;
            Double IMPUESTO_MARGINAL = 0.0;
            Double Cuota_Fija = 0.0;
            Double Impuesto = 0.0;
            Double Subsidio_Entregado_Efectivo = 0.0;
            Double Impuesto_Retener = 0.0;
            Double Salario_Diario = 0.0;
            Double Salario_Mensual = 0.0;
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.

            try
            {
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

                //CONSULTAMOS LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = _Informacion_Empleado(Empleado_ID);

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

                //Obtenemos los dias que tiene el periodo nominal.
                DIAS_PERIODO_NOMINAL = Cls_Utlidades_Nomina.Dias_Mes_Fijo;

                //Salario_Diario = Obtener_Salario_Diario_Empleado(Empleado_ID);
                Salario_Mensual = (Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo);

                if (Salario_Mensual > 0)
                {

                    //Paso II.- Se consulta la tabla de ISR [TAB_NOM_ISR]  para obtener el límite inferior (LI),
                    //          Procentaje (%) y Cuota Fija (CF). En base al Ingreso Gravable de la PV.
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Salario_Mensual, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR is DataTable)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //Obtenemos los valores de Limite Inferior, Couta Fija y Porcentaje de Impuesto Gravable
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                Limite_Inferior = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim());
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                            {
                                Cuota_Fija = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim());
                            }
                            if (!String.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim()))
                                Porcentaje_Exedente = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim());
                        }
                    }

                    Exedente = (Salario_Mensual - Limite_Inferior);
                    IMPUESTO_MARGINAL = (Exedente * Porcentaje_Exedente);
                    Impuesto = IMPUESTO_MARGINAL + Cuota_Fija;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el ISR de vacaciones. Error: [" + Ex.Message + "]");
            }
            return Impuesto;
        }

        public DataTable Calcular_ISPT_Tiempo_Extra_Dias_No_Laborados(String Empleado_ID)
        {
            Double E_Horas_Extra = Exenta_Tiempo_Extra;
            Double E_Dias_Festivos = Exenta_Dias_Festivos;
            Double Grava_Horas_Extra = Gravable_Tiempo_Extra;
            Double Grava_Dias_Festivos = Gravable_Dias_Festivos;
            Double Total_Grava_Horas_Extra_Dias_Festivos = 0.0;
            Double Total_Exenta_Horas_Extra_Dias_Festivos = 0.0;
            Double Tope = 0.0;//Salario Diario de la zona  por cinco.
            Double Horas_Gravadas = 0.0;
            Double Base_Gravable = 0.0;
            Double Limite_Inferior = 0.0;
            Double Exedente = 0.0;
            Double Porcentaje_Exedente = 0.0;
            Double IMPUESTO_MARGINAL = 0.0;
            Double Cuota_Fija = 0.0;
            Double Impuesto = 0.0;
            DataTable Dt_Tabulador_ISR;                 //Estructura que almacenara la informacion del registro obtenido de la tabla de TAB_NOM_ISR
            Double ISR_Retener_Empleado = 0;            //ISR a retener al Empleado 
            Double Cas_Empleado = 0;                    //Subsidio para el Empleado. La deduccion pasa a ser percepcion para el empleado.
            DataTable Dt_Resultado = new DataTable();   //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;           //Almacena el registro que contiene el resultado de los cálculos de ISR.
            Int32 DIAS_PERIODO_NOMINAL = 0;             //Variable que almacena la cantidad de dias del periodo nomina.

            try
            {
                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                //Obtenemos los dias que tiene el periodo nominal.
                DIAS_PERIODO_NOMINAL = Cls_Ope_Nom_Percepciones_Datos.Consultar_Dias_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                Total_Grava_Horas_Extra_Dias_Festivos = (Grava_Horas_Extra + Grava_Dias_Festivos);
                Total_Exenta_Horas_Extra_Dias_Festivos = (E_Horas_Extra + E_Dias_Festivos);
                Tope = 259.75;//Queda por definir.

                if (Total_Exenta_Horas_Extra_Dias_Festivos < Tope)
                {
                    Horas_Gravadas = Total_Grava_Horas_Extra_Dias_Festivos;
                }
                else
                {
                    Horas_Gravadas = (Total_Exenta_Horas_Extra_Dias_Festivos - Tope) + Total_Grava_Horas_Extra_Dias_Festivos;
                }

                Base_Gravable = Horas_Gravadas;

                if (Base_Gravable > 0) {
                    Dt_Tabulador_ISR = Cls_Ope_Nom_Deducciones_Datos.Consultar_Tabulador_ISR(Base_Gravable, DIAS_PERIODO_NOMINAL);

                    if (Dt_Tabulador_ISR != null)
                    {
                        if (Dt_Tabulador_ISR.Rows.Count > 0)
                        {
                            //Obtenemos los valores de Limite Inferior, Couta Fija y Porcentaje de Impuesto Gravable
                            if (!string.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim()))
                                Limite_Inferior = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][1].ToString().Trim());
                            if (!string.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim()))
                            {
                                Cuota_Fija = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][2].ToString().Trim());
                            }
                            if (!string.IsNullOrEmpty(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim()))
                                Porcentaje_Exedente = Convert.ToDouble(Dt_Tabulador_ISR.Rows[0][3].ToString().Trim());
                        }
                    }

                    Exedente = (Base_Gravable - Limite_Inferior);
                    IMPUESTO_MARGINAL = (Exedente * Porcentaje_Exedente);
                    Impuesto = (IMPUESTO_MARGINAL + Cuota_Fija);

                    if (Impuesto > 0)
                    {
                        //se convierete en una deducción para el empleado.
                        ISR_Retener_Empleado = Convert.ToDouble(String.Format("{0:0.00}", Impuesto));
                        Cas_Empleado = 0;

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = false;
                        Renglon_Resultado["Cantidad"] = ISR_Retener_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                    else
                    {
                        //Se convierte en una percepción para el empleado.
                        ISR_Retener_Empleado = 0;
                        Cas_Empleado = Impuesto * (-1);

                        Renglon_Resultado = Dt_Resultado.NewRow();
                        Renglon_Resultado["Percepcion_Deduccion"] = true;
                        Renglon_Resultado["Cantidad"] = Cas_Empleado;
                        Dt_Resultado.Rows.Add(Renglon_Resultado);
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el ISPT Tiempo Extra y Dias No Laborados. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }

        public DataTable Calcular_ISR_Total(String Empleado_ID)
        {
            DataTable Dt_Resultado = new DataTable();   //Variable que almacenara el resultado.
            DataRow Renglon_Resultado = null;           //Almacena el registro que contiene el resultado de los cálculos de ISR.
            Double ISR_Aguinaldo_Prima_Vacacional = 0.0;
            Double ISR_Vacaciones = 0.0;
            Double ISR_Sueldo = 0.0;
            Double ISR_Horas_Extra_Dias_Festivos = 0.0;
            Double ISR_Percepciones_Retiro = 0.0;
            Double ISR_Total = 0.0;

            try
            {
                //Creamos la estructura de la tabla de resultados.
                Dt_Resultado.Columns.Add("Percepcion_Deduccion", typeof(Boolean));//Columna que almacenará el si se aplicará como una retención o un subsidio para el empleado.
                Dt_Resultado.Columns.Add("Cantidad", typeof(Double));//Monto a retener o otorgar al empleado.

                ISR_Aguinaldo_Prima_Vacacional = Obtener_Cantidad_Tabla_Resultado(Calcular_ISPT_Prima_Vacacional_Aguinaldo(Empleado_ID));
                ISR_Vacaciones = Obtener_Cantidad_Tabla_Resultado(Calcular_ISR_Prima_Vacaciones_Finiquito(Empleado_ID));
                ISR_Sueldo = Obtener_Cantidad_Tabla_Resultado(Calcular_ISR_Sueldo_Finiquito(Empleado_ID));
                ISR_Horas_Extra_Dias_Festivos = Obtener_Cantidad_Tabla_Resultado(Calcular_ISPT_Tiempo_Extra_Dias_No_Laborados(Empleado_ID));
                ISR_Percepciones_Retiro = Obtener_Cantidad_Tabla_Resultado(Calcular_ISPT_Percepciones_Por_Retiro(Empleado_ID));

                ISR_Total = (ISR_Aguinaldo_Prima_Vacacional + ISR_Vacaciones + ISR_Sueldo + ISR_Horas_Extra_Dias_Festivos +
                    ISR_Percepciones_Retiro);

                Renglon_Resultado = Dt_Resultado.NewRow();
                Renglon_Resultado["Percepcion_Deduccion"] = false;
                Renglon_Resultado["Cantidad"] = ISR_Total;
                Dt_Resultado.Rows.Add(Renglon_Resultado);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al calcular el ISR total");
            }
            return Dt_Resultado;
        }

        public Double Obtener_Cantidad_Tabla_Resultado(DataTable Dt_Resultado)
        {
            Double Cantidad = 0.0;
            
            try
            {
                if (Dt_Resultado is DataTable) {
                    if (Dt_Resultado.Rows.Count > 0) {
                        foreach (DataRow FILA in Dt_Resultado.Rows) {
                            if (FILA is DataRow) {
                                if (!String.IsNullOrEmpty(FILA["Cantidad"].ToString().Trim()))
                                    Cantidad = Convert.ToDouble(FILA["Cantidad"].ToString().Trim());

                                if (!String.IsNullOrEmpty(FILA["Percepcion_Deduccion"].ToString().Trim()))
                                    if (FILA["Percepcion_Deduccion"].ToString().Trim().Contains("true"))
                                        Cantidad = Cantidad * (-1);
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

        #region (Conceptos Pagados de Mas)
        public Double Aguinaldo_Pagado_Mas(String Empleado_ID)
        {
            Cls_Ope_Nom_Percepciones_Negocio Obj_Calculo_Percepciones = new Cls_Ope_Nom_Percepciones_Negocio();
            String Mi_SQL = String.Empty;
            DataTable Dt_Parametros = null;
            DataTable Dt_Recibos_Empleados = null;
            String PERCEPCION_AGUINALDO = String.Empty;
            String NO_RECIBO = String.Empty;
            Double Cantidad_Aguinaldo = 0.0;
            DataTable Dt_Aguinaldo_Cobrado = null;
            DataTable Dt_Aguinaldo_Real = null;
            Double Cantidad_Aguinaldo_Cobrado = 0.0;
            Double Cantidad_Aguinaldo_Real = 0.0;
            Double Cantidad_Aguinaldo_Cobrado_Mas = 0.0;

            try
            {
                Mi_SQL = "SELECT " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ".*";
                Mi_SQL += " FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;

                Dt_Parametros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Parametros is DataTable)
                {
                    if (Dt_Parametros.Rows.Count > 0)
                    {
                        foreach (DataRow RENGLON in Dt_Parametros.Rows)
                        {
                            if (RENGLON is DataRow)
                            {
                                if (!String.IsNullOrEmpty(RENGLON[Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString().Trim()))
                                {
                                    PERCEPCION_AGUINALDO = RENGLON[Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString().Trim();

                                    Mi_SQL = "SELECT " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ".*";
                                    Mi_SQL += " FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados;
                                    Mi_SQL += " WHERE " + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "'";
                                    Mi_SQL += " AND " + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'";
                                    Mi_SQL += " AND " + Ope_Nom_Recibos_Empleados.Campo_Nomina_Generada + "='PVII_AGUINALDO'";
                                    Mi_SQL += " AND " + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + " IN ";
                                    Mi_SQL += "(SELECT " + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo;
                                    Mi_SQL += " FROM " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det;
                                    Mi_SQL += " WHERE " + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" + PERCEPCION_AGUINALDO + "')";

                                    Dt_Recibos_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                                    if (Dt_Recibos_Empleados is DataTable)
                                    {
                                        if (Dt_Recibos_Empleados.Rows.Count > 0)
                                        {
                                            Obj_Calculo_Percepciones.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                                            Obj_Calculo_Percepciones.P_Fecha_Final_Periodo_Nominal = Fecha_Final_Periodo_Nominal;
                                            Obj_Calculo_Percepciones.P_Fecha_Generar_Nomina = Fecha_Generar_Nomina;
                                            Obj_Calculo_Percepciones.P_Fecha_Inicia_Periodo_Nominal = Fecha_Inicia_Periodo_Nominal;
                                            Obj_Calculo_Percepciones.P_No_Nomina = No_Nomina;
                                            Obj_Calculo_Percepciones.P_Nomina_ID = Nomina_ID;
                                            Obj_Calculo_Percepciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;

                                            Dt_Aguinaldo_Cobrado = Obj_Calculo_Percepciones.Calcular_Aguinaldo(Empleado_ID);
                                            Dt_Aguinaldo_Real = Obj_Calculo_Percepciones.Calcular_Aguinaldo_Finiquito(Empleado_ID);

                                            Cantidad_Aguinaldo_Cobrado = Obtener_Cantidad(Dt_Aguinaldo_Cobrado);
                                            Cantidad_Aguinaldo_Real = Obtener_Cantidad(Dt_Aguinaldo_Real);
                                            Cantidad_Aguinaldo_Cobrado_Mas = (Cantidad_Aguinaldo_Cobrado - Cantidad_Aguinaldo_Real);
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
                throw new Exception("Error al obtener el aguinaldo cobrado de mas. Error: [ " + Ex.Message + "]");
            }
            return Cantidad_Aguinaldo_Cobrado_Mas;
        }

        public Double Prima_Vacacional_Pagada_Mas(String Empleado_ID)
        {
            Cls_Ope_Nom_Percepciones_Negocio Obj_Calculo_Percepciones = new Cls_Ope_Nom_Percepciones_Negocio();
            String Mi_SQL = String.Empty;
            DataTable Dt_Parametros = null;
            DataTable Dt_Recibos_Empleados = null;
            String PERCEPCION_PRIMA_VACACIONAL = String.Empty;
            String NO_RECIBO = String.Empty;
            Double Cantidad_PV = 0.0;
            DataTable Dt_PV_Cobrada = null;
            DataTable Dt_PV_Real = null;
            Double Cantidad_PV_Cobrada = 0.0;
            Double Cantidad_PV_Real = 0.0;
            Double Cantidad_PV_Cobrado_Mas = 0.0;
            Int32 Periodo_Vacacional_Actual = 0;

            try
            {
                Periodo_Vacacional_Actual = Obtener_Periodo_Vacacional();

                Mi_SQL = "SELECT " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ".*";
                Mi_SQL += " FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;

                Dt_Parametros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Parametros is DataTable)
                {
                    if (Dt_Parametros.Rows.Count > 0)
                    {
                        foreach (DataRow RENGLON in Dt_Parametros.Rows)
                        {
                            if (RENGLON is DataRow)
                            {
                                if (!String.IsNullOrEmpty(RENGLON[Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString().Trim()))
                                {
                                    PERCEPCION_PRIMA_VACACIONAL = RENGLON[Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString().Trim();

                                    Mi_SQL = "SELECT " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ".*";
                                    Mi_SQL += " FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados;
                                    Mi_SQL += " WHERE " + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "'";
                                    Mi_SQL += " AND " + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'";
                                    Mi_SQL += " AND (" + Ope_Nom_Recibos_Empleados.Campo_Nomina_Generada + "='PVII_AGUINALDO'";
                                    Mi_SQL += " OR " + Ope_Nom_Recibos_Empleados.Campo_Nomina_Generada + "='PVI')";
                                    Mi_SQL += " AND " + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + " IN ";
                                    Mi_SQL += "(SELECT " + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo;
                                    Mi_SQL += " FROM " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det;
                                    Mi_SQL += " WHERE " + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" + PERCEPCION_PRIMA_VACACIONAL + "')";

                                    Dt_Recibos_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                                    if (Dt_Recibos_Empleados is DataTable)
                                    {
                                        if (Dt_Recibos_Empleados.Rows.Count > 0)
                                        {
                                            Obj_Calculo_Percepciones.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                                            Obj_Calculo_Percepciones.P_Fecha_Final_Periodo_Nominal = Fecha_Final_Periodo_Nominal;
                                            Obj_Calculo_Percepciones.P_Fecha_Generar_Nomina = Fecha_Generar_Nomina;
                                            Obj_Calculo_Percepciones.P_Fecha_Inicia_Periodo_Nominal = Fecha_Inicia_Periodo_Nominal;
                                            Obj_Calculo_Percepciones.P_No_Nomina = No_Nomina;
                                            Obj_Calculo_Percepciones.P_Nomina_ID = Nomina_ID;
                                            Obj_Calculo_Percepciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;

                                            Dt_PV_Cobrada = Obj_Calculo_Percepciones.Calcular_Prima_Vacacional(Empleado_ID, Periodo_Vacacional_Actual);
                                            Dt_PV_Real = Obj_Calculo_Percepciones.Calcular_Prima_Vacacional_Finiquito(Empleado_ID);

                                            Cantidad_PV_Cobrada = Obtener_Cantidad(Dt_PV_Cobrada);
                                            Cantidad_PV_Real = Obtener_Cantidad(Dt_PV_Real);
                                            Cantidad_PV_Cobrado_Mas = (Cantidad_PV_Cobrada - Cantidad_PV_Real);
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
                throw new Exception("Error al obtener el aguinaldo cobrado de mas. Error: [ " + Ex.Message + "]");
            }
            return Cantidad_PV_Cobrado_Mas;
        }

        protected Double Obtener_Cantidad(DataTable Dt_Concepto)
        {
            Double Cantidad = 0.0;

            try
            {
                if (Dt_Concepto is DataTable) {
                    if (Dt_Concepto.Rows.Count > 0) {
                        foreach (DataRow CONCEPTO in Dt_Concepto.Rows) {
                            if (CONCEPTO is DataRow) {
                                if (CONCEPTO is DataRow) {
                                    if (!String.IsNullOrEmpty(CONCEPTO["Calculo"].ToString().Trim()))
                                    {
                                        Cantidad = Convert.ToDouble(CONCEPTO["Calculo"].ToString().Trim());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al extraer la cantidad de la percepcion. Error: [" + Ex.Message + "]");
            }
            return Cantidad;
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
        private Double Obtener_Salario_Diario_Empleado(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Consulta_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
            DataTable Dt_Empleados = null;//Variable que almacenara los datos del empleado consultado.
            DataTable Dt_Datos_Puesto = null;//Variable que almacenara la informacion del puesto del e            
            String Salario_Mensual_Puesto = "";//Variable que almacenara el salario del puesto del empleado.
            String Puesto_Empleado = "";//Variable que almacenara el Puesto_ID
            Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado que le corresponde segun el puesto.
            Double DIAS_MES = Cls_Utlidades_Nomina.Dias_Mes_Fijo;//Variable que almacenara los dias del mes que se tomaran para obtener el salario diario mensual.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

            try
            {
                //Consultamos el puesto del empleado que tiene actualmente.
                Consulta_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Empleados = Consulta_Empleados.Consulta_Empleados_General();
                //Validamos que exista algun registro que corresponda con el id del empleado buscado.
                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        //Obtenemos el puesto del empleado, que nos servira para obtener el salario mensual que le corresponde al puesto.
                        if (!String.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Puesto_ID].ToString())) Puesto_Empleado = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Puesto_ID].ToString();
                    }
                }

                //Consultar el salario mensual del puesto, para obtener el salario diario del empleado.
                Consulta_Empleados.P_Puesto_ID = Puesto_Empleado;
                Dt_Datos_Puesto = Consulta_Empleados.Consulta_Puestos_Empleados();//Consultamos la informacion del puesto
                //Validamos que exista algun resultado que corresponda con el id del puesto buscado.
                if (Dt_Datos_Puesto != null)
                {
                    if (Dt_Datos_Puesto.Rows.Count > 0)
                    {
                        //Obtenemos el salario diario del empleado, esto de acuerdo al salario mensual que le corresponde al puesto.
                        Salario_Mensual_Puesto = HttpUtility.HtmlDecode(Dt_Datos_Puesto.Rows[0][Cat_Puestos.Campo_Salario_Mensual].ToString());
                        if (!string.IsNullOrEmpty(Salario_Mensual_Puesto))
                        {
                            Salario_Diario = (Convert.ToDouble(Salario_Mensual_Puesto) / DIAS_MES);
                        }
                    }
                    else
                    {
                        Salario_Diario = 0;
                    }
                }
                else
                {
                    Salario_Diario = 0;
                }

                if (Salario_Diario == 0)
                {
                    INF_EMPLEADO = Consultar_Inf_Empleado(Empleado_ID);
                    Salario_Diario = Convert.ToDouble((String.IsNullOrEmpty(INF_EMPLEADO.P_Salario_Diario.ToString()) ? "0" : INF_EMPLEADO.P_Salario_Diario.ToString()));
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
        ///***************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Minutos_Retardo_Empleado
        /// DESCRIPCION : Obtener la cantidad de minutos de retardo que a tenido el empleado
        ///               en la catorcena anterior a la actual.
        ///               
        /// PARAMETROS  : Empleado_ID.- Empleado sobre el cual se hará la búsqueda de retardos.
        ///             
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Diciembre/2010 11:18 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///**************************************************************************************************************************** 
        private Double Obtener_Minutos_Retardo_Empleado(String Empleado_ID)
        {
            ///Variables de tipo tabla para almacenra las consultas.
            DataTable Dt_Faltas_Empleado = null;//Variable que almacenara la lista de faltas del empleado.         
            ///Variables utilizadas para obtener las faltas del empleado.                        
            Double Acumulado_Minutos_Retardo = 0;//Variable que almacenara la cantidad de faltas que a tenido el empleado en la catorcena anterior.
            Double Minutos_Retardo = 0.0;//Almacenar la cantidad de minutos de retardo.
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacena la informacion del parametro de nómina.
            Double PARAMETRO_MINUTOS_RETARDO = 0.0;//Variable que almacena el numero de minutos que se tomaran por considerar los retardos como faltas.

            try
            {
                //CONSULTAR INFORMACIÓN DE LOS PARÁMETROS DE NÓMINA.
                INF_PARAMETRO = _Informacion_Parametros_Nomina();
                PARAMETRO_MINUTOS_RETARDO = Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_Minutos_Retardo)) ? "0" : INF_PARAMETRO.P_Minutos_Retardo);

                //Consulta de las faltas que a tenido el empleado en el periodo a generar la nomina.
                Dt_Faltas_Empleado = Cls_Ope_Nom_Deducciones_Datos.Consultar_Faltas_Empleado_Periodo_Nominal(Empleado_ID, Nomina_ID, No_Nomina);

                //Validamos que la busqueda halla tenido resultados
                if (Dt_Faltas_Empleado is DataTable)
                {
                    foreach (DataRow Retardo in Dt_Faltas_Empleado.Rows)
                    {
                        if (Retardo is DataRow)
                        {
                            //Validamos que el registro de falta sea por inasistencia o justificada.
                            if (Retardo[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString().Equals("RETARDO"))
                            {
                                if (Retardo[Ope_Nom_Faltas_Empleado.Campo_Retardo].ToString().Equals("SI"))
                                {
                                    //Obtenemos el registro de la cantidad de minutos de retardo.
                                    Minutos_Retardo = Convert.ToDouble(Retardo[Ope_Nom_Faltas_Empleado.Campo_Cantidad].ToString());
                                    //Validamos que si la cantidad de minutos de retardo no sea mayor o igual a 15 minutos. si es asi cuenta como falta.
                                    if (Minutos_Retardo <= PARAMETRO_MINUTOS_RETARDO)
                                    {
                                        //Vamos obteniendo el acumulado de minutos de  retardo que a tenido el empleado en la catorcena anterior a la actual.
                                        Acumulado_Minutos_Retardo += Minutos_Retardo;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la cantidad de minutos de retardo que a tenido el empleado. Error: [" + Ex.Message + "]");
            }
            return Acumulado_Minutos_Retardo;
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

                if (Dt_Vacaciones is DataTable)
                {
                    foreach (DataRow Vacacion in Dt_Vacaciones.Rows)
                    {
                        if (Vacacion is DataRow)
                        {
                            if (!string.IsNullOrEmpty(Vacacion[OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString()))
                                Dias_Vacaciones += Convert.ToInt32(Vacacion[OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString());
                            //Cambiar el estatus de la tabla de detalles a pagado y por tomar!!
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
                            if (!string.IsNullOrEmpty(Vacacion[OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString()))
                            {
                                Dias_Vacaciones += Convert.ToInt32(Vacacion[OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString());

                                if (Actualizar_Estatus_Dias_Vacaciones)
                                {
                                    if (!string.IsNullOrEmpty(Vacacion[OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion].ToString()))
                                    {
                                        Cls_Ope_Nom_Percepciones_Datos.Cambiar_Estatus_Vacaciones(Vacacion[OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion].ToString());
                                    }
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
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Dias_Laborados_Empleado
        /// 
        /// DESCRIPCION : Consulta los dias que falto el empleado y toma las incidencias por concepto de faltas del empleado
        ///               y las resta los dias de la catorcena. Obteniendo de esta forma los dias laborados por empleado
        ///               durante la catorcena.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificador del empleado.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Enero/2010 10:27 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public Int32 Obtener_Dias_Laborados_Empleado(String Empleado_ID)
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
        private Double Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(String Empleado_ID)
        {
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
                INF_PARAMETRO = _Informacion_Parametros_Nomina();//CONSULTA LA INFORMACION DE LOS PARAMETROS.

                if (INF_PUESTO.P_Salario_Mensual > 0)
                {
                    //PASO I.- OBTENEMOS LA PSM.
                    PREVISION_SOCIAL_MULTIPLE = (INF_PUESTO.P_Salario_Mensual *
                        Convert.ToDouble((String.IsNullOrEmpty(INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple)) ?
                        "0" : INF_PARAMETRO.P_ISSEG_Porcentaje_Prevision_Social_Multiple));
                    //PASO II.- OBTENER EL SUELDO TOTAL [INTEGRANDO SUELDO DEL NIVEL MAS LA PSM]
                    SUELDO_TOTAL = (INF_PUESTO.P_Salario_Mensual + PREVISION_SOCIAL_MULTIPLE);
                }
                else
                {
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
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Dias_IMSS].ToString()))
                                INF_PARAMETROS.P_Dias_IMSS= PARAMETRO[Cat_Nom_Parametros.Campo_Dias_IMSS].ToString();

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Tope_ISSEG].ToString()))
                                INF_PARAMETROS.P_Tope_ISSEG = PARAMETRO[Cat_Nom_Parametros.Campo_Tope_ISSEG].ToString();
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

                if (Dt_IMSS is DataTable) {
                    if (Dt_IMSS.Rows.Count > 0) {
                        foreach (DataRow IMSS in Dt_IMSS.Rows) {
                            if (IMSS is DataRow) {
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
