using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Datos;
using Presidencia.Catalogo_Salarios_Minimos.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;
using Presidencia.Catalogo_Descuentos_Predial.Negocio;
using Presidencia.Catalogo_Instituciones_Recepcion_Pago_Predial.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using System.IO;
using ExportToExcel;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Movimientos.Negocio;

namespace Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio
{
    public class Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio
    {
        private Decimal Salario_Minimo;
        private Decimal Tope_Salarios_Minimos;  // tope valor fical (pensionado)
        private Decimal C_Minima;
        private int Anio_Calculo;
        private Dictionary<String, String> Dicc_IDs_Conceptos;
        private Dictionary<String, Decimal> Dicc_IDs_Tasas;
        private Dictionary<String, Decimal> _Dicc_Tasas_Anuales;
        private Dictionary<Int32, Decimal> _Dicc_Cuotas_Minimas;

        private String Tipo_Concepto;
        private String _Estatus;
        private String _Estatus_Excluir;             // cuentas a excluir de la generacion
        private String _Tipo_Suspension_Excluir;
        private Int32 Total_Cuentas;
        private String Estatus_Adeudo;      // PROYECCION o POR PAGAR

        private Dictionary<String, String> Dicc_Err_Cuentas;
        private Int32 _Total_Padron;
        private Int32 _Total_Adeudos_Generados;
        private Int32 _Total_Cuotas_Minimas;
        private Int32 _Total_Cuentas_Error;
        private Int32 _Total_Canceladas;
        private Int32 _Total_Bloqueadas;
        private Int32 _Total_Suspendidas;
        private Int32 _Total_Pendientes;
        private Int32 _Total_Exenciones;
        private Int32 _Total_Ordenes_Cuota_Minima = 0;

        private Decimal _Total_Recargos_Generados;
        private Decimal _Total_Recargos_Generados_Febrero;
        private Decimal _Total_Corriente;
        private Decimal _Total_Rezago;
        private String _Periodo_Rezago;
        private String _Periodo_Corriente;

        private Int32 _Cuentas_Archivo_Urbano = 0;
        private Int32 _Cuentas_Archivo_Rural = 0;
        private Int32 _Cuentas_Archivo_Foraneos = 0;
        private Decimal _Total_CM_Urbano = 0;
        private Decimal _Total_CM_Rural = 0;
        private Decimal _Total_CM_Foraneos = 0;
        private Decimal _Total_Adeudo_Archivo = 0;
        private Decimal _Total_Adeudo_Urbano = 0;
        private Decimal _Total_Adeudo_Rural = 0;
        private Decimal _Total_Adeudo_Foraneo = 0;
        private Decimal _Total_Recargos_Urbano = 0;
        private Decimal _Total_Recargos_Rural = 0;
        private Decimal _Total_Recargos_Foraneo = 0;
        private Decimal _Total_Rezago_Urbano = 0;
        private Decimal _Total_Rezago_Rural = 0;
        private Decimal _Total_Rezago_Foraneo = 0;
        private Decimal _Total_Honorarios_Urbano = 0;
        private Decimal _Total_Honorarios_Rural = 0;
        private Decimal _Total_Honorarios_Foraneo = 0;
        private Decimal _Total_Descuento_Enero_Urbano = 0;
        private Decimal _Total_Descuento_Enero_Rural = 0;
        private Decimal _Total_Descuento_Enero_Foraneo = 0;
        private Decimal _Total_Descuento_Enero = 0;
        private Decimal _Total_Descuento_Febrero_Urbano = 0;
        private Decimal _Total_Descuento_Febrero_Rural = 0;
        private Decimal _Total_Descuento_Febrero_Foraneo = 0;
        private Decimal _Total_Descuento_Febrero = 0;
        private Decimal _Total_Rezago_Acumulado = 0;
        private Decimal _Total_Honorarios_Acumulado = 0;
        private Decimal _Total_Recargos_Acumulado = 0;
        private Decimal _Total_Recargos_Febrero_Acumulado = 0;

        private String _Anio_Tabulador_Utilizar;
        private String _Mes_Tabulador_Utilizar;
        private String _Tabulador_Enero_Utilizar;
        private String _Tabulador_Febrero_Utilizar;

        private DataTable _Dt_Tasas;

        private String No_Orden_Variacion;
        private String Año_Orden_Variacion;

        Dictionary<Char, Int32> Gl_Convertir_Valores_Afanumericos;

        private static Dictionary<String, Decimal> Gl_Dicc_Tab_Recargos = new Dictionary<string, decimal>();

        ///********************************************************************************
        ///                                 METODOS DE ACCESO
        #region METODOS_ACCESO

        public Decimal p_Salario_Minimo
        {
            get { return Salario_Minimo; }
            set { Salario_Minimo = value; }
        }
        public Decimal p_Tope_Salarios_Minimos
        {
            get { return Tope_Salarios_Minimos; }
            set { Tope_Salarios_Minimos = value; }
        }
        public Decimal p_Cuota_Minima
        {
            get { return C_Minima; }
            set { C_Minima = value; }
        }
        public int p_Anio
        {
            get { return Anio_Calculo; }
            set { Anio_Calculo = value; }
        }
        public Dictionary<String, Decimal> p_Dicc_IDs_Tasas
        {
            get { return Dicc_IDs_Tasas; }
            set { Dicc_IDs_Tasas = value; }
        }
        public Dictionary<String, String> p_Dicc_IDs_Conceptos
        {
            get { return Dicc_IDs_Conceptos; }
            set { Dicc_IDs_Conceptos = value; }
        }
        public Dictionary<String, Decimal> p_Dicc_Tasas_Anuales
        {
            get { return _Dicc_Tasas_Anuales; }
            set { _Dicc_Tasas_Anuales = value; }
        }


        public String p_Tipo_Concepto
        {
            get { return Tipo_Concepto; }
            set { Tipo_Concepto = value; }
        }
        public String p_Estatus_Excluir
        {
            get { return _Estatus_Excluir; }
            set { _Estatus_Excluir = value; }
        }
        public String p_Tipo_Suspension_Excluir
        {
            get { return _Tipo_Suspension_Excluir; }
            set { _Tipo_Suspension_Excluir = value; }
        }
        public String p_Estatus
        {
            get { return _Estatus; }
            set { _Estatus = value; }
        }
        public Int32 p_Total_Cuentas
        {
            get { return Total_Cuentas; }
            set { Total_Cuentas = value; }
        }
        public String p_Estatus_Adeudo
        {
            get { return Estatus_Adeudo; }
            set { Estatus_Adeudo = value; }
        }

        public Dictionary<String, String> p_Errores_Cuentas
        {
            get { return Dicc_Err_Cuentas; }
            set { Dicc_Err_Cuentas = value; }
        }
        public Int32 p_Total_Padron
        {
            get { return _Total_Padron; }
            set { _Total_Padron = value; }
        }
        public Int32 p_Total_Adeudos_Generados
        {
            get { return _Total_Adeudos_Generados; }
            set { _Total_Adeudos_Generados = value; }
        }
        public Int32 p_Total_Cuotas_Minimas
        {
            get { return _Total_Cuotas_Minimas; }
            set { _Total_Cuotas_Minimas = value; }
        }
        public Int32 p_Total_Cuentas_Error
        {
            get { return _Total_Cuentas_Error; }
            set { _Total_Cuentas_Error = value; }
        }
        public Int32 p_Total_Canceladas
        {
            get { return _Total_Canceladas; }
            set { _Total_Canceladas = value; }
        }
        public Int32 p_Total_Bloqueadas
        {
            get { return _Total_Bloqueadas; }
            set { _Total_Bloqueadas = value; }
        }
        public Int32 p_Total_Suspendidas
        {
            get { return _Total_Suspendidas; }
            set { _Total_Suspendidas = value; }
        }
        public Int32 p_Total_Pendientes
        {
            get { return _Total_Pendientes; }
            set { _Total_Pendientes = value; }
        }
        public Int32 p_Total_Exenciones
        {
            get { return _Total_Exenciones; }
            set { _Total_Exenciones = value; }
        }
        public Int32 p_Total_Ordenes_Cuota_Minima
        {
            get { return _Total_Ordenes_Cuota_Minima; }
            set { _Total_Ordenes_Cuota_Minima = value; }
        }

        public Decimal p_Total_Recargos_Generados
        {
            get { return _Total_Recargos_Generados; }
            set { _Total_Recargos_Generados = value; }
        }
        public Decimal p_Total_Recargos_Generados_Febrero
        {
            get { return _Total_Recargos_Generados_Febrero; }
            set { _Total_Recargos_Generados_Febrero = value; }
        }
        public Decimal p_Total_Corriente
        {
            get { return _Total_Corriente; }
            set { _Total_Corriente = value; }
        }
        public Decimal p_Total_Rezago
        {
            get { return _Total_Rezago; }
            set { _Total_Rezago = value; }
        }
        public String p_Periodo_Rezago
        {
            get { return _Periodo_Rezago; }
            set { _Periodo_Rezago = value; }
        }
        public String p_Periodo_Corriente
        {
            get { return _Periodo_Corriente; }
            set { _Periodo_Corriente = value; }
        }

        public Int32 p_Cuentas_Archivo_Urbano
        {
            get { return _Cuentas_Archivo_Urbano; }
            set { _Cuentas_Archivo_Urbano = value; }
        }
        public Int32 p_Cuentas_Archivo_Rural
        {
            get { return _Cuentas_Archivo_Rural; }
            set { _Cuentas_Archivo_Rural = value; }
        }
        public Int32 p_Cuentas_Archivo_Foraneos
        {
            get { return _Cuentas_Archivo_Foraneos; }
            set { _Cuentas_Archivo_Foraneos = value; }
        }
        public Decimal p_Total_CM_Urbano
        {
            get { return _Total_CM_Urbano; }
            set { _Total_CM_Urbano = value; }
        }
        public Decimal p_Total_CM_Rural
        {
            get { return _Total_CM_Rural; }
            set { _Total_CM_Rural = value; }
        }
        public Decimal p_Total_CM_Foraneos
        {
            get { return _Total_CM_Foraneos; }
            set { _Total_CM_Foraneos = value; }
        }
        public Decimal p_Total_Adeudo_Archivo
        {
            get { return _Total_Adeudo_Archivo; }
            set { _Total_Adeudo_Archivo = value; }
        }
        public Decimal p_Total_Adeudo_Urbano
        {
            get { return _Total_Adeudo_Urbano; }
            set { _Total_Adeudo_Urbano = value; }
        }
        public Decimal p_Total_Adeudo_Rural
        {
            get { return _Total_Adeudo_Rural; }
            set { _Total_Adeudo_Rural = value; }
        }
        public Decimal p_Total_Adeudo_Foraneo
        {
            get { return _Total_Adeudo_Foraneo; }
            set { _Total_Adeudo_Foraneo = value; }
        }
        public Decimal p_Total_Recargos_Urbano
        {
            get { return _Total_Recargos_Urbano; }
            set { _Total_Recargos_Urbano = value; }
        }
        public Decimal p_Total_Recargos_Rural
        {
            get { return _Total_Recargos_Rural; }
            set { _Total_Recargos_Rural = value; }
        }
        public Decimal p_Total_Recargos_Foraneo
        {
            get { return _Total_Recargos_Foraneo; }
            set { _Total_Recargos_Foraneo = value; }
        }
        public Decimal p_Total_Rezago_Foraneo
        {
            get { return _Total_Rezago_Foraneo; }
            set { _Total_Rezago_Foraneo = value; }
        }
        public Decimal p_Total_Rezago_Urbano
        {
            get { return _Total_Rezago_Urbano; }
            set { _Total_Rezago_Urbano = value; }
        }
        public Decimal p_Total_Rezago_Rural
        {
            get { return _Total_Rezago_Rural; }
            set { _Total_Rezago_Rural = value; }
        }
        public Decimal p_Total_Honorarios_Foraneo
        {
            get { return _Total_Honorarios_Foraneo; }
            set { _Total_Honorarios_Foraneo = value; }
        }
        public Decimal p_Total_Honorarios_Urbano
        {
            get { return _Total_Honorarios_Urbano; }
            set { _Total_Honorarios_Urbano = value; }
        }
        public Decimal p_Total_Honorarios_Rural
        {
            get { return _Total_Honorarios_Rural; }
            set { _Total_Honorarios_Rural = value; }
        }

        public Decimal p_Total_Descuento_Enero
        {
            get { return _Total_Descuento_Enero; }
            set { _Total_Descuento_Enero = value; }
        }
        public Decimal p_Total_Descuento_Enero_Urbano
        {
            get { return _Total_Descuento_Enero_Urbano; }
            set { _Total_Descuento_Enero_Urbano = value; }
        }
        public Decimal p_Total_Descuento_Enero_Rural
        {
            get { return _Total_Descuento_Enero_Rural; }
            set { _Total_Descuento_Enero_Rural = value; }
        }
        public Decimal p_Total_Descuento_Enero_Foraneo
        {
            get { return _Total_Descuento_Enero_Foraneo; }
            set { _Total_Descuento_Enero_Foraneo = value; }
        }
        public Decimal p_Total_Descuento_Febrero
        {
            get { return _Total_Descuento_Febrero; }
            set { _Total_Descuento_Febrero = value; }
        }
        public Decimal p_Total_Descuento_Febrero_Urbano
        {
            get { return _Total_Descuento_Febrero_Urbano; }
            set { _Total_Descuento_Febrero_Urbano = value; }
        }
        public Decimal p_Total_Descuento_Febrero_Rural
        {
            get { return _Total_Descuento_Febrero_Rural; }
            set { _Total_Descuento_Febrero_Rural = value; }
        }
        public Decimal p_Total_Descuento_Febrero_Foraneo
        {
            get { return _Total_Descuento_Febrero_Foraneo; }
            set { _Total_Descuento_Febrero_Foraneo = value; }
        }
        public Decimal p_Total_Rezago_Acumulado
        {
            get { return _Total_Rezago_Acumulado; }
            set { _Total_Rezago_Acumulado = value; }
        }
        public Decimal p_Total_Honorarios_Acumulado
        {
            get { return _Total_Honorarios_Acumulado; }
            set { _Total_Honorarios_Acumulado = value; }
        }
        public Decimal p_Total_Recargos_Acumulado
        {
            get { return _Total_Recargos_Acumulado; }
            set { _Total_Recargos_Acumulado = value; }
        }
        public Decimal p_Total_Recargos_Febrero_Acumulado
        {
            get { return _Total_Recargos_Febrero_Acumulado; }
            set { _Total_Recargos_Febrero_Acumulado = value; }
        }

        public String p_Anio_Tabulador_Utilizar
        {
            get { return _Anio_Tabulador_Utilizar; }
            set { _Anio_Tabulador_Utilizar = value; }
        }
        public String p_Mes_Tabulador_Utilizar
        {
            get { return _Mes_Tabulador_Utilizar; }
            set { _Mes_Tabulador_Utilizar = value; }
        }
        public String p_Tabulador_Enero_Utilizar
        {
            get { return _Tabulador_Enero_Utilizar; }
            set { _Tabulador_Enero_Utilizar = value; }
        }
        public String p_Tabulador_Febrero_Utilizar
        {
            get { return _Tabulador_Febrero_Utilizar; }
            set { _Tabulador_Febrero_Utilizar = value; }
        }

        public DataTable p_Dt_Tasas
        {
            get { return _Dt_Tasas; }
            set { _Dt_Tasas = value; }
        }

        public String P_No_Orden_Variacion
        {
            get { return No_Orden_Variacion; }
            set { No_Orden_Variacion = value; }
        }

        public String P_Año_Orden_Variacion
        {
            get { return Año_Orden_Variacion; }
            set { Año_Orden_Variacion = value; }
        }
        #endregion METODOS_ACCESO

        #region METODOS_INTERFAZ

        public DataTable Consultar_Adeudos_Cuenta_Predial(String Cuenta_Predial, String Estatus, Int32 Desde_Anio, Int32 Hasta_Anio)
        {
            return Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial, Estatus, Desde_Anio, Hasta_Anio);
        }

        public DataTable Consultar_Adeudos_Cuenta_Predial_Con_Totales(String Cuenta_Predial, String Estatus, Int32 Desde_Anio, Int32 Hasta_Anio)
        {
            return Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Cuenta_Predial, Estatus, Desde_Anio, Hasta_Anio);
        }

        #endregion METODOS_INTERFAZ
        ///********************************************************************************
        ///                                 METODOS
        #region METODOS

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Parametros
        /// DESCRIPCIÓN: Llamar metodos para obtener cada parametro
        ///             Los mensajes de error los regresa como un String
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public void Obtener_Parametros()
        {
            Int32 Anio;
            if (p_Anio > 0)
            {
                Anio = p_Anio;
            }
            else
            {
                p_Anio = Anio = DateTime.Now.AddYears(1).Year;
            }

            Dictionary<String, String> IDs_Conceptos;
            Dictionary<String, Decimal> IDs_Tasas;

            // Obtener cuota minima de anio siguiente, si no tratar de obtener la del anio en curso
            p_Cuota_Minima = Obtener_Cuota_Minima(Anio);

            //Obtener salario minimo mediante la clase de negocio del catalogo de salarios minimos
            p_Salario_Minimo = Obtener_Salario_Minimo(Anio);

            // obtener las tasas de predial
            p_Dt_Tasas = Obtener_Tasas_Predial(out IDs_Conceptos, out IDs_Tasas);
            p_Dicc_IDs_Conceptos = IDs_Conceptos;
            p_Dicc_IDs_Tasas = IDs_Tasas;

            // obtener el total de cuentas a generar
            Obtener_Total_Cuentas();

            // obtener el tope de salarios minimos elevados al anio
            Obtener_Tope_Salarios_Minimos();

        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Cuota_Minima
        /// DESCRIPCIÓN: Consultar la cuota minima del anio proporcionado
        /// PARÁMETROS:
        ///         1. Anio: Entero que especifica el anio en el que se consulta la cuota minima
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 23-jul-2011 
        /// MODIFICÓ: Roberto González Oseguera
        /// FECHA_MODIFICÓ: 14-nov-2011
        /// CAUSA_MODIFICACIÓN: Se cambio para obtener todo el catalogo cuotas minimas y ponerlo en un diccionario
        ///*******************************************************************************************************
        public Decimal Obtener_Cuota_Minima(Int32 Anio)
        {
            DataTable Dt_Cuotas_Minimas;
            decimal Cuota_Minima;
            int Anio_Cuota = 0;
            try
            {
                // si el diccionario de cuotas minimas no existe, generarlo
                if (_Dicc_Cuotas_Minimas == null)
                {
                    _Dicc_Cuotas_Minimas = new Dictionary<int, decimal>();
                    Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
                    Cuotas_Minima.P_Anio = "";
                    Dt_Cuotas_Minimas = Cuotas_Minima.Consultar_Cuotas_Minimas();

                    // agregar cada cuota al diccionario de cuotas minimas
                    foreach (DataRow Dr_Cuota in Dt_Cuotas_Minimas.Rows)
                    {
                        // convertir y validar año y cuota
                        if (Int32.TryParse(Dr_Cuota["ANIO"].ToString(), out Anio_Cuota)
                            && Decimal.TryParse(Dr_Cuota["CUOTA"].ToString(), out Cuota_Minima))
                        {
                            // si no esta en el diccionario el año, agregarlo
                            if (!_Dicc_Cuotas_Minimas.ContainsKey(Anio_Cuota))
                            {
                                _Dicc_Cuotas_Minimas.Add(Anio_Cuota, Cuota_Minima);
                            }
                        }

                    }
                }

                if (_Dicc_Cuotas_Minimas.ContainsKey(Anio))            // si el año solicitado está en el diccionario, regresar valor
                {
                    return _Dicc_Cuotas_Minimas[Anio];
                }
                else                        // asignar 0
                {
                    return (decimal)0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Obtener_Cuota_Minima: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Obtener_Cuota_Minima
        
        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Diccionario_Grupos_Movimientos
        /// DESCRIPCIÓN: Consulta los movimientos y obtiene el grupo de cada un, regresa un 
        ///             diccionario con el id del movimiento y su grupo
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 06-mar-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public Dictionary<string,string> Obtener_Diccionario_Grupos_Movimientos()
        {
            DataTable Dt_Movimientos;
            Dictionary<string, string> Dic_Movimientos = new Dictionary<string, string>();
            var Consulta_Catalogo_Movimientos = new Cls_Cat_Pre_Movimientos_Negocio();
            string Movimiento_ID;

            try
            {
                Consulta_Catalogo_Movimientos.P_Campos_Dinamicos = Cat_Pre_Movimientos.Campo_Movimiento_ID + ", " + Cat_Pre_Movimientos.Campo_Grupo_Id;
                Dt_Movimientos = Consulta_Catalogo_Movimientos.Consultar_Movimientos();

                    // agregar cada dato al diccionario
                foreach (DataRow Dr_Movimiento in Dt_Movimientos.Rows)
                {
                    Movimiento_ID = Dr_Movimiento[Cat_Pre_Movimientos.Campo_Movimiento_ID].ToString();
                    // si no esta en el diccionario el año, agregarlo
                    if (!Dic_Movimientos.ContainsKey(Movimiento_ID))
                    {
                        Dic_Movimientos.Add(Movimiento_ID, Dr_Movimiento[Cat_Pre_Movimientos.Campo_Grupo_Id].ToString());
                    }
                }

                return Dic_Movimientos;
            }
            catch (Exception ex)
            {
                throw new Exception("Obtener_Diccionario_Grupos_Movimientos: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Obtener_Cuota_Minima

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Salario_Minimo
        /// DESCRIPCIÓN: Consultar el salario minimo del anio proporcionado
        /// PARÁMETROS:
        ///         1. Anio: Especifica el anio en el que se consulta el salario minimo
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 23-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public Decimal Obtener_Salario_Minimo(Int32 Anio)
        {
            decimal Salario;
            Cls_Cat_Pre_Salarios_Minimos_Negocio Rs_Salarios_Minimos = new Cls_Cat_Pre_Salarios_Minimos_Negocio();

            try
            {
                Salario = Rs_Salarios_Minimos.Consultar_Salario_Anio(Anio.ToString());
                if (Salario > 0)            // si se obtuvo un valor mayor a cero, asignar salario minimo
                {
                    return Salario;
                }
                else                        // si no, tratar de obtener otro salario
                {
                    return (decimal)0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Obtener_Salario_Minimo: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Obtener_Salario_Minimo

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Tasas_Predial
        /// DESCRIPCIÓN: Consultar el salario minimo del anio proporcionado
        /// PARÁMETROS:
        ///         1. Anio: Especifica el anio en el que se consulta el salario minimo
        ///         2. IDs_Conceptos: Diccionario de conceptos (ID y nombre)
        ///         3. IDs_Tasas: Diccionario con tasas (ID y monto de la tasa)
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 23-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public DataTable Obtener_Tasas_Predial(out Dictionary<String, String> IDs_Conceptos, out Dictionary<String, Decimal> IDs_Tasas)
        {
            try
            {
                // datatable para la consulta, diccionarios para ID, identificador y tasa
                DataTable Dt_Tasas;
                DataTable Dt_Tasas_Anuales;
                IDs_Conceptos = new Dictionary<String, String>();
                IDs_Tasas = new Dictionary<String, Decimal>();
                _Dicc_Tasas_Anuales = new Dictionary<String, Decimal>();
                Decimal Tasa = 0;
                Dt_Tasas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Tasas_Conceptos(this);
                // si el numero de filas obtenidas de la consulta es mayor a 0, cargar datos encontrados a los diccionarios
                if (Dt_Tasas.Rows.Count > 0)
                {
                    foreach (DataRow Fila_Tasa in Dt_Tasas.Rows)
                    {
                        if (Decimal.TryParse(Fila_Tasa[Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual].ToString(), out Tasa)
                            && !IDs_Conceptos.ContainsKey(Fila_Tasa[Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID].ToString())
                            && !IDs_Tasas.ContainsKey(Fila_Tasa[Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID].ToString()))
                        {
                            IDs_Conceptos.Add(Fila_Tasa[Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID].ToString(),
                                Fila_Tasa[Cat_Pre_Tasas_Predial.Campo_Descripcion].ToString());
                            IDs_Tasas.Add(Fila_Tasa[Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID].ToString(), Tasa);
                        }
                    }
                }
                // obtener las tasas anuales
                Dt_Tasas_Anuales = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Tasas_Anuales();
                // si el numero de filas obtenidas de la consulta es mayor a 0, cargar datos encontrados en el diccionario
                if (Dt_Tasas_Anuales.Rows.Count > 0)
                {
                    foreach (DataRow Fila_Tasa in Dt_Tasas_Anuales.Rows)
                    {
                        // si hay una tasa decimal y el diccionario no contiene ya el ID, agregar al diccionario
                        if (Decimal.TryParse(Fila_Tasa[Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual].ToString(), out Tasa)
                            && !_Dicc_Tasas_Anuales.ContainsKey(Fila_Tasa[Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID].ToString()))
                        {
                            _Dicc_Tasas_Anuales.Add(Fila_Tasa[Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID].ToString(), Tasa);
                        }
                    }
                }

                return Dt_Tasas;
            }
            catch (Exception ex)
            {
                throw new Exception("Obtener_Tasas_Predial: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Obtener_Tasas_Predial

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Total_Cuentas
        /// DESCRIPCIÓN: Consultar el total de cuentas a generar, excluir canceladas, bloquedas y 
        ///             suspendidas o flageladas
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private void Obtener_Total_Cuentas()
        {
            try
            {
                p_Estatus_Excluir = " NOT IN ('CANCELADA','BLOQUEADA','PENDIENTE') ";
                p_Tipo_Suspension_Excluir = " NOT IN ('AMBAS', 'PREDIAL') ";
                p_Total_Cuentas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(p_Estatus_Excluir, null, p_Tipo_Suspension_Excluir);
            }
            catch (Exception ex)
            {
                throw new Exception("Obtener_Total_Cuentas: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Obtener_Total_Cuentas

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Tope_Salarios_Minimos
        /// DESCRIPCIÓN: Consultar el tope de salario minimos (del catalogo de parametros) para el 
        ///         exedente de valor y guarda el valor de salarios minimos elevados al anio
        ///             Tope * Salario minimo * 365
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public void Obtener_Tope_Salarios_Minimos()
        {
            Cls_Ope_Pre_Parametros_Negocio Rs_Cosulta_Tope_Salarios = new Cls_Ope_Pre_Parametros_Negocio();
            DataTable Dt_Parametros;
            Decimal Tope_Salarios;

            try
            {
                Dt_Parametros = Rs_Cosulta_Tope_Salarios.Consultar_Parametros();
                // si se obtuvieron resultados de la consulta, calcular el tope de salarios minimos elevados al anio
                if (Dt_Parametros.Rows.Count > 0)
                {
                    // si se obtiene un numero de la consulta en el campo Tope Salario, calcular y almacenar el valor 
                    if (Decimal.TryParse(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Tope_Salario].ToString(), out Tope_Salarios))
                    {
                        p_Tope_Salarios_Minimos = Tope_Salarios * p_Salario_Minimo * 365;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Obtener_Tope_Salarios_Minimos: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Obtener_Tope_Salarios_Minimos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Validar_Parametros
        /// DESCRIPCIÓN: Validar presencia de parametros para realizar calculos 
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public String Validar_Parametros()
        {
            String Mensajes_Error = "";

            // verificar que hay salario minimo
            if (p_Salario_Minimo <= 0)
            {
                Mensajes_Error += "<br />Falta ingresar el salario mínimo para el " + p_Anio;
            }

            // Verificar que hay tope salarial
            if (p_Tope_Salarios_Minimos <= 0 && p_Salario_Minimo > 0)
            {
                Mensajes_Error += "<br />No se encontró el tope de salarios mínimos para calcular excedente de valor.";
            }

            // Verificar que hay tope salarial
            if (p_Cuota_Minima <= 0)
            {
                Mensajes_Error += "<br />No se encontró la cuota mínima para el " + p_Anio;
            }

            // si no se encontraron tasas, regresar con mensaje
            if (_Dicc_Tasas_Anuales.Count <= 0)
            {
                Mensajes_Error += "<br />No se encontraron tasas para calcular impuesto.";
            }

            return Mensajes_Error;
        }// termina metodo Validar_Parametros

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Proyeccion
        /// DESCRIPCIÓN: Realizar la proyeccion de adeudos, validando antes 
        /// PARÁMETROS:
        ///             1. Sumatoria_Adeudos_Generados: datatable donde se regresan los 
        ///                 montos anual y por bimestre generados
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public String Generar_Proyeccion(out DataTable Sumatoria_Adeudos_Generados)
        {
            Sumatoria_Adeudos_Generados = new DataTable();
            String Mensaje_Error = "";

            // si falta la cuota minima o el tope de salarios minimos o si no hay tasas, regresar con mensaje
            if (p_Cuota_Minima <= 0 || _Dicc_Tasas_Anuales.Count <= 0 || p_Tope_Salarios_Minimos <= 0)
            {
                return "Faltan parámetros para generar la proyección";
            }
            else
            {
                // eliminar registros de la tabla temporal
                Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Eliminar_Adeudos_Temporales();
                p_Estatus_Adeudo = "POR PAGAR";

                // conteo de cuentas excluidas
                p_Total_Padron = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, null, null);
                p_Total_Bloqueadas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, "BLOQUEADA", null);
                p_Total_Canceladas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, "CANCELADA", null);
                p_Total_Suspendidas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, null, " IN ('AMBAS','PREDIAL') ");
                p_Total_Pendientes = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, "PENDIENTE", null);

                // llamar al metodo que genera los adeudos pasando como parametro la tabla de adeudos temporal
                Mensaje_Error = Generar_Adeudos(Tmp_Pre_Adeudos_Predial.Tabla_Tmp_Pre_Adeudos_Predial);

                // conteo de adeudos generados
                //p_Total_Adeudos_Generados = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Adeudos(
                //    Tmp_Pre_Adeudos_Predial.Tabla_Tmp_Pre_Adeudos_Predial,
                //    "PROYECCION",
                //    DateTime.Now.ToString("dd/MM/yyyy"),
                //    DateTime.Now.ToString("dd/MM/yyyy")
                //    );
                Sumatoria_Adeudos_Generados = (DataTable)Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Monto_Adeudos_Generados(
                    Tmp_Pre_Adeudos_Predial.Tabla_Tmp_Pre_Adeudos_Predial,
                    DateTime.Now.ToString("dd/MM/yyyy"),
                    DateTime.Now.ToString("dd/MM/yyyy")
                    );

                return Mensaje_Error;
            }

        }// termina metodo Generar_Proyeccion

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Adeudos
        /// DESCRIPCIÓN: Leer cuentas datos de cuentas, calcular cuota anual y bimestral y escribirla en 
        ///             la tabla que llega como parámetro
        /// PARÁMETROS:
        ///             1. Tabla. nombre de la tabla en la que se generaran los adeudos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Generar_Adeudos(String Tabla)
        {
            DataTable Datos_Cuentas;
            String Mensaje_Error = "";
            Int32 Contador_Cuotas_Minimas = 0;
            Int32 Contador_Inserciones = 0;
            Decimal Contador_Recargos_Generados = 0;
            Int32 Contador_Exentos = 0;
            Int32 Total_Ordenes = 0;
            // parametros
            Int32 Anio_Actual = 0;
            Int32 Anio_Generar = 0;
            Dictionary<String, Decimal> Tasas = _Dicc_Tasas_Anuales;
            Dictionary<String, String> Errores_Cuentas = new Dictionary<string, string>();
            Decimal Valor_Fiscal;
            DateTime Fecha_Inicio_Anio_Generar;
            // calculos cuenta
            Decimal Cuota_Anual = 0;
            Decimal[] Cuota_Bimestral = { 0, 0, 0, 0, 0, 0 };
            String Usuario_Creo = Sessiones.Cls_Sessiones.Nombre_Empleado;
            Decimal Porcentaje_Exencion = 0;
            DateTime Termino_Exencion;

            Anio_Actual = DateTime.Now.Year;
            // establecer año a calcular igual a la propiedad p_anio o en su defecto, el año actual mas uno
            Anio_Generar = p_Anio > 0 ? p_Anio : Anio_Actual + 1;
            Fecha_Inicio_Anio_Generar = DateTime.Parse("01/01/" + Anio_Generar.ToString());

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Consecutivo_No_Adeudo = 0;

            // si el diccionario de cuotas minimas es nulo, llamar metodo para consultar cuotas minimas
            if (_Dicc_Cuotas_Minimas == null)
            {
                Obtener_Cuota_Minima(Anio_Actual);
            }

            // si los parametros no estan presentes, regresar con mensaje
            if (C_Minima <= 0 || p_Salario_Minimo <= 0 || Tasas.Count <= 0 || p_Tope_Salarios_Minimos <= 0)
            {
                return "No se han especificado parámetros.";
            }

            try
            {
                p_Estatus_Excluir = " NOT IN ('CANCELADA','PENDIENTE') ";
                //p_Tipo_Suspension_Excluir = " NVL(c." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ", ' ') NOT IN ('AMBAS', 'PREDIAL') ";

                // obtener listado de cuentas a considerar
                Datos_Cuentas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Datos_Cuentas(p_Estatus_Excluir, String.Empty, p_Tipo_Suspension_Excluir);
                // si no se recibieron datos abandonar el metodo con mensaje de error
                if (Datos_Cuentas.Rows.Count <= 0)
                {
                    return "Error al tratar de leer los datos de las cuentas.<br />";
                }
                // abrir conexion con la base de datos
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;

                // obtener consecutivo para insertar adeudos
                Consecutivo_No_Adeudo = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Consecutivo_No_Adeudo(Tabla);

                // calcular la cuota para cada cuenta
                foreach (DataRow Cuenta in Datos_Cuentas.Rows)
                {
                    String Mensajes_Error_Cuentas = "";
                    Decimal Total_Recargos;
                    Decimal Total_Adeudo_Rezago;
                    Decimal Cuota_Anual_Anterior = 0;

                    // parametros de la cuenta
                    String Cuenta_Predial_ID = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    String Numero_Cuenta_Predial = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                    String Cuota_Fija = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString();
                    String Tipo_Beneficio = Cuenta[Cat_Pre_Casos_Especiales.Campo_Tipo].ToString();
                    String ID_Tasa = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
                    Decimal Costo_m2;
                    Decimal Diferencia_Construccion;
                    Decimal.TryParse(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString(), out Valor_Fiscal);
                    Decimal.TryParse(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString(), out Diferencia_Construccion);
                    Decimal.TryParse(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Costo_m2].ToString(), out Costo_m2);
                    Decimal.TryParse(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString(), out Cuota_Anual_Anterior);
                    // obtener recargos de la cuenta
                    Mensajes_Error_Cuentas = Generar_Recargos_Predial(Cuenta_Predial_ID, out Total_Recargos, out Total_Adeudo_Rezago, Anio_Generar);
                    if (Mensajes_Error_Cuentas.Length > 0)
                    {
                        Errores_Cuentas.Add(Numero_Cuenta_Predial, Mensajes_Error_Cuentas);
                    }
                    // si se obtuvieron recargos
                    if (Total_Adeudo_Rezago > 0 && Total_Recargos > 0)
                    {
                        Contador_Recargos_Generados += Total_Recargos;
                    }
                    // verificar termino exencion para asignar o no porcentaje de exencion
                    if (DateTime.TryParse(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString(), out Termino_Exencion))
                    {
                        if (Termino_Exencion >= Fecha_Inicio_Anio_Generar)
                        {
                            if (Decimal.TryParse(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString(), out Porcentaje_Exencion)
                                && Porcentaje_Exencion > 0)
                            {
                                Contador_Exentos += 1;
                            }
                        }
                    }


                    // verificar parametros de la cuenta
                    Mensajes_Error_Cuentas = Validar_Datos_Cuenta(Diferencia_Construccion, Costo_m2, ID_Tasa, Cuota_Fija, Tipo_Beneficio);
                    if (Mensajes_Error_Cuentas.Length > 0)
                    {
                        Errores_Cuentas.Add(Numero_Cuenta_Predial, Mensajes_Error_Cuentas);
                        continue;
                    }

                    Mensajes_Error_Cuentas = "";
                    // llamar metodo que calcula la cuota anual con los datos consultados de la cuenta
                    Mensajes_Error_Cuentas = Calcular_Cuota_Anual(
                        Valor_Fiscal,
                        Cuota_Fija,
                        Tipo_Beneficio,
                        Costo_m2,
                        ID_Tasa,
                        Diferencia_Construccion,
                        Porcentaje_Exencion,
                        out Cuota_Anual
                        );
                    // si hubo errores
                    if (Mensajes_Error_Cuentas.Length > 0)
                    {
                        if (!Errores_Cuentas.ContainsKey(Numero_Cuenta_Predial))
                        {
                            Errores_Cuentas.Add(Numero_Cuenta_Predial, Mensajes_Error_Cuentas);
                        }
                        else
                        {
                            Errores_Cuentas[Numero_Cuenta_Predial] = Errores_Cuentas[Numero_Cuenta_Predial] + "<br/>" + Mensajes_Error_Cuentas;
                        }
                        continue;
                    }

                    // si la cuota anual calculada es mayor que la cuota minima, prorratear en los seis bimestres
                    if (Cuota_Anual > C_Minima)
                    {
                        Cuota_Bimestral[0] = Decimal.Round(Cuota_Anual / 6, 2);     // cuota bimestral en todos los bimestres (anual / 6)
                        Cuota_Bimestral[1] = Cuota_Bimestral[0];
                        Cuota_Bimestral[2] = Cuota_Bimestral[0];
                        Cuota_Bimestral[3] = Cuota_Bimestral[0];
                        Cuota_Bimestral[4] = Cuota_Bimestral[0];
                        Cuota_Bimestral[5] = Cuota_Bimestral[0];
                    }
                    else        // se asigna la cuota minima en el primer bimestre
                    {
                        Contador_Cuotas_Minimas++;  //incrementar contador de cuotas minimas
                        Cuota_Anual = C_Minima;
                        Cuota_Bimestral[0] = C_Minima;
                        Cuota_Bimestral[1] = 0;
                        Cuota_Bimestral[2] = 0;
                        Cuota_Bimestral[3] = 0;
                        Cuota_Bimestral[4] = 0;
                        Cuota_Bimestral[5] = 0;

                        // verifica si la cuenta no tiene cuota minima (si la cuota anual es igual a uno de los valores en el diccionario de cuotas minimas)
                        if (!_Dicc_Cuotas_Minimas.ContainsValue(Cuota_Anual_Anterior))
                        {
                            // incrementar el numero de ordenes
                            Total_Ordenes++;
                        }
                    }

                    Contador_Inserciones += Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Alta_Adeudos(
                        Tabla,
                        Usuario_Creo,
                        p_Estatus_Adeudo,
                        Cuenta_Predial_ID,
                        Cuota_Anual,
                        Cuota_Bimestral,
                        p_Anio,
                        Consecutivo_No_Adeudo++,
                        Comando);
                }

                // aplicar cambios en base de datos
                Transaccion.Commit();
                Conexion.Close();

                p_Errores_Cuentas = Errores_Cuentas;
                _Total_Cuotas_Minimas = Contador_Cuotas_Minimas;
                _Total_Adeudos_Generados = Contador_Inserciones;
                _Total_Recargos_Generados = Contador_Recargos_Generados;
                _Total_Exenciones = Contador_Exentos;
                _Total_Ordenes_Cuota_Minima = Total_Ordenes;
            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Generar_Adeudos: " + Ex.Message.ToString(), Ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Generar_Adeudos: " + ex.Message.ToString(), ex);
            }
            finally
            {
                Conexion.Close();
            }
            return Mensaje_Error;
        } // termina metodo Generar_Adeudos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Aplicar_Adeudos
        /// DESCRIPCIÓN: Pasar los adeudos de la tabla temporal de la proyección a la tabla de adeudos de predial
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 02-dic-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Aplicar_Adeudos()
        {
            DataTable Dt_Cuentas;
            String Mensaje_Error = "";
            Int32 Contador_Cuotas_Minimas = 0;
            Int32 Total_Ordenes = 0;
            // parametros
            Int32 Anio_Actual = 0;
            Int32 Anio_Generar = 0;
            var Tasas = _Dicc_Tasas_Anuales;
            //var Errores_Cuentas = new Dictionary<string, string>();
            // calculos cuenta
            //Decimal Cuota_Anual = 0;
            //Decimal[] Cuota_Bimestral = { 0, 0, 0, 0, 0, 0 };
            //String Usuario_Creo = Sessiones.Cls_Sessiones.Nombre_Empleado;

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            Anio_Actual = DateTime.Now.Year;
            // establecer año a calcular igual a la propiedad p_anio o en su defecto, el año actual mas uno
            Anio_Generar = p_Anio > 0 ? p_Anio : Anio_Actual + 1;

            try
            {
                // si la cantidad de adeudos en la tabla temporal es menor a cero, regresar con mensaje
                if (Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Adeudos(Tmp_Pre_Adeudos_Predial.Tabla_Tmp_Pre_Adeudos_Predial, null, null, null) <= 0)
                {
                    return "No hay adeudos por aplicar.";
                }

                // consultar cuota minima del año a generar adeudo
                C_Minima = Obtener_Cuota_Minima(Anio_Generar);
                // obtener listado de cuentas con cuota mínima en el primer bimestre y cero en el segundo bimestre
                Dt_Cuentas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Cuentas_Con_Cuota_Minima(C_Minima);

                // abrir conexion con la base de datos
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;

                // calcular la cuota para cada cuenta
                foreach (DataRow Cuenta in Dt_Cuentas.Rows)
                {
                    //Cuota_Anual = C_Minima;
                    //String Ultimo_Movimiento_Cuenta = "";
                    //String Grupo_Movimiento_Cuenta = "";
                    //String Tipo_Predio_Actual_Cuenta = "";
                    //DataTable Dt_Movimientos_Cuenta;

                    Decimal Cuota_Anual_Anterior = 0;

                    // parametros de la cuenta
                    //String Cuenta_Predial_ID = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    Decimal.TryParse(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString(), out Cuota_Anual_Anterior);


                    // verificar si la cuenta predial no tiene cuota minima dada de alta, para generar orden de variacion
                    // (se compara la cuota anual registrada para la cuenta con las cuotas minimas del sistema)
                    //if (!_Dicc_Cuotas_Minimas.ContainsValue(Cuota_Anual_Anterior))
                    //{
                        //// consultar ultimo movimiento de la cuenta
                        //String No_Orden = "";
                        //Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Resumen = new Cls_Ope_Pre_Resumen_Predio_Negocio();
                        //Rs_Resumen.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                        //Dt_Movimientos_Cuenta = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Ultimo_Movimiento(Cuenta_Predial_ID);
                        //if (Dt_Movimientos_Cuenta.Rows.Count > 0)
                        //{
                        //    Ultimo_Movimiento_Cuenta = Dt_Movimientos_Cuenta.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID].ToString();
                        //    Grupo_Movimiento_Cuenta = Dt_Movimientos_Cuenta.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID].ToString();
                        //    Tipo_Predio_Actual_Cuenta = Dt_Movimientos_Cuenta.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID].ToString();
                        //}
                        //// generar orden de variacion con actualización de cuota anual
                        //Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                        //Orden_Variacion.P_Generar_Orden_Anio = Anio_Actual.ToString();
                        //Orden_Variacion.P_Generar_Orden_Cuenta_ID = Cuenta_Predial_ID;
                        //Orden_Variacion.P_Generar_Orden_Movimiento_ID = Ultimo_Movimiento_Cuenta;
                        //Orden_Variacion.P_Generar_Orden_Estatus = "ACEPTADA";
                        //Orden_Variacion.P_Generar_Orden_Obserbaciones = "GENERACION DE ADEUDOS " + Anio_Calculo;
                        //Orden_Variacion.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual, Cuota_Anual.ToString());
                        //No_Orden = Orden_Variacion.Generar_Orden_Variacion();
                        //Orden_Variacion.Aplicar_Variacion();
                        //// agregar numero de nota
                        //Orden_Variacion.P_Orden_Variacion_ID = No_Orden;
                        //Orden_Variacion.P_Año = Anio_Actual;
                        //Orden_Variacion.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                        //Orden_Variacion.P_Grupo_Movimiento_ID = Grupo_Movimiento_Cuenta;
                        //Orden_Variacion.P_Tipo_Predio_ID = Tipo_Predio_Actual_Cuenta;
                        //Orden_Variacion.P_No_Nota = Obtener_Dato_Consulta(ref Comando,
                        //    "NVL(MAX(" + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + "), 0) + 1",
                        //    Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Orden_Variacion, Ope_Pre_Ordenes_Variacion.Campo_Anio
                        //    + " = " + Orden_Variacion.P_Año + " AND " + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID
                        //    + " = '" + Orden_Variacion.P_Grupo_Movimiento_ID + "' AND "
                        //    + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + " = '" + Orden_Variacion.P_Tipo_Predio_ID + "'");
                        //Orden_Variacion.P_Fecha_Nota = DateTime.Now;
                        //Orden_Variacion.P_No_Nota_Impreso = "NO";
                        //Orden_Variacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        //Orden_Variacion.Modificar_Orden_Variacion();
                        //// incrementar el numero de ordenes
                        Total_Ordenes++;
                    //}
                    Contador_Cuotas_Minimas++;

                }

                _Total_Cuotas_Minimas = Contador_Cuotas_Minimas;
                _Total_Ordenes_Cuota_Minima = Total_Ordenes;
                _Total_Adeudos_Generados = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Aplicacion_Adeudos_Proyeccion(Comando);

                // aplicar cambios en base de datos
                Transaccion.Commit();
                Conexion.Close();

            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Generar_Adeudos: " + Ex.Message.ToString(), Ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Generar_Adeudos: " + ex.Message.ToString(), ex);
            }
            finally
            {
                Conexion.Close();
            }
            return Mensaje_Error;
        } // termina metodo Generar_Adeudos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Ordenes_Variacion_Cuota_Minima
        /// DESCRIPCIÓN: De la tabla temporal de adeudo se recuperan las cuentas con cuota mínima y se genera 
        ///             una orden de variación para las cuentas que no tengan registrada cuota mínima
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 14-ene-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public String Generar_Ordenes_Variacion_Cuota_Minima()
        {
            DataTable Dt_Cuentas;
            String Mensaje_Error = "";
            Int32 Total_Ordenes = 0;
            string Filtro_Estatus;
            Int32 Ultimo_Numero_Orden = 0;
            Int32 Numero_Orden_Generada;
            int Ultimo_Numero_Nota;
            // parametros
            Int32 Anio_Actual = 0;
            Int32 Anio_Generar = 0;
            Dictionary<String, Decimal> Tasas = _Dicc_Tasas_Anuales;
            Dictionary<String, String> Dic_Ultimos_Movimientos;
            Dictionary<String, String> Dic_Grupos_Movimientos;
            Dictionary<String, int> Dic_Numeros_Nota = new Dictionary<string, int>();

            var Errores_Cuentas = new Dictionary<string, string>();
            // calculos cuenta
            Decimal Cuota_Anual = 0;
            String Usuario_Creo = Sessiones.Cls_Sessiones.Nombre_Empleado;
            var Consultar_Datos_Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            Anio_Actual = DateTime.Now.Year;
            // establecer año a calcular igual a la propiedad p_anio o en su defecto, el año actual mas uno
            Anio_Generar = p_Anio > 0 ? p_Anio : Anio_Actual + 1;
            Filtro_Estatus = " NOT IN ('CANCELADA','PENDIENTE','BAJA','TEMPORAL')";

            try
            {
                // obtener los ultimos movimientos de las cuentas
                Dic_Ultimos_Movimientos = Obtener_Ultimos_Movimientos_ID();
                // obtener los ultimos movimientos de las cuentas
                Dic_Grupos_Movimientos = Obtener_Diccionario_Grupos_Movimientos();

                // consultar cuota minima del año a generar adeudo
                C_Minima = Obtener_Cuota_Minima(Anio_Generar);
                // obtener listado de cuentas con cuota mínima en el primer bimestre y cero en el segundo bimestre
                Dt_Cuentas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Cuentas_Adeudo_Menor_Cuota_Minima(C_Minima, Filtro_Estatus);

                // abrir conexion con la base de datos
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;

                // calcular la cuota para cada cuenta
                foreach (DataRow Cuenta in Dt_Cuentas.Rows)
                {
                    Cuota_Anual = C_Minima;
                    String Ultimo_Movimiento_Cuenta = "";
                    String Grupo_Movimiento_Cuenta = "";
                    String Tipo_Predio_Actual_Cuenta = "";
                    string Anio_Grupo_TipoPredio;

                    // parametros de la cuenta
                    String Cuenta_Predial_ID = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    Tipo_Predio_Actual_Cuenta = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();

                    // ultimo movimiento de la cuenta
                    if (Dic_Ultimos_Movimientos.ContainsKey(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString()))
                    {
                        Ultimo_Movimiento_Cuenta = Dic_Ultimos_Movimientos[Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString()];
                    }
                    else
                    {
                        Ultimo_Movimiento_Cuenta = "";
                    }
                    // grupo de movimiento de la cuenta
                    if (Ultimo_Movimiento_Cuenta.Length > 0 && Dic_Grupos_Movimientos.ContainsKey(Ultimo_Movimiento_Cuenta))
                    {
                        Grupo_Movimiento_Cuenta = Dic_Grupos_Movimientos[Ultimo_Movimiento_Cuenta];
                    }
                    else
                    {
                        Grupo_Movimiento_Cuenta = "";
                    }

                    Anio_Grupo_TipoPredio = Anio_Actual + Grupo_Movimiento_Cuenta + Tipo_Predio_Actual_Cuenta;
                    // si en el diccionario ya hay un valor para el numero de nota Anio_Grupo_TipoPredio, tomarlo
                    if (Dic_Numeros_Nota.ContainsKey(Anio_Grupo_TipoPredio))
                    {
                        Ultimo_Numero_Nota = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Alta_Variacion(
                            Comando,
                            Cuenta_Predial_ID,
                            Anio_Actual,
                            Anio_Generar,
                            Grupo_Movimiento_Cuenta,
                            Ultimo_Movimiento_Cuenta,
                            Tipo_Predio_Actual_Cuenta,
                            Cuota_Anual,
                            Usuario_Creo,
                            Dic_Numeros_Nota[Anio_Grupo_TipoPredio],
                            Ultimo_Numero_Orden,
                            out Numero_Orden_Generada
                            );
                        Ultimo_Numero_Orden = Numero_Orden_Generada;
                        // actualizar diccionario con números de nota
                        Dic_Numeros_Nota[Anio_Grupo_TipoPredio] = Ultimo_Numero_Nota;
                        // incrementar el numero de ordenes
                        Total_Ordenes++;
                    }
                    else
                    {
                        Ultimo_Numero_Nota = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Alta_Variacion(
                            Comando,
                            Cuenta_Predial_ID,
                            Anio_Actual,
                            Anio_Generar,
                            Grupo_Movimiento_Cuenta,
                            Ultimo_Movimiento_Cuenta,
                            Tipo_Predio_Actual_Cuenta,
                            Cuota_Anual,
                            Usuario_Creo,
                            0,
                            Ultimo_Numero_Orden,
                            out Numero_Orden_Generada
                            );
                        Ultimo_Numero_Orden = Numero_Orden_Generada;
                        // agregar numero de nota al diccionario
                        Dic_Numeros_Nota.Add(Anio_Grupo_TipoPredio, Ultimo_Numero_Nota);
                        // incrementar el numero de ordenes
                        Total_Ordenes++;
                    }
                }
                _Total_Ordenes_Cuota_Minima = Total_Ordenes;

                // aplicar cambios en base de datos
                Transaccion.Commit();
                Conexion.Close();

            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Generar_Ordenes_Variacion_Cuota_Minima: " + Ex.Message.ToString(), Ex);
            }
            catch (Exception ex)
            {
                Transaccion.Rollback();
                throw new Exception("Generar_Ordenes_Variacion_Cuota_Minima: " + ex.Message.ToString(), ex);
            }
            finally
            {
                Conexion.Close();
            }
            return Mensaje_Error;
        } // termina metodo Generar_Ordenes_Variacion_Cuota_Minima

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Calcular_Cuota_Anual
        /// DESCRIPCIÓN: Calcular la cuota anual dados los datos de una cuenta
        ///             Para las cuentas con beneficio calcula excedente de construccion (para 6 bimestres)
        ///             y de valor si aplica y a las cuentas con financiamiento calcula ambas y toma la mayor
        /// PARÁMETROS:
        /// 	1. Valor_Fiscal: Valor fiscal del predio
        /// 	2. Cuota_Fija: string que indica si tiene cuota fija (SI/NO) 
        /// 	3. Tipo_Beneficio: SENITUD/PENSIONADO/JUBILADO/FINANCIAMIENTO
        /// 	4. Valor_m2: Indica el costo del metro cuadrado para el predio
        /// 	5. ID_Tasa: ID de la tasa del predio
        /// 	6. Diferencia_Construccion: metros cuadrados de diferencia de construccion del predio
        /// 	7. (out) Cuota_Anual: Decimal en el que se almacena la cuota anual calculada
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Calcular_Cuota_Anual(
            Decimal Valor_Fiscal,
            String Cuota_Fija,
            String Tipo_Beneficio,
            Decimal Valor_m2,
            String ID_Tasa,
            Decimal Diferencia_Construccion,
            Decimal Porcentaje_Exencion,
            out Decimal Cuota_Anual)
        {
            String Mensaje_Error = "";
            // limpiar variables para calculo de cuotas
            Cuota_Anual = 0;

            // si los parametros no estan presentes, regresar con mensaje de error
            if (p_Cuota_Minima <= 0 || p_Salario_Minimo <= 0 || _Dicc_Tasas_Anuales.Count <= 0)
            {
                return "No se puede calcular adeudo sin haber especificado parámetros.";
            }

            try
            {
                // calcular la cuota anual. Si tiene cuota fija, verificar excedente de valor
                if (Cuota_Fija == "SI")
                {
                    Decimal Excedente_Construccion = 0;
                    Decimal Excedente_Valor = 0;

                    // si el valor fiscal excede del tope de salario minimos, calcular diferencia * tasa + Cuota minima = cuota anual
                    if (Valor_Fiscal > p_Tope_Salarios_Minimos)
                    {
                        Excedente_Valor = (Valor_Fiscal - p_Tope_Salarios_Minimos) *
                            _Dicc_Tasas_Anuales[ID_Tasa] / 1000;
                    }
                    // validar que si hay diferencias de construccion se tenga el tipo de beneficio
                    if (Diferencia_Construccion > 0)
                    {
                        // si hay diferencias de construccion, calcular la cuota del excedente
                        if (String.IsNullOrEmpty(Tipo_Beneficio))
                        {
                            return "Hay diferencias de construcción, pero la cuenta no tiene tipo de beneficio.";
                        }
                        // si el tipo de beneficio es por financiamiento, calcular diferencias de construccion
                        if (Tipo_Beneficio == "FINANCIAMIENTO")
                        {
                            Excedente_Construccion = (Diferencia_Construccion * Valor_m2 * 6 * (_Dicc_Tasas_Anuales[ID_Tasa] / 1000));
                        }
                    }

                    // si la cuota por excedente de construccion es mayor que la cuota por excedente de valor
                    if (Excedente_Construccion > Excedente_Valor)
                    {
                        Cuota_Anual = Excedente_Construccion + p_Cuota_Minima;
                    }
                    else
                    {
                        Cuota_Anual = Excedente_Valor + p_Cuota_Minima;
                    }
                }
                else
                {
                    Cuota_Anual = (Valor_Fiscal * _Dicc_Tasas_Anuales[ID_Tasa]) / 1000;
                    Cuota_Anual = Decimal.Round(Cuota_Anual, 2);                // redondear cuota anual a dos digitos

                    // calcular con porcentaje de exencion
                    if (Porcentaje_Exencion > 0)
                    {
                        Cuota_Anual -= Cuota_Anual * Porcentaje_Exencion / 100;
                    }
                }
                return Mensaje_Error;
            }
            catch (Exception ex)
            {
                throw new Exception("Calcular_Cuota_Anual: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Calcular_Cuota_Anual

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Impuesto_Cierre_Anual
        /// DESCRIPCIÓN: Obtener parametros, validar que estan completos y generar adeudos
        /// PARÁMETROS:
        ///             1. Sumatoria_Adeudos_Generados: datatable donde se regresan los 
        ///                 montos anual y por bimestre generados
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 27-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public String Generar_Impuesto_Cierre_Anual(out System.Data.DataTable Sumatoria_Adeudos_Generados)
        {
            String Mensaje_Error = "";
            Sumatoria_Adeudos_Generados = new DataTable();

            try
            {
                // llamar metodo que obtiene los parametros
                Obtener_Parametros();

                // estatus de cuentas a excluir
                p_Estatus_Excluir = " NOT IN ('CANCELADA','BLOQUEADA') ";
                p_Tipo_Suspension_Excluir = " IN ('AMBAS', 'PREDIAL') ";
                // estatus de adeudos generados
                p_Estatus_Adeudo = "POR PAGAR";

                // Validar Parametros 
                Mensaje_Error = Validar_Parametros();
                if (Mensaje_Error.Length > 0)
                {
                    // si la validacion regreso mensajes de error, regresar mensaje
                    return Mensaje_Error;
                }

                // conteo de cuentas excluidas
                p_Total_Padron = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, null, null);
                p_Total_Bloqueadas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, "BLOQUEADA", null);
                p_Total_Canceladas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, "CANCELADA", null);
                p_Total_Suspendidas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, null, " IN ('AMBAS','PREDIAL') ");
                p_Total_Pendientes = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, "PENDIENTE", null);

                // aplicar los adeudos generados en la proyeccion
                Mensaje_Error = Aplicar_Adeudos();

                // conteo de adeudos generados
                //p_Total_Adeudos_Generados = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Adeudos(
                //    Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial,
                //    "POR PAGAR",
                //    DateTime.Now.ToString("dd/MM/yyyy"),
                //    DateTime.Now.ToString("dd/MM/yyyy")
                //    );
                Sumatoria_Adeudos_Generados = (DataTable)Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Monto_Adeudos_Generados(
                    Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial,
                    DateTime.Now.ToString("dd/MM/yyyy"),
                    DateTime.Now.ToString("dd/MM/yyyy")
                    );

                return Mensaje_Error;
            }
            catch (Exception ex)
            {
                throw new Exception("Generar_Impuesto_Cierre_Anual: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Generar_Impuesto_Cierre_Anual

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Validar_Datos_Cuenta
        /// DESCRIPCIÓN: Valida que la informacion de la cuenta para realizar el calculo de 
        ///             impuesto este completa
        /// PARÁMETROS:
        /// 	1. Diferencia_Construccion: cantidad de metros cuadrados de diferencia de construccion
        /// 	2. Costo_m2: Costo de metro cuadrado de la cuenta, se requiere para calcular diferencias de construccion
        /// 	3. ID_Tasa: Tasa para calcular impuesto, debe estar registrada
        /// 	4. Cuota_Fija: SI o NO tiene cuota fija la cuenta
        /// 	5. Tipo_Beneficio: Debe tener tipo de beneficio si tiene cuota fija
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 27-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Validar_Datos_Cuenta(
            Decimal Diferencia_Construccion,
            Decimal Costo_m2,
            String ID_Tasa,
            String Cuota_Fija,
            String Tipo_Beneficio)
        {
            String Mensaje = "";
            if (!_Dicc_Tasas_Anuales.ContainsKey(ID_Tasa))
                if (Mensaje.Length == 0)
                {
                    Mensaje = "No se encontró la tasa de la cuenta predial.";
                }
                else
                {
                    Mensaje += "<br />No se encontró la tasa de la cuenta predial.";
                }
            // validar diferencias de construccion y costo de metro cuadrado
            if (Diferencia_Construccion > 0)
            {
                // solo para cuentas con financiamiento
                if (Tipo_Beneficio == "FINANCIAMIENTO")
                {
                    if (Costo_m2 <= 0)
                    {
                        if (Mensaje.Length == 0)
                        {
                            Mensaje = "No tiene costo de metro cuadrado para calcular Diferencias de construcción.";
                        }
                        else
                        {
                            Mensaje += "<br />No tiene costo de metro cuadrado para calcular Diferencias de construcción.";
                        }
                    }
                }
            }
            return Mensaje;
        } // termina metodo Validar_Datos_Cuenta

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Formar_Tabla_Recargos
        /// DESCRIPCIÓN: Crear tabla con columnas para almacenar recargos
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 08-ago-2011 
        /// MODIFICÓ: Roberto González Oseguera
        /// FECHA_MODIFICÓ: 28-mar-2012
        /// CAUSA_MODIFICACIÓN: Se agrega columna NO_ADEUDO
        ///*******************************************************************************************************
        private DataTable Formar_Tabla_Recargos()
        {
            // tabla y columnas
            DataTable Dt_Recargos = new DataTable();

            // agregar columnas a la tabla
            Dt_Recargos.Columns.Add(new DataColumn("PERIODO", typeof(string)));
            Dt_Recargos.Columns.Add(new DataColumn("TASA", typeof(decimal)));
            Dt_Recargos.Columns.Add(new DataColumn("ADEUDO", typeof(decimal)));
            Dt_Recargos.Columns.Add(new DataColumn("RECARGOS", typeof(decimal)));
            Dt_Recargos.Columns.Add(new DataColumn("NO_ADEUDO", typeof(string)));

            // regresar tabla adeudos 
            return Dt_Recargos;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Calcular_Recargos_Predial
        /// DESCRIPCIÓN: Obtener tabulador de recargos, adeudos de la cuenta y del rezago para calcular 
        ///             los recargos de una cuenta
        /// PARÁMETROS:
        ///             1. Cuenta predial. Numero de cuenta a la que se calcularán adeudos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 04-ago-2011 
        /// MODIFICÓ: Roberto González Oseguera
        /// FECHA_MODIFICÓ: 21-oct-2011
        /// CAUSA_MODIFICACIÓN: Cambiar el periodo corriente a todo adeudo del año en curso en lugar de sólo los bimestres vencidos
        ///*******************************************************************************************************
        public DataTable Calcular_Recargos_Predial(String Cuenta_Predial)
        {
            Int32 Mes_Actual = DateTime.Now.Month;
            Int32 Anio_Actual = DateTime.Now.Year;
            Int32 Anio_Corriente;
            Decimal Total_Recargos = 0;
            Decimal Total_Rezago = 0;
            Decimal Total_Corriente = 0;
            String Periodo_Rezago_Desde = "0-0";
            String Periodo_Rezago_Hasta = "0-0";
            String Periodo_Corriente_Desde = "0-0";
            String Periodo_Corriente_Hasta = "";
            Decimal Anio;
            Int32 Anio_Tabulador;
            String Temp_Bimestre_Corriente = "0-0";
            Dictionary<String, Decimal> Dicc_Tabulador_recargos;
            var Parametro_Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
            var Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();

            Anio_Corriente = Parametro_Anio_Corriente.Consultar_Anio_Corriente();

            // tabla para adeudos
            DataTable Dt_Recargos = Formar_Tabla_Recargos();

            var Rs_Recargos_Cuentas = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
            DataTable Dt_Adeudos;

            try
            {
                // consultar adeudos de la cuenta 
                Dt_Adeudos = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial, null, 0, 0);
                if (Dt_Adeudos == null || Dt_Adeudos.Rows.Count == 0)
                {
                    if (Dt_Adeudos == null)
                    {
                        Dt_Adeudos = new DataTable();
                        Dt_Adeudos.Columns.Add("ANIO", typeof(Int32));
                        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_1", typeof(Decimal));
                        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_2", typeof(Decimal));
                        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_3", typeof(Decimal));
                        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_4", typeof(Decimal));
                        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_5", typeof(Decimal));
                        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_6", typeof(Decimal));
                    }

                    if (P_No_Orden_Variacion != null && P_Año_Orden_Variacion != null)
                    {
                        if (P_No_Orden_Variacion.Trim() != "" && P_Año_Orden_Variacion.Trim() != "")
                        {
                            DataTable Dt_Diferencias;
                            DataRow Dr_Adedudos;
                            String Periodo = "";
                            Boolean Periodo_Corriente_Validado = false;
                            Boolean Periodo_Rezago_Validado = false;
                            int Desde_Bimestre;
                            int Hasta_Bimestre;

                            Orden_Variacion.P_Cuenta_Predial_ID = Cuenta_Predial;
                            Orden_Variacion.P_Generar_Orden_No_Orden = P_No_Orden_Variacion;
                            Orden_Variacion.P_Generar_Orden_Anio = P_Año_Orden_Variacion;
                            Dt_Diferencias = Orden_Variacion.Consulta_Diferencias();
                            foreach (DataRow Dr_Diferencias in Dt_Diferencias.Rows)
                            {
                                Dr_Adedudos = Dt_Adeudos.NewRow();
                                Periodo = Obtener_Periodos_Bimestre(Dr_Diferencias["PERIODO"].ToString(), out Periodo_Corriente_Validado, out Periodo_Rezago_Validado);
                                if (Periodo.Trim() != "")
                                {
                                    Desde_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                                    Hasta_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(1));
                                    Dr_Adedudos["ANIO"] = Dr_Diferencias["PERIODO"].ToString().Substring(Dr_Diferencias["PERIODO"].ToString().Length - 4);
                                    for (int Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                    {
                                        Dr_Adedudos["ADEUDO_BIMESTRE_" + Cont_Bimestres.ToString()] = Convert.ToDecimal(Dr_Diferencias["IMPORTE"]) / (Hasta_Bimestre - Desde_Bimestre + 1);
                                    }
                                }
                                Dt_Adeudos.Rows.Add(Dr_Adedudos);
                            }
                        }
                    }
                }

                // obtener el tabulador como diccionario, si se especifica mes del tabulador en _Tabulador_Enero_Utilizar
                if (!String.IsNullOrEmpty(_Tabulador_Enero_Utilizar))
                {
                    if (!String.IsNullOrEmpty(_Anio_Tabulador_Utilizar)) // se especifica anio tabulador
                    {
                        Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(_Tabulador_Enero_Utilizar, _Anio_Tabulador_Utilizar);
                    }
                    else    // si no se especifica anio, tomar el corriente
                    {
                        Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(_Tabulador_Enero_Utilizar, Anio_Corriente.ToString());
                    }
                }
                // obtener el tabulador como diccionario, si se especifica mes del tabulador en _Mes_Tabulador_Utilizar
                else if (!String.IsNullOrEmpty(_Mes_Tabulador_Utilizar))
                {
                    if (!String.IsNullOrEmpty(_Anio_Tabulador_Utilizar)) // se especifica anio tabulador
                    {
                        Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(_Mes_Tabulador_Utilizar, _Anio_Tabulador_Utilizar);
                    }
                    else    // si no se especifica anio, tomar el corriente
                    {
                        Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(_Mes_Tabulador_Utilizar, Anio_Corriente.ToString());
                    }
                }
                // si se especifico el anio de traslado sin el mes, 
                else if (Int32.TryParse(_Anio_Tabulador_Utilizar, out Anio_Tabulador) && Anio_Tabulador > 0)
                {
                    Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Tabulador);
                }
                else        // si no se especifica mes ni anio, tomar actuales
                {
                    Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);
                }

                // para cada adeudo encontrado,
                foreach (DataRow Adeudo in Dt_Adeudos.Rows)
                {
                    // obtener el anio del adeudo
                    if (Decimal.TryParse(Adeudo[Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio))
                    {

                        // si el anio es anterior al actual, todos los bimestres son rezago
                        if (Anio < Anio_Corriente)
                        {
                            DataRow Nuevo_Recargo = Dt_Recargos.NewRow();
                            for (int i = 1; i <= 6; i++)
                            {
                                Decimal Adeudo_Bimestre;
                                Nuevo_Recargo = Dt_Recargos.NewRow();
                                Nuevo_Recargo["PERIODO"] = i.ToString() + Anio.ToString();
                                // verificar que hay tasa para calcular el bimestre
                                if (Dicc_Tabulador_recargos.ContainsKey(i.ToString() + Anio.ToString()))
                                {
                                    Nuevo_Recargo["TASA"] = Dicc_Tabulador_recargos[i.ToString() + Anio.ToString()];
                                }
                                else    // si no hay tasa para el bimestre arrojar nueva excepcion
                                {
                                    Nuevo_Recargo["TASA"] = 0;
                                    //throw new Exception("No se encontró la tasa para calcular los recargos del periodo 0" + i + "-" + Anio + "<br />");
                                }
                                Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + i.ToString()].ToString(), out Adeudo_Bimestre);
                                Nuevo_Recargo["ADEUDO"] = Adeudo_Bimestre;
                                Nuevo_Recargo["RECARGOS"] = Math.Round(Math.Round(((Decimal)Nuevo_Recargo["TASA"] * (Decimal)Nuevo_Recargo["ADEUDO"] / 100), 3), 2);
                                Nuevo_Recargo["NO_ADEUDO"] = Adeudo["NO_ADEUDO"];
                                Dt_Recargos.Rows.Add(Nuevo_Recargo);
                                Total_Rezago += Adeudo_Bimestre;
                                Total_Recargos += (Decimal)Nuevo_Recargo["RECARGOS"];
                                // si hay adeudo, escribir periodo rezago
                                if (Adeudo_Bimestre > 0 && Periodo_Rezago_Desde == "0-0")
                                {
                                    Periodo_Rezago_Desde = "0" + i + "-" + Anio;
                                }
                                Periodo_Rezago_Hasta = "0" + i + "-" + Anio;
                            }

                        }
                        // para los adeudo del anio actual, validar corriente y rezago (se considera corriente el año actual)
                        else if (Anio == Anio_Corriente)
                        {
                            int Numero_Bimestre = 1;
                            DataRow Nuevo_Recargo;

                            for (Numero_Bimestre = 1; Numero_Bimestre <= 6; Numero_Bimestre++)
                            {
                                Decimal Adeudo_Bimestre;
                                Nuevo_Recargo = Dt_Recargos.NewRow();
                                Nuevo_Recargo["PERIODO"] = Numero_Bimestre.ToString() + Anio.ToString();
                                // verificar que hay tasa para calcular el bimestre
                                if (Dicc_Tabulador_recargos.ContainsKey(Numero_Bimestre.ToString() + Anio.ToString()))
                                {
                                    Nuevo_Recargo["TASA"] = Dicc_Tabulador_recargos[Numero_Bimestre.ToString() + Anio.ToString()];
                                }
                                else    // si no hay tasa para el bimestre arrojar nueva excepcion
                                {
                                    Nuevo_Recargo["TASA"] = 0;
                                    //throw new Exception("No se encontró la tasa para calcular los recargos del periodo 0" + Numero_Bimestre + "-" + Anio + "<br />");
                                }
                                Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()].ToString(), out Adeudo_Bimestre);
                                Nuevo_Recargo["ADEUDO"] = Adeudo_Bimestre;
                                Nuevo_Recargo["RECARGOS"] = Math.Round(Math.Round((Decimal)Nuevo_Recargo["TASA"] * (Decimal)Nuevo_Recargo["ADEUDO"] / 100, 3), 2);
                                Nuevo_Recargo["NO_ADEUDO"] = Adeudo["NO_ADEUDO"];
                                Dt_Recargos.Rows.Add(Nuevo_Recargo);
                                Total_Corriente += Adeudo_Bimestre;
                                Total_Recargos += (Decimal)Nuevo_Recargo["RECARGOS"];
                                // si hay adeudo, escribir periodo corriente
                                if (Adeudo_Bimestre > 0 && Periodo_Corriente_Desde == "0-0")
                                {
                                    Periodo_Corriente_Desde = "0" + Numero_Bimestre + "-" + Anio;
                                }
                                Periodo_Corriente_Hasta = "0" + Numero_Bimestre + "-" + Anio;
                            }
                        }
                    }
                }
                // copiar montos totales en propiedades
                p_Total_Corriente = Total_Corriente;
                p_Total_Rezago = Total_Rezago;
                p_Total_Recargos_Generados = Decimal.Round(Total_Recargos, 2);
                if (Periodo_Corriente_Desde == "0-0")
                {
                    Periodo_Corriente_Desde = Temp_Bimestre_Corriente;
                }
                p_Periodo_Corriente = Periodo_Corriente_Desde + "  " + Periodo_Corriente_Hasta;
                p_Periodo_Rezago = Periodo_Rezago_Desde + "  " + Periodo_Rezago_Hasta;

                return Dt_Recargos;
            }
            catch (Exception ex)
            {
                throw new Exception("Calcular_Recargos_Predial: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Calcular_Recargos_Predial

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Calcular_Adeudos_Predial_Siguiente_Anio
        /// DESCRIPCIÓN: Obtener tabulador de recargos, adeudos de la cuenta y del rezago para calcular 
        ///             los recargos de una cuenta considerando los adeudos del siguiente anio
        /// PARÁMETROS:
        ///             1. Cuenta predial. Numero de cuenta a la que se calcularán adeudos
        ///             2. Dicc_Tabulador_Enero. Diccionario con el tabulador de recargos de enero
        ///             3. Dicc_Tabulador_Febrero. Diccionario con el tabulador de recargos de febrero
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 13-sep-2011 
        /// MODIFICÓ: Roberto González Oseguera
        /// FECHA_MODIFICÓ: 2-nov-2011
        /// CAUSA_MODIFICACIÓN: Agregar como parámetros los diccionarios de tabulador de recargos en lugar de consultarlos en cada llamada
        ///*******************************************************************************************************
        public DataTable Calcular_Adeudos_Predial_Siguiente_Anio(String Cuenta_Predial, Dictionary<String, Decimal> Dicc_Tabulador_Enero, Dictionary<String, Decimal> Dicc_Tabulador_Febrero)
        {
            Int32 Mes_Actual = DateTime.Now.Month;
            Int32 Anio_Generar = p_Anio;
            Decimal Total_Recargos = 0;
            Decimal Total_Recargos_Febrero = 0;
            Decimal Total_Rezago = 0;
            Decimal Total_Corriente = 0;
            String Periodo_Rezago_Desde = "0-0";
            String Periodo_Rezago_Hasta = "0-0";
            String Periodo_Corriente_Desde = "0-0";
            String Periodo_Corriente_Hasta = "";
            Decimal Anio;
            String Temp_Bimestre_Corriente = "0-0";

            // tabla para adeudos
            DataTable Dt_Recargos = Formar_Tabla_Recargos();

            DataTable Dt_Adeudos;

            try
            {
                // consultar adeudos de la cuenta 
                Dt_Adeudos = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial, null, 0, 0);

                // para cada adeudo encontrado,
                foreach (DataRow Adeudo in Dt_Adeudos.Rows)
                {
                    // obtener el anio del adeudo
                    if (Decimal.TryParse(Adeudo[Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio))
                    {

                        // si el anio es anterior al actual, todos los bimestres son rezago
                        if (Anio < Anio_Generar)
                        {
                            DataRow Nuevo_Recargo = Dt_Recargos.NewRow();
                            for (int i = 1; i <= 6; i++)
                            {
                                Decimal Adeudo_Bimestre;
                                Nuevo_Recargo = Dt_Recargos.NewRow();
                                Nuevo_Recargo["PERIODO"] = i.ToString() + Anio.ToString();
                                // verificar que hay tasa para calcular el bimestre en el tabulador de ENERO
                                if (Dicc_Tabulador_Enero.ContainsKey(i.ToString() + Anio.ToString()))
                                {
                                    Nuevo_Recargo["TASA"] = Dicc_Tabulador_Enero[i.ToString() + Anio.ToString()];
                                }
                                else    // si no hay tasa para el bimestre arrojar nueva excepcion
                                {
                                    Nuevo_Recargo["TASA"] = 0;
                                    //throw new Exception("No se encontró la tasa para calcular los recargos del periodo 0" + i + "-" + Anio + "<br />");
                                }
                                // obtener adeudo del bimestre
                                Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + i.ToString()].ToString(), out Adeudo_Bimestre);
                                // calcular recargos con el tabulador de febrero
                                if (Dicc_Tabulador_Febrero.ContainsKey(i.ToString() + Anio.ToString()))
                                {
                                    Total_Recargos_Febrero += Math.Round((Dicc_Tabulador_Febrero[i.ToString() + Anio.ToString()] * Adeudo_Bimestre / 100M), 2, MidpointRounding.AwayFromZero);
                                }
                                Nuevo_Recargo["ADEUDO"] = Adeudo_Bimestre;
                                Nuevo_Recargo["RECARGOS"] = Math.Round(((decimal)Nuevo_Recargo["TASA"] * (decimal)Nuevo_Recargo["ADEUDO"] / 100M), 2, MidpointRounding.AwayFromZero);
                                Dt_Recargos.Rows.Add(Nuevo_Recargo);
                                Total_Rezago += Adeudo_Bimestre;
                                Total_Recargos += (decimal)Nuevo_Recargo["RECARGOS"];
                                // si hay adeudo, escribir periodo rezago
                                if (Adeudo_Bimestre > 0 && Periodo_Rezago_Desde == "0-0")
                                {
                                    Periodo_Rezago_Desde = "0" + i + "-" + Anio;
                                }
                                Periodo_Rezago_Hasta = "0" + i + "-" + Anio;
                            }

                        }
                        // para los adeudo del anio siguiente, solo corriente, sumar, sin generar entrada en la tabla
                        else
                        {
                            Periodo_Corriente_Desde = "01-" + Anio;
                            for (int Numero_Bimestre = 1; Numero_Bimestre <= 6; Numero_Bimestre++)
                            {
                                Decimal Adeudo_Bimestre;
                                Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()].ToString(), out Adeudo_Bimestre);
                                if (Adeudo_Bimestre > 0)
                                {
                                    // solo sumar el corriente
                                    Total_Corriente += Adeudo_Bimestre;
                                    // si hay adeudo, escribir periodo 
                                    Periodo_Corriente_Hasta = "0" + Numero_Bimestre + "-" + Anio;
                                }
                            }
                        }
                    }
                }
                // copiar montos totales en propiedades
                p_Total_Corriente = Total_Corriente;
                p_Total_Rezago = Total_Rezago;
                p_Total_Recargos_Generados = Total_Recargos;
                p_Total_Recargos_Generados_Febrero = Total_Recargos_Febrero;
                if (Periodo_Corriente_Desde == "0-0")
                {
                    Periodo_Corriente_Desde = Temp_Bimestre_Corriente;
                }
                p_Periodo_Corriente = Periodo_Corriente_Desde + "  " + Periodo_Corriente_Hasta;
                p_Periodo_Rezago = Periodo_Rezago_Desde + "  " + Periodo_Rezago_Hasta;

                return Dt_Adeudos;
            }
            catch (Exception ex)
            {
                throw new Exception("Calcular_Adeudos_Predial_Siguiente_Anio: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Calcular_Adeudos_Predial_Siguiente_Anio

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Tabulador_Recargos_Enero_Febrero
        /// DESCRIPCIÓN: Obtener tabulador de recargos de enero y febrero si se especificaron en las propiedades
        /// PARÁMETROS:
        ///             1. Cuenta predial. Numero de cuenta a la que se calcularán adeudos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 13-sep-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public void Obtener_Tabulador_Recargos_Enero_Febrero(out Dictionary<String, Decimal> Dicc_Tabulador_Enero, out Dictionary<String, Decimal> Dicc_Tabulador_Febrero)
        {
            Int32 Mes_Actual = DateTime.Now.Month;
            Int32 Anio_Actual = DateTime.Now.Year;
            Int32 Anio_Tabulador;
            Cls_Cat_Pre_Tabulador_Recargos_Negocio Consultar_Tabulador = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();

            try
            {
                // si se especifico tabulador paro ENERO, obtener el tabulador como diccionario
                if (!String.IsNullOrEmpty(_Tabulador_Enero_Utilizar))
                {
                    if (!String.IsNullOrEmpty(_Anio_Tabulador_Utilizar)) // se especifica anio tabulador
                    {
                        Dicc_Tabulador_Enero = Consultar_Tabulador.Consultar_Tabulador_Recargos_Diccionario(_Tabulador_Enero_Utilizar, _Anio_Tabulador_Utilizar);
                    }
                    else    // si no se especifica anio, tomar el actual
                    {
                        Dicc_Tabulador_Enero = Consultar_Tabulador.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);
                    }
                }
                // si se especifico el anio de traslado sin el mes, 
                else if (Int32.TryParse(_Anio_Tabulador_Utilizar, out Anio_Tabulador) && Anio_Tabulador > 0)
                {
                    Dicc_Tabulador_Enero = Consultar_Tabulador.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Tabulador);
                }
                else        // si no se especifica mes ni anio, tomar actuales
                {
                    Dicc_Tabulador_Enero = Consultar_Tabulador.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);
                }

                // si se especifico tabulador paro FEBRERO, obtener el tabulador como diccionario
                if (!String.IsNullOrEmpty(_Tabulador_Febrero_Utilizar))
                {
                    if (!String.IsNullOrEmpty(_Anio_Tabulador_Utilizar)) // se especifica anio tabulador
                    {
                        Dicc_Tabulador_Febrero = Consultar_Tabulador.Consultar_Tabulador_Recargos_Diccionario(_Tabulador_Febrero_Utilizar, _Anio_Tabulador_Utilizar);
                    }
                    else    // si no se especifica anio, tomar el actual
                    {
                        Dicc_Tabulador_Febrero = Consultar_Tabulador.Consultar_Tabulador_Recargos_Diccionario(DateTime.Now.Month, Anio_Actual);
                    }
                }
                // si se especifico el anio de traslado sin el mes, 
                else if (Int32.TryParse(_Anio_Tabulador_Utilizar, out Anio_Tabulador) && Anio_Tabulador > 0)
                {
                    Dicc_Tabulador_Febrero = Consultar_Tabulador.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Tabulador);
                }
                else        // si no se especifica mes ni anio, tomar actuales
                {
                    Dicc_Tabulador_Febrero = Consultar_Tabulador.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Obtener_Tabulador_Recargos_Enero_Febrero: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Obtener_Tabulador_Recargos_Enero_Febrero

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Calcular_Recargos_Predial_Hasta_Bimestre
        /// DESCRIPCIÓN: Obtener tabulador de recargos, adeudos de la cuenta y del rezago para calcular 
        ///             los recargos de una cuenta desde el adeudo mas antiguo hasta un bimestre dado
        /// PARÁMETROS:
        ///             1. Cuenta predial. Numero de cuenta a la que se calcularán adeudos
        ///             2. Hasta_Anio. Anio del periodo final para el calculo
        ///             3. Hasta_Bimestre. Bimestre del periodo final para el calculo
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 08-ago-2011 
        /// MODIFICÓ: Roberto González Oseguera
        /// FECHA_MODIFICÓ: 21-oct-2011
        /// CAUSA_MODIFICACIÓN: Cambiar el periodo corriente a todo adeudo del año en curso en lugar de sólo los vencidos
        ///*******************************************************************************************************
        public DataTable Calcular_Recargos_Predial_Hasta_Bimestre(String Cuenta_Predial, Int32 Hastas_Anio, Int32 Hasta_Bimestre)
        {
            Int32 Mes_Actual = DateTime.Now.Month;
            Int32 Anio_Actual = DateTime.Now.Year;
            Int32 Anio_Corriente;
            Decimal Total_Recargos = 0;
            Decimal Total_Rezago = 0;
            Decimal Total_Corriente = 0;
            String Periodo_Rezago_Desde = "-";
            String Periodo_Corriente_Desde = "-";
            String Periodo_Rezago_Hasta = "-";
            String Periodo_Corriente_Hasta = "-";
            Decimal Anio;
            Int32 Anio_Tabulador = 0;
            Dictionary<String, Decimal> Dicc_Tabulador_recargos = new Dictionary<String, Decimal>();

            Cls_Ope_Pre_Parametros_Negocio Parametro_Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
            Anio_Corriente = Parametro_Anio_Corriente.Consultar_Anio_Corriente();

            // tabla para adeudos
            DataTable Dt_Recargos = Formar_Tabla_Recargos();

            Cls_Cat_Pre_Tabulador_Recargos_Negocio Rs_Recargos_Cuentas = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
            DataTable Dt_Adeudos;

            //try
            //{
            // consultar adeudos de la cuenta
            Dt_Adeudos = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial, null, 0, Hastas_Anio);

            // obtener el tabulador como diccionario, si se especifica mes del tabulador
            if (!String.IsNullOrEmpty(_Mes_Tabulador_Utilizar))
            {
                if (!String.IsNullOrEmpty(_Anio_Tabulador_Utilizar)) // se especifica anio tabulador
                {
                    Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(_Mes_Tabulador_Utilizar, _Anio_Tabulador_Utilizar);
                }
                else    // si no se especifica anio, tomar el actual
                {
                    Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(DateTime.Now.Month, Anio_Corriente);
                }
            }
            // si se especifico el anio de traslado sin el mes, 
            else if (Int32.TryParse(_Anio_Tabulador_Utilizar, out Anio_Tabulador) && Anio_Tabulador > 0)
            {
                Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Tabulador);
            }
            else        // si no se especifica mes ni anio, tomar actuales
            {
                Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);
            }
            // para cada adeudo encontrado,
            foreach (DataRow Adeudo in Dt_Adeudos.Rows)
            {
                // obtener el anio del adeudo
                if (Decimal.TryParse(Adeudo[Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio))
                {
                    // si el anio es anterior al actual, todos los bimestres son rezago
                    if (Anio < Anio_Corriente)
                    {
                        DataRow Nuevo_Recargo = Dt_Recargos.NewRow();
                        for (int i = 1; i <= 6; i++)
                        {
                            Decimal Adeudo_Bimestre;
                            Nuevo_Recargo = Dt_Recargos.NewRow();
                            Nuevo_Recargo["PERIODO"] = i.ToString() + Anio.ToString();
                            // verificar que hay tasa para calcular el bimestre
                            if (Dicc_Tabulador_recargos.ContainsKey(i.ToString() + Anio.ToString()))
                            {
                                Nuevo_Recargo["TASA"] = Dicc_Tabulador_recargos[i.ToString() + Anio.ToString()];
                            }
                            else    // si no hay tasa para el bimestre arrojar nueva excepcion
                            {
                                Nuevo_Recargo["TASA"] = 0;
                                //throw new Exception("No se encontró la tasa para calcular los recargos del periodo 0" + i + "-" + Anio + "<br />");
                            }
                            Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + i.ToString()].ToString(), out Adeudo_Bimestre);
                            Nuevo_Recargo["ADEUDO"] = Adeudo_Bimestre;
                            Nuevo_Recargo["RECARGOS"] = Math.Round(Math.Round(((Decimal)Nuevo_Recargo["TASA"] * (Decimal)Nuevo_Recargo["ADEUDO"] / 100), 3), 2);
                            Dt_Recargos.Rows.Add(Nuevo_Recargo);
                            Total_Rezago += (Decimal)Nuevo_Recargo["ADEUDO"];
                            Total_Recargos += (Decimal)Nuevo_Recargo["RECARGOS"];
                            // si hay adeudo, escribir periodo rezago
                            if (Adeudo_Bimestre > 0 && Periodo_Rezago_Desde == "-")
                            {
                                Periodo_Rezago_Desde = "0" + i + "-" + Anio;
                            }
                            Periodo_Rezago_Hasta = "0" + i + "-" + Anio;
                            // al llegar al anio y bimestre especificado como parametro (Hasta_Anio, Hasta_Bimestre), salir del bucle
                            if (Anio == Hastas_Anio && i == Hasta_Bimestre)
                            {
                                break;
                            }
                        }

                    }
                    // para los adeudo del anio actual (periodo corriente)
                    else if (Anio == Anio_Corriente)
                    {
                        int Numero_Bimestre = 1;
                        DataRow Nuevo_Recargo;

                        for (Numero_Bimestre = 1; Numero_Bimestre <= 6; Numero_Bimestre++)
                        {
                            Decimal Adeudo_Bimestre;
                            Nuevo_Recargo = Dt_Recargos.NewRow();
                            Nuevo_Recargo["PERIODO"] = Numero_Bimestre.ToString() + Anio.ToString();
                            // verificar que hay tasa para calcular el bimestre
                            if (Dicc_Tabulador_recargos.ContainsKey(Numero_Bimestre.ToString() + Anio.ToString()))
                            {
                                Nuevo_Recargo["TASA"] = Dicc_Tabulador_recargos[Numero_Bimestre.ToString() + Anio.ToString()];
                            }
                            else    // si no hay tasa para el bimestre arrojar nueva excepcion
                            {
                                Nuevo_Recargo["TASA"] = 0;
                                //throw new Exception("No se encontró la tasa para calcular los recargos del periodo 0" + Numero_Bimestre + "-" + Anio + "<br />");
                            }
                            Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()].ToString(), out Adeudo_Bimestre);
                            Nuevo_Recargo["ADEUDO"] = Adeudo_Bimestre;
                            Nuevo_Recargo["RECARGOS"] = Math.Round(Math.Round((Decimal)Nuevo_Recargo["TASA"] * (Decimal)Nuevo_Recargo["ADEUDO"] / 100, 3), 2);
                            Dt_Recargos.Rows.Add(Nuevo_Recargo);
                            Total_Corriente += Adeudo_Bimestre;
                            Total_Recargos += (Decimal)Nuevo_Recargo["RECARGOS"];
                            // si hay adeudo, escribir periodo corriente
                            if (Adeudo_Bimestre > 0 && Periodo_Corriente_Desde == "-")
                            {
                                Periodo_Corriente_Desde = "0" + Numero_Bimestre + "-" + Anio;
                            }
                            Periodo_Corriente_Hasta = "0" + Numero_Bimestre + "-" + Anio;
                            // al llegar al anio y bimestre especificado como parametro (Hasta_Anio, Hasta_Bimestre), salir del bucle
                            if (Anio == Hastas_Anio && Numero_Bimestre == Hasta_Bimestre)
                            {
                                break;
                            }
                        }

                        //// recorrer los siguientes adeudos (impuesto predial corriente)
                        //while (Numero_Bimestre <= 6 && Bnd_Hasta_Bimestre == false)
                        //{
                        //    Decimal Adeudo_Bimestre;
                        //    // entradas en la tabla del adeudo corriente
                        //    Nuevo_Recargo = Dt_Recargos.NewRow();
                        //    Nuevo_Recargo["PERIODO"] = Numero_Bimestre.ToString() + Anio.ToString();
                        //    Nuevo_Recargo["TASA"] = 0;
                        //    Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()].ToString(), out Adeudo_Bimestre);
                        //    Nuevo_Recargo["ADEUDO"] = Adeudo_Bimestre;
                        //    Nuevo_Recargo["RECARGOS"] = 0;
                        //    Dt_Recargos.Rows.Add(Nuevo_Recargo);

                        //    // sumar el corriente
                        //    if (Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()].ToString().Trim() != "")
                        //    {
                        //        Total_Corriente += (Decimal)Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()];
                        //    }
                        //    // si hay adeudo, escribir periodo rezago
                        //    if (Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()].ToString ().Trim ()!="")
                        //    {
                        //        if ((Decimal)Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()] > 0 && Periodo_Corriente_Desde == "-")
                        //        {
                        //            Periodo_Corriente_Desde = "0" + Numero_Bimestre + "-" + Anio;
                        //        }
                        //    }
                        //    Periodo_Corriente_Hasta = "0" + Numero_Bimestre + "-" + Anio;
                        //    // al llegar al anio y bimestre especificado como parametro (Hasta_Anio, Hasta_Bimestre), salir del bucle
                        //    if (Anio == Hastas_Anio && Numero_Bimestre == Hasta_Bimestre)
                        //    {
                        //        break;
                        //    }
                        //    Numero_Bimestre++;
                        //}
                    }
                }
            }

            // copiar montos totales en propiedades
            p_Total_Corriente = Total_Corriente;
            p_Total_Rezago = Total_Rezago;
            p_Total_Recargos_Generados = Decimal.Round(Total_Recargos, 2);
            p_Periodo_Corriente = Periodo_Corriente_Desde + "  " + Periodo_Corriente_Hasta;
            p_Periodo_Rezago = Periodo_Rezago_Desde + "  " + Periodo_Rezago_Hasta;

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Calcular_Recargos_Predial_Hasta_Bimestre: " + ex.Message.ToString(), ex);
            //}
            return Dt_Recargos;
        }// termina metodo Generar_Impuesto_Cierre_Anual


        public DataTable Consultar_Cuentas_Exencion_Vigente(String Estatus, String Fecha_Vigencia)
        {
            return Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Cuentas_Exencion_Vigente(Estatus, Fecha_Vigencia);
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Recargos_Predial
        /// DESCRIPCIÓN: Generar los recargos para una cuenta dada, tomando los
        /// PARÁMETROS:
        ///             1. Cuenta predial. Numero de cuenta a la que se calcularán adeudos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 04-ago-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Generar_Recargos_Predial(String Cuenta_Predial, out Decimal Total_Recargos, out Decimal Total_Rezago, Int32 Anio_Corriente)
        {
            Int32 Mes_Actual = DateTime.Now.Month;
            Int32 Anio_Actual = DateTime.Now.Year;
            Int32 Anio_Generar;
            Total_Recargos = 0;
            Total_Rezago = 0;
            Int32 Anio;
            Int32 Anio_Tabulador = 0;
            Decimal Tasa;

            DataTable Dt_Adeudos;

            Anio_Generar = Anio_Corriente > 0 ? Anio_Corriente : Anio_Actual;

            // si el diccionario con el tabulador de recargos no contiene datos, llenarlo
            if (Gl_Dicc_Tab_Recargos.Count <= 0)
            {
                Cls_Cat_Pre_Tabulador_Recargos_Negocio Rs_Recargos_Cuentas = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
                // obtener el tabulador como diccionario, si se especifica mes del tabulador
                if (!String.IsNullOrEmpty(_Mes_Tabulador_Utilizar))
                {
                    if (!String.IsNullOrEmpty(_Anio_Tabulador_Utilizar)) // se especifica anio tabulador
                    {
                        Gl_Dicc_Tab_Recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(_Mes_Tabulador_Utilizar, _Anio_Tabulador_Utilizar);
                    }
                    else    // si no se especifica anio, tomar el actual
                    {
                        Gl_Dicc_Tab_Recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(DateTime.Now.Month, Anio_Actual);
                    }
                }
                // si se especifico el anio de traslado sin el mes, 
                else if (Int32.TryParse(_Anio_Tabulador_Utilizar, out Anio_Tabulador) && Anio_Tabulador > 0)
                {
                    Gl_Dicc_Tab_Recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Tabulador);
                }
                else        // si no se especifica mes ni anio, tomar actuales
                {
                    Gl_Dicc_Tab_Recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);
                }
            }

            try
            {
                // consultar adeudos de la cuenta con estatus por pagar
                Dt_Adeudos = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial, null, 0, 0);


                // para cada adeudo encontrado,
                foreach (DataRow Adeudo in Dt_Adeudos.Rows)
                {
                    // obtener el anio del adeudo
                    if (Int32.TryParse(Adeudo[Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio))
                    {

                        // si el anio es anterior al actual, todos los bimestres son rezago
                        if (Anio < Anio_Generar)
                        {
                            for (int i = 1; i <= 6; i++)
                            {
                                Decimal Adeudo_Bimestre;
                                Decimal Recargo;
                                // verificar que hay tasa para calcular el bimestre
                                if (Gl_Dicc_Tab_Recargos.ContainsKey(i.ToString() + Anio.ToString()))
                                {
                                    Tasa = Gl_Dicc_Tab_Recargos[i.ToString() + Anio.ToString()];
                                }
                                else    // si no hay tasa para el bimestre arrojar nueva excepcion
                                {
                                    Tasa = 0;
                                    //return "No se encontró la tasa para calcular los recargos del periodo 0" + i + "-" + Anio + "<br />";
                                }
                                Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + i.ToString()].ToString(), out Adeudo_Bimestre);
                                Recargo = Decimal.Round(Tasa * Adeudo_Bimestre / 100, 2);
                                Total_Rezago += Adeudo_Bimestre;
                                Total_Recargos += Recargo;
                            }

                        }
                        // para los adeudo del anio actual, validar corriente y rezago
                        else if (Anio == Anio_Generar)
                        {
                            int Numero_Bimestre = 1;
                            int Bimestre_Rezago = Mes_Actual / 2;
                            Decimal Recargo;

                            for (Numero_Bimestre = 1; Numero_Bimestre <= Bimestre_Rezago; Numero_Bimestre++)
                            {
                                Decimal Adeudo_Bimestre;
                                // verificar que hay tasa para calcular el bimestre
                                if (Gl_Dicc_Tab_Recargos.ContainsKey(Numero_Bimestre.ToString() + Anio.ToString()))
                                {
                                    Tasa = Gl_Dicc_Tab_Recargos[Numero_Bimestre.ToString() + Anio.ToString()];
                                }
                                else    // si no hay tasa para el bimestre regresar mensaje
                                {
                                    Tasa = 0;
                                    //return "No se encontró la tasa para calcular los recargos del periodo 0" + Numero_Bimestre + "-" + Anio + "<br />";
                                }

                                Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()].ToString(), out Adeudo_Bimestre);
                                Recargo = Decimal.Round(Tasa * Adeudo_Bimestre / 100, 2);
                                Total_Rezago += Adeudo_Bimestre;
                                Total_Recargos += Recargo;
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception("Calcular_Recargos_Predial: " + ex.Message.ToString(), ex);
            }
        }// termina metodo Generar_Recargos_Predial

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Archivo_Adeudos
        /// DESCRIPCIÓN: Generar archivo xls con adeudos de cuentas
        /// PARÁMETROS:
        /// 		1. Orden: Filtros para el orden en la consulta de las cuentas
        /// 		2. Tipo_Predio: Tipo de predio a incluir en el archivo
        /// 		3. Foraneo: (SI, NO) Indica si se incluyen predios con domicilio de notificacion foraneo
        /// 		4. Nombre_Archivo: Nombre del archivo a generar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 07-sep-2011
        /// MODIFICÓ: Roberto González Oseguera
        /// FECHA_MODIFICÓ: 10-oct-2011
        /// CAUSA_MODIFICACIÓN: Cambio de librería a utilizar para la generación de archivo de Excel
        ///*******************************************************************************************************
        public String Generar_Archivo_Adeudos(String Orden, String Tipo_Predio, String Foraneo, String Nombre_Archivo)
        {
            Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

            DataTable Dt_Datos_Cuentas;
            String Mensaje_Error = "";
            // parametros
            String Usuario_Creo = Sessiones.Cls_Sessiones.Nombre_Empleado;
            String Filtros_Dinamicos = "";
            DateTime Fecha_Limite_Enero;
            DateTime Fecha_Limite_Febrero;
            Decimal Descuento_PP_Enero = 0;
            Decimal Descuento_PP_Febrero = 0;
            DataTable Dt_Archivo_Adeudos;
            DataTable Dt_Lineas_Captura;
            DataRow Dr_Renglon_Cuenta;
            String Ruta_Archivo = "";
            String Ultimo_Movimiento_Cuenta = "";
            Int32 Anio_Generacion_Int = 0;
            String Anio_Generacion = "";

            DataRow Dr_Linea_Captura;
            string LC_Enero = "";
            string LC_Febrero = "";
            string Meses = "";

            if (p_Anio > 0)
            {
                Anio_Generacion_Int = p_Anio;
                Anio_Generacion = Anio_Generacion_Int.ToString();
            }
            else
            {
                Anio_Generacion_Int = (DateTime.Now.Year + 1);
                Anio_Generacion = Anio_Generacion_Int.ToString();
            }

            // obtener ultimo dia habil de enero y febrero
            Fecha_Limite_Enero = Obtener_Ultimo_Dia_Habil("01/feb/" + Anio_Generacion);
            Fecha_Limite_Febrero = Obtener_Ultimo_Dia_Habil("01/mar/" + Anio_Generacion);

            // obtener las instituciones con linea de captura y los convenios de las instituciones
            Dictionary<String, String> Dic_Convenios;
            Dictionary<String, String> Dic_Institucion_ID;
            Dictionary<String, String> Dic_Instituciones = Obtener_Instituciones_Diccionario(out Dic_Convenios, out Dic_Institucion_ID);
            Dictionary<String, String> Dic_Ultimos_Movimientos;
            // obtener tabulador de recargos
            Dictionary<String, Decimal> Dicc_Tabulador_Enero;
            Dictionary<String, Decimal> Dicc_Tabulador_Febrero;
            Obtener_Tabulador_Recargos_Enero_Febrero(out Dicc_Tabulador_Enero, out Dicc_Tabulador_Febrero);
            // obtener decuentos del catalogo de descuentos
            Obtener_Descuentos_Pronto_Pago(out Descuento_PP_Enero, out Descuento_PP_Febrero, Anio_Generacion_Int);

            p_Total_Padron = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Total_Cuentas(null, null, null);

            // si no hay cuota minima, obtenerla
            if (p_Cuota_Minima <= 0)
                p_Cuota_Minima = Obtener_Cuota_Minima(Anio_Generacion_Int);

            try
            {
                // si el estatus no contiene valores, agregar valores por defecto
                if (String.IsNullOrEmpty(p_Estatus_Excluir))
                {
                    p_Estatus_Excluir = " NOT IN ('CANCELADA','TEMPORAL','BAJA','PENDIENTE') ";
                    p_Tipo_Suspension_Excluir = " IN ('AMBAS', 'PREDIAL') ";
                }
                // establecer el domicilio foraneo en los filtros dinamicos
                if (Foraneo != "SI")
                {
                    Filtros_Dinamicos = " NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", 'NO') != 'SI' ";
                }
                else
                {
                    Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " = 'SI' ";
                }
                if (!String.IsNullOrEmpty(Tipo_Predio))
                {
                    Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = '" + Tipo_Predio + "'   ";
                }
                if (!String.IsNullOrEmpty(Orden))
                {
                    Filtros_Dinamicos += " ORDER BY " + Orden;
                }

                // obtener datos de cuentas a considerar
                Dt_Datos_Cuentas = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Datos_Cuentas(p_Estatus_Excluir, Filtros_Dinamicos);

                // obtener los ultimos movimientos de las cuentas
                Dic_Ultimos_Movimientos = Obtener_Ultimos_Movimientos();

                // si no se recibieron datos abandonar el metodo con mensaje de error
                if (Dt_Datos_Cuentas.Rows.Count <= 0)
                {
                    return "Error al tratar de leer los datos de las cuentas.<br />";
                }

                Ruta_Archivo = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);

                Dt_Archivo_Adeudos = Generar_Tabla_Archivo_Adeudos(Dic_Instituciones);
                Dt_Lineas_Captura = Formar_Tabla_Lineas_Captura();

                // darle nombre a la tabla (se asigna a la pestaña del libro de excel)
                if (Nombre_Archivo.Contains("Urbano"))
                {
                    Dt_Archivo_Adeudos.TableName = "ADEUDOS_URBANOS_" + Anio_Generacion;
                }
                else if (Nombre_Archivo.Contains("Rustico"))
                {
                    Dt_Archivo_Adeudos.TableName = "ADEUDOS_RUSTICOS_" + Anio_Generacion;
                }
                else if (Nombre_Archivo.Contains("Foraneo"))
                {
                    Dt_Archivo_Adeudos.TableName = "ADEUDOS_FORANEOS_" + Anio_Generacion;
                }
                else
                {
                    Dt_Archivo_Adeudos.TableName = "ADEUDOS_" + Anio_Generacion;
                }
                
                // insertar los datos para cada cuenta de la consulta
                foreach (DataRow Cuenta in Dt_Datos_Cuentas.Rows)
                {
                    // ultimo movimiento de la cuenta
                    if (Dic_Ultimos_Movimientos.ContainsKey(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString()))
                    {
                        Ultimo_Movimiento_Cuenta = Dic_Ultimos_Movimientos[Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString()];
                    }
                    else
                    {
                        Ultimo_Movimiento_Cuenta = "";
                    }

                    // consultar adeudos de la cuenta
                    DataTable Dt_Adeudos;
                    Dt_Adeudos = Calcular_Adeudos_Predial_Siguiente_Anio(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString(), Dicc_Tabulador_Enero, Dicc_Tabulador_Febrero);
                    // validar que la cuenta tiene adeudo
                    if (Dt_Adeudos.Rows.Count > 0)
                    {
                        //String[] Periodo_Corriente = _Periodo_Corriente.Split(' ');
                        String[] Periodo_Rezago = _Periodo_Rezago.Split(' ');
                        String Desde_Periodo;
                        String Hasta_Periodo;
                        String Periodo_Corriente_Inicial;
                        String Periodo_Corriente_Final;
                        String Periodo_Rezago_Inicial;
                        String Periodo_Rezago_Final;
                        Decimal Monto_Adeudo_Rezago = p_Total_Rezago;
                        Decimal Monto_Adeudo_Corriente = p_Total_Corriente;
                        Decimal Monto_Impuesto_Predial = 0;
                        Decimal Monto_Impuesto_Predial_Febrero = 0;
                        Decimal Descuento_Enero = 0;
                        Decimal Descuento_Febrero = 0;
                        Decimal Cantidad = 0;
                        Decimal Cuota_Bimestral = 0;
                        String Linea_Captura;

                        // obtener periodos adeudo corriente
                        Periodo_Corriente_Inicial = "01/" + Anio_Generacion;
                        Periodo_Corriente_Final = "06/" + Anio_Generacion;
                        // separar periodo corriente si hay adeudo corriente
                        //if (_Total_Corriente > 0 && Periodo_Corriente.Length >= 3)
                        //{
                        //    Periodo_Corriente_Inicial = Periodo_Corriente[0].Replace("-", "/");
                        //    Periodo_Corriente_Final = Periodo_Corriente[2].Replace("-", "/");
                        //}
                        // separar periodo rezago si hay adeudo rezago
                        if (_Total_Rezago > 0 && Periodo_Rezago.Length >= 3)
                        {
                            Periodo_Rezago_Inicial = Periodo_Rezago[0].Replace("-", "/");
                            Periodo_Rezago_Final = Periodo_Rezago[2].Replace("-", "/");
                        }
                        else
                        {
                            Periodo_Rezago_Inicial = "";
                            Periodo_Rezago_Final = "";
                        }
                        // si hay periodo rezago, tomar como periodo inicial, si no, tomar el periodo corriente
                        if (Periodo_Rezago_Inicial != "")
                        {
                            Desde_Periodo = Periodo_Rezago_Inicial.Substring(3, Periodo_Rezago_Inicial.Length - 3)
                                + Periodo_Rezago_Inicial.Substring(1, 1);
                        }
                        else
                        {
                            Desde_Periodo = Periodo_Corriente_Inicial.Substring(3, Periodo_Corriente_Inicial.Length - 3)
                                 + Periodo_Corriente_Inicial.Substring(1, 1);
                        }
                        // si hay adeudo corriente
                        if (_Total_Corriente > 0)
                        {
                            Hasta_Periodo = Periodo_Corriente_Final.Substring(3, Periodo_Corriente_Final.Length - 3)
                                + Periodo_Corriente_Final.Substring(1, 1);
                        }
                        else
                        {
                            Mensaje_Error += Cuenta["CUENTA_PREDIAL"].ToString() + " la cuenta no tiene adeudos, ";
                            continue;
                        }

                        // calcular montos descuentos
                        Monto_Impuesto_Predial = Math.Round(Math.Round(Monto_Adeudo_Corriente + Monto_Adeudo_Rezago + _Total_Recargos_Generados, 3), 2);
                        Monto_Impuesto_Predial_Febrero = Math.Round(Math.Round(Monto_Adeudo_Corriente + Monto_Adeudo_Rezago + _Total_Recargos_Generados_Febrero, 3), 2);
                        if (Monto_Adeudo_Corriente > p_Cuota_Minima)
                        {
                            Descuento_Enero = Math.Round(Math.Round(Monto_Adeudo_Corriente * (Descuento_PP_Enero / 100), 3), 2);
                            Descuento_Febrero = Math.Round(Math.Round(Monto_Adeudo_Corriente * (Descuento_PP_Febrero / 100), 3), 2);
                            // sumatoria de descuentos
                        }
                        else
                        {
                            Periodo_Corriente_Final = "6/" + Anio_Generacion;
                            _Total_Cuotas_Minimas++;
                        }
                        // cuota bimestral
                        if (Decimal.TryParse(Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString(), out Cuota_Bimestral)
                            && Cuota_Bimestral > 0)
                        {
                            Cuota_Bimestral = Cuota_Bimestral / 6;
                        }
                        // redondear a dos decimales
                        Math.Round(Math.Round(_Total_Corriente, 3), 2);
                        Math.Round(Math.Round(_Total_Rezago, 3), 2);
                        Math.Round(Math.Round(_Total_Recargos_Generados, 3), 2);
                        Math.Round(Math.Round(_Total_Recargos_Generados_Febrero, 3), 2);

                        Dr_Renglon_Cuenta = Dt_Archivo_Adeudos.NewRow();
                        // insertar valores en las celdas

                        Dr_Renglon_Cuenta[0] = Cuenta["CUENTA_PREDIAL"].ToString();
                        Dr_Renglon_Cuenta[1] = Cuenta["PROPIETARIO_CTA"].ToString();
                        Dr_Renglon_Cuenta[2] = Cuenta["NOMBRE_CALLE"].ToString();
                        Dr_Renglon_Cuenta[3] = Cuenta["NO_EXTERIOR"].ToString();
                        Dr_Renglon_Cuenta[4] = Cuenta["NO_INTERIOR"].ToString();
                        Dr_Renglon_Cuenta[5] = Cuenta["COLONIA_UBICACION"].ToString();
                        Dr_Renglon_Cuenta[6] = "IRAPUATO";
                        Dr_Renglon_Cuenta[7] = "GUANAJUATO";
                        Dr_Renglon_Cuenta[8] = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() == "NO" ?
                            Cuenta["CALLE_LOCAL_NOTIFICACION"].ToString()
                            : Cuenta["CALLE_NOTIFICACION"].ToString();
                        Dr_Renglon_Cuenta[9] = Cuenta["NO_EXTERIOR_NOTIFICACION"].ToString();
                        Dr_Renglon_Cuenta[10] = Cuenta["NO_INTERIOR_NOTIFICACION"].ToString();
                        Dr_Renglon_Cuenta[11] = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() == "NO" ?
                            Cuenta["COLONIA_LOCAL_NOTIFICACION"].ToString()
                            : Cuenta["COLONIA_NOTIFICACION"].ToString();
                        Dr_Renglon_Cuenta[12] = Cuenta["CIUDAD_NOTIFICACION"].ToString();
                        Dr_Renglon_Cuenta[13] = Cuenta["ESTADO_NOTIFICACION"].ToString();
                        Dr_Renglon_Cuenta[14] = Ultimo_Movimiento_Cuenta;
                        Dr_Renglon_Cuenta[15] = Cuenta["EFECTOS"].ToString();
                        Dr_Renglon_Cuenta[16] = Cuenta["IMPUESTO_PREDIAL"].ToString(); ;
                        Decimal.TryParse(Cuenta["VALOR_FISCAL"].ToString(), out Cantidad);
                        Dr_Renglon_Cuenta[17] = Cantidad.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[18] = Cuota_Bimestral.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[19] = Periodo_Rezago_Inicial + "-" + Periodo_Rezago_Final;
                        Dr_Renglon_Cuenta[20] = _Total_Rezago.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[21] = Periodo_Corriente_Inicial + "-" + Periodo_Corriente_Final;
                        Dr_Renglon_Cuenta[22] = _Total_Corriente.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[23] = _Total_Recargos_Generados.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[24] = _Total_Recargos_Generados_Febrero.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[25] = "0";
                        Dr_Renglon_Cuenta[26] = Descuento_Enero.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[27] = Descuento_Febrero.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[28] = Monto_Impuesto_Predial.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[29] = Monto_Impuesto_Predial_Febrero.ToString("#,##0.00");
                        Dr_Renglon_Cuenta[30] = (Monto_Impuesto_Predial - Descuento_Enero).ToString("#,##0.00");
                        Dr_Renglon_Cuenta[31] = (Monto_Impuesto_Predial_Febrero - Descuento_Febrero).ToString("#,##0.00");
                        // lineas de captura para casa institucion en el diccionario de instituciones
                        foreach (KeyValuePair<String, String> Instituciones in Dic_Instituciones)
                        {
                            Linea_Captura = "";
                            switch (Instituciones.Value)
                            {
                                case "ACREMEX":
                                    if (Instituciones.Key.EndsWith("ENERO"))
                                    {
                                        Linea_Captura = Linea_Captura_ACREMEX(
                                            "01", Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Desde_Periodo,
                                            Hasta_Periodo,
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", "").PadLeft(11, '0'));
                                        LC_Enero = Linea_Captura;
                                    }
                                    else if (Instituciones.Key.EndsWith("FEBRERO"))
                                    {
                                        Linea_Captura = Linea_Captura_ACREMEX(
                                            "02", Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Desde_Periodo,
                                            Hasta_Periodo,
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", "").PadLeft(11, '0'));
                                        LC_Febrero = Linea_Captura;
                                    }
                                    Meses = "ENERO_FEBRERO";
                                    break;
                                case "BANAMEX":
                                    if (Instituciones.Key.EndsWith("ENERO"))
                                    {
                                        Linea_Captura = Linea_Captura_Banamex(
                                            Dic_Convenios.ContainsKey("BANAMEX") ? Dic_Convenios["BANAMEX"] : "",
                                            Fecha_Limite_Enero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Enero = Linea_Captura;
                                    }
                                    else if (Instituciones.Key.EndsWith("FEBRERO"))
                                    {
                                        Linea_Captura = Linea_Captura_Banamex(
                                            Dic_Convenios.ContainsKey("BANAMEX") ? Dic_Convenios["BANAMEX"] : "",
                                            Fecha_Limite_Febrero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Febrero = Linea_Captura;
                                    }
                                    Meses = "ENERO_FEBRERO";
                                    break;
                                case "BANORTE":
                                    Linea_Captura = Dic_Convenios.ContainsKey("BANORTE")
                                        ? Dic_Convenios["BANORTE"] + " " + Cuenta["CUENTA_PREDIAL"].ToString()
                                        : "" + Cuenta["CUENTA_PREDIAL"].ToString();
                                    LC_Enero = Linea_Captura;
                                    Meses = "SOLO_ENERO";
                                    break;
                                case "BANCOMER":
                                    if (Instituciones.Key.EndsWith("ENERO"))
                                    {
                                        Linea_Captura = Linea_Captura_Bancomer(
                                            Dic_Convenios.ContainsKey("BANCOMER") ? Dic_Convenios["BANCOMER"] : "",
                                            Fecha_Limite_Enero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Enero = Linea_Captura;
                                    }
                                    else if (Instituciones.Key.EndsWith("FEBRERO"))
                                    {
                                        Linea_Captura = Linea_Captura_Bancomer(
                                            Dic_Convenios.ContainsKey("BANCOMER") ? Dic_Convenios["BANCOMER"] : "",
                                            Fecha_Limite_Febrero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Febrero = Linea_Captura;
                                    }
                                    Meses = "ENERO_FEBRERO";
                                    break;
                                case "HSBC":
                                    if (Instituciones.Key.EndsWith("ENERO"))
                                    {
                                        Linea_Captura = Linea_Captura_HSBC(
                                            Fecha_Limite_Enero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Enero = Linea_Captura;
                                    }
                                    else if (Instituciones.Key.EndsWith("FEBRERO"))
                                    {
                                        Linea_Captura = Linea_Captura_HSBC(
                                            Fecha_Limite_Febrero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Febrero = Linea_Captura;
                                    }
                                    Meses = "ENERO_FEBRERO";
                                    break;
                                case "BAJIO":
                                    if (Instituciones.Key.EndsWith("ENERO"))
                                    {
                                        Linea_Captura = Linea_Captura_BBajio(
                                            Fecha_Limite_Enero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Enero = Linea_Captura;
                                    }
                                    else if (Instituciones.Key.EndsWith("FEBRERO"))
                                    {
                                        Linea_Captura = Linea_Captura_BBajio(
                                            Fecha_Limite_Febrero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Febrero = Linea_Captura;
                                    }
                                    Meses = "ENERO_FEBRERO";
                                    break;
                                case "SCOTIABANK":
                                    if (Instituciones.Key.EndsWith("ENERO"))
                                    {
                                        Linea_Captura = Linea_Captura_Scotia(
                                            Fecha_Limite_Enero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Enero = Linea_Captura;
                                    }
                                    else if (Instituciones.Key.EndsWith("FEBRERO"))
                                    {
                                        Linea_Captura = Linea_Captura_Scotia(
                                            Fecha_Limite_Febrero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Febrero = Linea_Captura;
                                    }
                                    Meses = "ENERO_FEBRERO";
                                    break;
                                case "SANTANDER":
                                    if (Instituciones.Key.EndsWith("ENERO"))
                                    {
                                        Linea_Captura = Linea_Captura_Santander(
                                            Fecha_Limite_Enero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Enero = Linea_Captura;
                                    }
                                    else if (Instituciones.Key.EndsWith("FEBRERO"))
                                    {
                                        Linea_Captura = Linea_Captura_Santander(
                                            Fecha_Limite_Febrero,
                                            Cuenta["CUENTA_PREDIAL"].ToString(),
                                            Monto_Impuesto_Predial.ToString("0.00").Replace(".", ""));
                                        LC_Febrero = Linea_Captura;
                                    }
                                    Meses = "ENERO_FEBRERO";
                                    break;
                                case "OXXO / SUPERBARA":
                                    Linea_Captura = Linea_Captura_Oxxo(Cuenta["CUENTA_PREDIAL"].ToString());
                                    LC_Enero = Linea_Captura;
                                    Meses = "SOLO_ENERO";
                                    break;
                            }
                            Dr_Renglon_Cuenta[Instituciones.Key] = Linea_Captura;
                            // agregar renglon con lineas de captura para ope_pre_lineas_captura si ya contienen datos
                            if (Dic_Institucion_ID.ContainsKey(Instituciones.Key.Replace("ENERO", "").Replace("FEBRERO", "")) && ((Meses == "ENERO_FEBRERO" && LC_Enero.Length > 0 && LC_Febrero.Length > 0) || (Meses == "SOLO_ENERO" && LC_Enero.Length > 0)))
                            {
                                Dr_Linea_Captura = Dt_Lineas_Captura.NewRow();
                                Dr_Linea_Captura["CUENTA_PREDIAL_ID"] = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                                Dr_Linea_Captura["INSTITUCION_ID"] = Dic_Institucion_ID[Instituciones.Key.Replace("ENERO", "").Replace("FEBRERO", "")];
                                Dr_Linea_Captura["TIPO_PREDIO_ID"] = Cuenta[Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                                Dr_Linea_Captura["ANIO"] = Anio_Generacion_Int;
                                Dr_Linea_Captura["LINEA_CAPTURA_ENERO"] = LC_Enero;
                                Dr_Linea_Captura["LINEA_CAPTURA_FEBRERO"] = LC_Febrero;

                                Dt_Lineas_Captura.Rows.Add(Dr_Linea_Captura);
                                Meses = "";
                                LC_Enero = "";
                                LC_Febrero = "";
                            }
                        }
                        Dr_Renglon_Cuenta["TIPO_PREDIO"] = Cuenta["TIPO_PREDIO"].ToString();
                        Dr_Renglon_Cuenta["SECTOR"] = Cuenta["SECTOR"].ToString();

                        Dt_Archivo_Adeudos.Rows.Add(Dr_Renglon_Cuenta);

                        // sumar totales
                        Total_Cuentas++;
                        _Total_Recargos_Acumulado += _Total_Recargos_Generados;
                        _Total_Recargos_Febrero_Acumulado += _Total_Recargos_Generados_Febrero;
                        _Total_Rezago_Acumulado += _Total_Rezago;
                        _Total_Adeudo_Archivo += Monto_Impuesto_Predial;
                        _Total_Descuento_Enero += Descuento_Enero;
                        _Total_Descuento_Febrero += Descuento_Febrero;
                    }
                    else
                    {
                        Mensaje_Error += Cuenta["CUENTA_PREDIAL"].ToString() + " sin adeudos, ";
                    }

                }

                if (File.Exists(Ruta_Archivo))
                {
                    File.Delete(Ruta_Archivo);
                }
                // Guardar archivo
                CreateExcelFile.CreateExcelDocument(Dt_Archivo_Adeudos, Ruta_Archivo);
                // alta de lineas de captura
                Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Alta_Lineas_Captura(Anio_Generacion_Int, Tipo_Predio, Dt_Lineas_Captura, Usuario_Creo);
            }
            catch (Exception ex)
            {
                throw new Exception("Generar_Archivo_Adeudos: " + ex.Message.ToString(), ex);
            }

            return Mensaje_Error;
        } // termina metodo Generar_Archivo_Adeudos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Tabla_Archivo_Adeudos
        /// DESCRIPCIÓN: Regresa una tabla vacia con las columnas para el archivo de adeudos
        /// PARÁMETROS:
        ///             1. Dic_Instituciones: Diccionario con los titulos de las lineas de captura
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 22-dic-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private DataTable Generar_Tabla_Archivo_Adeudos(Dictionary<String, String> Dic_Instituciones)
        {
            DataTable Dt_Archivo_Adeudos;

            Dt_Archivo_Adeudos = new DataTable();

            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("CVEPREDIO", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("NOMCAUS", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("NOMCALUBI", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("NUMEXTUBI", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("NUMINTUBI", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("NOMCOLUBI", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("CIUDADUBI", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("ESTADOUBI", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("NOMCALDOM", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("NUMEXTDOM", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("NUMINTDOM", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("NOMCOLDOM", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("CIUDADDOM", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("ESTADODOM", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("CVEMOVI", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("EFECTOS", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("TASA", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("VALFISCAL", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("CUOTABIM", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("PERIOREZA", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("ADEUDOREZA", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("PERIOCORR", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("ADEUDOCORR", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("RECARGOSENE", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("RECARGOSFEB", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("HONDECOB", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("DESCENERO", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("DESCFEBRERO", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("SUBTOTALENE", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("SUBTOTALFEB", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("TOTALENERO", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("TOTALFEBRERO", typeof(String)));
            foreach (KeyValuePair<String, String> Instituciones in Dic_Instituciones)
            {
                Dt_Archivo_Adeudos.Columns.Add(new DataColumn(Instituciones.Key, typeof(String)));
            }
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("TIPO_PREDIO", typeof(String)));
            Dt_Archivo_Adeudos.Columns.Add(new DataColumn("SECTOR", typeof(String)));


            return Dt_Archivo_Adeudos;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Formar_Tabla_Lineas_Captura
        /// DESCRIPCIÓN: Regresa una tabla vacia con las columnas para insertar las líneas de captura en ope_pre_lineas_captura
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 14-mar-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private DataTable Formar_Tabla_Lineas_Captura()
        {
            DataTable Dt_Lineas_Captura = new DataTable();

            Dt_Lineas_Captura.Columns.Add(new DataColumn("CUENTA_PREDIAL_ID", typeof(String)));
            Dt_Lineas_Captura.Columns.Add(new DataColumn("INSTITUCION_ID", typeof(String)));
            Dt_Lineas_Captura.Columns.Add(new DataColumn("TIPO_PREDIO_ID", typeof(String)));
            Dt_Lineas_Captura.Columns.Add(new DataColumn("ANIO", typeof(int)));
            Dt_Lineas_Captura.Columns.Add(new DataColumn("LINEA_CAPTURA_ENERO", typeof(String)));
            Dt_Lineas_Captura.Columns.Add(new DataColumn("LINEA_CAPTURA_FEBRERO", typeof(String)));

            return Dt_Lineas_Captura;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Cambiar_Equivalentes_Alfanumericos_035
        /// DESCRIPCIÓN: Inicializar parametros para formar lineas de captura
        ///             Gl_Convertir_Valores_Afanumericos: diccionario global para convertir valores 
        ///                         alfanumericos en un consecutivo numerico
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 14-sep-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private void Cambiar_Equivalentes_Alfanumericos_035()
        {
            Gl_Convertir_Valores_Afanumericos = new Dictionary<Char, Int32>();

            Gl_Convertir_Valores_Afanumericos.Add('0', 0);
            Gl_Convertir_Valores_Afanumericos.Add('1', 1);
            Gl_Convertir_Valores_Afanumericos.Add('2', 2);
            Gl_Convertir_Valores_Afanumericos.Add('3', 3);
            Gl_Convertir_Valores_Afanumericos.Add('4', 4);
            Gl_Convertir_Valores_Afanumericos.Add('5', 5);
            Gl_Convertir_Valores_Afanumericos.Add('6', 6);
            Gl_Convertir_Valores_Afanumericos.Add('7', 7);
            Gl_Convertir_Valores_Afanumericos.Add('8', 8);
            Gl_Convertir_Valores_Afanumericos.Add('9', 9);
            Gl_Convertir_Valores_Afanumericos.Add('A', 10);
            Gl_Convertir_Valores_Afanumericos.Add('B', 11);
            Gl_Convertir_Valores_Afanumericos.Add('C', 12);
            Gl_Convertir_Valores_Afanumericos.Add('D', 13);
            Gl_Convertir_Valores_Afanumericos.Add('E', 14);
            Gl_Convertir_Valores_Afanumericos.Add('F', 15);
            Gl_Convertir_Valores_Afanumericos.Add('G', 16);
            Gl_Convertir_Valores_Afanumericos.Add('H', 17);
            Gl_Convertir_Valores_Afanumericos.Add('I', 18);
            Gl_Convertir_Valores_Afanumericos.Add('J', 19);
            Gl_Convertir_Valores_Afanumericos.Add('K', 20);
            Gl_Convertir_Valores_Afanumericos.Add('L', 21);
            Gl_Convertir_Valores_Afanumericos.Add('M', 22);
            Gl_Convertir_Valores_Afanumericos.Add('N', 23);
            Gl_Convertir_Valores_Afanumericos.Add('O', 24);
            Gl_Convertir_Valores_Afanumericos.Add('P', 25);
            Gl_Convertir_Valores_Afanumericos.Add('Q', 26);
            Gl_Convertir_Valores_Afanumericos.Add('R', 27);
            Gl_Convertir_Valores_Afanumericos.Add('S', 28);
            Gl_Convertir_Valores_Afanumericos.Add('T', 29);
            Gl_Convertir_Valores_Afanumericos.Add('U', 30);
            Gl_Convertir_Valores_Afanumericos.Add('V', 31);
            Gl_Convertir_Valores_Afanumericos.Add('W', 32);
            Gl_Convertir_Valores_Afanumericos.Add('X', 33);
            Gl_Convertir_Valores_Afanumericos.Add('Y', 34);
            Gl_Convertir_Valores_Afanumericos.Add('Z', 35);
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Cambiar_Equivalentes_Alfanumericos_09
        /// DESCRIPCIÓN: Cambia el valor para traducir los caracteres alfabeticos en numericos con valores de 0 a 9
        ///             Gl_Convertir_Valores_Afanumericos: diccionario global para convertir valores 
        ///                         alfanumericos en un consecutivo numerico
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 30-sep-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private void Cambiar_Equivalentes_Alfanumericos_09()
        {
            Gl_Convertir_Valores_Afanumericos = new Dictionary<Char, Int32>();

            Gl_Convertir_Valores_Afanumericos.Add('0', 0);
            Gl_Convertir_Valores_Afanumericos.Add('1', 1);
            Gl_Convertir_Valores_Afanumericos.Add('2', 2);
            Gl_Convertir_Valores_Afanumericos.Add('3', 3);
            Gl_Convertir_Valores_Afanumericos.Add('4', 4);
            Gl_Convertir_Valores_Afanumericos.Add('5', 5);
            Gl_Convertir_Valores_Afanumericos.Add('6', 6);
            Gl_Convertir_Valores_Afanumericos.Add('7', 7);
            Gl_Convertir_Valores_Afanumericos.Add('8', 8);
            Gl_Convertir_Valores_Afanumericos.Add('9', 9);
            Gl_Convertir_Valores_Afanumericos.Add('A', 1);
            Gl_Convertir_Valores_Afanumericos.Add('B', 2);
            Gl_Convertir_Valores_Afanumericos.Add('C', 3);
            Gl_Convertir_Valores_Afanumericos.Add('D', 4);
            Gl_Convertir_Valores_Afanumericos.Add('E', 5);
            Gl_Convertir_Valores_Afanumericos.Add('F', 6);
            Gl_Convertir_Valores_Afanumericos.Add('G', 7);
            Gl_Convertir_Valores_Afanumericos.Add('H', 8);
            Gl_Convertir_Valores_Afanumericos.Add('I', 9);
            Gl_Convertir_Valores_Afanumericos.Add('J', 1);
            Gl_Convertir_Valores_Afanumericos.Add('K', 2);
            Gl_Convertir_Valores_Afanumericos.Add('L', 3);
            Gl_Convertir_Valores_Afanumericos.Add('M', 4);
            Gl_Convertir_Valores_Afanumericos.Add('N', 5);
            Gl_Convertir_Valores_Afanumericos.Add('O', 6);
            Gl_Convertir_Valores_Afanumericos.Add('P', 7);
            Gl_Convertir_Valores_Afanumericos.Add('Q', 8);
            Gl_Convertir_Valores_Afanumericos.Add('R', 9);
            Gl_Convertir_Valores_Afanumericos.Add('S', 2);
            Gl_Convertir_Valores_Afanumericos.Add('T', 3);
            Gl_Convertir_Valores_Afanumericos.Add('U', 4);
            Gl_Convertir_Valores_Afanumericos.Add('V', 5);
            Gl_Convertir_Valores_Afanumericos.Add('W', 6);
            Gl_Convertir_Valores_Afanumericos.Add('X', 7);
            Gl_Convertir_Valores_Afanumericos.Add('Y', 8);
            Gl_Convertir_Valores_Afanumericos.Add('Z', 9);
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Instituciones_Diccionario
        /// DESCRIPCIÓN: Obtiene el catalogo de instituciones y lo regresa como un diccionario
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 3-oct-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private Dictionary<String, String> Obtener_Instituciones_Diccionario(out Dictionary<String, String> Dic_Convenios, out Dictionary<String, String> Dic_Institucion_ID)
        {
            var Rs_Instituciones = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
            Dictionary<String, String> Dic_Instituciones = new Dictionary<String, String>();
            DataTable Dt_Instituciones;

            Dic_Convenios = new Dictionary<String, String>();
            Dic_Institucion_ID = new Dictionary<String, String>();

            Rs_Instituciones.P_Filtro = "";
            Rs_Instituciones.P_Estatus = "VIGENTE";
            Dt_Instituciones = Rs_Instituciones.Consultar_Institucion();

            if (Dt_Instituciones != null)
            {
                foreach (DataRow fila_institucion in Dt_Instituciones.Rows)
                {
                    // si el campo linea de captura no esta vacio y la llave aun no esta en el diccionario, incluirla (enero)
                    if (fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Enero].ToString() != ""
                        && !Dic_Instituciones.ContainsKey(fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion].ToString() + "ENERO"))
                    {
                        Dic_Instituciones.Add(fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion].ToString() + "ENERO"
                            , fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Enero].ToString());
                    }
                    // si el campo linea de captura no esta vacio y la llave aun no esta en el diccionario, incluirla (febrero)
                    if (fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Febrero].ToString() != ""
                        && !Dic_Instituciones.ContainsKey(fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion].ToString() + "FEBRERO"))
                    {
                        Dic_Instituciones.Add(fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion].ToString() + "FEBRERO"
                            , fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Febrero].ToString());
                    }
                    // generar diccionario de convenios
                    if (fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio].ToString() != ""
                        && !Dic_Convenios.ContainsKey(fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio].ToString()))
                    {
                        Dic_Convenios.Add(fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion].ToString()
                            , fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio].ToString());
                    }
                    // generar diccionario institucion_ID
                    if (fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion].ToString() != ""
                        && !Dic_Institucion_ID.ContainsKey(fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion].ToString()))
                    {
                        Dic_Institucion_ID.Add(fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion].ToString()
                            , fila_institucion[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id].ToString());
                    }

                }
            }

            return Dic_Instituciones;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Ultimos_Movimientos
        /// DESCRIPCIÓN: Comsulta los movimientos de las cuentas, agrega el ultimo movimiento de cada cuenta 
        ///             a un diccionario (an la consulta el primer movimiento de cada cuenta es el más reciente)
        ///             y regresa el diccionario 
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 23-dic-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private Dictionary<String, String> Obtener_Ultimos_Movimientos()
        {
            DataTable Dt_Ultimos_Movimientos;
            String Cuenta_Anterior = "";
            String Cuenta_ID = "";

            Dictionary<String, String> Dic_Ultimos_Movimientos = new Dictionary<String, String>();

            Dt_Ultimos_Movimientos = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Movimientos_Aceptados_Cuentas();

            if (Dt_Ultimos_Movimientos != null)
            {
                // para cada registro encontrado, 
                for (int Movimiento = 0; Movimiento < Dt_Ultimos_Movimientos.Rows.Count; Movimiento++)
                {
                    Cuenta_ID = Dt_Ultimos_Movimientos.Rows[Movimiento][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    // comparar con el registro anterior 
                    if (Cuenta_Anterior != Cuenta_ID && !Dic_Ultimos_Movimientos.ContainsKey(Cuenta_ID))
                    {
                        Cuenta_Anterior = Cuenta_ID;
                        Dic_Ultimos_Movimientos.Add(Cuenta_ID, Dt_Ultimos_Movimientos.Rows[Movimiento]["ULTIMO_MOVIMIENTO"].ToString());
                    }
                }
            }

            return Dic_Ultimos_Movimientos;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Ultimos_Movimientos_ID
        /// DESCRIPCIÓN: Comsulta los movimientos de las cuentas, agrega el ID del último movimiento de cada cuenta 
        ///             a un diccionario (an la consulta el primer movimiento de cada cuenta es el más reciente)
        ///             y regresa el diccionario 
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 06-mar-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private Dictionary<String, String> Obtener_Ultimos_Movimientos_ID()
        {
            DataTable Dt_Ultimos_Movimientos;
            String Cuenta_Anterior = "";
            String Cuenta_ID = "";

            Dictionary<String, String> Dic_Ultimos_Movimientos = new Dictionary<String, String>();

            Dt_Ultimos_Movimientos = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Movimientos_Aceptados_Cuentas();

            if (Dt_Ultimos_Movimientos != null)
            {
                // para cada registro encontrado, 
                for (int Movimiento = 0; Movimiento < Dt_Ultimos_Movimientos.Rows.Count; Movimiento++)
                {
                    Cuenta_ID = Dt_Ultimos_Movimientos.Rows[Movimiento][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    // comparar con el registro anterior 
                    if (Cuenta_Anterior != Cuenta_ID && !Dic_Ultimos_Movimientos.ContainsKey(Cuenta_ID))
                    {
                        Cuenta_Anterior = Cuenta_ID;
                        Dic_Ultimos_Movimientos.Add(Cuenta_ID, Dt_Ultimos_Movimientos.Rows[Movimiento][Cat_Pre_Movimientos.Campo_Movimiento_ID].ToString());
                    }
                }
            }

            return Dic_Ultimos_Movimientos;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Descuentos_Pronto_Pago
        /// DESCRIPCIÓN: Obtiene los descuentos del catalogo
        /// PARÁMETROS:
        /// 		1. Descuento_PP_Enero: Variable en la que se asignara el porcentaje de descuento para enero
        /// 		2. Descuento_PP_Febrero: Variable en la que se asignara el porcentaje de descuento para febrero
        /// 		3. Anio: Año a consultar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 11-oct-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private void Obtener_Descuentos_Pronto_Pago(out Decimal Descuento_PP_Enero, out Decimal Descuento_PP_Febrero, Int32 Anio)
        {
            Cls_Cat_Pre_Descuentos_Predial_Negocio Rs_Descuentos = new Cls_Cat_Pre_Descuentos_Predial_Negocio();

            Rs_Descuentos.P_Anio = Anio;
            Rs_Descuentos = Rs_Descuentos.Consultar_Datos_Descuento_Predial();

            Decimal.TryParse(Rs_Descuentos.P_Enero.ToString(), out Descuento_PP_Enero);
            Decimal.TryParse(Rs_Descuentos.P_Febrero.ToString(), out Descuento_PP_Febrero);

        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Ultimo_Dia_Habil
        /// DESCRIPCIÓN: Calcula el ultimo dia habil del mes restando dias a la fecha inicial indicada
        /// PARÁMETROS:
        /// 		1. Fecha_Inicial: String con la fecha inicial a considerar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 06-oct-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private DateTime Obtener_Ultimo_Dia_Habil(String Fecha_Inicial)
        {
            String Dia = "";
            DateTime Fecha;
            DataTable Dt_Dia_Festivo;
            Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Negocio = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

            try
            {
                if (DateTime.TryParse(Fecha_Inicial, out Fecha))
                {
                    do
                    {
                        Fecha = Fecha.AddDays(-1);
                        Dia = Fecha.ToString("dddd");
                        Dias_Negocio.P_Anio = Fecha.Year.ToString();
                        Dias_Negocio.P_Fecha_Inicial_Busqueda = Fecha.ToString("dd/MM/yyyy");
                        Dias_Negocio.P_Fecha_Final_Busqueda = Fecha.ToString("dd/MM/yyyy");

                        Dt_Dia_Festivo = Dias_Negocio.Consultar_Dias();

                    } while (Dia == "sábado" || Dia == "sabado" || Dia == "domingo" || Dia == "saturday" || Dia == "sunday" || Dt_Dia_Festivo.Rows.Count > 0);
                }
                return Fecha;
            }
            catch (Exception Ex)
            {
                throw new Exception("Ocurrio un Error al calcular la Fecha Error: " + Ex.Message);
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Calcular_Producto_Factor
        /// DESCRIPCIÓN: Calcula la suma de los productos, convirtiendo los caracteres a us equivalente
        /// PARÁMETROS:
        /// 		1. Cadena_Numero: Cadena de caracteres con lo numeros a factorizar
        /// 		2. Factor_Peso: Arreglo de enteros con el factor peso que se utilizara para factorizar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 17-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private Int32 Calcular_Producto_Factor(String Cadena_Numero, Int32[] Factor_Peso)
        {
            Char[] Arreglo_Numeros;
            Int32 Producto = 0;
            Int32 Cont_Factor = 0;

            Arreglo_Numeros = Cadena_Numero.ToCharArray();

            for (int num = Arreglo_Numeros.Length - 1; num >= 0; num--)
            {

                Producto += Gl_Convertir_Valores_Afanumericos[Arreglo_Numeros[num]] * Factor_Peso[Cont_Factor++];

                // si el contador llega al final del arreglo, reiniciar
                if (Cont_Factor >= Factor_Peso.Length)
                {
                    Cont_Factor = 0;
                }
            }

            return Producto;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Calcular_Producto_Factor
        /// DESCRIPCIÓN: Calcula la suma de los productos, convirtiendo los caracteres a us equivalente
        ///             y regresa la cadena convertida

        /// PARÁMETROS:
        /// 		1. Cadena_Numero: Cadena de caracteres con lo numeros a factorizar
        /// 		2. Factor_Peso: Arreglo de enteros con el factor peso que se utilizara para factorizar
        /// 		3. Cadena_Traducida: Cadena de texto traducida, se forma con los caracteres convertidos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 17-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private Int32 Calcular_Producto_Factor(String Cadena_Numero, Int32[] Factor_Peso, out String Cadena_Traducida)
        {
            Char[] Arreglo_Numeros;
            Int32 Producto = 0;
            Int32 Cont_Factor = 0;

            Arreglo_Numeros = Cadena_Numero.ToCharArray();
            Cadena_Traducida = "";

            for (int num = Arreglo_Numeros.Length - 1; num >= 0; num--)
            {

                Producto += Gl_Convertir_Valores_Afanumericos[Arreglo_Numeros[num]] * Factor_Peso[Cont_Factor++];
                Cadena_Traducida = Gl_Convertir_Valores_Afanumericos[Arreglo_Numeros[num]].ToString() + Cadena_Traducida;

                // si el contador llega al final del arreglo, reiniciar
                if (Cont_Factor >= Factor_Peso.Length)
                {
                    Cont_Factor = 0;
                }
            }

            return Producto;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Linea_Captura_ACEMEX
        /// DESCRIPCIÓN: Formar linea de captura
        /// PARÁMETROS:
        /// 		1. Mes: numero de mes
        /// 		2. Cuenta: numero de cuenta
        /// 		3. Periodo_Inicial: periodo inicial (AnioBim)
        /// 		4. Periodo_Final: periodo final (AnioBim)
        /// 		5. Monto: Importe a pagar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 08-sep-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Linea_Captura_ACREMEX(String Mes, String Cuenta, String Periodo_Inicial, String Periodo_Final, String Monto)
        {
            String Linea_Captura = Mes + Cuenta + Periodo_Inicial + Periodo_Final + Monto;

            Cambiar_Equivalentes_Alfanumericos_035();

            char[] caracteres = Linea_Captura.ToCharArray();
            int num = 9;
            int Total = 0;
            for (int i = caracteres.Length - 1; i >= 0; i--)
            {
                if (num == 9)
                    num = 2;
                Total = Gl_Convertir_Valores_Afanumericos[caracteres[i]] * num++;
            }
            int residuo = Total % 2;

            int verificador = 11 - residuo;

            return Mes + Cuenta + Periodo_Inicial + Periodo_Final + verificador.ToString();
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Linea_Captura_BBajio
        /// DESCRIPCIÓN: Formar linea de captura Banco del Bajio
        /// PARÁMETROS:
        /// 		1. Fecha: fecha limite de pago
        /// 		2. Cuenta: numero de cuenta predial
        /// 		3. Monto: importe a pagar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 14-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Linea_Captura_BBajio(DateTime Fecha_Vencimiento, String Cuenta, String Monto)
        {
            String Linea_Captura = "";
            String Fecha_Condensada = "";
            DateTime Fecha_Limite = Fecha_Vencimiento;
            Int32 Anio = 0;
            Int32 Mes = 0;
            Int32 Dia = 0;
            Int32[] Factor_Peso_Importe = { 7, 3, 1 };
            Int32[] Factor_Peso_Linea_Captura = { 11, 13, 17, 19, 23 };
            Int32 Importe_Condensado = 0;
            Int32 Linea_Captura_Condensada = 0;
            Int32 Digitos_Verificadores = 0;

            Cambiar_Equivalentes_Alfanumericos_035();

            /// obtener fecha condensada
            // Al año se le resta el número 1988 y se multiplica por el número 372.
            Anio = (Fecha_Limite.Year - 1988) * 372;
            // Al mes se le resta la unidad (1) y se multiplica por el número 31.
            Mes = (Fecha_Limite.Month - 1) * 31;
            // Al día se le resta la unidad (1).
            Dia = Fecha_Limite.Day - 1;
            // Se suman y el resultado es la fecha condensada
            Fecha_Condensada = (Anio + Mes + Dia).ToString();

            // obtener monto condensado
            Importe_Condensado = Calcular_Producto_Factor(Monto, Factor_Peso_Importe);
            Importe_Condensado = Importe_Condensado % 10;

            // obtener producto cuenta + fecha condensada + importe condensado
            Linea_Captura_Condensada = Calcular_Producto_Factor(Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString(), Factor_Peso_Linea_Captura);
            // se obtiene el residuo del producto obtenido entre 97
            Digitos_Verificadores = Linea_Captura_Condensada % 97;
            // se suma la unidad al resultado
            Digitos_Verificadores += 1;

            // formar la linea de captura cuenta + fecha condensada + importe condensado + digitos verificadores
            Linea_Captura = Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + Digitos_Verificadores.ToString();

            return Linea_Captura;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Linea_Captura_Bancomer
        /// DESCRIPCIÓN: Formar linea de captura Bancomer
        /// PARÁMETROS:
        /// 		1. Fecha: fecha limite de pago
        /// 		2. Cuenta: numero de cuenta predial
        /// 		3. Monto: importe a pagar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 17-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Linea_Captura_Bancomer(String Convenio, DateTime Fecha_Vencimiento, String Cuenta, String Monto)
        {
            String Linea_Captura = "";
            String Fecha_Condensada = "";
            DateTime Fecha_Limite = Fecha_Vencimiento;
            Int32 Anio = 0;
            Int32 Mes = 0;
            Int32 Dia = 0;
            Int32[] Factor_Peso_Importe = { 7, 3, 1 };
            Int32[] Factor_Peso_Linea_Captura = { 11, 13, 17, 19, 23 };
            Int32 Importe_Condensado = 0;
            Int32 Linea_Captura_Condensada = 0;
            Int32 Digitos_Verificadores = 0;
            String Cadena_Traducida;

            Cambiar_Equivalentes_Alfanumericos_035();

            /// obtener fecha condensada
            // Al año se le resta el número 1988 y se multiplica por el número 372.
            Anio = (Fecha_Limite.Year - 1988) * 372;
            // Al mes se le resta la unidad (1) y se multiplica por el número 31.
            Mes = (Fecha_Limite.Month - 1) * 31;
            // Al día se le resta la unidad (1).
            Dia = Fecha_Limite.Day - 1;
            // Se suman y el resultado es la fecha condensada
            Fecha_Condensada = (Anio + Mes + Dia).ToString();

            // obtener monto condensado
            Importe_Condensado = Calcular_Producto_Factor(Monto, Factor_Peso_Importe);
            Importe_Condensado = Importe_Condensado % 10;

            // obtener producto cuenta + fecha condensada + importe condensado
            Linea_Captura_Condensada = Calcular_Producto_Factor(Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + "2", Factor_Peso_Linea_Captura, out Cadena_Traducida);
            // se obtiene el residuo del producto obtenido entre 97
            Digitos_Verificadores = Linea_Captura_Condensada % 97;
            // se suma la unidad al resultado
            Digitos_Verificadores += 1;

            // formar la linea de captura cuenta + fecha condensada + importe condensado + digitos verificadores
            Linea_Captura = Cadena_Traducida + Digitos_Verificadores.ToString();

            // si la linea de captura tiene mas de 20 caracteres, reducir
            if (Linea_Captura.Length > 20)
            {
                Linea_Captura = Linea_Captura.Substring(Linea_Captura.Length - 20, 20);
            }

            return "Cie " + Convenio + " Ref " + Linea_Captura;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Linea_Captura_Banamex
        /// DESCRIPCIÓN: Formar linea de captura Banamex
        /// PARÁMETROS:
        ///         1. Tipo_Pago: Numero asignado por el banco
        /// 		2. Fecha_Vencimiento: fecha limite de pago
        /// 		3. Cuenta: numero de cuenta predial
        /// 		4. Monto: importe a pagar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 30-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Linea_Captura_Banamex(String Tipo_Pago, DateTime Fecha_Vencimiento, String Cuenta, String Monto)
        {
            String Linea_Captura = "";
            String Fecha_Condensada = "";
            DateTime Fecha_Limite = Fecha_Vencimiento;
            Int32 Anio = 0;
            Int32 Mes = 0;
            Int32 Dia = 0;
            Int32[] Factor_Peso_Importe = { 7, 3, 1 };
            Int32[] Factor_Peso_Linea_Captura = { 11, 13, 17, 19, 23 };
            Int32 Importe_Condensado = 0;
            Int32 Linea_Captura_Condensada = 0;
            Int32 Digitos_Verificadores = 0;
            String Cadena_Traducida;

            /// obtener fecha condensada
            // Al año se le resta el número 1988 y se multiplica por el número 372.
            Anio = (Fecha_Limite.Year - 1988) * 372;
            // Al mes se le resta la unidad (1) y se multiplica por el número 31.
            Mes = (Fecha_Limite.Month - 1) * 31;
            // Al día se le resta la unidad (1).
            Dia = Fecha_Limite.Day - 1;
            // Se suman y el resultado es la fecha condensada
            Fecha_Condensada = (Anio + Mes + Dia).ToString();

            Cambiar_Equivalentes_Alfanumericos_09();

            // obtener monto condensado
            Importe_Condensado = Calcular_Producto_Factor(Monto, Factor_Peso_Importe);
            Importe_Condensado = Importe_Condensado % 10;

            // obtener producto cuenta + fecha condensada + importe condensado
            Linea_Captura_Condensada = Calcular_Producto_Factor(Tipo_Pago + Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + "2", Factor_Peso_Linea_Captura, out Cadena_Traducida);
            // se obtiene el residuo del producto obtenido entre 97
            Digitos_Verificadores = Linea_Captura_Condensada % 97;
            // se suma la unidad al resultado
            Digitos_Verificadores += 1;

            // formar la linea de captura cuenta + fecha condensada + importe condensado + digitos verificadores
            Linea_Captura = Tipo_Pago + Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + "2" + Digitos_Verificadores.ToString();

            return "B: " + Linea_Captura;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Linea_Captura_HSBC
        /// DESCRIPCIÓN: Formar linea de captura HSBC
        /// PARÁMETROS:
        /// 		1. Fecha: fecha limite de pago
        /// 		2. Cuenta: numero de cuenta predial
        /// 		3. Monto: importe a pagar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 1-oct-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Linea_Captura_HSBC(DateTime Fecha_Vencimiento, String Cuenta, String Monto)
        {
            String Linea_Captura = "";
            String Fecha_Condensada = "";
            DateTime Fecha_Limite = Fecha_Vencimiento;
            Int32 Anio = 0;
            Int32 Mes = 0;
            Int32 Dia = 0;
            Int32[] Factor_Peso_Importe = { 7, 3, 1 };
            Int32[] Factor_Peso_Linea_Captura = { 11, 13, 17, 19, 23 };
            Int32 Importe_Condensado = 0;
            Int32 Linea_Captura_Condensada = 0;
            Int32 Digitos_Verificadores = 0;
            String Cadena_Traducida;

            /// obtener fecha condensada
            // Al año se le resta el número 1988 y se multiplica por el número 372.
            Anio = (Fecha_Limite.Year - 1988) * 372;
            // Al mes se le resta la unidad (1) y se multiplica por el número 31.
            Mes = (Fecha_Limite.Month - 1) * 31;
            // Al día se le resta la unidad (1).
            Dia = Fecha_Limite.Day - 1;
            // Se suman y el resultado es la fecha condensada
            Fecha_Condensada = (Anio + Mes + Dia).ToString();

            Cambiar_Equivalentes_Alfanumericos_09();

            // obtener monto condensado
            Importe_Condensado = Calcular_Producto_Factor(Monto, Factor_Peso_Importe);
            Importe_Condensado = Importe_Condensado % 10;

            // obtener producto cuenta + fecha condensada + importe condensado
            Linea_Captura_Condensada = Calcular_Producto_Factor(Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + "2", Factor_Peso_Linea_Captura, out Cadena_Traducida);
            // se obtiene el residuo del producto obtenido entre 97
            Digitos_Verificadores = Linea_Captura_Condensada % 97;
            // se suma la unidad al resultado
            Digitos_Verificadores += 1;

            // formar la linea de captura cuenta + fecha condensada + importe condensado + digitos verificadores
            Linea_Captura = Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + "2" + Digitos_Verificadores.ToString().PadLeft(2, '0');

            return Linea_Captura;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Linea_Captura_Santander
        /// DESCRIPCIÓN: Formar linea de captura Santander
        /// PARÁMETROS:
        /// 		1. Fecha: fecha limite de pago
        /// 		2. Cuenta: numero de cuenta predial
        /// 		3. Monto: importe a pagar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 1-oct-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Linea_Captura_Santander(DateTime Fecha_Vencimiento, String Cuenta, String Monto)
        {
            String Linea_Captura = "";
            String Fecha_Condensada = "";
            DateTime Fecha_Limite = Fecha_Vencimiento;
            Int32 Anio = 0;
            Int32 Mes = 0;
            Int32 Dia = 0;
            Int32[] Factor_Peso_Importe = { 7, 3, 1 };
            Int32[] Factor_Peso_Linea_Captura = { 11, 13, 17, 19, 23 };
            Int32 Importe_Condensado = 0;
            Int32 Linea_Captura_Condensada = 0;
            Int32 Digitos_Verificadores = 0;
            String Cadena_Traducida;

            /// obtener fecha condensada
            // Al año se le resta el número 1988 y se multiplica por el número 372.
            Anio = (Fecha_Limite.Year - 1988) * 372;
            // Al mes se le resta la unidad (1) y se multiplica por el número 31.
            Mes = (Fecha_Limite.Month - 1) * 31;
            // Al día se le resta la unidad (1).
            Dia = Fecha_Limite.Day - 1;
            // Se suman y el resultado es la fecha condensada
            Fecha_Condensada = (Anio + Mes + Dia).ToString();

            Cambiar_Equivalentes_Alfanumericos_035();

            // obtener monto condensado
            Importe_Condensado = Calcular_Producto_Factor(Monto, Factor_Peso_Importe);
            Importe_Condensado = Importe_Condensado % 10;

            // obtener producto cuenta + fecha condensada + importe condensado
            Linea_Captura_Condensada = Calcular_Producto_Factor(Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + "A", Factor_Peso_Linea_Captura, out Cadena_Traducida);
            // se obtiene el residuo del producto obtenido entre 97
            Digitos_Verificadores = Linea_Captura_Condensada % 97;
            // se suma la unidad al resultado
            Digitos_Verificadores += 1;

            // formar la linea de captura cuenta + fecha condensada + importe condensado + digitos verificadores
            Linea_Captura = Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + "2" + Digitos_Verificadores.ToString().PadLeft(2, '0');

            return Linea_Captura;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Linea_Captura_Scotia
        /// DESCRIPCIÓN: Formar linea de captura Santander
        /// PARÁMETROS:
        /// 		1. Fecha: fecha limite de pago
        /// 		2. Cuenta: numero de cuenta predial
        /// 		3. Monto: importe a pagar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 1-oct-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Linea_Captura_Scotia(DateTime Fecha_Vencimiento, String Cuenta, String Monto)
        {
            String Linea_Captura = "";
            String Fecha_Condensada = "";
            DateTime Fecha_Limite = Fecha_Vencimiento;
            Int32 Anio = 0;
            Int32 Mes = 0;
            Int32 Dia = 0;
            Int32[] Factor_Peso_Importe = { 7, 3, 1 };
            Int32[] Factor_Peso_Linea_Captura = { 11, 13, 17, 19, 23 };
            Int32 Importe_Condensado = 0;
            Int32 Linea_Captura_Condensada = 0;
            Int32 Digitos_Verificadores = 0;
            String Cadena_Traducida;

            /// obtener fecha condensada
            // Al año se le resta el número 1988 y se multiplica por el número 372.
            Anio = (Fecha_Limite.Year - 1988) * 372;
            // Al mes se le resta la unidad (1) y se multiplica por el número 31.
            Mes = (Fecha_Limite.Month - 1) * 31;
            // Al día se le resta la unidad (1).
            Dia = Fecha_Limite.Day - 1;
            // Se suman y el resultado es la fecha condensada
            Fecha_Condensada = (Anio + Mes + Dia).ToString();

            Cambiar_Equivalentes_Alfanumericos_035();

            // obtener monto condensado
            Importe_Condensado = Calcular_Producto_Factor(Monto, Factor_Peso_Importe);
            Importe_Condensado = Importe_Condensado % 10;

            // obtener producto cuenta + fecha condensada + importe condensado
            Linea_Captura_Condensada = Calcular_Producto_Factor(Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + "E", Factor_Peso_Linea_Captura, out Cadena_Traducida);
            // se obtiene el residuo del producto obtenido entre 97
            Digitos_Verificadores = Linea_Captura_Condensada % 97;
            // se suma la unidad al resultado
            Digitos_Verificadores += 1;

            // formar la linea de captura cuenta + fecha condensada + importe condensado + digitos verificadores
            Linea_Captura = Cuenta + Fecha_Condensada.ToString() + Importe_Condensado.ToString() + "E" + Digitos_Verificadores.ToString();

            return Linea_Captura;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Linea_Captura_Oxxo
        /// DESCRIPCIÓN: Formar linea de captura OXXO / SUPERBARA
        /// PARÁMETROS:
        /// 		1. Fecha: fecha limite de pago
        /// 		2. Cuenta: numero de cuenta predial
        /// 		3. Monto: importe a pagar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 1-oct-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Linea_Captura_Oxxo(String Cuenta)
        {
            Int32[] Factor_Peso_Linea_Captura = { 11, 13, 17, 19, 23 };
            String Cadena_Traducida;

            Cambiar_Equivalentes_Alfanumericos_035();

            // obtener cadena traducida
            Calcular_Producto_Factor("0" + Cuenta, Factor_Peso_Linea_Captura, out Cadena_Traducida);

            return Cadena_Traducida;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Int32 Obtener_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones)
        {
            String Mi_SQL;
            Int32 Dato_Consulta = 0;

            try
            {
                Mi_SQL = "SELECT " + Campo;
                if (Tabla != "")
                {
                    Mi_SQL += " FROM " + Tabla;
                }
                if (Condiciones != "")
                {
                    Mi_SQL += " WHERE " + Condiciones;
                }

                //OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Cmmd.CommandText = Mi_SQL;
                Dato_Consulta = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString());
                if (Convert.IsDBNull(Dato_Consulta))
                {
                    Dato_Consulta = 1;
                }
                else
                {
                    Dato_Consulta = Dato_Consulta + 1;
                }
            }
            catch (OracleException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception(Ex.ToString());
            }
            return Dato_Consulta;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Obtener_Periodos_Bimestre
        ///DESCRIPCIÓN          : Valida la cadena indicada para obtener los periodos de la Bimestres quitando los Años
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        private String Obtener_Periodos_Bimestre(String Periodos, out Boolean Periodo_Corriente_Validado, out Boolean Periodo_Rezago_Validado)
        {
            String Periodo = "";
            int Indice = 0;
            Periodo_Corriente_Validado = false;
            Periodo_Rezago_Validado = false;

            if (Periodos.IndexOf("-") >= 0)
            {
                if (Periodos.Split('-').Length == 2)
                {
                    //Valida el segundo nodo del arreglo
                    if (Periodos.Split('-').GetValue(1).ToString().IndexOf("/") >= 0)
                    {
                        Periodo = Periodos.Split('-').GetValue(0).ToString().Trim().Substring(0, 1);
                        Periodo += "-";
                        Periodo += Periodos.Split('-').GetValue(1).ToString().Trim().Substring(0, 1);
                        Periodo_Rezago_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Split('-').GetValue(0).ToString().Replace("/", "-").Trim();
                        Periodo_Corriente_Validado = true;
                    }
                }
                else
                {
                    if (Periodos.Contains("/"))
                    {
                        Indice = Periodos.IndexOf("/");
                        Periodo = Periodos.Substring(Indice - 1, 1);
                        Periodo += "-";
                        Indice = Periodos.IndexOf("/", Indice + 1);
                        Periodo += Periodos.Substring(Indice - 1, 1);
                        Periodo_Rezago_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Substring(0, 3);
                        Periodo_Corriente_Validado = true;
                    }
                }
            }
            else
            {
                if (Periodos.Trim().IndexOf(" ") >= 0)
                {
                    if (Periodos.Split(' ').GetValue(0).ToString().Contains("/"))
                    {
                        Periodo = Periodos.Split(' ').GetValue(0).ToString().Replace("/", "-").Trim();
                        Periodo_Corriente_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Substring(0, 3);
                        Periodo_Corriente_Validado = true;
                    }
                }
            }
            return Periodo;
        }

        #endregion METODOS


        #region ACCESO_CAPAS_DATOS
        public int Eliminar_Adeudos_Predial()
        {
            return Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Eliminar_Adeudos_Predial(p_Anio.ToString());
        }

        public DataTable Consultar_Ordenes_Cuota_Minima(String Fecha, String Observaciones)
        {
            return Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Ordenes_Cuota_Minima(Fecha, Observaciones);
        }

        public DataTable Consultar_Cuentas_Adeudo_Menor_Cuota_Minima(Decimal Cuota_Minima, string Estatus)
        {
            return Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Cuentas_Adeudo_Menor_Cuota_Minima(Cuota_Minima, Estatus);
        }

        #endregion

    }
}
