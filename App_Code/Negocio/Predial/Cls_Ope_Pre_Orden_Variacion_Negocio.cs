using System;
using System.Data;
using System.Data.OracleClient;
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
using Operacion_Predial_Orden_Variacion.Datos;
using Operacion_Predial_Ordenes_Variacion.Datos;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Constantes;

namespace Operacion_Predial_Orden_Variacion.Negocio
{

    public class Cls_Ope_Pre_Orden_Variacion_Negocio
    {
        #region Variables Internas

        private String Orden_Variacion_ID;
        private String Contrarecibo;
        private Int32 Año;
        private String Cuenta_Predial_ID;
        private String Concepto_Predial_ID;
        private String Caso_Especial_ID;
        private String Multa_ID;
        private String Cuenta_Predial;
        private String Efectos_Año;
        private Int32 Efectos_Bimestre;
        private Double Tasa;
        private String Periodo_Corriente_Inicial;
        private String Periodo_Corriente_Termina;
        private Double Cuota_Anual;
        private Double Cuota_Minima;
        private Double Exencion;
        private String Cuota_Fija;
        private Double Cuota_Bimestral;
        private DateTime Fecha_Avaluo;
        private DateTime Fecha_Termina_Exencion;
        private String Periodo_Rezago;
        private String Fecha_Periodo_Rezago_Inicia;
        private String Fecha_Periodo_Rezago_Termina;
        private String Diferencia_Construccion;
        private String Domicilio_Foraneo;
        private String Predio_Colindante;
        private Double Base_Impuesto;
        private String RFC_Propietario;
        private Double Minimo_Elevado_Año;
        private String Constancia_No_Adeudo;
        private Double Tasa_Traslado_Dominio;
        private Double Recargos;
        private Double Total;
        private String Tipo;
        private String Usuario;
        private String Usuario_Valido;
        private String Uso_Suelo;
        private String Valor_Fiscal;
        private String Cuenta_Origen;
        private String Estado_Predial;
        private String Estatus_Cuenta;
        private String Estatus_Orden;
        private String Tipo_Propietario;
        private String Nombre_Propietario;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private String Superficie_Total;
        private String Ubicacion_Cuenta;
        private String CP_Propietario;
        private String No_Cuota_Fija;
        private String No_Diferencia;
        private String Colonia_Cuenta;
        private String Exterior_Cuenta;
        private String Interior_Cuenta;
        private String Clave_Catastral;
        private String Propietario_ID;
        private String Estado_Propietario;
        private String Colonia_Propietario;
        private String Ciudad_Propietario;
        private String Interior_Propietario;
        private String Exterior_Propietario;
        private String Domilicio_Propietario;
        private String Superficie_Construida;
        private String No_Copropietarios_Cuenta;
        private Int32 No_Nota;
        private DateTime Fecha_Nota;
        private String Grupo_Movimiento_ID;
        private String Tipo_Predio_ID;
        private String No_Nota_Impreso;
        private Boolean Join_Contrarecibo;
        private String Fecha_Creo;
        private String Fecha_Modifico;
        private String Fecha_Valido;
        private String Unir_Tablas;
        private String Campo;
        private String Ciudad_ID_Notificacion;
        private String Estado_ID_Notificacion;

        //OBSERVACIONES
        private String Observaciones_Observacion_ID;
        private String Observaciones_No_Orden_Variacion;
        private Int32 Observaciones_Año;
        private String Observaciones_Descripcion;
        private String Observaciones_Usuraio;

        //Variables para generar orden de variacion
        private String Generar_Orden_No_Orden;
        private String Generar_Orden_Anio;
        private String Generar_Orden_Movimiento_ID;
        private String Generar_Orden_Cuenta_ID;
        private String Generar_Orden_Estatus;
        private String Generar_Orden_Obserbaciones;
        private DataTable Generar_Orden_Dt_Detalles = new DataTable();

        private Boolean Incluir_Campos_Foraneos;
        private Boolean Incluir_Campos_Detalles;
        private String Campos_Detalles;
        private DataTable Dt_Observaciones;
        private DataTable Dt_Propietarios = new DataTable();
        private DataTable Dt_Copropietarios = new DataTable();
        private DataTable Dt_Contribuyentes = new DataTable();
        private DataTable Dt_Diferencias = new DataTable();

        //Variables para filtrar por fechas
        private String Generar_Orden_Fecha_Inicial;
        private String Generar_Orden_Fecha_Final;

        private OracleTransaction Trans;
        private OracleCommand Cmmd;

        //Detalles Cuota fija
        private String Cuota_Fija_ID;
        private String Cuota_Fija_Plazo;
        private String Cuota_Fija_Excedente_Cons;
        private String Cuota_Fija_Excedente_Valor;
        private String Cuota_Fija_Total;
        private String Cuota_Fija_Caso_Especial;
        private String Cuota_Fija_Tasa_ID;
        private String Cuota_Fija_Tasa_Valor;
        private String Cuota_Fija_Cuota_Minima;
        private String Cuota_Fija_Excedente_Cons_Total;
        private String Cuota_Fija_Excedente_Valor_Total;

        //Datos Propietarios
        private String Propietario_Propietario_ID;
        private String Propietario_Cuenta_Predial_ID;
        private String Propietario_Contribuyente_ID;
        private String Propietario_Tipo;
        private String Propietario_Usuario;
        private Boolean Propietario_Filtra_Estatus;

        //Datos Copropietarios
        private String Copropietario_Propietario_ID;
        private String Copropietario_Cuenta_Predial_ID;
        private String Copropietario_Contribuyente_ID;
        private String Copropietario_Tipo;
        private String Copropietario_Usuario;
        private Boolean Copropietario_Filtra_Estatus;

        //Datos Contrarecibos
        private String Contrarecibo_No_Contrarecibo;
        private String Contrarecibo_Cuenta_Predial_ID;
        private Int64 Contrarecibo_No_Escritoria;
        private DateTime Contrarecibo_Fecha_Escritura;
        private DateTime Contrarecibo_Fecha_Liberacion;
        private DateTime Contrarecibo_Fecha_Pago;
        private String Contrarecibo_Estatus;
        private String Contrarecibo_Notario_ID;
        private String Contrarecibo_Listado_ID;
        private Int16 Contrarecibo_Anio;
        private String Contrarecibo_Usuario;

        //Datos Diferencias
        private String Diferencias_Estatus;
        private Boolean Suma_Variacion_Diferencias;
        private Boolean Suma_Variacion_Adeudos;
        private Boolean Cancelando_Cuenta;
        private Boolean Reactivando_Cuenta;
        private Decimal Total_Recargos;
        private Decimal Total_Corriente;
        private Decimal Total_Rezago;

        //Para Reporte de Reactivacion
        private DataTable Dt_Reactivacion;

        private String Tipo_Suspencion_Cuenta_Predial = null;

        private long Maximo_Registros = 0;

        private String Cargar_Modulos;

        private Boolean Ignorar_Historial_Ordenes_Aceptadas = false;

        private Decimal Cuota_Fija_Nueva = 0;
        private Decimal Cuota_Fija_Anterior = 0;
        #endregion

        #region Variables Publicas

        #region Variables publicas Cuota_Fija
        public String P_Cuota_Fija_ID
        {
            get { return Cuota_Fija_ID; }
            set { Cuota_Fija_ID = value; }
        }

        public String P_Cuota_Fija_Plazo
        {
            get { return Cuota_Fija_Plazo; }
            set { Cuota_Fija_Plazo = value; }
        }

        public String P_Cuota_Fija_Excedente_Cons
        {
            get { return Cuota_Fija_Excedente_Cons; }
            set { Cuota_Fija_Excedente_Cons = value; }
        }

        public String P_Cuota_Fija_Excedente_Valor
        {
            get { return Cuota_Fija_Excedente_Valor; }
            set { Cuota_Fija_Excedente_Valor = value; }
        }

        public String P_Cuota_Fija_Total
        {
            get { return Cuota_Fija_Total; }
            set { Cuota_Fija_Total = value; }
        }

        public String P_Cuota_Fija_Caso_Especial
        {
            get { return Cuota_Fija_Caso_Especial; }
            set { Cuota_Fija_Caso_Especial = value; }
        }

        public String P_Cuota_Fija_Tasa_ID
        {
            get { return Cuota_Fija_Tasa_ID; }
            set { Cuota_Fija_Tasa_ID = value; }
        }

        public String P_Cuota_Fija_Tasa_Valor
        {
            get { return Cuota_Fija_Tasa_Valor; }
            set { Cuota_Fija_Tasa_Valor = value; }
        }

        public String P_Cuota_Fija_Cuota_Minima
        {
            get { return Cuota_Fija_Cuota_Minima; }
            set { Cuota_Fija_Cuota_Minima = value; }
        }


        public String P_Cuota_Fija_Excedente_Cons_Total
        {
            get { return Cuota_Fija_Excedente_Cons_Total; }
            set { Cuota_Fija_Excedente_Cons_Total = value; }
        }

        public String P_Cuota_Fija_Excedente_Valor_Total
        {
            get { return Cuota_Fija_Excedente_Valor_Total; }
            set { Cuota_Fija_Excedente_Valor_Total = value; }
        }

        public Decimal P_Cuota_Fija_Nueva
        {
            get { return Cuota_Fija_Nueva; }
            set { Cuota_Fija_Nueva = value; }
        }

        public Decimal P_Cuota_Fija_Anterior
        {
            get { return Cuota_Fija_Anterior; }
            set { Cuota_Fija_Anterior = value; }
        }
        #endregion

        #region Variables Publicas para generacion de orden de variacion

        public String P_Estatus_Orden
        {
            get { return Estatus_Orden; }
            set { Estatus_Orden = value; }
        }

        public DataTable P_Dt_Diferencias
        {
            get { return Dt_Diferencias; }
            set { Dt_Diferencias = value; }
        }

        public String P_Generar_Orden_Movimiento_ID
        {
            get { return Generar_Orden_Movimiento_ID; }
            set { Generar_Orden_Movimiento_ID = value; }
        }

        public String P_Generar_Orden_No_Orden
        {
            get { return Generar_Orden_No_Orden; }
            set { Generar_Orden_No_Orden = value; }
        }

        public String P_Generar_Orden_Anio
        {
            get { return Generar_Orden_Anio; }
            set { Generar_Orden_Anio = value; }
        }

        public String P_Generar_Orden_Cuenta_ID
        {
            get { return Generar_Orden_Cuenta_ID; }
            set { Generar_Orden_Cuenta_ID = value; }
        }
        public String P_Generar_Orden_Estatus
        {
            get { return Generar_Orden_Estatus; }
            set { Generar_Orden_Estatus = value; }
        }
        public String P_Generar_Orden_Obserbaciones
        {
            get { return Generar_Orden_Obserbaciones; }
            set { Generar_Orden_Obserbaciones = value; }
        }
        public DataTable P_Generar_Orden_Dt_Detalles
        {
            get { return Generar_Orden_Dt_Detalles; }
            set { Generar_Orden_Dt_Detalles = value; }
        }

        #endregion

        #region Variables Públicas Datos Propietarios
        public String P_Propietario_Propietario_ID
        {
            get { return Propietario_Propietario_ID; }
            set { Propietario_Propietario_ID = value; }
        }

        public String P_Propietario_Cuenta_Predial_ID
        {
            get { return Propietario_Cuenta_Predial_ID; }
            set { Propietario_Cuenta_Predial_ID = value; }
        }

        public String P_Propietario_Contribuyente_ID
        {
            get { return Propietario_Contribuyente_ID; }
            set { Propietario_Contribuyente_ID = value; }
        }

        public String P_Propietario_Tipo
        {
            get { return Propietario_Tipo; }
            set { Propietario_Tipo = value; }
        }

        public String P_Propietario_Usuario
        {
            get { return Propietario_Usuario; }
            set { Propietario_Usuario = value; }
        }

        public Boolean P_Propietario_Filtra_Estatus
        {
            get { return Propietario_Filtra_Estatus; }
            set { Propietario_Filtra_Estatus = value; }
        }
        #endregion


        #region Variables Públicas Datos Copropietarios
        public String P_Copropietario_Propietario_ID
        {
            get { return Copropietario_Propietario_ID; }
            set { Copropietario_Propietario_ID = value; }
        }

        public String P_Copropietario_Cuenta_Predial_ID
        {
            get { return Copropietario_Cuenta_Predial_ID; }
            set { Copropietario_Cuenta_Predial_ID = value; }
        }

        public String P_Copropietario_Contribuyente_ID
        {
            get { return Copropietario_Contribuyente_ID; }
            set { Copropietario_Contribuyente_ID = value; }
        }

        public String P_Copropietario_Tipo
        {
            get { return Copropietario_Tipo; }
            set { Copropietario_Tipo = value; }
        }

        public String P_Copropietario_Usuario
        {
            get { return Copropietario_Usuario; }
            set { Copropietario_Usuario = value; }
        }

        public Boolean P_Copropietario_Filtra_Estatus
        {
            get { return Copropietario_Filtra_Estatus; }
            set { Copropietario_Filtra_Estatus = value; }
        }
        #endregion

        #region Variables Públicas Datos Contrarecibos
        public String P_Contrarecibo_No_Contrarecibo
        {
            get { return Contrarecibo_No_Contrarecibo; }
            set { Contrarecibo_No_Contrarecibo = value; }
        }

        public String P_Contrarecibo_Cuenta_Predial_ID
        {
            get { return Contrarecibo_Cuenta_Predial_ID; }
            set { Contrarecibo_Cuenta_Predial_ID = value; }
        }

        public Int64 P_Contrarecibo_No_Escritoria
        {
            get { return Contrarecibo_No_Escritoria; }
            set { Contrarecibo_No_Escritoria = value; }
        }

        public DateTime P_Contrarecibo_Fecha_Escritura
        {
            get { return Contrarecibo_Fecha_Escritura; }
            set { Contrarecibo_Fecha_Escritura = value; }
        }

        public DateTime P_Contrarecibo_Fecha_Liberacion
        {
            get { return Contrarecibo_Fecha_Liberacion; }
            set { Contrarecibo_Fecha_Liberacion = value; }
        }

        public DateTime P_Contrarecibo_Fecha_Pago
        {
            get { return Contrarecibo_Fecha_Pago; }
            set { Contrarecibo_Fecha_Pago = value; }
        }

        public String P_Contrarecibo_Estatus
        {
            get { return Contrarecibo_Estatus; }
            set { Contrarecibo_Estatus = value; }
        }

        public String P_Contrarecibo_Notario_ID
        {
            get { return Contrarecibo_Notario_ID; }
            set { Contrarecibo_Notario_ID = value; }
        }

        public String P_Contrarecibo_Listado_ID
        {
            get { return Contrarecibo_Listado_ID; }
            set { Contrarecibo_Listado_ID = value; }
        }

        public Int16 P_Contrarecibo_Anio
        {
            get { return Contrarecibo_Anio; }
            set { Contrarecibo_Anio = value; }
        }

        public String P_Contrarecibo_Usuario
        {
            get { return Contrarecibo_Usuario; }
            set { Contrarecibo_Usuario = value; }
        }
        #endregion

        #region Variables Públicas Datos Diferencias
        public String P_Diferencias_Estatus
        {
            get { return Diferencias_Estatus; }
            set { Diferencias_Estatus = value; }
        }

        public Boolean P_Suma_Variacion_Diferencias
        {
            get { return Suma_Variacion_Diferencias; }
            set { Suma_Variacion_Diferencias = value; }
        }

        public Boolean P_Suma_Variacion_Adeudos
        {
            get { return Suma_Variacion_Adeudos; }
            set { Suma_Variacion_Adeudos = value; }
        }

        public Boolean P_Cancelando_Cuenta
        {
            get { return Cancelando_Cuenta; }
            set { Cancelando_Cuenta = value; }
        }

        public Boolean P_Reactivando_Cuenta
        {
            get { return Reactivando_Cuenta; }
            set { Reactivando_Cuenta = value; }
        }

        public Decimal P_Total_Recargos
        {
            get { return Total_Recargos; }
            set { Total_Recargos = value; }
        }

        public Decimal P_Total_Corriente
        {
            get { return Total_Corriente; }
            set { Total_Corriente = value; }
        }

        public Decimal P_Total_Rezago
        {
            get { return Total_Rezago; }
            set { Total_Rezago = value; }
        }
        #endregion

        #region Variables Publicas Datos Generales Cuenta

        public DataTable P_Dt_Contribuyentes
        {
            get { return Dt_Contribuyentes; }
            set { Dt_Contribuyentes = value; }
        }

        public String P_CP_Propietario
        {
            get { return CP_Propietario; }
            set { CP_Propietario = value; }
        }

        public String P_Contrarecibo
        {
            get { return Contrarecibo; }
            set { Contrarecibo = value; }
        }

        public String P_Exterior_Propietario
        {
            get { return Exterior_Propietario; }
            set { Exterior_Propietario = value; }
        }
        public String P_Interior_Propietario
        {
            get { return Interior_Propietario; }
            set { Interior_Propietario = value; }
        }

        public String P_Orden_Variacion_ID
        {
            get { return Orden_Variacion_ID; }
            set { Orden_Variacion_ID = value; }
        }

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }

        public Int32 P_Año
        {
            get { return Año; }
            set { Año = value; }
        }

        public String P_Estado_Predial
        {
            get { return Estado_Predial; }
            set { Estado_Predial = value; }
        }

        public String P_Exterior_Cuenta
        {
            get { return Exterior_Cuenta; }
            set { Exterior_Cuenta = value; }
        }

        public Double P_Cuota_Minima
        {
            get { return Cuota_Minima; }
            set { Cuota_Minima = value; }
        }

        public String P_Clave_Catastral
        {
            get { return Clave_Catastral; }
            set { Clave_Catastral = value; }
        }

        public String P_Interior_Cuenta
        {
            get { return Interior_Cuenta; }
            set { Interior_Cuenta = value; }
        }

        public String P_Ubicacion_Cuenta
        {
            get { return Ubicacion_Cuenta; }
            set { Ubicacion_Cuenta = value; }
        }

        public String P_Diferencia_Construccion
        {
            get { return Diferencia_Construccion; }
            set { Diferencia_Construccion = value; }
        }

        public String P_Tipo_Propietario
        {
            get { return Tipo_Propietario; }
            set { Tipo_Propietario = value; }
        }
        public String P_Colonia_Cuenta
        {
            get { return Colonia_Cuenta; }
            set { Colonia_Cuenta = value; }
        }

        public String P_Superficie_Total
        {
            get { return Superficie_Total; }
            set { Superficie_Total = value; }
        }

        public String P_Superficie_Construida
        {
            get { return Superficie_Construida; }
            set { Superficie_Construida = value; }
        }

        public String P_Propietario_ID
        {
            get { return Propietario_ID; }
            set { Propietario_ID = value; }
        }

        public String P_Estado_Propietario
        {
            get { return Estado_Propietario; }
            set { Estado_Propietario = value; }
        }

        public String P_Ciudad_Propietario
        {
            get { return Ciudad_Propietario; }
            set { Ciudad_Propietario = value; }
        }

        public String P_Colonia_Propietario
        {
            get { return Colonia_Propietario; }
            set { Colonia_Propietario = value; }
        }

        public String P_Estatus_Cuenta
        {
            get { return Estatus_Cuenta; }
            set { Estatus_Cuenta = value; }
        }

        public String P_Uso_Suelo
        {
            get { return Uso_Suelo; }
            set { Uso_Suelo = value; }
        }

        public String P_RFC_Propietario
        {
            get { return RFC_Propietario; }
            set { RFC_Propietario = value; }
        }

        public String P_Cuenta_Origen
        {
            get { return Cuenta_Origen; }
            set { Cuenta_Origen = value; }
        }

        public String P_Valor_Fiscal
        {
            get { return Valor_Fiscal; }
            set { Valor_Fiscal = value; }
        }

        public String P_Nombre_Propietario
        {
            get { return Nombre_Propietario; }
            set { Nombre_Propietario = value; }
        }

        public String P_Domilicio_Propietario
        {
            get { return Domilicio_Propietario; }
            set { Domilicio_Propietario = value; }
        }

        public String P_Concepto_Predial_ID
        {
            get { return Concepto_Predial_ID; }
            set { Concepto_Predial_ID = value; }
        }

        public String P_Caso_Especial_ID
        {
            get { return Caso_Especial_ID; }
            set { Caso_Especial_ID = value; }
        }

        public String P_Multa_ID
        {
            get { return Multa_ID; }
            set { Multa_ID = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }

        public String P_Efectos_Año
        {
            get { return Efectos_Año; }
            set { Efectos_Año = value; }
        }

        public Int32 P_Efectos_Bimestre
        {
            get { return Efectos_Bimestre; }
            set { Efectos_Bimestre = value; }
        }

        public Double P_Tasa
        {
            get { return Tasa; }
            set { Tasa = value; }
        }

        public String P_Periodo_Corriente_Inicial
        {
            get { return Periodo_Corriente_Inicial; }
            set { Periodo_Corriente_Inicial = value; }
        }

        public String P_Periodo_Corriente_Termina
        {
            get { return Periodo_Corriente_Termina; }
            set { Periodo_Corriente_Termina = value; }
        }

        public Double P_Cuota_Anual
        {
            get { return Cuota_Anual; }
            set { Cuota_Anual = value; }
        }

        public Double P_Cuota_Bimestral
        {
            get { return Cuota_Bimestral; }
            set { Cuota_Bimestral = value; }
        }

        public Double P_Exencion
        {
            get { return Exencion; }
            set { Exencion = value; }
        }

        public String P_Cuota_Fija
        {
            get { return Cuota_Fija; }
            set { Cuota_Fija = value; }
        }

        public DateTime P_Fecha_Termina_Exencion
        {
            get { return Fecha_Termina_Exencion; }
            set { Fecha_Termina_Exencion = value; }
        }

        public DateTime P_Fecha_Avaluo
        {
            get { return Fecha_Avaluo; }
            set { Fecha_Avaluo = value; }
        }

        public String P_Periodo_Rezago
        {
            get { return Periodo_Rezago; }
            set { Periodo_Rezago = value; }
        }

        public String P_Fecha_Periodo_Rezago_Inicia
        {
            get { return Fecha_Periodo_Rezago_Inicia; }
            set { Fecha_Periodo_Rezago_Inicia = value; }
        }

        public String P_Fecha_Periodo_Rezago_Termina
        {
            get { return Fecha_Periodo_Rezago_Termina; }
            set { Fecha_Periodo_Rezago_Termina = value; }
        }

        public String P_Domicilio_Foraneo
        {
            get { return Domicilio_Foraneo; }
            set { Domicilio_Foraneo = value; }
        }

        public String P_Predio_Colindante
        {
            get { return Predio_Colindante; }
            set { Predio_Colindante = value; }
        }

        public Double P_Base_Impuesto
        {
            get { return Base_Impuesto; }
            set { Base_Impuesto = value; }
        }

        public Double P_Minimo_Elevado_Año
        {
            get { return Minimo_Elevado_Año; }
            set { Minimo_Elevado_Año = value; }
        }

        public Double P_Tasa_Traslado_Dominio
        {
            get { return Tasa_Traslado_Dominio; }
            set { Tasa_Traslado_Dominio = value; }
        }

        public String P_Constancia_No_Adeudo
        {
            get { return Constancia_No_Adeudo; }
            set { Constancia_No_Adeudo = value; }
        }

        public Double P_Recargos
        {
            get { return Recargos; }
            set { Recargos = value; }
        }

        public Double P_Total
        {
            get { return Total; }
            set { Total = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Usuario_Valido
        {
            get { return Usuario_Valido; }
            set { Usuario_Valido = value; }
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

        public Boolean P_Incluir_Campos_Foraneos
        {
            get { return Incluir_Campos_Foraneos; }
            set { Incluir_Campos_Foraneos = value; }
        }

        public Boolean P_Incluir_Campos_Detalles
        {
            get { return Incluir_Campos_Detalles; }
            set { Incluir_Campos_Detalles = value; }
        }

        public String P_Campos_Detalles
        {
            get { return Campos_Detalles; }
            set { Campos_Detalles = value; }
        }

        public DataTable P_Dt_Observaciones
        {
            get { return Dt_Observaciones; }
            set { Dt_Observaciones = value; }
        }

        public DataTable P_Dt_Propietarios
        {
            get { return Dt_Propietarios; }
            set { Dt_Propietarios = value; }
        }

        public DataTable P_Dt_Copropietarios
        {
            get { return Dt_Copropietarios; }
            set { Dt_Copropietarios = value; }
        }

        #endregion

        public String P_No_Copropietarios_Cuenta
        {
            get { return No_Copropietarios_Cuenta; }
            set { No_Copropietarios_Cuenta = value; }
        }

        public Int32 P_No_Nota
        {
            get { return No_Nota; }
            set { No_Nota = value; }
        }

        public DateTime P_Fecha_Nota
        {
            get { return Fecha_Nota; }
            set { Fecha_Nota = value; }
        }

        public String P_Grupo_Movimiento_ID
        {
            get { return Grupo_Movimiento_ID; }
            set { Grupo_Movimiento_ID = value; }
        }

        public String P_Tipo_Predio_ID
        {
            get { return Tipo_Predio_ID; }
            set { Tipo_Predio_ID = value; }
        }

        public String P_No_Nota_Impreso
        {
            get { return No_Nota_Impreso; }
            set { No_Nota_Impreso = value; }
        }

        public Boolean P_Join_Contrarecibo
        {
            get { return Join_Contrarecibo; }
            set { Join_Contrarecibo = value; }
        }

        public String P_Unir_Tablas
        {
            get { return Unir_Tablas; }
            set { Unir_Tablas = value; }
        }

        public String P_Fecha_Creo
        {
            get { return Fecha_Creo; }
            set { Fecha_Creo = value; }
        }

        public String P_Fecha_Modifico
        {
            get { return Fecha_Modifico; }
            set { Fecha_Modifico = value; }
        }

        public String P_Fecha_Valido
        {
            get { return Fecha_Valido; }
            set { Fecha_Valido = value; }
        }

        public String P_No_Diferencia
        {
            get { return No_Diferencia; }
            set { No_Diferencia = value; }
        }
        public String P_No_Cuota_Fija
        {
            get { return No_Cuota_Fija; }
            set { No_Cuota_Fija = value; }
        }
        public OracleTransaction P_Trans
        {
            get { return Trans; }
            set { Trans = value; }
        }
        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }

        public String P_Observaciones_Observacion_ID
        {
            get { return Observaciones_Observacion_ID; }
            set { Observaciones_Observacion_ID = value; }
        }
        public String P_Observaciones_No_Orden_Variacion
        {
            get { return Observaciones_No_Orden_Variacion; }
            set { Observaciones_No_Orden_Variacion = value; }
        }
        public Int32 P_Observaciones_Año
        {
            get { return Observaciones_Año; }
            set { Observaciones_Año = value; }
        }
        public String P_Observaciones_Descripcion
        {
            get { return Observaciones_Descripcion; }
            set { Observaciones_Descripcion = value; }
        }
        public String P_Observaciones_Usuraio
        {
            get { return Observaciones_Usuraio; }
            set { Observaciones_Usuraio = value; }
        }
        public DataTable P_Dt_Reactivacion
        {
            get { return Dt_Reactivacion; }
            set { Dt_Reactivacion = value; }
        }
        public String P_Campo
        {
            get { return Campo; }
            set { Campo = value; }
        }

        public String P_Ciudad_ID_Notificacion
        {
            get { return Ciudad_ID_Notificacion; }
            set { Ciudad_ID_Notificacion = value; }
        }

        public String P_Estado_ID_Notificacion
        {
            get { return Estado_ID_Notificacion; }
            set { Estado_ID_Notificacion = value; }
        }
        #region Filtro de Fechas

        public String P_Generar_Orden_Fecha_Inicial
        {
            get { return Generar_Orden_Fecha_Inicial; }
            set { Generar_Orden_Fecha_Inicial = value; }
        }

        public String P_Generar_Orden_Fecha_Final
        {
            get { return Generar_Orden_Fecha_Final; }
            set { Generar_Orden_Fecha_Final = value; }
        }

        #endregion

        public String P_Tipo_Suspencion_Cuenta_Predial
        {
            get { return Tipo_Suspencion_Cuenta_Predial; }
            set { Tipo_Suspencion_Cuenta_Predial = value; }
        }

        public long P_Maximo_Registros
        {
            get { return Maximo_Registros; }
            set
            {
                if (value > 0) { Maximo_Registros = value; }
                else { Maximo_Registros = 0; }
            }
        }

        public String P_Cargar_Modulos
        {
            get { return Cargar_Modulos; }
            set { Cargar_Modulos = value; }
        }

        public Boolean P_Ignorar_Historial_Ordenes_Aceptadas
        {
            get { return Ignorar_Historial_Ordenes_Aceptadas; }
            set { Ignorar_Historial_Ordenes_Aceptadas = value; }
        }

        #endregion

        #region Metodos
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Cls_Ope_Pre_Orden_Variacion_Negocio
        ///DESCRIPCIÓN: Constructor de la clase de negocio que inicializa los datatables que almacenaran los detalles de algunos cambios en la cuenta
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:25:29 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************           
        public Cls_Ope_Pre_Orden_Variacion_Negocio()
        {
            this.Generar_Orden_Dt_Detalles.Columns.Add("CAMPO");
            this.Generar_Orden_Dt_Detalles.Columns.Add("DATO_NUEVO");

            this.Dt_Diferencias.Columns.Add(Ope_Pre_Diferencias_Detalle.Campo_Periodo);
            this.Dt_Diferencias.Columns.Add(Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal);
            this.Dt_Diferencias.Columns.Add(Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID);
            this.Dt_Diferencias.Columns.Add(Ope_Pre_Diferencias_Detalle.Campo_Tipo_Diferencia);
            this.Dt_Diferencias.Columns.Add(Ope_Pre_Diferencias_Detalle.Campo_Importe);
            this.Dt_Diferencias.Columns.Add(Ope_Pre_Diferencias_Detalle.Campo_Tipo_Periodo);
            this.Dt_Diferencias.Columns.Add(Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral);
            this.Dt_Diferencias.Columns.Add("TASA");

            this.Dt_Contribuyentes.Columns.Add(Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID);
            this.Dt_Contribuyentes.Columns.Add(Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus);
            this.Dt_Contribuyentes.Columns.Add(Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo);

            Suma_Variacion_Adeudos = true;
            Suma_Variacion_Diferencias = true;
            Cancelando_Cuenta = false;
            Cancelando_Cuenta = false;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Combos
        ///DESCRIPCIÓN: llena un dataset con los datos a mostrar en los combos del formulario
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:26:14 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************            
        public DataSet Consulta_Combos()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Combos();
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Combos
        ///DESCRIPCIÓN: llena un dataset con los datos a mostrar en los combos del formulario
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:26:14 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************            
        public DataTable Consulta_Valor_Orden(String Campo_Consultar, String Cuenta_ID, String Anio)
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Valor_Orden(Campo_Consultar, Cuenta_ID, Anio);
        }
        public DataTable Consulta_Valor_Orden(String Campo_Consultar, String Cuenta_ID, String Anio, String Condicion, String Ordenamiento)
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Valor_Orden(Campo_Consultar, Cuenta_ID, Anio, Condicion, Ordenamiento);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Combos
        ///DESCRIPCIÓN: llena un dataset con los datos a mostrar en los combos del formulario
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:26:14 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************            
        public DataTable Consultar_Ultima_Cuota_Fija()
        {
            DataTable Detalles_Cuota_Fija;
            string No_Cuota_Fija = "";
            Detalles_Cuota_Fija = Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Valor_Orden(this.P_Campo, this.P_Cuenta_Predial_ID, this.Generar_Orden_Anio);
            if (Detalles_Cuota_Fija != null)
            {
                if (Detalles_Cuota_Fija.Rows.Count > 0)
                    No_Cuota_Fija = Detalles_Cuota_Fija.Rows[0][0].ToString();
            }
            if (!string.IsNullOrEmpty(No_Cuota_Fija))
            {
                this.No_Cuota_Fija = No_Cuota_Fija;
                return this.Consultar_Cuota_Fija_Detalles();
            }
            else
            {
                return null;
            }
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta
        ///DESCRIPCIÓN: Consulta de los datos de la cuenta que seran mostrados en el formulario
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:27:15 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataSet Consulta_Datos_Cuenta()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Datos_Cuenta(this);
        }

        public DataSet Consulta_Datos_Cuenta_Sin_Contrarecibo()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Datos_Cuenta_Sin_Contrarecibo(this);
        }

        public void Alta_Cuenta()
        {
            Cls_Ope_Orden_Variacion_Datos.Alta_Cuenta(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Calles
        ///DESCRIPCIÓN: Consulta las calles de una colonia en especifico
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:27:48 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consulta_Calles()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Calles(this);
        }

        ///NOMBRE DE LA FUNCIÓN: Consulta_Ciudades
        ///DESCRIPCIÓN: Consulta las ciudades en un estado
        ///CREO: jtoledo
        ///FECHA_CREO: 09/08/2011 02:27:48 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consulta_Ciudades()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Ciudades(this);
        }
        public String Consulta_Nombre_Calle(String ID)
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Nombre_Calle(ID);
        }
        public String Consulta_Nombre_Colonia(String ID)
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Nombre_Colonia(ID);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Generar_Orden_Variacion
        ///DESCRIPCIÓN: se genera la orden de variacion con los datos de negocio
        ///             especificando el ID del movimiento, el ID de la cuenta a afectar
        ///             las observaciones, el estatus y una tabla con el nombre del campo 
        ///             o los campos su valor anterior y su valor modificado
        ///PARAMETROS: Objeto de la capa de Negocios de la Orden de Variacion
        ///CREO: jtoledo
        ///FECHA_CREO: 10/Ago/2011 12:24:27 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Generar_Orden_Variacion()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Generar_Orden_Variacion(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Agregar_Variacion
        ///DESCRIPCIÓN: se agrega un dato a modificar en la cuenta se almacena en un datatable y en la tabla de detalles de la orden
        ///PARAMETROS: String Campo(Nombre del campo a afectar), String Dato_Nuevo (Nuevo valor)
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:24:06 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public void Agregar_Variacion(String Campo, String Dato_Nuevo)
        {
            try
            {
                DataRow Dr_Agregar_Variacion = this.Generar_Orden_Dt_Detalles.NewRow();
                Dr_Agregar_Variacion["CAMPO"] = Campo;
                Dr_Agregar_Variacion["DATO_NUEVO"] = Dato_Nuevo;
                this.Generar_Orden_Dt_Detalles.Rows.Add(Dr_Agregar_Variacion);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Propietario
        ///DESCRIPCIÓN: Agrega un regitstro de un contribuyente a la relacion de la cuenta
        ///PARAMETROS: String Contribuyente_ID, String Estatus, String Tipo
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:23:14 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************       
        public void Alta_Propietario(String Contribuyente_ID, String Estatus, String Tipo)
        {
            try
            {
                DataRow Dr_Agregar_Prop = this.Dt_Contribuyentes.NewRow();
                Dr_Agregar_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID] = Contribuyente_ID;
                Dr_Agregar_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus] = Estatus;
                Dr_Agregar_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo] = Tipo;
                this.Dt_Contribuyentes.Rows.Add(Dr_Agregar_Prop);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Agregar_Diferencias
        ///DESCRIPCIÓN: se almancenan los datos de las difrencias en el predio
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:22:20 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public void Agregar_Diferencias(String D_Valor_Fiscal, String D_Tasa_Predial_ID, String D_Tipo_Diferencia, String D_Tipo_Periodo, String D_Importe, String D_Periodo, String D_Tasa)
        {
            try
            {
                DataRow Dr_Agregar_Diferencia = this.Dt_Diferencias.NewRow();
                Dr_Agregar_Diferencia[Ope_Pre_Diferencias_Detalle.Campo_Periodo] = D_Periodo;
                Dr_Agregar_Diferencia[Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal] = D_Valor_Fiscal;
                Dr_Agregar_Diferencia[Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID] = D_Tasa_Predial_ID;
                Dr_Agregar_Diferencia[Ope_Pre_Diferencias_Detalle.Campo_Tipo_Diferencia] = D_Tipo_Diferencia;
                Dr_Agregar_Diferencia[Ope_Pre_Diferencias_Detalle.Campo_Importe] = D_Importe;
                Dr_Agregar_Diferencia[Ope_Pre_Diferencias_Detalle.Campo_Tipo_Periodo] = D_Tipo_Periodo;
                Dr_Agregar_Diferencia["TASA"] = D_Tasa;

                this.Generar_Orden_Dt_Detalles.Rows.Add(Dr_Agregar_Diferencia);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuota_Fija_Detalles
        ///DESCRIPCIÓN: consultar los de talles de la cuota fija de la cuenta
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:17:19 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consultar_Cuota_Fija_Detalles()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Cuota_Fija_Detalles(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Co_Propietarios
        ///DESCRIPCIÓN: se consulta la lista de los copropietaeios que tiene la cuenta
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:18:04 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consulta_Co_Propietarios()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Co_Propietarios(this);
        }

        public DataTable Consulta_Cancelacion_Cuenta_Predial()
        {
            return Cls_Ope_Orden_Variacion_Datos.Consultar_Cuentas_Canceladas(this);
        }
        public DataTable Consultar_Cuentas_Reactivadas()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Cuentas_Reactivadas(this);
        }
        public DataTable Consultar_Ordenes_Bajas_Directas()
        {
            return Cls_Ope_Orden_Variacion_Datos.Consultar_Ordenes_Bajas_Directas(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta_Datos
        ///DESCRIPCIÓN: consulta de datos generales de la cuenta
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:18:33 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataSet Consulta_Datos_Cuenta_Datos()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Datos_Cuenta_Datos(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Id_Movimiento
        ///DESCRIPCIÓN: se consulta el ID del movimiento especificado con su descripcion
        ///PARAMETROS: String Movimiento: Descripcion del movimiento
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:19:19 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public void Consulta_Id_Movimiento(String Movimiento)
        {
            this.P_Generar_Orden_Movimiento_ID = Cls_Ope_Orden_Variacion_Datos.Consulta_Id_Movimiento(Movimiento);
        }
        public DataTable Consultar_Ordenes_Variacion()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Ordenes_Variacion(this);
        }

        public DataTable Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta(this);
        }

        public DataTable Consultar_Historial_Estatus_Ordenes_Estatus_Contrarecibo()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Historial_Estatus_Ordenes_Estatus_Contrarecibo(this);
        }

        public DataTable Consultar_Ultima_Orden_Con_Adeudos()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Ultima_Orden_Con_Adeudos(this);
        }

        public DataTable Consulta_Diferencias()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Diferencias(this);
        }

        public DataTable Consultar_Adeudos_Predial()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Adeudos_Predial(this);
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Obtener_Adeudos_Cuenta_Aplicando_Diferencias_Orden
        ///DESCRIPCIÓN              : Se consultan los Adeudos de la Cuenta y les Aplica las Bajas/Altas de la Orden indicada
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 11/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Obtener_Adeudos_Cuenta_Aplicando_Diferencias_Orden()
        {
            Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
            Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Adeudos_Predial = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();

            DataTable Dt_Diferencias_Orden = null;
            DataTable Dt_Adeudos_Cuenta = null;
            DataTable Dt_Diferencias = null;

            Dictionary<String, Decimal> Dic_Adeudos_Diferencias = new Dictionary<string, decimal>();

            String Periodo = "";
            Decimal Cuota_Bimestral = 0;
            Boolean Periodo_Corriente_Validado = false;
            Boolean Periodo_Rezago_Validado = false;
            //Boolean Cuotas_Minimas_Encontradas_Año = false;
            //Boolean Cuotas_Minimas_Encontradas_Periodo = false;
            Decimal Sum_Adeudos_Año = 0;
            Decimal Sum_Adeudos_Periodo = 0;
            int Cont_Adeudos_Año = 0;
            int Cont_Adeudos_Periodo = 0;
            int Cont_Cuotas_Minimas_Año = 0;
            int Cont_Cuotas_Minimas_Periodo = 0;
            int Desde_Bimestre = 0;
            int Hasta_Bimestre = 0;
            int Cont_Bimestres = 0;
            int Año_Periodo = 0;
            int Desde_Anio = DateTime.Now.Year + 1;
            int Hasta_Anio = 0;
            int Tmp_Desde_Anio = 0;
            int Tmp_Hasta_Anio = 0;
            int Mes_Actual = DateTime.Now.Month;
            int Anio_Actual = DateTime.Now.Year;
            int Signo = 1;
            Decimal Cuota_Anual = 0;
            Decimal Cuota_Minima_Año = 0;
            Decimal Cuota_Fija = 0;
            Boolean Nueva_Cuota_Fija = false;
            Decimal Importe_Rezago = 0;
            Decimal Valor_Fiscal = 0;
            Decimal Tasa_Diferencias = 0;
            //Decimal Total_Adeudo_Impuesto = 0;
            Decimal Total_Adeudo_Año = 0;
            //String Periodo_Inicial = "-";
            //String Periodo_Final = "-";

            if (this.P_Cuota_Fija_Nueva != 0
                && this.P_Cuota_Fija_Nueva != this.P_Cuota_Fija_Anterior)
            {
                Nueva_Cuota_Fija = true;
            }

            if (this.P_Dt_Diferencias != null)
            {
                Dt_Diferencias_Orden = this.P_Dt_Diferencias;
            }
            else
            {
                Dt_Diferencias_Orden = this.Consulta_Diferencias();
            }
            if (Dt_Diferencias_Orden != null)
            {
                Dt_Diferencias_Orden.DefaultView.Sort = "TIPO DESC";
                Dt_Diferencias_Orden = Dt_Diferencias_Orden.DefaultView.ToTable();
            }
            Dt_Adeudos_Cuenta = Adeudos_Predial.Calcular_Recargos_Predial(this.P_Cuenta_Predial_ID);

            if (Dt_Adeudos_Cuenta != null)
            {
                //Procesa cada Adeudo de la Cuenta para agregarlo al Diccionario
                foreach (DataRow Fila_Adeudos in Dt_Adeudos_Cuenta.Rows)
                {
                    Periodo = Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Trim();
                    Cuota_Bimestral = Convert.ToDecimal(Fila_Adeudos["ADEUDO"]);
                    Dic_Adeudos_Diferencias.Add(Periodo, Cuota_Bimestral);
                    Desde_Anio = Convert.ToInt16(Dic_Adeudos_Diferencias.First().Key.Substring(Dic_Adeudos_Diferencias.First().Key.Length - 4));
                    Hasta_Anio = Convert.ToInt16(Dic_Adeudos_Diferencias.Last().Key.Substring(Dic_Adeudos_Diferencias.First().Key.Length - 4));
                }
            }

            Dt_Diferencias = new DataTable();
            Dt_Diferencias.Columns.Add(new DataColumn("AÑO", typeof(int)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_1", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_2", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_3", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_4", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_5", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_6", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("CUOTA_ANUAL", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("ADEUDO_TOTAL_AÑO", typeof(Decimal)));

            if (Dic_Adeudos_Diferencias.Count > 0)
            {
                DataRow Dr_Diferencias;
                for (Año_Periodo = Desde_Anio; Año_Periodo <= Hasta_Anio; Año_Periodo++)
                {
                    Dr_Diferencias = Dt_Diferencias.NewRow();
                    Dr_Diferencias["AÑO"] = Año_Periodo;
                    for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                    {
                        Periodo = Cont_Bimestres.ToString() + Año_Periodo.ToString();
                        if (Dic_Adeudos_Diferencias.ContainsKey(Periodo))
                        {
                            Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Convert.ToDecimal(Dic_Adeudos_Diferencias[Periodo]);
                        }
                    }
                    Dr_Diferencias["CUOTA_ANUAL"] = 0;
                    Dt_Diferencias.Rows.Add(Dr_Diferencias);
                }
            }

            if (Dt_Diferencias_Orden != null)
            {
                if (Cls_Ope_Pre_Ordenes_Variacion_Datos.Obtener_Anio_Minimo_Maximo(out Tmp_Desde_Anio, out Tmp_Hasta_Anio, Dt_Diferencias_Orden, Ope_Pre_Diferencias_Detalle.Campo_Periodo, 2))
                {
                    // si el valor regresado por la funcion es menor, asignar, si no, dejar el mismo
                    if (Tmp_Desde_Anio < Desde_Anio)
                    {
                        Desde_Anio = Tmp_Desde_Anio;
                    }
                    // si el valor regresado por la funcion es mayor, asignarlo a la variable, si no, dejar el mismo
                    if (Tmp_Hasta_Anio > Hasta_Anio)
                    {
                        Hasta_Anio = Tmp_Hasta_Anio;
                    }
                }

                if (Dt_Diferencias_Orden.Rows.Count > 0)
                {
                    //Procesa cada Diferencia de la Orden para aplicar los Adeudos de la Cuenta
                    foreach (DataRow Fila_Adeudos in Dt_Diferencias_Orden.Rows)
                    {
                        if (Fila_Adeudos["TIPO"] != DBNull.Value)
                        {
                            if (Fila_Adeudos["TIPO"].ToString().Trim() == "ALTA")
                            {
                                Signo = 1;
                            }
                            else
                            {
                                if (Fila_Adeudos["TIPO"].ToString().Trim() == "BAJA")
                                {
                                    Signo = -1;
                                }
                            }
                        }

                        Cuota_Anual = Convert.ToDecimal(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral]) * 6;
                        Año_Periodo = Convert.ToInt16(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Substring(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Length - 4));
                        Cuota_Minima_Año = Convert.ToDecimal(Cuotas_Minimas.Consultar_Cuota_Minima_Anio(Año_Periodo.ToString()));
                        if (Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Importe] != DBNull.Value)
                        {
                            Importe_Rezago = Convert.ToDecimal(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Importe]);
                            Valor_Fiscal = Convert.ToDecimal(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal]);
                            Tasa_Diferencias = Convert.ToDecimal(Fila_Adeudos["TASA"]) / 1000;
                            Periodo = Cls_Ope_Pre_Ordenes_Variacion_Datos.Obtener_Periodos_Bimestre(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString(), out Periodo_Corriente_Validado, out Periodo_Rezago_Validado);
                            if (Periodo.Trim() != "")
                            {
                                Desde_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                                Hasta_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(1));

                                //Cuotas_Minimas_Encontradas_Año = false;
                                Cont_Cuotas_Minimas_Año = 0;
                                Cont_Adeudos_Año = 0;
                                Sum_Adeudos_Año = 0;
                                //Cuotas_Minimas_Encontradas_Periodo = false;
                                Cont_Cuotas_Minimas_Periodo = 0;
                                Cont_Adeudos_Periodo = 0;
                                Sum_Adeudos_Periodo = 0;

                                Dt_Adeudos_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(this.P_Cuenta_Predial_ID, null, Año_Periodo, Año_Periodo);
                                if (Dt_Adeudos_Cuenta != null)
                                {
                                    if (Dt_Adeudos_Cuenta.Rows.Count > 0)
                                    {
                                        //Contador de los Adeudos/Cuotas del Año
                                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                                        {
                                            if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                            {
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                                {
                                                    //Cuotas_Minimas_Encontradas_Año = true;
                                                    Cont_Cuotas_Minimas_Año += 1;
                                                }
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                                {
                                                    Cont_Adeudos_Año += 1;
                                                    Sum_Adeudos_Año += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                                }
                                            }
                                        }
                                        //Contador de los Adeudos/Cuotas del Periodo indicado
                                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                        {
                                            if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                            {
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                                {
                                                    //Cuotas_Minimas_Encontradas_Periodo = true;
                                                    Cont_Cuotas_Minimas_Periodo += 1;
                                                }
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                                {
                                                    Cont_Adeudos_Periodo += 1;
                                                    Sum_Adeudos_Periodo += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                                }
                                            }
                                        }
                                    }
                                }

                                DataRow Dr_Diferencias = null;
                                Boolean Diferencia_Encontrada = false;
                                foreach (DataRow Dr_Diferencia in Dt_Diferencias.Rows)
                                {
                                    if (Dr_Diferencia["AÑO"].ToString() == Año_Periodo.ToString())
                                    {
                                        Dr_Diferencias = Dr_Diferencia;
                                        Diferencia_Encontrada = true;
                                        break;
                                    }
                                }
                                if (!Diferencia_Encontrada)
                                {
                                    Dr_Diferencias = Dt_Diferencias.NewRow();
                                    Dr_Diferencias["AÑO"] = Año_Periodo;
                                }
                                Decimal Adeudos_Diferencias = 0;
                                //VALIDACIONES PARA CASOS DE CUOTAS MÍNIMAS Y APLICACIÓN DE ADEUDOS
                                //if (Cont_Cuotas_Minimas_Periodo == 1 && Importe_Rezago != Cuota_Minima_Año && !Nueva_Cuota_Fija)
                                //{
                                //    //SUMA LA CUOTA MÍNIMA AL IMPORTE Y EL RESULTADO LO PRORRATEA EN EL PERIODO INDICADO
                                //    for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                //    {
                                //        Adeudos_Diferencias = Convert.ToDecimal((Buscar_Dato_Diccionario(Dic_Adeudos_Diferencias, Cont_Bimestres.ToString() + Año_Periodo.ToString()) + ToDecimal((Importe_Rezago + Cuota_Minima_Año) / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo)).ToString("0.00"));
                                //        Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Adeudos_Diferencias;
                                //    }
                                //}
                                //else
                                {
                                    if (((Importe_Rezago == Cuota_Minima_Año)
                                            || (((Sum_Adeudos_Periodo - Importe_Rezago) == Cuota_Minima_Año && Signo < 0)))
                                        && !Nueva_Cuota_Fija
                                        && !(Importe_Rezago == Cuota_Minima_Año && (Hasta_Bimestre - Desde_Bimestre + 1) == 1))
                                    {
                                        //APLICA LA CUOTA MÍNIMA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CEROS
                                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                        {
                                            if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                            {
                                                Cuota_Minima_Año = 0;
                                            }
                                            if (Importe_Rezago == Cuota_Minima_Año || Signo > 0)
                                            {
                                                Adeudos_Diferencias = Convert.ToDecimal((Buscar_Dato_Diccionario(Dic_Adeudos_Diferencias, Cont_Bimestres.ToString() + Año_Periodo.ToString()) + Convert.ToDecimal(Convert.ToDecimal(Cuota_Minima_Año * Signo).ToString("0.00"))).ToString("0.00"));
                                            }
                                            else
                                            {
                                                Adeudos_Diferencias = Convert.ToDecimal(Convert.ToDecimal(Cuota_Minima_Año).ToString("0.00"));
                                            }
                                            Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Adeudos_Diferencias;
                                        }
                                    }
                                    else
                                    {
                                        if ((Valor_Fiscal * Tasa_Diferencias) <= Cuota_Minima_Año
                                            && (Sum_Adeudos_Periodo + Importe_Rezago) == Cuota_Minima_Año
                                            && Signo > 0)
                                        {
                                            //APLICA LA CUOTA MÍNIMA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CEROS
                                            for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                            {
                                                if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                                {
                                                    Cuota_Minima_Año = 0;
                                                }
                                                Adeudos_Diferencias = Convert.ToDecimal(Convert.ToDecimal(Cuota_Minima_Año).ToString("0.00"));
                                                Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Adeudos_Diferencias;
                                            }
                                        }
                                        else
                                        {
                                            if (Nueva_Cuota_Fija && Signo < 0)
                                            {
                                                //APLICA LA CUOTA FIJA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CERO
                                                if (Cuota_Fija_Nueva != 0)
                                                {
                                                    Cuota_Fija = Sum_Adeudos_Periodo - Importe_Rezago; //Convert.ToDecimal(Obtener_Dato_Consulta(Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija, Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas, Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = '" + Cuota_Fija_Nueva + "'"));
                                                }
                                                for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                                {
                                                    if (Cont_Bimestres > Desde_Bimestre && Cuota_Fija != 0)
                                                    {
                                                        Cuota_Fija = 0;
                                                    }
                                                    Adeudos_Diferencias = Convert.ToDecimal(Convert.ToDecimal(Cuota_Fija).ToString("0.00"));
                                                    Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Adeudos_Diferencias;
                                                }
                                            }
                                            else
                                            {
                                                //PRORRATEA EL IMPORTE EN EL PERIODO INDICADO
                                                for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                                {
                                                    Adeudos_Diferencias = Convert.ToDecimal((Buscar_Dato_Diccionario(Dic_Adeudos_Diferencias, Cont_Bimestres.ToString() + Año_Periodo.ToString()) + Convert.ToDecimal(Convert.ToDecimal(Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo).ToString("0.00"))).ToString("0.00"));
                                                    Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Adeudos_Diferencias;
                                                }
                                            }
                                        }
                                    }
                                }
                                Dr_Diferencias["CUOTA_ANUAL"] = Cuota_Anual;
                                if (!Diferencia_Encontrada)
                                {
                                    Dt_Diferencias.Rows.Add(Dr_Diferencias);
                                }
                            }
                        }
                    }
                }
            }

            //Pone Ceros en las celdas que estén vacías y Suma los Totales.
            if (Dt_Diferencias != null)
            {
                foreach (DataRow Dr_Diferencias in Dt_Diferencias.Rows)
                {
                    Total_Adeudo_Año = 0;
                    for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                    {
                        if (Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] == DBNull.Value)
                        {
                            Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = 0;
                        }
                        Total_Adeudo_Año += Convert.ToDecimal(Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()]);
                    }
                    Dr_Diferencias["ADEUDO_TOTAL_AÑO"] = Total_Adeudo_Año;
                }
                Dt_Diferencias.DefaultView.Sort = "AÑO ASC";
                Dt_Diferencias = Dt_Diferencias.DefaultView.Table;
            }

            return Dt_Diferencias;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Buscar_Dato_Diccionario
        ///DESCRIPCIÓN          : Valida para la clave indicada, si existe un Dato o Valor y lo devuelve, en caso contrario devuelve Cero
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 16/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        private Decimal Buscar_Dato_Diccionario(Dictionary<String, Decimal> Dic_Adeudos_Diferencias, String Periodo)
        {
            Decimal Dato_Diccionario = 0;
            if (Dic_Adeudos_Diferencias.ContainsKey(Periodo))
            {
                Dato_Diccionario = Dic_Adeudos_Diferencias[Periodo];
            }
            return Dato_Diccionario;
        }

        public Boolean Modificar_Orden_Variacion()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Modificar_Orden_Variacion(this);
        }

        public Boolean Insertar_Observaciones_Variacion()
        {
            return Cls_Ope_Orden_Variacion_Datos.Insertar_Observaciones_Variacion(this);
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Obtener_Variacion_Cuenta
        ///DESCRIPCIÓN          : Interfáz para armar un DataTable con los campos de la Orden Consultada y la Última Orden de Variación anterior Aceptada
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 13/Febrero/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Obtener_Variacion_Cuenta()
        {
            DataTable Dt_Orden_Variacion;
            DataTable Dt_Ultima_Orden_Variacion_Aplicada;
            DataTable Dt_Variacion_Cuenta;
            DataRow Dr_Variacion_Cuenta;
            Int32 Row_Index = 0;
            Int32 Cont_Columna;
            Int32 Cont_Fila;
            String Nombre_Campo;
            String Temp_No_Orden_Variacin;
            Int32 Temp_Año_Orden_Variacion;
            String Temp_Generar_Orden_Estatus;

            //Consulta la Orden de Variación Actual
            Dt_Orden_Variacion = this.Consultar_Ordenes_Variacion();

            Temp_Año_Orden_Variacion = this.P_Año;
            Temp_Generar_Orden_Estatus = this.P_Generar_Orden_Estatus;
            this.P_Año = 0;
            this.P_Generar_Orden_Estatus = "ACEPTADA";
            //Consulta todas la Órdenes de Variación Aceptadas
            Dt_Ultima_Orden_Variacion_Aplicada = this.Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta();
            this.P_Año = Temp_Año_Orden_Variacion;
            this.P_Generar_Orden_Estatus = Temp_Generar_Orden_Estatus;

            if (Dt_Ultima_Orden_Variacion_Aplicada != null && !Ignorar_Historial_Ordenes_Aceptadas)
            {
                if (Dt_Ultima_Orden_Variacion_Aplicada.Rows.Count > 0)
                {
                    Row_Index = Dt_Ultima_Orden_Variacion_Aplicada.Rows.Count - 1;
                    for (Cont_Fila = Dt_Ultima_Orden_Variacion_Aplicada.Rows.Count - 1; Cont_Fila >= 0; Cont_Fila--)
                    {
                        if (Dt_Ultima_Orden_Variacion_Aplicada.Rows[Cont_Fila][Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion].ToString().Trim() == Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion].ToString().Trim())
                        {
                            Row_Index = Cont_Fila - 1;
                            break;
                        }
                    }
                    Temp_No_Orden_Variacin = this.P_Orden_Variacion_ID;
                    Temp_Año_Orden_Variacion = this.P_Año;
                    if (Row_Index >= 0)
                    {
                        this.P_Orden_Variacion_ID = Dt_Ultima_Orden_Variacion_Aplicada.Rows[Row_Index]["NO_ORDEN_VARIACION"].ToString();
                        this.P_Año = Convert.ToInt16(Dt_Ultima_Orden_Variacion_Aplicada.Rows[Row_Index]["ANIO"]);
                        //Consulta la Última Orden de Variación Aceptada
                        Dt_Ultima_Orden_Variacion_Aplicada = this.Consultar_Ordenes_Variacion();
                    }
                    else
                    {
                        Dt_Ultima_Orden_Variacion_Aplicada = new DataTable();
                    }
                    this.P_Orden_Variacion_ID = Temp_No_Orden_Variacin;
                    this.P_Año = Temp_Año_Orden_Variacion;
                }
                else
                {
                    Dt_Ultima_Orden_Variacion_Aplicada = new DataTable();
                }
            }
            else
            {
                Dt_Ultima_Orden_Variacion_Aplicada = new DataTable();
            }

            if (Dt_Ultima_Orden_Variacion_Aplicada.Rows.Count == 0)
            {
                Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                Cuenta_Predial.P_Cuenta_Predial_ID = this.Cuenta_Predial_ID;
                Dt_Ultima_Orden_Variacion_Aplicada = Cuenta_Predial.Consultar_Cuenta();
                if (Dt_Ultima_Orden_Variacion_Aplicada != null)
                {
                    if (Dt_Ultima_Orden_Variacion_Aplicada.Columns.Contains("ESTATUS")
                        && !Dt_Ultima_Orden_Variacion_Aplicada.Columns.Contains("ESTATUS_CUENTA"))
                    {
                        Dt_Ultima_Orden_Variacion_Aplicada.Columns[Cat_Pre_Cuentas_Predial.Campo_Estatus].ColumnName = "ESTATUS_CUENTA";
                    }
                }
            }

            Dt_Variacion_Cuenta = new DataTable();
            Dt_Variacion_Cuenta.Columns.Add(new DataColumn("NOMBRE_CAMPO", typeof(String)));
            Dt_Variacion_Cuenta.Columns.Add(new DataColumn("DATO_NUEVO", typeof(String)));
            Dt_Variacion_Cuenta.Columns.Add(new DataColumn("DATO_ANTERIOR", typeof(String)));
            Dt_Variacion_Cuenta.Columns.Add(new DataColumn("DIFERENTE", typeof(Boolean)));

            for (Cont_Columna = 0; Cont_Columna < Dt_Orden_Variacion.Columns.Count; Cont_Columna++)
            {
                Dr_Variacion_Cuenta = Dt_Variacion_Cuenta.NewRow();
                Nombre_Campo = Dt_Orden_Variacion.Columns[Cont_Columna].ColumnName;
                Dr_Variacion_Cuenta["NOMBRE_CAMPO"] = Nombre_Campo;
                if (Dt_Orden_Variacion.Rows[0][Nombre_Campo] != DBNull.Value)
                {
                    Dr_Variacion_Cuenta["DATO_NUEVO"] = Dt_Orden_Variacion.Rows[0][Nombre_Campo];
                }
                //if (!Dt_Ultima_Orden_Variacion_Aplicada.Columns.Contains(Nombre_Campo))
                //{
                //    Dt_Ultima_Orden_Variacion_Aplicada.Columns.Add(new DataColumn(Nombre_Campo, typeof(String)));
                //}
                if (Dt_Ultima_Orden_Variacion_Aplicada.Columns.Contains(Nombre_Campo))
                {
                    if (Dt_Ultima_Orden_Variacion_Aplicada.Rows[0][Nombre_Campo] != DBNull.Value)
                    {
                        Dr_Variacion_Cuenta["DATO_ANTERIOR"] = Dt_Ultima_Orden_Variacion_Aplicada.Rows[0][Nombre_Campo];
                    }
                }
                if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                {
                    Dr_Variacion_Cuenta["DATO_NUEVO"] = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString().ToUpper().Trim();
                }
                if (Dr_Variacion_Cuenta["DATO_ANTERIOR"] != DBNull.Value)
                {
                    Dr_Variacion_Cuenta["DATO_ANTERIOR"] = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString().ToUpper().Trim();
                }
                if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value && Dr_Variacion_Cuenta["DATO_ANTERIOR"] != DBNull.Value)
                {
                    Dr_Variacion_Cuenta["DIFERENTE"] = !Dr_Variacion_Cuenta["DATO_NUEVO"].ToString().Equals(Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString());
                }
                else
                {
                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] == DBNull.Value && Dr_Variacion_Cuenta["DATO_ANTERIOR"] == DBNull.Value)
                    {
                        Dr_Variacion_Cuenta["DIFERENTE"] = false;
                    }
                    else
                    {
                        Dr_Variacion_Cuenta["DIFERENTE"] = true;
                    }
                }
                Dt_Variacion_Cuenta.Rows.Add(Dr_Variacion_Cuenta);
            }

            return Dt_Variacion_Cuenta;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Aplicar_Variacion
        ///DESCRIPCIÓN: se lee el datatable de los detalles de las variaciones y se aplican los cambios a la cuenta
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:20:29 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public Boolean Aplicar_Variacion_Orden()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Aplicar_Variacion_Orden(this);
        }

        public DataTable Consultar_Adeudos()
        {
            return Cls_Ope_Orden_Variacion_Datos.Consultar_Adeudos(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Reporte
        ///DESCRIPCIÓN: consulta para llenar reporte
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/24/2011 09:21:01 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataSet Consulta_Datos_Reporte()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Datos_Reporte(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Reporte_Movimientos
        ///DESCRIPCIÓN          : Interfáz para Consultar_Datos_Reporte_Movimientos
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Datos_Reporte_Movimientos()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Datos_Reporte_Movimientos(this);
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_General_Contrarecibo
        ///DESCRIPCIÓN: consulta de la tabla
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 20/Ago/2011 11:50:12 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************     
        public DataTable Consulta_General_Contrarecibo()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_General_Contrarecibo(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta_Generales
        ///DESCRIPCIÓN: consulta de la tabla
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 21/Ago/2011 8:00:22 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************     
        public DataSet Consulta_Datos_Cuenta_Generales()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Datos_Cuenta_Generales(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Propietario
        ///DESCRIPCIÓN: consulta de la tabla
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 22/Ago/2011 8:00:22 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        public DataSet Consulta_Datos_Propietario()
        {
            return Cls_Ope_Orden_Variacion_Datos.Consulta_Datos_Propietario(this);
        }

        /////******************************************************************************* 
        /////NOMBRE DE LA FUNCIÓN     : Consulta_Datos_Copropietarios
        /////DESCRIPCIÓN              : consulta de la tabla
        /////PARAMETROS: 
        /////CREO                     : Antonio Salvaldor Benavides Guardado
        /////FECHA_CREO               : 31/Agosto/2011
        /////MODIFICO: 
        /////FECHA_MODIFICO:
        /////CAUSA_MODIFICACIÓN:
        /////******************************************************************************* 
        //public DataSet Consulta_Datos_Copropietarios()
        //{
        //    return Cls_Ope_Orden_Variacion_Datos.Consulta_Datos_Copropietarios(this);
        //}

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Consultar_Propietarios_Variacion
        ///DESCRIPCIÓN              : consulta de la tabla
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvaldor Benavides Guardado
        ///FECHA_CREO               : 06/Septiembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        public DataSet Consultar_Propietarios_Variacion()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Propietarios_Variacion(this);
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Obtener_Campos_Tabla
        ///DESCRIPCIÓN: consulta los campos de una tabla en especifico
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 22/Ago/2011 8:03:39 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        public DataSet Obtener_Campos_Tabla(String Tabla)
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Obtener_Campos_Tabla(Tabla);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Fundamento
        ///DESCRIPCIÓN: consulta las calles deacuerdo al ID de la colonia
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 11:50:12 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        public String Consulta_Fundamento(String Caso_Esp)
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consulta_Fundamento(Caso_Esp);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Cambio_Propietarios
        ///DESCRIPCIÓN: se lee el datatable de las bajas y altas de los propietarios
        ///             y se insertan en los detalles de los propietarios tabla que se 
        ///             leera cuando se valide la orden de variacion
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/23/2011 09:26:47 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cambio_Propietarios()
        {
            Cls_Ope_Orden_Variacion_Datos.Cambio_Propietarios(this);
        }

        public string Alta_Beneficio_Couta_Fija()
        {
            return Cls_Ope_Orden_Variacion_Datos.Alta_Beneficio_Couta_Fija(this);
        }

        public DataTable Consulta_Calles_Sin_Colonia(String Calle_ID)
        {
            return Cls_Ope_Orden_Variacion_Datos.Consulta_Calles_Sin_Colonia(Calle_ID);
        }

        public Boolean Aplicar_Variacion_Propietarios()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Aplicar_Variacion_Propietarios(this);
        }

        public Boolean Aplicar_Variacion_Copropietarios()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Aplicar_Variacion_Copropietarios(this);
        }

        public Boolean Aplicar_Variacion_Diferencias()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Aplicar_Variacion_Diferencias(this);
        }

        public void Alta_Diferencias()
        {
            Cls_Ope_Pre_Ordenes_Variacion_Datos.Alta_Diferencias(this);
        }

        public String Consultar_Valor_Tasa(string p)
        {
            return Cls_Ope_Orden_Variacion_Datos.Consultar_Valor_Tasa(p);
        }

        public void Modificar_Contrarecibo()
        {
            Cls_Ope_Orden_Variacion_Datos.Modificar_Contrarecibo(this);
        }

        //poner en la capa de negocios
        public Boolean Modificar_Calculo_Traslado()
        {
            return Cls_Ope_Orden_Variacion_Datos.Modificar_Calculo_Traslado(this);
        }

        public string Consultar_Historial(string p)
        {
            return Cls_Ope_Orden_Variacion_Datos.Consultar_Historial(p);
        }
        public DataTable Consultar_Grupo_Mov(string p)
        {
            return Cls_Ope_Orden_Variacion_Datos.Consultar_Grupo_Mov(p);
        }
        public String Consultar_Cuenta_Existente_ID(string p)
        {
            return Cls_Ope_Orden_Variacion_Datos.Consultar_Cuenta_Existente_ID(p);
        }

        public void Eliminar_Orden()
        {
            Cls_Ope_Pre_Ordenes_Variacion_Datos.Eliminar_Orden(this);
        }
        #endregion

        #region [Metodos Nueva Tabla Orden Variacion]
        public String Generar_Ordenes_Variacion()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Generar_Orden_Variacion(this);
        }
        public void Modificar_Orden_Variacion_Generada()
        {
            Cls_Ope_Pre_Ordenes_Variacion_Datos.Modificar_Orden_Variacion_Generada(this);
        }
        #region [Propietarios]
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Consultar_Datos_Copropietarios_Variacion
        ///DESCRIPCIÓN              : consulta de la tabla
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvaldor Benavides Guardado
        ///FECHA_CREO               : 31/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        public DataSet Consultar_Copropietarios_Variacion()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Copropietarios_Variacion(this);
        }
        #endregion

        #region [Convenios, Restructuras, Impuestos]
        public DataTable Consultar_Domicilio_Y_Propietario()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Domicilio_Y_Propietario(this);
        }

        public DataTable Consultar_Ordenes_Variacion_Contrarecibos()
        {
            return Cls_Ope_Pre_Ordenes_Variacion_Datos.Consultar_Ordenes_Variacion_Contrarecibos(this);
        }
        #endregion

        #endregion



        public void Quitar_Beneficio_Agregar_Observacion()
        {
            Cls_Ope_Pre_Ordenes_Variacion_Datos.Quitar_Beneficio_Agregar_Observacion(this);
        }
    }
}