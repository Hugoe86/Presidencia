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
using Presidencia.IMSS.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Puestos.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Cat_Terceros.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Utilidades_Nomina;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using System.Text;
using Presidencia.Parametros_Contables.Negocio;


namespace Presidencia.Ayudante_Informacion
{
    public class Cls_Ayudante_Nom_Informacion
    {

        #region (Calculo Salarios Empleado)
        /// ***********************************************************************************
        /// Nombre: Obtener_Cantidad_Diaria
        /// 
        /// Descripción: Obtiene la cantidad diaria que el empleado percibe incluyendo
        ///              si el empleado aplica para el calculo de ISSEG.
        ///              
        ///             Sin ISSEG ----> Salario Diario del Empleado
        ///             Con ISSEG ----> Cantidad_Diaria = ([Sueldo + PSM]/30.42)
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 03/Octubre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        public static Double Obtener_Cantidad_Diaria(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
            Double Salario_Diario = 0.0;

            try
            {
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
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Salario_Diario;
        }
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
        public static Double Obtener_Cantidad_Diaria_Sueldo_Puesto_ISSEG(String Empleado_ID)
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
        public static Double Obtener_Cantidad_Diaria_PSM_ISSEG(String Empleado_ID)
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
        public static Double Obtener_Cantidad_Suma_Sueldo_Mas_PSM_Diaria_ISSEG(String Empleado_ID)
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
        public static Double Obtener_Cantidad_Salario_Diario_Empleado_No_Aplica_ISSEG(String Empleado_ID)
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
        public static Cls_Cat_Empleados_Negocios _Informacion_Empleado(String Filtro)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();
            Cls_Cat_Empleados_Negocios Obj_Emplados = new Cls_Cat_Empleados_Negocios();
            DataTable Dt_Empleados = null;

            try
            {
                if (!String.IsNullOrEmpty(Filtro))
                {
                    if (Filtro.Length == 10)
                        Obj_Emplados.P_Empleado_ID = Filtro;
                    if(Filtro.Length ==6)
                        Obj_Emplados.P_No_Empleado = Filtro;

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

                                    if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Alta_Isseg].ToString()))
                                        INF_EMPLEADO.P_Fecha_Alta_ISSEG = EMPLEADO[Cat_Empleados.Campo_Fecha_Alta_Isseg].ToString();
                                }
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
        public static Cls_Cat_Nom_Parametros_Negocio _Informacion_Parametros_Nomina()
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
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Despensa].ToString()))
                                INF_PARAMETROS.P_Percepcion_Despensa = PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Despensa].ToString();

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

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Tope_ISSEG].ToString()))
                                INF_PARAMETROS.P_Tope_ISSEG = PARAMETRO[Cat_Nom_Parametros.Campo_Tope_ISSEG].ToString();

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Proveedor_Fonacot].ToString()))
                                INF_PARAMETROS.P_Proveedor_Fonacot = PARAMETRO[Cat_Nom_Parametros.Campo_Proveedor_Fonacot].ToString();
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
        public static Cls_Cat_Nom_Terceros_Negocio _Informacion_Terceros(String Tercero_ID)
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
        public static Cls_Cat_Nom_Zona_Economica_Negocio _Informacion_Zona_Economica()
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
        public static Cls_Cat_Nom_Zona_Economica_Negocio _Informacion_Zona_Economica(String Zona_ID)
        {
            Cls_Cat_Nom_Zona_Economica_Negocio Obj_Zona_Economica = new Cls_Cat_Nom_Zona_Economica_Negocio();//Variable de conexión con la capa de negocios.
            Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = new Cls_Cat_Nom_Zona_Economica_Negocio();//Variable que almacena la información de la zona económica.
            DataTable Dt_Zona_Economica = null;//Variable que almacena la información del registro búscado.

            try
            {
                //CONSULTAMOS LA INFORMACIÓN DE LA ZONA ECONÓMICA.
                Obj_Zona_Economica.P_Zona_ID = Zona_ID;
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
        public static Cls_Cat_Tipos_Nominas_Negocio _Informacion_Tipo_Nomina(String Tipo_Nomina_ID)
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
        public static Cls_Cat_Puestos_Negocio _Informacion_Puestos(String Puesto_ID)
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
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Aplica_Fondo_Retiro].ToString().Trim()))
                                    INF_PUESTO.P_Aplica_Fondo_Retiro = PUESTO[Cat_Puestos.Campo_Aplica_Fondo_Retiro].ToString().Trim();
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim()))
                                    INF_PUESTO.P_Puesto_ID = PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim();
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Nombre].ToString().Trim()))
                                    INF_PUESTO.P_Nombre = PUESTO[Cat_Puestos.Campo_Nombre].ToString().Trim();
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Salario_Mensual].ToString().Trim()))
                                    INF_PUESTO.P_Salario_Mensual = Convert.ToDouble(PUESTO[Cat_Puestos.Campo_Salario_Mensual].ToString().Trim());
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Aplica_PSM].ToString()))
                                    INF_PUESTO.P_Aplica_Psm = PUESTO[Cat_Puestos.Campo_Aplica_PSM].ToString();
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
        public static Cls_Cat_Nom_Sindicatos_Negocio _Informacion_Sindicato(String Sindicato_ID)
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
        public static Cls_Tab_Nom_IMSS_Negocio _Informacion_IMSS()
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
        /// ***********************************************************************************
        /// Nombre: _Informacion_Percepcion_Deduccion
        /// 
        /// Descripción: Consulta la información de la tabla de percepciones y/o deducciones.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 04/Octubre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        public static Cls_Cat_Nom_Percepciones_Deducciones_Business _Informacion_Percepcion_Deduccion(Cls_Cat_Nom_Percepciones_Deducciones_Business Object_Filters)
        {
            Cls_Cat_Nom_Percepciones_Deducciones_Business INF_PERCEPCION_DEDUCCION = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
            DataTable Dt_Percepcion_Deduccion = null;

            try
            {
                Dt_Percepcion_Deduccion = Object_Filters.Consultar_Percepciones_Deducciones_General();

                if (Dt_Percepcion_Deduccion is DataTable) {
                    if (Dt_Percepcion_Deduccion.Rows.Count > 0) {
                        foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepcion_Deduccion.Rows) {
                            if (PERCEPCION_DEDUCCION is DataRow) {
                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_PERCEPCION_DEDUCCION_ID = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_NOMBRE = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_CLAVE = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_TIPO = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_TIPO_ASIGNACION = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_PORCENTAJE_GRAVABLE = Convert.ToDouble(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString());

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_GRAVABLE = Convert.ToDouble(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString());

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Estatus].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_ESTATUS = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Estatus].ToString();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Concepto].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_Concepto = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Concepto].ToString();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Aplica_IMSS].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_APLICA_IMSS = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Aplica_IMSS].ToString();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_APLICAR= PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString();

                                if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Comentarios].ToString()))
                                    INF_PERCEPCION_DEDUCCION.P_COMENTARIOS = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Comentarios].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la información de la percepcion y/o deduccion. Error:[" + Ex.Message + "]");
            }
            return INF_PERCEPCION_DEDUCCION;
        }
        /// ***********************************************************************************
        /// Nombre: _Informacion_Parametro_Contable
        /// 
        /// Descripción: Consulta la información de la tabla de percepciones y/o deducciones.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 04/Octubre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        public static Cls_Cat_Nom_Parametros_Contables_Negocio _Informacion_Parametro_Contable()
        {
            Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETROS_CONTABLES = new Cls_Cat_Nom_Parametros_Contables_Negocio();//Variable de conexión a la capa de negocios. 
            Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = new Cls_Cat_Nom_Parametros_Contables_Negocio();//Variable de conexión a la capa de negocios. 
            DataTable Dt_Parametros_Contables = null;//Variable que almacena un listado de parámetros contables.

            try
            {
                Dt_Parametros_Contables = INF_PARAMETROS_CONTABLES.Consultar_Parametros_Contables();

                if (Dt_Parametros_Contables is DataTable)
                {
                    if (Dt_Parametros_Contables.Rows.Count > 0)
                    {
                        foreach (DataRow PARAMETRO in Dt_Parametros_Contables.Rows)
                        {
                            if (PARAMETRO is DataRow)
                            {

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Aportaciones_IMSS].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Aportaciones_IMSS = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Aportaciones_IMSS].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Aportaciones_ISSEG].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Aportaciones_ISSEG = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Aportaciones_ISSEG].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Cuotas_Fondo_Retiro].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Cuotas_Fondo_Retiro = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Cuotas_Fondo_Retiro].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Dietas].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Dietas = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Dietas].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Estimulos_Productividad_Eficiencia].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Estimulos_Productividad_Eficiencia = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Estimulos_Productividad_Eficiencia].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Gratificaciones_Fin_Anio].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Gratificaciones_Fin_Anio = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Gratificaciones_Fin_Anio].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Honorarios_Asimilados].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Honorarios_Asimilados = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Honorarios_Asimilados].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Horas_Extra].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Horas_Extra = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Horas_Extra].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Impuestos_Sobre_Nominas].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Impuestos_Sobre_Nominas = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Impuestos_Sobre_Nominas].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Parametro_ID].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_PrimaryKey_Parametro_ID = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Parametro_ID].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Participacipaciones_Vigilancia].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Participacipaciones_Vigilancia = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Participacipaciones_Vigilancia].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Pensiones].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Pensiones = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Pensiones].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Prestaciones = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones_Establecidas_Condiciones_Trabajo].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Prestaciones_Establecidas_Condiciones_Trabajo = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones_Establecidas_Condiciones_Trabajo].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prevision_Social_Multiple].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Prevision_Social_Multiple = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prevision_Social_Multiple].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prima_Dominical].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Prima_Dominical = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prima_Dominical].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prima_Vacacional].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Prima_Vacacional = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prima_Vacacional].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Remuneraciones_Eventuales].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Remuneraciones_Eventuales = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Remuneraciones_Eventuales].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Sueldos_Base].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Sueldos_Base = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Sueldos_Base].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Honorarios].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Honorarios =
                                        PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Honorarios].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Seguros].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Seguros =
                                        PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Seguros].ToString();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Liquidacion_Indemnizacion].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Liquidaciones_Indemnizacion =
                                        PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Liquidacion_Indemnizacion].ToString
                                            ();

                                if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones_Retiro].ToString()))
                                    INF_PARAMETRO_CONTABLE.P_Prestaciones_Retiro =
                                        PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones_Retiro].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los parámetros contables. Error: [" + Ex.Message + "]");
            }
            return INF_PARAMETRO_CONTABLE;
        }
        #endregion

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
        public static Cls_Cat_Empleados_Negocios _Informacion_Empleado_Nombre(String Nombre_Empleado)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();
            Cls_Cat_Empleados_Negocios Obj_Emplados = new Cls_Cat_Empleados_Negocios();
            DataTable Dt_Empleados = null;

            try
            {
                if (!String.IsNullOrEmpty(Nombre_Empleado))
                {
                    Obj_Emplados.P_Nombre = Nombre_Empleado;
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
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la información del empleado. Error: [" + Ex.Message + "]");
            }
            return INF_EMPLEADO;
        }
        /// ************************************************************************************************************************
        /// Nombre: Cambiar_Foreign_Key_Por_Nombre
        /// 
        /// Descripción: Cambia la Foreign Key de la tabla por el nombre o descripción a mostrar.
        /// 
        /// Parámetros: Foreign Key.- Campo que se consultara para obtener su valor.
        ///             Cierre_Consulta.- Complemento de la consulta tabla en la ciál se encuentra la foreign key.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 23/Septiembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ************************************************************************************************************************
        public static StringBuilder Cambiar_Foreign_Key_Por_Nombre(String Foreign_Key, String Cierre_Consulta) {

            StringBuilder Select_Interno = new StringBuilder();//Variable que almacenara la subconsulta de la foreign key a consultar.

            try
            {
                switch (Foreign_Key)
                {
                    case "NOMINA_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio);
                        Select_Interno.Append(" FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS CALENDARIO_NOMINA ");
                        break;
                    case "DEPENDENCIA_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
                        Select_Interno.Append(" WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS UNIDAD_RESPONSABLE ");
                        break;
                    case "PUESTO_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Cat_Puestos.Tabla_Cat_Puestos);
                        Select_Interno.Append(" WHERE " + Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS PUESTO ");
                        break;
                    case "EMPLEADO_ID":
                        Select_Interno.Append(" (SELECT (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Select_Interno.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Select_Interno.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                        Select_Interno.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                        Select_Interno.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS EMPLEADO ");
                        break;
                    case "TIPO_NOMINA_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                        Select_Interno.Append(" FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS TIPO_NOMINA ");
                        break;
                    case "PERCEPCION_DEDUCCION_ID":
                        Select_Interno.Append(" (SELECT (" + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' ' || ");
                        Select_Interno.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") ");
                        Select_Interno.Append(" FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS " + Cierre_Consulta.Split(new Char[] { '.' })[1]);
                        break;
                    case "CUENTA_CONTABLE_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta);
                        Select_Interno.Append(" FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                        Select_Interno.Append(" WHERE " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS CUENTA_CONTABLE ");
                        break;
                    case "REQUISITO_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados + "." + Cat_Nom_Requisitos_Empleados.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados + "." + Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS REQUISITO ");
                        break;
                    case "SINDICATO_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Sindicato_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS SINDICATO ");
                        break;
                    case "PLAZA_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas + "." + Cat_Nom_Plazas.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas + "." + Cat_Nom_Plazas.Campo_Plaza_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS PLAZA ");
                        break;
                    case "ESCOLARIDAD_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Escolaridad.Tabla_Cat_Nom_Escolaridad + "." + Cat_Nom_Escolaridad.Campo_Escolaridad);
                        Select_Interno.Append(" FROM " + Cat_Nom_Escolaridad.Tabla_Cat_Nom_Escolaridad);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Escolaridad.Tabla_Cat_Nom_Escolaridad + "." + Cat_Nom_Escolaridad.Campo_Escolaridad_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS ESCOLARIDAD ");
                        break;
                    case "TERCERO_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + "." + Cat_Nom_Terceros.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + "." + Cat_Nom_Terceros.Campo_Tercero_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS TERCEROS ");
                        break;
                    case "TIPO_CONTRATO_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos + "." + Cat_Nom_Tipos_Contratos.Campo_Descripcion);
                        Select_Interno.Append(" FROM " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos + "." + Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS TIPO_CONTRATO ");
                        break;
                    case "ZONA_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + "." + Cat_Nom_Zona_Economica.Campo_Zona_Economica);
                        Select_Interno.Append(" FROM " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + "." + Cat_Nom_Zona_Economica.Campo_Zona_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS ZONA_ECONOMICA ");
                        break;
                    case "BANCO_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS BANCO ");
                        break;
                    case "RELOJ_CHECADOR_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Clave);
                        Select_Interno.Append(" FROM " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS RELOJ_CHECADOR ");
                        break;
                    case "INDEMNIZACION_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + "." + Cat_Nom_Indemnizacion.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + "." + Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS INDEMNIZACION ");
                        break;
                    case "PROVEEDOR_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + "." + Cat_Nom_Proveedores.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + "." + Cat_Nom_Proveedores.Campo_Proveedor_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS PROVEEDOR ");
                        break;
                    case "ANTIGUEDAD_SINDICATO_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Anios);
                        Select_Interno.Append(" FROM " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato);
                        Select_Interno.Append(" WHERE " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS ANTIGUEDAD_SINDICATO ");
                        break;
                    case "TURNO_ID":
                        Select_Interno.Append(" (SELECT " + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Descripcion);
                        Select_Interno.Append(" FROM " + Cat_Turnos.Tabla_Cat_Turnos);
                        Select_Interno.Append(" WHERE " + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Turno_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS TURNO ");
                        break;
                    case "ROL_ID":
                        Select_Interno.Append(" (SELECT " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Apl_Cat_Roles.Campo_Nombre);
                        Select_Interno.Append(" FROM " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles);
                        Select_Interno.Append(" WHERE " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Apl_Cat_Roles.Campo_Rol_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS ROL ");
                        break;
                    case "FUENTE_FINANCIAMIENTO_ID":
                        Select_Interno.Append(" (SELECT (" + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || ");
                        Select_Interno.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ") ");
                        Select_Interno.Append(" FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                        Select_Interno.Append(" WHERE " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS FTE_FINANCIAMIENTO ");
                        break;
                    case "PROYECTO_PROGRAMA_ID":
                        Select_Interno.Append(" (SELECT (" + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Clave + " || ' ' || ");
                        Select_Interno.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Descripcion + ") ");
                        Select_Interno.Append(" FROM " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
                        Select_Interno.Append(" WHERE " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS PROYECTO_PROGRAMA ");
                        break;
                    case "AREA_FUNCIONAL_ID":
                        Select_Interno.Append(" (SELECT (" + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Clave + " || ' ' || ");
                        Select_Interno.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Descripcion + ") ");
                        Select_Interno.Append(" FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                        Select_Interno.Append(" WHERE " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS PROYECTO_PROGRAMA ");
                        break;
                    case "PARTIDA_ID":
                        Select_Interno.Append(" (SELECT (" + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " || ' ' || ");
                        Select_Interno.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + ") ");
                        Select_Interno.Append(" FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                        Select_Interno.Append(" WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = ");
                        Select_Interno.Append(Cierre_Consulta + ") AS PARTIDA ");
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar la cadena para obtener el equivalente a la relacion de");
            }
            return Select_Interno;
        }
    }
}
