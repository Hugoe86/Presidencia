using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Operacion_Predial_Convenios_Predial.Datos;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;

namespace Presidencia.Operacion_Predial_Convenios_Predial.Negocio
{

    public class Cls_Ope_Pre_Convenios_Predial_Negocio
    {

        #region Varibles Internas

        private String No_Convenio;
        private String Cuenta_Predial_ID;
        private String Propietario_ID;
        private String Realizo;
        private String No_Reestructura;
        private String Estatus;
        private String Estatus_Cancelacion_Cuenta;
        private String Solicitante;
        private String RFC;
        private Int32 Numero_Parcialidades;
        private String Periodicidad_Pago;
        private String Hasta_Periodo;
        private DateTime Fecha;
        private DateTime Fecha_Vencimiento;
        private String Observaciones;
        private Decimal Descuento_Recargos_Ordinarios;
        private Decimal Descuento_Recargos_Moratorios;
        private Decimal Descuento_Multas;
        private Decimal Total_Adeudo;
        private Decimal Total_Descuento;
        private Decimal Sub_Total;
        private Decimal Porcentaje_Anticipo;
        private Decimal _Adeudo_Corriente;
        private Decimal _Adeudo_Rezago;
        private Decimal Total_Predial = 0;
        private Decimal Total_Recargos = 0;
        private Decimal Total_Moratorios = 0;
        private Decimal Total_Honorarios = 0;
        private Decimal Total_Anticipo = 0;
        private Decimal Total_Convenio = 0;
        private String Usuario;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private Boolean Campos_Foraneos;
        private DataTable Dt_Parcialidades;
        private DataTable Dt_Desglose_Parcialidades;
        private Boolean Mostrar_Ultimo_Convenio;
        private Boolean Reestructura;
        private Boolean Join_Contrarecibo;
        private String Contrarecibo_Estatus;
        private String _No_Descuento;
        private String _Ruta_Convenio_Escaneado;
        private String _Parcialidades_Manual = "";

        private bool _Validar_Convenios_Cumplidos;
        //Para las validaciones

        private Boolean Incluir_Campos_Foraneos;
        private String Generar_Orden_Estatus;
        private String Orden_Variacion_ID;
        private String Cuenta_Predial;
        private String Generar_Orden_Movimiento_ID;
        private String Contrarecibo;
        private String Generar_Orden_Anio;
        private DataTable Generar_Orden_Dt_Detalles;
        private DataTable Dt_Observaciones;
        private String Observaciones_No_Orden_Variacion;
        private String Año;
        private String Observaciones_Observacion_ID;

        private OracleCommand Cmmd;

        #endregion

        #region Varibles Publicas

        public String P_No_Convenio
        {
            get { return No_Convenio; }
            set { No_Convenio = value; }
        }

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }

        public String P_Propietario_ID
        {
            get { return Propietario_ID; }
            set { Propietario_ID = value; }
        }

        public String P_Realizo
        {
            get { return Realizo; }
            set { Realizo = value; }
        }

        public String P_No_Reestructura
        {
            get { return No_Reestructura; }
            set { No_Reestructura = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Estatus_Cancelacion_Cuenta
        {
            get { return Estatus_Cancelacion_Cuenta; }
            set { Estatus_Cancelacion_Cuenta = value; }
        }


        public String P_Solicitante
        {
            get { return Solicitante; }
            set { Solicitante = value; }
        }

        public String P_RFC
        {
            get { return RFC; }
            set { RFC = value; }
        }

        public Int32 P_Numero_Parcialidades
        {
            get { return Numero_Parcialidades; }
            set { Numero_Parcialidades = value; }
        }

        public String P_Periodicidad_Pago
        {
            get { return Periodicidad_Pago; }
            set { Periodicidad_Pago = value; }
        }

        public String P_Hasta_Periodo
        {
            get { return Hasta_Periodo; }
            set { Hasta_Periodo = value; }
        }

        public DateTime P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public DateTime P_Fecha_Vencimiento
        {
            get { return Fecha_Vencimiento; }
            set { Fecha_Vencimiento = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public Decimal P_Descuento_Recargos_Ordinarios
        {
            get { return Descuento_Recargos_Ordinarios; }
            set { Descuento_Recargos_Ordinarios = value; }
        }

        public Decimal P_Descuento_Recargos_Moratorios
        {
            get { return Descuento_Recargos_Moratorios; }
            set { Descuento_Recargos_Moratorios = value; }
        }

        public Decimal P_Descuento_Multas
        {
            get { return Descuento_Multas; }
            set { Descuento_Multas = value; }
        }

        public Decimal P_Total_Adeudo
        {
            get { return Total_Adeudo; }
            set { Total_Adeudo = value; }
        }

        public Decimal P_Total_Descuento
        {
            get { return Total_Descuento; }
            set { Total_Descuento = value; }
        }

        public Decimal P_Sub_Total
        {
            get { return Sub_Total; }
            set { Sub_Total = value; }
        }

        public Decimal P_Porcentaje_Anticipo
        {
            get { return Porcentaje_Anticipo; }
            set { Porcentaje_Anticipo = value; }
        }

        public Decimal P_Adeudo_Corriente
        {
            get { return _Adeudo_Corriente; }
            set { _Adeudo_Corriente = value; }
        }

        public Decimal P_Adeudo_Rezago
        {
            get { return _Adeudo_Rezago; }
            set { _Adeudo_Rezago = value; }
        }

        public Decimal P_Total_Predial
        {
            get { return Total_Predial; }
            set { Total_Predial = value; }
        }

        public Decimal P_Total_Recargos
        {
            get { return Total_Recargos; }
            set { Total_Recargos = value; }
        }

        public Decimal P_Total_Moratorios
        {
            get { return Total_Moratorios; }
            set { Total_Moratorios = value; }
        }

        public Decimal P_Total_Honorarios
        {
            get { return Total_Honorarios; }
            set { Total_Honorarios = value; }
        }

        public Decimal P_Total_Anticipo
        {
            get { return Total_Anticipo; }
            set { Total_Anticipo = value; }
        }

        public Decimal P_Total_Convenio
        {
            get { return Total_Convenio; }
            set { Total_Convenio = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value; }
        }

        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value; }
        }

        public String P_Contrarecibo_Estatus
        {
            get { return Contrarecibo_Estatus; }
            set { Contrarecibo_Estatus = value; }
        }
        public String P_Ruta_Convenio_Escaneado
        {
            get { return _Ruta_Convenio_Escaneado; }
            set { _Ruta_Convenio_Escaneado = value; }
        }
        public String P_Parcialidades_Manual
        {
            get { return _Parcialidades_Manual; }
            set { _Parcialidades_Manual = value; }
        }

        public bool P_Validar_Convenios_Cumplidos
        {
            get { return _Validar_Convenios_Cumplidos; }
            set { _Validar_Convenios_Cumplidos = value; }
        }


        public String P_No_Descuento
        {
            get { return _No_Descuento; }
            set { _No_Descuento = value; }
        }

        public String P_Agrupar_Dinamico
        {
            get { return Agrupar_Dinamico; }
            set { Agrupar_Dinamico = value; }
        }

        public String P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value; }
        }

        public Boolean P_Campos_Foraneos
        {
            get { return Campos_Foraneos; }
            set { Campos_Foraneos = value; }
        }

        public DataTable P_Dt_Parcialidades
        {
            get { return Dt_Parcialidades; }
            set { Dt_Parcialidades = value; }
        }
        public DataTable P_Dt_Desglose_Parcialidades
        {
            get { return Dt_Desglose_Parcialidades; }
            set { Dt_Desglose_Parcialidades = value; }
        }

        public Boolean P_Mostrar_Ultimo_Convenio
        {
            get { return Mostrar_Ultimo_Convenio; }
            set { Mostrar_Ultimo_Convenio = value; }
        }

        public Boolean P_Incluir_Campos_Foraneos
        {
            get { return Incluir_Campos_Foraneos; }
            set { Incluir_Campos_Foraneos = value; }
        }

        public Boolean P_Join_Contrarecibo
        {
            get { return Join_Contrarecibo; }
            set { Join_Contrarecibo = value; }
        }

        public String P_Generar_Orden_Estatus
        {
            get { return Generar_Orden_Estatus; }
            set { Generar_Orden_Estatus = value; }
        }

        public String P_Orden_Variacion_ID
        {
            get { return Orden_Variacion_ID; }
            set { Orden_Variacion_ID = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }

        public String P_Generar_Orden_Movimiento_ID
        {
            get { return Generar_Orden_Movimiento_ID; }
            set { Generar_Orden_Movimiento_ID = value; }
        }

        public String P_Contrarecibo
        {
            get { return Contrarecibo; }
            set { Contrarecibo = value; }
        }

        public String P_Generar_Orden_Anio
        {
            get { return Generar_Orden_Anio; }
            set { Generar_Orden_Anio = value; }
        }

        public DataTable P_Generar_Orden_Dt_Detalles
        {
            get { return Generar_Orden_Dt_Detalles; }
            set { Generar_Orden_Dt_Detalles = value; }
        }

        public DataTable P_Dt_Observaciones
        {
            get { return Dt_Observaciones; }
            set { Dt_Observaciones = value; }
        }

        public String P_Observaciones_No_Orden_Variacion
        {
            get { return Observaciones_No_Orden_Variacion; }
            set { Observaciones_No_Orden_Variacion = value; }
        }

        public String P_Año
        {
            get { return Año; }
            set { Año = value; }
        }

        public String P_Observaciones_Observacion_ID
        {
            get { return Observaciones_Observacion_ID; }
            set { Observaciones_Observacion_ID = value; }
        }

        public Boolean P_Reestructura
        {
            get { return Reestructura; }
            set { Reestructura = value; }
        }

        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Convenio_Predial()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Alta_Convenio_Predial(this);
        }

        public Boolean Alta_Reestructura_Convenio_Predial()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Alta_Reestructura_Convenio_Predial(this);
        }

        public Boolean Modificar_Convenio_Predial()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Modificar_Convenio_Predial(this);
        }

        public Boolean Modificar_Reestructura_Convenio_Predial()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Modificar_Reestructura_Convenio_Predial(this);
        }

        public Boolean Modificar_Estatus_Convenio_Reestructura()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Modificar_Estatus_Convenio_Reestructura(this);
        }

        public int Actualizar_Ruta_Convenio_Escaneado()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Actualizar_Ruta_Convenio_Escaneado(this);
        }

        public DataTable Consultar_Propietarios_Variacion(String Cuenta_Predial_ID, String No_Orden_Variacion, String Anio_Orden)
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Propietarios_Variacion(Cuenta_Predial_ID, No_Orden_Variacion, Anio_Orden);
        }

        public DataTable Consultar_Ordenes_Variacion()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Ordenes_Variacion(this);
        }

        public DataTable Consultar_Adeudos_Cuenta()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Adeudos_Cuenta(this);
        }

        public DataTable Consultar_Adeudos_Convenio(Boolean Total_O_A_Pagar)
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Adeudos_Convenio(this, Total_O_A_Pagar);
        }

        public DataTable Consultar_Estatus_Archivo_Convenio()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Estatus_Archivo_Convenio(this);
        }

        public DataTable Consultar_Parcialidades_Ultimo_Convenio()
        {
            return Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Parcialidades_Ultimo_Convenio(this);
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Convenio_Predial
        /// DESCRIPCIÓN: Llama al método en la capa de datos para consultar convenios y cambia el 
        ///             estatus del convenio si está por pagar pero tiene parcialidades vencidas
        ///             Sólo si se especifica la propiedad _Validar_Convenios_Cumplidos
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-feb-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public DataTable Consultar_Convenio_Predial()
        {
            DataTable Dt_Convenios;
            DataTable Dt_Parcialidades_Convenio;
            var Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
            DateTime Fecha_Periodo;
            DateTime Fecha_Vencimiento;
            int Dias = 0;
            int Meses = 0;
            int Contador_Convenios;
            int Limite_Convenios;
            int Parcialidad;

            String Orden_Dinamico;

            // si no se especifica _Validar_Convenios_Cumplidos se regresa la consulta sin modificar
            if (_Validar_Convenios_Cumplidos == false)
            {
                return Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Convenio_Predial(this);
            }
            else
            {
                Dt_Convenios = Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Convenio_Predial(this);
            }
            // almacenar orden dinamico en variable local y borrar propiedad para evitar error al consultar parcialidades
            Orden_Dinamico = this.P_Ordenar_Dinamico;
            this.P_Ordenar_Dinamico = "";

            // limitar el numero de convenios para evitar que se tarde demasiado
            Limite_Convenios = Dt_Convenios.Rows.Count > 400 ? 400 : Dt_Convenios.Rows.Count;

            // asignar nulo al datatable de parcialidades
            P_Dt_Parcialidades = null;

            // recorrer los convenios
            for (Contador_Convenios = Limite_Convenios - 1; Contador_Convenios >= 0; Contador_Convenios--)
            {
                // si el estatus es ACTIVO, consultar las parcialidades del convenio
                if (Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Estatus].ToString().Trim() == "ACTIVO")
                {
                    this.P_No_Convenio = Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                    Dt_Parcialidades_Convenio = Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Parcialidades_Ultimo_Convenio(this);
                    P_Dt_Parcialidades = Dt_Parcialidades_Convenio;

                    // recorrer las parcialidades del convenio
                    for (Parcialidad = 0; Parcialidad < Dt_Parcialidades_Convenio.Rows.Count; Parcialidad++)
                    {
                        // si el estatus de la parcialidad es POR PAGAR
                        if (Dt_Parcialidades_Convenio.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString().Trim() == "POR PAGAR")
                        {
                            // obtener la fecha de vencimiento de la parcialidad
                            DateTime.TryParse(Dt_Parcialidades_Convenio.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString().Trim(), out Fecha_Periodo);
                            Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo.ToShortDateString(), "10");
                            // obtener el tiempo transcurrido desde la fecha de vencimiento
                            Calcular_Tiempo_Entre_Fechas(Fecha_Vencimiento, DateTime.Now, out Dias, out Meses);
                            // si el numero de dias transcurridos en mayor que cero, el convenio esta vencido
                            if (Dias > 0)
                            {
                                // actualizar el estatus de las siguientes parcialidades
                                for (int Contar_Parcialidades = Parcialidad; Contar_Parcialidades < Dt_Parcialidades_Convenio.Rows.Count; Contar_Parcialidades++)
                                {
                                    Dt_Parcialidades_Convenio.Rows[Contar_Parcialidades].BeginEdit();
                                    Dt_Parcialidades_Convenio.Rows[Contar_Parcialidades][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus] = "INCUMPLIDO";
                                    Dt_Parcialidades_Convenio.Rows[Contar_Parcialidades].EndEdit();
                                }
                                Dt_Parcialidades_Convenio.AcceptChanges();
                                // actualizar el estatus del convenio
                                Dt_Convenios.Rows[Contador_Convenios].BeginEdit();
                                Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Estatus] = "INCUMPLIDO";
                                Dt_Convenios.Rows[Contador_Convenios].EndEdit();
                                Dt_Convenios.AcceptChanges();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    P_Dt_Parcialidades = Dt_Parcialidades_Convenio;
                }
                
                // si el estatus es TERMINADO, consultar las parcialidades
                if (Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Estatus].ToString().Trim() == "TERMINADO" && P_Dt_Parcialidades == null)
                {
                    this.P_No_Convenio = Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                    Dt_Parcialidades_Convenio = Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Parcialidades_Ultimo_Convenio(this);
                    P_Dt_Parcialidades = Dt_Parcialidades_Convenio.Copy();
                    continue;
                }

                int Hasta_Anio = 0;
                decimal Adeudo_Predial = 0;
                decimal Monto;
                // si el estatus es INCUMPLIDO, consultar las parcialidades del convenio para validar que aún hay adeudo
                if (Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Estatus].ToString().Trim() == "INCUMPLIDO")
                {
                    DataTable Dt_Adeudos;
                    var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
                    // si P_Dt_Parcialidades no contiene datos, consultar parcialidades, si no, tomar de la propiedad
                    if (P_Dt_Parcialidades != null)
                    {
                        Dt_Parcialidades_Convenio = P_Dt_Parcialidades;
                    }
                    else
                    {
                        this.P_No_Convenio = Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                        Dt_Parcialidades_Convenio = Cls_Ope_Pre_Convenios_Predial_Datos.Consultar_Parcialidades_Ultimo_Convenio(this);
                        P_Dt_Parcialidades = Dt_Parcialidades_Convenio;
                    }

                    // comprobar que la consulta de parcializades regresó resultados
                    if (Dt_Parcialidades_Convenio != null && Dt_Parcialidades_Convenio.Rows.Count > 0)
                    {
                        // obtener el ultimo bimestre incluido
                        Parcialidad = Dt_Parcialidades_Convenio.Rows.Count - 1;
                        while (Parcialidad >= 0)
                        {
                            // obtener el ultimo periodo incluido en el convenio
                            if (Hasta_Anio <= 0)
                            {
                                string Periodo_Parcialidad = Dt_Parcialidades_Convenio.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString();
                                if (Periodo_Parcialidad.Trim().Length >= 13)
                                {
                                    int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 4, 4), out Hasta_Anio);
                                    break;
                                }
                            }
                            Parcialidad--;
                        }

                        // consulta de adeudos
                        string Cuenta_ID_Busqueda = Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id].ToString();
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

                    // si no hay adeudo cancelar convenio
                    if (Adeudo_Predial <= 0)
                    {
                        // recorrer las parcialidades del convenio
                        for (Parcialidad = 0; Parcialidad < Dt_Parcialidades_Convenio.Rows.Count; Parcialidad++)
                        {
                            // si el estatus de la parcialidad es INCUMPLIDO
                            if (Dt_Parcialidades_Convenio.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString().Trim() == "INCUMPLIDO")
                            {
                                // actualizar el estatus de las siguientes parcialidades
                                for (int Contar_Parcialidades = Parcialidad; Contar_Parcialidades < Dt_Parcialidades_Convenio.Rows.Count; Contar_Parcialidades++)
                                {
                                    Dt_Parcialidades_Convenio.Rows[Contar_Parcialidades].BeginEdit();
                                    Dt_Parcialidades_Convenio.Rows[Contar_Parcialidades][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus] = "CANCELADO";
                                    Dt_Parcialidades_Convenio.Rows[Contar_Parcialidades].EndEdit();
                                }
                                Dt_Parcialidades_Convenio.AcceptChanges();
                                // actualizar el estatus del convenio
                                Dt_Convenios.Rows[Contador_Convenios].BeginEdit();
                                Dt_Convenios.Rows[Contador_Convenios][Ope_Pre_Convenios_Predial.Campo_Estatus] = "CANCELADO";
                                Dt_Convenios.Rows[Contador_Convenios].EndEdit();
                                Dt_Convenios.AcceptChanges();

                                break;
                            }
                        }
                        P_Dt_Parcialidades = Dt_Parcialidades_Convenio;
                    }
                }
                else // pasar al siguiente convenio
                {
                    continue;
                }

            }
            // restaurar valor de orden dinamico
            this.P_Ordenar_Dinamico = Orden_Dinamico;

            return Dt_Convenios;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Recargos_Moratorios
        /// DESCRIPCIÓN: Regresa un decimal con el total de recargos moratorios para una cuenta dada (propiedad de la clase)
        ///             Consulta el último convenio de la cuenta y si está vencido lee los adeudos por
        ///             pagar y calcula el monto de recargos moratorios, tomando en cuenta adeudos 
        ///             vencido que no entraron en el convenio
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 23-feb-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public decimal Obtener_Recargos_Moratorios()
        {
            var Consulta_Convenios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
            DataTable Dt_Parcialidades;
            DataTable Dt_Convenios;
            Decimal Recargos_Moratorios = 0;
            Decimal Honorarios = 0;
            Decimal Monto_Impuesto = 0;
            Decimal Monto_Base = 0;
            Decimal Adeudo_Honorarios = 0;
            Decimal Adeudo_Moratorios = 0;
            String No_Convenio = "";
            int Parcialidad = 0;
            DateTime Fecha_Vencimiento = DateTime.MinValue;
            int Meses_Transcurridos = 0;
            int Hasta_Anio = 0;
            int Hasta_Bimestre = 0;

            // si no se especifica cuenta predial, regresar cero
            if (string.IsNullOrEmpty(P_Cuenta_Predial_ID))
            {
                return 0;
            }

            // omitir convenios de cuentas canceladas, si no hay filtros por estatus
            if (string.IsNullOrEmpty(this.P_Estatus))
            {
                this.P_Estatus = " !='CUENTA_CANCELADA'";
            }

            // consultar convenios de la cuenta
            this.P_Ordenar_Dinamico = Ope_Pre_Convenios_Predial.Campo_Fecha + " DESC,"
                + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " DESC";
            this.P_Validar_Convenios_Cumplidos = true;
            Dt_Convenios = this.Consultar_Convenio_Predial();
            // si la consulta arrojó resultado, utilizar el primer registro (convenio mas reciente)
            if (Dt_Convenios != null && Dt_Convenios.Rows.Count > 0)
            {
                No_Convenio = Dt_Convenios.Rows[0][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                // consultar las parcialidades del ultimo convenio guardado (convenio o ultima reestructura)
                Dt_Parcialidades = this.Dt_Parcialidades;
                if (Dt_Parcialidades != null)
                {
                    // llamar metodo para determinar si el convenio esta vencido
                    if (Convenio_Vencido(Dt_Parcialidades))
                    {
                        Parcialidad = Dt_Parcialidades.Rows.Count - 1;

                        // recorrer la tabla de parcialidades hasta encontrar parcialidades con estatus PAGADO
                        while (Parcialidad >= 0)
                        {
                            // obtener el ultimo periodo incluido en el convenio
                            if (Hasta_Anio <= 0 || Hasta_Bimestre <= 0)
                            {
                                string Periodo_Parcialidad = Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString();
                                if (Periodo_Parcialidad.Trim().Length >= 13)
                                {
                                    int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 4, 4), out Hasta_Anio);
                                    int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 6, 1), out Hasta_Bimestre);
                                }
                            }

                            // si la parcialidad tiene estatus diferente de PAGADO, sumar adeudos
                            if (Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() != "PAGADO")
                            {
                                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString(), out Honorarios);
                                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString(), out Recargos_Moratorios);
                                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString(), out Monto_Impuesto);
                                DateTime.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Vencimiento);
                                Adeudo_Honorarios += Honorarios;
                                Adeudo_Moratorios += Recargos_Moratorios;
                                Monto_Base += Monto_Impuesto;
                            }
                            Parcialidad--;
                        }

                        // agregar adeudos vencidos despues de convenio
                        Monto_Base += Adeudos_Predial_Actuales_Despues_Convenio(Cuenta_Predial_ID, Hasta_Anio, Hasta_Bimestre);

                        // restar adeudos de bimestres que no han vencido (si el año es mayor al actual o es igual con el bimestre mayor al actual)
                        if (Hasta_Anio > DateTime.Now.Year || (Hasta_Anio == DateTime.Now.Year && Hasta_Bimestre >= DateTime.Now.Month / 2))
                        {
                            Monto_Base -= Adeudos_Predial_Sin_Vencer(Cuenta_Predial_ID, Hasta_Anio, Hasta_Bimestre);
                        }

                        Meses_Transcurridos = Calcular_Meses_Entre_Fechas(Fecha_Vencimiento, DateTime.Now);
                        Recargos_Moratorios = Calcular_Recargos_Moratorios(Monto_Base, Meses_Transcurridos);
                    }
                }
                else
                {
                    Recargos_Moratorios = 0;
                }
            }

            return Math.Round(Recargos_Moratorios + Adeudo_Moratorios, 2, MidpointRounding.AwayFromZero);
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Adeudos_Predial_Actuales_Despues_Convenio
        /// DESCRIPCIÓN: Regresa la suma de los adeudos vencidos despues del periodo indicado como parametro
        /// PARÁMETROS:
        /// 		1. Cuenta_Predial_ID: id de la cuenta predial para consultar adeudos
        /// 		2. Desde_Anio: Año del periodo inicial a tomar
        /// 		3. Desde_Bimestre: bimestre del periodo inicial a tomar
        /// CREO: Nombre del programador
        /// FECHA_CREO: 18-feb-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private decimal Adeudos_Predial_Actuales_Despues_Convenio(string Cuenta_Predial_ID, int Desde_Anio, int Desde_Bimestre)
        {
            decimal Adeudos_Despues_Convenio = 0;
            var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            DataTable Dt_Adeudos;
            int Anio_Actual = DateTime.Now.Year;
            int Bimestre_Vencido;

            Bimestre_Vencido = DateTime.Now.Month % 2 == 0 ? DateTime.Now.Month / 2 : (DateTime.Now.Month / 2) + 1;

            // periodo a partir del cual se va a tomar en cuenta (desde_bimestre + 1)
            Desde_Bimestre++;
            if (Desde_Bimestre > 6)
            {
                Desde_Bimestre = 1;
                Desde_Anio++;
            }

            // consultar adeudos actuales de la cuenta
            Dt_Adeudos = Consulta_Adeudos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial_ID, "POR PAGAR", 0, 0);
            // agregar adeudos vencidos a la fecha
            if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
            {
                // recorrer todas las filas de la tabla de adeudos
                for (int Contador_Filas = 0; Contador_Filas < Dt_Adeudos.Rows.Count; Contador_Filas++)
                {
                    int Anio_Adeudo;
                    int.TryParse(Dt_Adeudos.Rows[Contador_Filas][Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio_Adeudo);
                    // si el año es menor que Desde_Anio pasar al siguiente adeudo
                    if (Anio_Adeudo < Desde_Anio)
                    {
                        continue;
                    }
                    // si el año del adeudo es igual al año desde el que se calculan los moratorios, agregar solo los adeudos despues del bimestre indicado
                    else if (Anio_Adeudo == Desde_Anio)
                    {
                        int Hasta_Bimestre = 6;
                        // si es el año actual, tomar hasta el bimestre vencido
                        if (Anio_Adeudo == Anio_Actual)
                            Hasta_Bimestre = Bimestre_Vencido;
                        // recorrer los bimestres para agregar adeudos
                        for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                        {
                            decimal Adeudo_Bimestre;
                            decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                            Adeudos_Despues_Convenio += Adeudo_Bimestre;
                        }
                    }
                    // si el año es mayor que el año especificado, agregar adeudos al monto total
                    else if (Anio_Adeudo > Desde_Anio)
                    {
                        int Hasta_Bimestre = 6;
                        // si es el año actual, tomar hasta el bimestre vencido
                        if (Anio_Adeudo == Anio_Actual)
                            Hasta_Bimestre = Bimestre_Vencido;
                        // recorrer los bimestres para agregar al adeudo
                        for (int Contador_Bimestres = 1; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                        {
                            decimal Adeudo_Bimestre;
                            decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                            Adeudos_Despues_Convenio += Adeudo_Bimestre;
                        }
                    }

                } // for
            }

            return Adeudos_Despues_Convenio;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Adeudos_Predial_Sin_Vencer
        /// DESCRIPCIÓN: Regresa la suma de los adeudos aún no vencidos incluidos en el convenio
        ///             Consulta adeudos predial y suma los adeudos a partir del bimestre vencido actual
        /// PARÁMETROS:
        /// 		1. Cuenta_Predial_ID: id de la cuenta predial para consultar adeudos
        /// 		2. Hasta_Anio: año del último bimestre incluido en el convenio
        /// 		3. Ultimo_Bimestre: bimestre del último periodo incluido en el convenio
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 01-may-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private decimal Adeudos_Predial_Sin_Vencer(string Cuenta_Predial_ID, int Hasta_Anio, int Ultimo_Bimestre)
        {
            decimal Adeudos_Sin_Vencer = 0;
            var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            DataTable Dt_Adeudos;
            int Anio_Actual = DateTime.Now.Year;
            int Bimestre_Vencido;

            Bimestre_Vencido = DateTime.Now.Month % 2 == 0 ? DateTime.Now.Month / 2 : (DateTime.Now.Month / 2) + 1;

            // consultar adeudos actuales de la cuenta
            Dt_Adeudos = Consulta_Adeudos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial_ID, "POR PAGAR", 0, 0);
            // validar que se obtuvieron datos de la consulta de adeudos
            if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
            {
                // recorrer todas las filas de la tabla de adeudos
                for (int Contador_Filas = 0; Contador_Filas < Dt_Adeudos.Rows.Count; Contador_Filas++)
                {
                    int Anio_Adeudo;
                    int.TryParse(Dt_Adeudos.Rows[Contador_Filas][Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio_Adeudo);
                    // si el año es menor que Hasta_Anio pasar al siguiente adeudo
                    if (Anio_Adeudo >= Anio_Actual)
                    {
                        int Desde_Bimestre = 1;
                        int Hasta_Bimestre = 6;
                        if (Hasta_Anio == Anio_Actual)
                        {
                            Hasta_Bimestre = Ultimo_Bimestre;
                            Desde_Bimestre = Bimestre_Vencido + 1;
                            // recorrer los bimestres para agregar adeudos
                            for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                            {
                                decimal Adeudo_Bimestre;
                                decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                                Adeudos_Sin_Vencer += Adeudo_Bimestre;
                            }
                        }
                        else if (Hasta_Anio > Anio_Actual)
                        {
                            Hasta_Bimestre = Ultimo_Bimestre;
                            // recorrer los bimestres para agregar adeudos
                            for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                            {
                                decimal Adeudo_Bimestre;
                                decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                                Adeudos_Sin_Vencer += Adeudo_Bimestre;
                            }
                        }
                    }
                } // for
            }

            return Adeudos_Sin_Vencer;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Calcular_Recargos_Moratorios
        /// DESCRIPCIÓN: Calcular los recargos moratorios para una cantidad a partir de una fecha dados
        ///             (el numero de meses por el porcentaje de recargos por el monto base)
        /// PARÁMETROS:
        /// 		1. Monto_Base: Cantidad a la que se van a calcular los recargos
        /// 		2. Meses: Numero de meses a considedar para el calculo de recargos 
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 21-nov-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private Decimal Calcular_Recargos_Moratorios(Decimal Monto_Base, Int32 Meses)
        {
            var Parametros = new Cls_Ope_Pre_Parametros_Negocio();
            DataTable Dt_Parametros;
            Decimal Recargos_Moratorios = 0;
            Decimal Porcentaje_Recargos = 0;

            // recuperar el porcentaje de recargos moratorios de la tabla de parametros
            Dt_Parametros = Parametros.Consultar_Parametros();
            if (Dt_Parametros != null)
            {
                if (Dt_Parametros.Rows.Count > 0)
                {
                    Decimal.TryParse(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Recargas_Traslado].ToString(), out Porcentaje_Recargos);
                }
            }

            // obtener el producto de los meses por el porcentaje de recargos
            Porcentaje_Recargos *= Meses;

            // calcular recargos
            Recargos_Moratorios = Monto_Base * Porcentaje_Recargos / 100;

            return Recargos_Moratorios;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Calcular_Meses_Entre_Fechas
        /// DESCRIPCIÓN: Regresa un enteron con el numero de meses vencidos entre dos fechas
        ///             (tomando el primer dia de cada mes)
        /// PARÁMETROS:
        /// 		1. Desde_Fecha: Fecha inicial a comparar
        /// 		2. Hasta_Fecha: Fecha final a comparar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 05-dic-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private Int32 Calcular_Meses_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha)
        {
            DateTime Fecha_Inicial;
            DateTime Fecha_Final;
            int Meses = 0;

            // establecer fecha inicial como el primer día del mes en Desde_Fecha
            DateTime.TryParse(Desde_Fecha.ToString("01-MMM-yyyy"), out Fecha_Inicial);
            // tomar la fecha en Hasta_Fecha (ignorando la hora)
            DateTime.TryParse(Hasta_Fecha.ToString("dd-MMM-yyyy"), out Fecha_Final);

            // validar que se obtuvo una fecha inicial
            if (Fecha_Inicial != DateTime.MinValue)
            {
                // aumentar el numero de meses mientras la fecha inicial mas los meses no supere la fecha final
                while (Fecha_Final >= Fecha_Inicial.AddMonths(Meses))
                {
                    Meses++;
                }
            }

            return Meses;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Convenio_Vencido
        /// DESCRIPCIÓN: Revisar las parcialidades en busca de parcialidades vencidas 
        ///             parcialidades sin pagar con fecha de vencimiento de hace mas de 10 dias habiles
        ///             Regresa verdadero si el convenio esta vencido.
        /// PARÁMETROS:
        /// 		1. Dt_Parcialidades: datatable con parcialidades de un convenio
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 13-dic-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private bool Convenio_Vencido(DataTable Dt_Parcialidades)
        {
            var Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
            DateTime Fecha_Periodo;
            DateTime Fecha_Vencimiento;
            int Dias = 0;
            int Meses = 0;
            bool Convenio_Vencido = false;

            // recorrer las parcialidades del convenio
            for (int Pago = 0; Pago < Dt_Parcialidades.Rows.Count; Pago++)
            {
                // si el estatus de la parcialidad es INCUMPLIDO
                if (Dt_Parcialidades.Rows[Pago][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString().Trim() == "INCUMPLIDO")
                {
                    Convenio_Vencido = true;
                    // abandonar el ciclo for
                    break;
                }
                else if (Dt_Parcialidades.Rows[Pago][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString().Trim() == "POR PAGAR")
                {
                    // obtener la fecha de vencimiento de la parcialidad
                    DateTime.TryParse(Dt_Parcialidades.Rows[Pago][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Periodo);
                    Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo.ToShortDateString(), "10");
                    // obtener el tiempo transcurrido desde la fecha de vencimiento
                    Calcular_Tiempo_Entre_Fechas(Fecha_Vencimiento, DateTime.Now, out Dias, out Meses);
                    // si el numero de dias transcurridos en mayor que cero, escribir fecha de vencimiento
                    if (Dias > 0)
                    {
                        Convenio_Vencido = true;
                    }
                    // abandonar el ciclo for
                    break;
                }
            }
            return Convenio_Vencido;
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


        #endregion

    }
}