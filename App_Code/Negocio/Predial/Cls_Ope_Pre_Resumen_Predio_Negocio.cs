using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Operacion_Resumen_Predio.Datos;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Datos;
using Presidencia.Catalogo_Salarios_Minimos.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
/// <summary>
/// Summary description for Cls_Ope_Pre_Resumen_Predio_Negocio
/// </summary>
/// 
namespace Presidencia.Operacion_Resumen_Predio.Negocio
{
    public class Cls_Ope_Pre_Resumen_Predio_Negocio
    {
        private Decimal Salario_Minimo;
        private Decimal Tope_Salarios_Minimos;  // tope valor fical (pensionado)
        private Decimal C_Minima;
        private int Anio_Calculo;
        private Dictionary<String, String> Dicc_IDs_Conceptos;
        private Dictionary<String, Decimal> Dicc_IDs_Tasas;

        private String Tipo_Concepto;
        private String _Estatus;
        private String _Estatus_Excluir;             // cuentas a excluir de la generacion
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

        private Decimal _Total_Recargos_Generados;
        private Decimal _Total_Corriente;
        private Decimal _Total_Rezago;
        private String _Periodo_Rezago;
        private String _Periodo_Corriente;

        private DateTime Desde_Fecha;
        private DateTime Hasta_Fecha;
        private String Orden_Dinamico;

        private bool _Validar_Convenios_Cumplidos;
        private bool _Incluir_Propietario;

        private Boolean Validar_Contrarecibos_Pagados;

    	public Cls_Ope_Pre_Resumen_Predio_Negocio()
    	{
        }

        #region Variables Internas
            private String Cuenta_Predial_ID = null;
            private String Cuenta_Predial = null;
            private String Tipo_Predio = null;
            private String Tasa_Predial_ID = null;
            private String Estado_Predio = null;
            private String Calle_ID = null;
            private String Colonia_ID = null;
            private String Ciudad_ID = null;
            private String Propietario_ID = null;
            private String Contribuyente_ID = null;
            private String Uso_Suelo_ID = null;
            private String No_Convenio = null;
            private String No_Cuota_Fija = null;
        #endregion

        #region Variables Publicas

            public String P_Tasa_Predial_ID
            {
                get
                {
                    return Tasa_Predial_ID;
                }
                set
                {
                    Tasa_Predial_ID = value;
                }
            }
            public String P_Ciudad_ID
            {
                get
                {
                    return Ciudad_ID;
                }
                set
                {
                    Ciudad_ID = value;
                }
            }
            public String P_No_Cuota_Fija
            {
                get
                {
                    return No_Cuota_Fija;
                }
                set
                {
                    No_Cuota_Fija = value;
                }
            }
            public String P_No_Convenio
            {
                get
                {
                    return No_Convenio;
                }
                set
                {
                    No_Convenio = value;
                }
            }
            public String P_Cuenta_Predial
            {
                get
                {
                    return Cuenta_Predial;
                }
                set
                {
                    Cuenta_Predial = value;
                }
            }
            public String P_Uso_Suelo_ID
            {
                get
                {
                    return Uso_Suelo_ID;
                }
                set
                {
                    Uso_Suelo_ID = value;
                }
            }
            public String P_Colonia_ID
            {
                get
                {
                    return Colonia_ID;
                }
                set
                {
                    Colonia_ID = value;
                }
            }
            public String P_Calle_ID
            {
                get
                {
                    return Calle_ID;
                }
                set
                {
                    Calle_ID = value;
                }
            }
            public String P_Contribuyente_ID
            {
                get
                {
                    return Contribuyente_ID;
                }
                set
                {
                    Contribuyente_ID = value;
                }
            }
            public String P_Propietario_ID
            {
                get
                {
                    return Propietario_ID;
                }
                set
                {
                    Propietario_ID = value;
                }
            }
            public String P_Estado_Predio
            {
                get
                {
                    return Estado_Predio;
                }
                set
                {
                    Estado_Predio = value;
                }
            }
            public String P_Tipo_Predio
            {
                get
                {
                    return Tipo_Predio;
                }
                set
                {
                    Tipo_Predio = value;
                }
            }
            public String P_Cuenta_Predial_ID
            {
                get { 
                    return Cuenta_Predial_ID; 
                }
                set { 
                    Cuenta_Predial_ID = value; 
                }
            }


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

            public Decimal p_Total_Recargos_Generados
            {
                get { return _Total_Recargos_Generados; }
                set { _Total_Recargos_Generados = value; }
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

            public bool P_Validar_Convenios_Cumplidos
            {
                get { return _Validar_Convenios_Cumplidos; }
                set { _Validar_Convenios_Cumplidos = value; }
            }

            public bool P_Incluir_Propietario
            {
                get { return _Incluir_Propietario; }
                set { _Incluir_Propietario = value; }
            }

            public bool P_Validar_Contrarecibos_Pagados
            {
                get { return Validar_Contrarecibos_Pagados; }
                set { Validar_Contrarecibos_Pagados = value; }
            }

            public DateTime P_Desde_Fecha
            {
                get { return Desde_Fecha; }
                set { Desde_Fecha = value; }
            }

            public DateTime P_Hasta_Fecha
            {
                get { return Hasta_Fecha; }
                set { Hasta_Fecha = value; }
            }
            public String p_Orden_Dinamico
            {
                get { return Orden_Dinamico; }
                set { Orden_Dinamico = value; }
            }


        #endregion

        #region Metodos
            public DataTable Consultar_Beneficio()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Beneficio(this);
            }
            public DataTable Consultar_Descuentos_Recargos()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Descuentos_Recargos(this);
            }
            public DataTable Consultar_Descuentos_Pronto_Pago()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Descuentos_Pronto_Pago(this);
            }

            public DataTable Consultar_Adeudos_Cuenta_Predial_Con_Totales(String Cuenta_Predial, String Estatus, Int32 Desde_Anio, Int32 Hasta_Anio)
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Cuenta_Predial, Estatus, Desde_Anio, Hasta_Anio);
            }

            public DataTable Consultar_Adeudos_Cancelados_Cuenta_Predial_Con_Totales(String Cuenta_Predial, String No_Orden_Variacion, String Estatus, Int32 Desde_Anio, Int32 Hasta_Anio)
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Adeudos_Cancelados_Cuenta_Predial_Con_Totales(Cuenta_Predial, No_Orden_Variacion, Estatus, Desde_Anio, Hasta_Anio);
            }

            public DataTable Consultar_Estatus_Cuentas()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Estatus_Cuentas(this);
            }

            public DataTable Consultar_Tasa()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Tasa(this);
            }
            public DataTable Detalles_Cuenta()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Detalles_Cuenta(this);
            }
            public DataTable Consultar_Ultimo_Movimiento()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Ultimo_Movimiento(this);
            }
            public DataTable Consultar_Ciudad()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Ciudad(this);
            }
            public DataTable Consultar_Imprimir_Resumen_Generales()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Imprimir_Resumen_Generales(this);
            }
            public DataSet  Consulta_Datos_Cuenta_Generales()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consulta_Datos_Cuenta_Generales(this);
            }

            public DataTable Consultar_Estado_Cuenta()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Estado_Cuenta(this);
            }
            public DataTable Consultar_Convenios_Detalles()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Convenios_Detalles(this);
            }
            public DataTable Consultar_Cuota_Fija_Detalles()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Cuota_Fija_Detalles(this);
            }

            ///*******************************************************************************************************
            /// NOMBRE_FUNCIÓN: Consultar_Convenios
            /// DESCRIPCIÓN: Llama al método en la capa de datos para consultar convenios y cambia el 
            ///             estatus del convenio de predial si está por pagar pero tiene parcialidades vencidas
            ///             Sólo si se especifica la propiedad _Validar_Convenios_Cumplidos
            /// PARÁMETROS:
            /// CREO: Roberto González Oseguera
            /// FECHA_CREO: 24-feb-2012
            /// MODIFICÓ: 
            /// FECHA_MODIFICÓ: 
            /// CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public DataTable Consultar_Convenios()
            {
                DataTable Dt_Convenios;
                DataTable Dt_Parcialidades;
                var Consultar_Convenios_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();
                var Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                DateTime Fecha_Periodo;
                DateTime Fecha_Vencimiento;
                int Dias = 0;
                int Meses = 0;
                int Contador_Convenios;
                int Limite_Convenios;
                int Parcialidad;

                // si no se especifica _Validar_Convenios_Cumplidos se regresa la consulta sin modificar
                if (_Validar_Convenios_Cumplidos == false)
                {
                    return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Convenios(this);
                }
                else
                {
                    Dt_Convenios = Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Convenios(this);
                }

                // limitar el numero de convenios para evitar que se tarde demasiado
                Limite_Convenios = Dt_Convenios.Rows.Count > 400 ? 400 : Dt_Convenios.Rows.Count;

                // recorrer los convenios
                for (Contador_Convenios = Limite_Convenios - 1; Contador_Convenios >= 0; Contador_Convenios--)
                {
                    string Numero_Convenio = Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                    // si el estatus es ACTIVO, consultar las parcialidades del convenio
                    if (Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Estatus].ToString().Trim() == "ACTIVO"
                        && Numero_Convenio.StartsWith("CPRE") && Numero_Convenio.Length > 4)
                    {
                    // recuperar numero de convenio (quitar CPRE y agregar ceros)
                        Consultar_Convenios_Predial.P_No_Convenio = Numero_Convenio.Substring(4, Numero_Convenio.Length - 4).PadLeft(10, '0');
                        Dt_Parcialidades = Consultar_Convenios_Predial.Consultar_Parcialidades_Ultimo_Convenio();

                        // recorrer las parcialidades del convenio
                        for (Parcialidad = 0; Parcialidad < Dt_Parcialidades.Rows.Count; Parcialidad++)
                        {
                            // si el estatus de la parcialidad es POR PAGAR
                            if (Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString().Trim() == "POR PAGAR")
                            {
                                // obtener la fecha de vencimiento de la parcialidad
                                DateTime.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString().Trim(), out Fecha_Periodo);
                                Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo.ToShortDateString(), "10");
                                // obtener el tiempo transcurrido desde la fecha de vencimiento
                                Calcular_Tiempo_Entre_Fechas(Fecha_Vencimiento, DateTime.Now, out Dias, out Meses);
                                // si el numero de dias transcurridos en mayor que cero, el convenio esta vencido
                                if (Dias > 0)
                                {
                                    // actualizar el estatus del convenio
                                    Dt_Convenios.Rows[Contador_Convenios].BeginEdit();
                                    Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Estatus] = "INCUMPLIDO";
                                    Dt_Convenios.AcceptChanges();
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                    }

                    int Hasta_Anio = 0;
                    decimal Adeudo_Predial = 0;
                    decimal Monto;
                    // si el estatus es INCUMPLIDO, consultar las parcialidades del convenio para validar que aún hay adeudo
                    if (Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Estatus].ToString().Trim() == "INCUMPLIDO"
                        && Numero_Convenio.StartsWith("CPRE") && Numero_Convenio.Length > 4)
                    {
                        DataTable Dt_Adeudos;
                        var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
                        Consultar_Convenios_Predial.P_No_Convenio = Numero_Convenio.Substring(4, Numero_Convenio.Length - 4).PadLeft(10, '0');
                        Dt_Parcialidades = Consultar_Convenios_Predial.Consultar_Parcialidades_Ultimo_Convenio();

                        // comprobar que la consulta de parcializades regresó resultados
                        if (Dt_Parcialidades != null && Dt_Parcialidades.Rows.Count > 0)
                        {
                            // obtener el ultimo bimestre incluido
                            Parcialidad = Dt_Parcialidades.Rows.Count - 1;
                            while (Parcialidad >= 0)
                            {
                                // obtener el ultimo periodo incluido en el convenio
                                if (Hasta_Anio <= 0)
                                {
                                    string Periodo_Parcialidad = Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString();
                                    if (Periodo_Parcialidad.Trim().Length >= 13)
                                    {
                                        int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 4, 4), out Hasta_Anio);
                                        break;
                                    }
                                }
                                Parcialidad--;
                            }

                            // si no se tiene el ID de la cuenta, consultar a partir de la cuenta predial
                            if (string.IsNullOrEmpty(P_Cuenta_Predial_ID) && !string.IsNullOrEmpty(P_Cuenta_Predial))
                            {
                                var Consulta_Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                                Consulta_Cuenta.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                                Consulta_Cuenta.P_Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '"
                                    + P_Cuenta_Predial + "'";
                                DataTable Dt_Datos_Cuenta = Consulta_Cuenta.Consultar_Cuenta();
                                // validar datos regresados por la cuenta
                                if (Dt_Datos_Cuenta != null && Dt_Datos_Cuenta.Rows.Count > 0)
                                {
                                    // consulta de adeudos
                                    string Cuenta_ID_Busqueda = Dt_Datos_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                                    Dt_Adeudos = Consulta_Adeudos.Consultar_Adeudos_Cuenta_Predial(Cuenta_ID_Busqueda, "POR PAGAR", Hasta_Anio, Hasta_Anio);
                                    if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
                                    {
                                        for (int i = 1; i <= 6; i++)
                                        {
                                            decimal.TryParse(Dt_Adeudos.Rows[0]["ADEUDO_BIMESTRE_" + i].ToString(), out Monto);
                                            Adeudo_Predial += Monto;
                                        }
                                    }
                                }
                            }
                        }

                        // si no ahy adeudo cancelar convenio
                        if (Adeudo_Predial <= 0)
                        {
                            // recorrer las parcialidades del convenio
                            for (Parcialidad = 0; Parcialidad < Dt_Parcialidades.Rows.Count; Parcialidad++)
                            {
                                // actualizar el estatus del convenio
                                Dt_Convenios.Rows[Contador_Convenios].BeginEdit();
                                Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Estatus] = "CANCELADO";
                                Dt_Convenios.Rows[Contador_Convenios].EndEdit();
                                Dt_Convenios.AcceptChanges();
                            }
                        }
                    }
                    else // pasar al siguiente convenio
                    {
                        continue;
                    }

                }
                return Dt_Convenios;
            }

            ///*******************************************************************************************************
            /// NOMBRE_FUNCIÓN: Calcular_Tiempo_Entre_Fechas
            /// DESCRIPCIÓN: Calcular numero de dias y meses transcurridos entre una fecha y otra
            /// PARÁMETROS:
            /// 		1. Desde_Fecha: Fecha inferior a tomar
            /// 		2. Hasta_Fecha: Fecha final hasta la que se calcula
            /// 		3. Dias: Se almacenan los dias de diferencia entre las fechas
            /// 		4. Meses: Almacena los meses transcurridos entre una fecha y otra
            /// CREO: Roberto González Oseguera
            /// FECHA_CREO: 12-ago-2011
            /// MODIFICÓ: 
            /// FECHA_MODIFICÓ: 
            /// CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            private void Calcular_Tiempo_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha, out Int32 Dias, out Int32 Meses)
            {
                TimeSpan Transcurrido = Hasta_Fecha - Desde_Fecha;
                if (Transcurrido > TimeSpan.Parse("0"))
                {
                    DateTime Tiempo = DateTime.MinValue + Transcurrido;
                    Meses = ((Tiempo.Year - 1) * 12) + (Tiempo.Month - 1);

                    long tickDiff = Hasta_Fecha.Ticks - Desde_Fecha.Ticks;
                    tickDiff = tickDiff / 10000000; // segundos
                    Dias = (int)(tickDiff / 86400);
                }
                else
                {
                    Dias = 0;
                    Meses = 0;
                }
            }

            public DataTable Consultar_Historial_Movimientos()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Historial_Movimientos(this);
            }
            public DataTable Consultar_Historial_Pagos()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Historial_Pagos(this);
            }
            public DataSet Consulta_Datos_Cuenta_Impuestos()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consulta_Datos_Cuenta_Impuestos(this);
            }
            public DataTable Consultar_Uso_Predio()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Uso_Predio(this);
            }
            public DataTable Consultar_Cuenta_Predial()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Cuenta_Predial(this);
            }
            public DataTable Consultar_Tipo_Predio()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Tipo_Predio(this);
            }

            public DataTable Consultar_Estado_Predio_Propietario()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Estado_Predio_Propietario(this);
            }
            public DataTable Consultar_Estado_Predio()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Estado_Predio(this);
            }
            public DataTable Consultar_Propietario()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Propietario(this);
            }
            public DataTable Consultar_Contribuyentes()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Contribuyentes(this);
            }
            public DataTable Consultar_Calle_Generales()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Calle_Generales(this);
            }
            public DataTable Consultar_Colonia_Generales()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Colonia_Generales(this);
            }
            public DataTable Consultar_Copropietarios()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Copropietarios(this);
            }
            public DataTable Consultar_Pagos_Fraccionamientos()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Pagos_Fraccionamientos(this);
            }
            public DataTable Consultar_Pagos_Derechos_Supervision()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Pagos_Derechos_Supervision(this);
            }
            public DataTable Consultar_Pagos_Constancias()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Pagos_Constancias(this);
            }
            public DataTable Consultar_Pagos_Traslado()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Pagos_Traslado(this);
            }
            public DataTable Consultar_Pagos_Predial_Por_Periodo()
            {
                return Cls_Ope_Pre_Resumen_Predio_Datos.Consultar_Pagos_Predial_Por_Periodo(this);
            }
            ///*******************************************************************************************************
            /// NOMBRE_FUNCIÓN: Formar_Tabla_Recargos
            /// DESCRIPCIÓN: Crear tabla con columnas para almacenar recargos
            /// PARÁMETROS:
            /// CREO: Roberto González Oseguera
            /// FECHA_CREO: 08-ago-2011 
            /// MODIFICÓ: 
            /// FECHA_MODIFICÓ: 
            /// CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            private DataTable Formar_Tabla_Recargos()
            {
                // tabla y columnas
                DataTable Dt_Recargos = new DataTable();
                DataColumn Dc_Periodo = new DataColumn();
                DataColumn Dc_Tasa = new DataColumn();
                DataColumn Dc_Adeudo = new DataColumn();
                DataColumn Dc_Recargo = new DataColumn();

                // tipo de dato de las columnas
                Dc_Periodo.DataType = System.Type.GetType("System.String");
                Dc_Tasa.DataType = System.Type.GetType("System.Decimal");
                Dc_Adeudo.DataType = System.Type.GetType("System.Decimal");
                Dc_Recargo.DataType = System.Type.GetType("System.Decimal");
                // nombre de columnas
                Dc_Periodo.ColumnName = "PERIODO";
                Dc_Tasa.ColumnName = "TASA";
                Dc_Adeudo.ColumnName = "ADEUDO";
                Dc_Recargo.ColumnName = "RECARGOS";
                // agregar columnas a la tabla
                Dt_Recargos.Columns.Add(Dc_Periodo);
                Dt_Recargos.Columns.Add(Dc_Tasa);
                Dt_Recargos.Columns.Add(Dc_Adeudo);
                Dt_Recargos.Columns.Add(Dc_Recargo);
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
            /// MODIFICÓ: 
            /// FECHA_MODIFICÓ: 
            /// CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public DataTable Calcular_Recargos_Predial(String Cuenta_Predial)
            {
                Int32 Mes_Actual = DateTime.Now.Month;
                Int32 Anio_Actual = DateTime.Now.Year;
                Decimal Total_Recargos = 0;
                Decimal Total_Rezago = 0;
                Decimal Total_Corriente = 0;
                String Periodo_Rezago_Desde = "-";
                String Periodo_Rezago_Hasta = "-";
                String Periodo_Corriente_Desde = "-";
                String Periodo_Corriente_Hasta = "";
                Decimal Anio;
                Dictionary<String, Decimal> Dicc_Tabulador_recargos = new Dictionary<String, Decimal>();

                // tabla para adeudos
                DataTable Dt_Recargos = Formar_Tabla_Recargos();

                Cls_Cat_Pre_Tabulador_Recargos_Negocio Rs_Recargos_Cuentas = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
                DataTable Dt_Adeudos;

                try
                {
                    // consultar adeudos de la cuenta con estatus por pagar
                    Dt_Adeudos = Cls_Ope_Pre_Generar_Adeudo_Predial_Datos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial, null, 0, 0);

                    // obtener el tabulador como diccionario
                    Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);


                    // para cada adeudo encontrado,
                    foreach (DataRow Adeudo in Dt_Adeudos.Rows)
                    {
                        // obtener el anio del adeudo
                        if (Decimal.TryParse(Adeudo[Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio))
                        {

                            // si el anio es anterior al actual, todos los bimestres son rezago
                            if (Anio < DateTime.Now.Year)
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
                                        throw new Exception("No se encontró la tasa para calcular los recargos del periodo 0" + i + "-" + Anio + "<br />");
                                    }
                                    Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + i.ToString()].ToString(), out Adeudo_Bimestre);
                                    Nuevo_Recargo["ADEUDO"] = Adeudo_Bimestre;
                                    Nuevo_Recargo["RECARGOS"] = (Decimal)Nuevo_Recargo["TASA"] * (Decimal)Nuevo_Recargo["ADEUDO"] / 100;
                                    Dt_Recargos.Rows.Add(Nuevo_Recargo);
                                    Total_Rezago += (Decimal)Nuevo_Recargo["ADEUDO"];
                                    Total_Recargos += (Decimal)Nuevo_Recargo["RECARGOS"];
                                    // si hay adeudo, escribir periodo rezago
                                    if (Adeudo_Bimestre > 0 && Periodo_Rezago_Desde == "-")
                                    {
                                        Periodo_Rezago_Desde = "0" + i + "-" + Anio;
                                    }
                                    Periodo_Rezago_Hasta = "0" + i + "-" + Anio;
                                }

                            }
                            // para los adeudo del anio actual, validar corriente y rezago
                            else if (Anio == DateTime.Now.Year)
                            {
                                int Numero_Bimestre = 1;
                                DataRow Nuevo_Recargo;
                                int Bimestre_Rezago = Mes_Actual / 2;

                                for (Numero_Bimestre = 1; Numero_Bimestre <= Bimestre_Rezago; Numero_Bimestre++)
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
                                        throw new Exception("No se encontró la tasa para calcular los recargos del periodo 0" + Numero_Bimestre + "-" + Anio + "<br />");
                                    }
                                    Nuevo_Recargo["TASA"] = Dicc_Tabulador_recargos[Numero_Bimestre.ToString() + Anio.ToString()];
                                    Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()].ToString(), out Adeudo_Bimestre);
                                    Nuevo_Recargo["ADEUDO"] = Adeudo_Bimestre;
                                    Nuevo_Recargo["RECARGOS"] = Decimal.Round((Decimal)Nuevo_Recargo["TASA"] * (Decimal)Nuevo_Recargo["ADEUDO"] / 100, 2);
                                    Dt_Recargos.Rows.Add(Nuevo_Recargo);
                                    Total_Rezago += (Decimal)Nuevo_Recargo["ADEUDO"];
                                    Total_Recargos += (Decimal)Nuevo_Recargo["RECARGOS"];
                                    // si hay adeudo, escribir periodo rezago
                                    if (Adeudo_Bimestre > 0 && Periodo_Rezago_Desde == "-")
                                    {
                                        Periodo_Rezago_Desde = "0" + Numero_Bimestre + "-" + Anio;
                                    }
                                    Periodo_Rezago_Hasta = "0" + Numero_Bimestre + "-" + Anio;
                                }
                                while (Numero_Bimestre <= 6)
                                {
                                    Decimal Adeudo_Bimestre;
                                    // entradas en la tabla del adeudo corriente
                                    Nuevo_Recargo = Dt_Recargos.NewRow();
                                    Nuevo_Recargo["PERIODO"] = Numero_Bimestre.ToString() + Anio.ToString();
                                    Nuevo_Recargo["TASA"] = 0;
                                    Decimal.TryParse(Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()].ToString(), out Adeudo_Bimestre);
                                    Nuevo_Recargo["ADEUDO"] = Adeudo_Bimestre;
                                    Nuevo_Recargo["RECARGOS"] = 0;
                                    Dt_Recargos.Rows.Add(Nuevo_Recargo);

                                    // solo sumar el corriente
                                    Total_Corriente += (Decimal)Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()];
                                    // si hay adeudo, escribir periodo rezago
                                    if ((Decimal)Adeudo["ADEUDO_BIMESTRE_" + Numero_Bimestre.ToString()] > 0 && Periodo_Corriente_Desde == "-")
                                    {
                                        Periodo_Corriente_Desde = "0" + Numero_Bimestre + "-" + Anio;
                                    }
                                    Periodo_Corriente_Hasta = "0" + Numero_Bimestre + "-" + Anio;
                                    Numero_Bimestre++;
                                }
                            }
                        }
                    }
                    // copiar montos totales en propiedades
                    p_Total_Corriente = Total_Corriente;
                    p_Total_Rezago = Total_Rezago;
                    p_Total_Recargos_Generados = Decimal.Round(Total_Recargos, 2);
                    p_Periodo_Corriente = Periodo_Corriente_Desde + "  " + Periodo_Corriente_Hasta;
                    p_Periodo_Rezago = Periodo_Rezago_Desde + "  " + Periodo_Rezago_Hasta;

                    return Dt_Recargos;
                }
                catch (Exception ex)
                {
                    throw new Exception("Calcular_Recargos_Predial: " + ex.Message.ToString(), ex);
                }
            }// termina metodo Generar_Impuesto_Cierre_Anual
        #endregion



    }
}
