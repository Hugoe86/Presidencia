using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Datos;
using System.Data.OracleClient;

namespace Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio
{
    public class Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio
    {
        public Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio()
        {
        }

/// --------------------------------------- PROPIEDADES ---------------------------------------
#region PROPIEDADES

        private String _No_Calculo;
        private Int32 _Anio_Calculo;
        private String _Cuenta_Predial;
        private String _Cuenta_Predial_ID;
        private String SubConcepto_ID;
        private String _Concepto_ID;
        private String _No_Orden_Variacion;
        private Int32 _Anio_Orden;
        private String _No_Contrarecibo;
        private Int32 _Anio_Contrarecibo;
        private String _Predio_Colindante;
        private String _Base_Impuesto;
        private String _Tasas_ID;
        private String _Multa_ID;
        private String _Fecha_Escritura;
        private String _Impuesto_Div_Lot;
        private String _Monto_Multa;
        private String _Monto_Recargos;
        private String _Monto_Traslado;
        private String _Monto_Division;
        private String _Monto_Total_Pagar;
        private String _Tipo;
        private String _Costo_Constancia;
        private String _Observaciones;
        private String _Origen;
        private String _Estatus;
        private String _Nombre_Usuario;
        private String _Estatus_Orden;
        private String _Estatus_Descuento;
        private String _Numero_Adeudo;
        private String _Base_Impuesto_Division;
        private String _Minimo_Elevado_Anio;
        private String _Fundamento;
        private OracleCommand _Cmd_Calculo;
        private String _Filtro_Estatus;

        private String _Filtro_Dinamico = "";
        private String _Campos_Dinamicos = "";
        private String _Ordenar_Dinamico = "";
        private bool _Incluir_Campos_Foraneos;
        private bool _Incluir_Observaciones;
        private bool _Incluir_Datos_Cuenta;
        DateTime _Fecha_Creo;
        DataTable _Dt_Observaciones;

        private String _Referencia;
        private String _Clave_Ingreso_ID;
        private String _Descripcion;
        private String _Fecha_Tramite;
        private String _Fecha_Vencimiento_Pasivo;
        private String _Monto_Pasivo;
        private String _Recargos;
        private String _Estatus_Pasivo;
        private String _Dependencia_ID;
        private String _Contribuyente;

        private Boolean Mostrar_Contrarecibos_Sin_Calculo;

        #endregion


/// --------------------------------------- Propiedades públicas ---------------------------------------
#region (Propiedades Publicas)
        public String P_No_Calculo
        {
            get { return _No_Calculo; }
            set { _No_Calculo = value; }
        }
        public Int32 P_Anio_Calculo
        {
            get { return _Anio_Calculo; }
            set { _Anio_Calculo = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return _Cuenta_Predial; }
            set { _Cuenta_Predial = value; }
        }
        public String P_Cuenta_Predial_ID
        {
            get { return _Cuenta_Predial_ID; }
            set { _Cuenta_Predial_ID = value; }
        }
        public String P_SubConcepto_ID
        {
            get { return SubConcepto_ID; }
            set { SubConcepto_ID = value; }
        }
        public String P_Concepto_ID
        {
            get { return _Concepto_ID; }
            set { _Concepto_ID = value; }
        }
        public String P_No_Orden_Variacion
        {
            get { return _No_Orden_Variacion; }
            set { _No_Orden_Variacion = value; }
        }
        public Int32 P_Anio_Orden
        {
            get { return _Anio_Orden; }
            set { _Anio_Orden = value; }
        }
        public String P_No_Contrarecibo
        {
            get { return _No_Contrarecibo; }
            set { _No_Contrarecibo = value; }
        }
        public Int32 P_Anio_Contrarecibo
        {
            get { return _Anio_Contrarecibo; }
            set { _Anio_Contrarecibo = value; }
        }
        public String P_Predio_Colindante
        {
            get { return _Predio_Colindante; }
            set { _Predio_Colindante = value; }
        }
        public String P_Base_Impuesto
        {
            get { return _Base_Impuesto; }
            set { _Base_Impuesto = value; }
        }
        public String P_Tasas_ID
        {
            get { return _Tasas_ID; }
            set { _Tasas_ID = value; }
        }
        public String P_Multa_ID
        {
            get { return _Multa_ID; }
            set { _Multa_ID = value; }
        }
        public String P_Fecha_Escritura
        {
            get { return _Fecha_Escritura; }
            set { _Fecha_Escritura = value; }
        }
        public String P_Impuesto_Div_Lot
        {
            get { return _Impuesto_Div_Lot; }
            set { _Impuesto_Div_Lot = value; }
        }
        public String P_Monto_Multa
        {
            get { return _Monto_Multa; }
            set { _Monto_Multa = value; }
        }
        public String P_Monto_Traslado
        {
            get { return _Monto_Traslado; }
            set { _Monto_Traslado = value; }
        }
        public String P_Monto_Division
        {
            get { return _Monto_Division; }
            set { _Monto_Division = value; }
        }
        public String P_Monto_Recargos
        {
            get { return _Monto_Recargos; }
            set { _Monto_Recargos = value; }
        }
        public String P_Monto_Total_Pagar
        {
            get { return _Monto_Total_Pagar; }
            set { _Monto_Total_Pagar = value; }
        }
        public String P_Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }
        public String P_Costo_Constancia
        {
            get { return _Costo_Constancia; }
            set { _Costo_Constancia = value; }
        }
        public String P_Observaciones
        {
            get { return _Observaciones; }
            set { _Observaciones = value; }
        }
        public String P_Origen
        {
            get { return _Origen; }
            set { _Origen = value; }
        }
        public String P_Estatus
        {
            get { return _Estatus; }
            set { _Estatus = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return _Nombre_Usuario; }
            set { _Nombre_Usuario = value; }
        }
        public String P_Estatus_Orden
        {
            get { return _Estatus_Orden; }
            set { _Estatus_Orden = value; }
        }
        public String P_Estatus_Descuento
        {
            get { return _Estatus_Descuento; }
            set { _Estatus_Descuento = value; }
        }
        public String P_Numero_Adeudo
        {
            get { return _Numero_Adeudo; }
            set { _Numero_Adeudo = value; }
        }
        public String P_Minimo_Elevado_Anio
        {
            get { return _Minimo_Elevado_Anio; }
            set { _Minimo_Elevado_Anio = value; }
        }
        public String P_Base_Impuesto_Division
        {
            get { return _Base_Impuesto_Division; }
            set { _Base_Impuesto_Division = value; }
        }
        public String P_Fundamento
        {
            get { return _Fundamento; }
            set { _Fundamento = value; }
        }
        public OracleCommand P_Cmd_Calculo
        {
            get { return _Cmd_Calculo; }
            set { _Cmd_Calculo = value; }
        }
        public String P_Filtro_Estatus
        {
            get { return _Filtro_Estatus; }
            set { _Filtro_Estatus = value; }
        }
        public String P_Filtro_Dinamico
        {
            get { return _Filtro_Dinamico; }
            set { _Filtro_Dinamico = value; }
        }
        public String P_Campos_Dinamicos
        {
            get { return _Campos_Dinamicos; }
            set { _Campos_Dinamicos = value; }
        }
        public String P_Ordenar_Dinamico
        {
            get { return _Ordenar_Dinamico; }
            set { _Ordenar_Dinamico = value; }
        }
        public bool P_Incluir_Campos_Foraneos
        {
            get { return _Incluir_Campos_Foraneos; }
            set { _Incluir_Campos_Foraneos = value; }
        }
        public bool P_Incluir_Observaciones
        {
            get { return _Incluir_Observaciones; }
            set { _Incluir_Observaciones = value; }
        }
        public bool P_Incluir_Generales_Cuenta
        {
            get { return _Incluir_Datos_Cuenta; }
            set { _Incluir_Datos_Cuenta = value; }
        }
        public DateTime P_Fecha_Creo
        {
            get { return _Fecha_Creo; }
            set { _Fecha_Creo = value; }
        }
        public DataTable P_Dt_Observaciones
        {
            get { return _Dt_Observaciones; }
            set { _Dt_Observaciones = value; }
        }


        public String P_Referencia
        {
            get { return _Referencia; }
            set { _Referencia = value; }
        }
        public String P_Clave_Ingreso_ID
        {
            get { return _Clave_Ingreso_ID; }
            set { _Clave_Ingreso_ID = value; }
        }
        public String P_Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
        public String P_Fecha_Tramite
        {
            get { return _Fecha_Tramite; }
            set { _Fecha_Tramite = value; }
        }
        public String P_Fecha_Vencimiento_Pasivo
        {
            get { return _Fecha_Vencimiento_Pasivo; }
            set { _Fecha_Vencimiento_Pasivo = value; }
        }
        public String P_Monto_Pasivo
        {
            get { return _Monto_Pasivo; }
            set { _Monto_Pasivo = value; }
        }
        public String P_Recargos
        {
            get { return _Recargos; }
            set { _Recargos = value; }
        }
        public String P_Estatus_Pasivo
        {
            get { return _Estatus_Pasivo; }
            set { _Estatus_Pasivo = value; }
        }
        public String P_Dependencia_ID
        {
            get { return _Dependencia_ID; }
            set { _Dependencia_ID = value; }
        }
        public String P_Contribuyente
        {
            get { return _Contribuyente; }
            set { _Contribuyente = value; }
        }

        public Boolean P_Mostrar_Contrarecibos_Sin_Calculo
        {
            get { return Mostrar_Contrarecibos_Sin_Calculo; }
            set { Mostrar_Contrarecibos_Sin_Calculo = value; }
        }
#endregion


/// --------------------------------------- Metodos ---------------------------------------
#region (Metodos)
        public int Alta_Calculo()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Alta_Calculo(this);
        }

        public DataTable Consulta_Calculos()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consulta_Calculos(this);
        }
        public DataTable Consulta_Calculos_Contrarecibo()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consulta_Calculos_Contrarecibo(this);
        }
        public DataTable Consulta_Calculos_Contrarecibo_Cancelado()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consulta_Calculos_Contrarecibo_Cancelado(this);
        }
        public DataTable Consulta_Pendientes_Calcular_Rechazados()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consulta_Pendientes_Calcular_Rechazados(this);
        }
        public DataTable Consultar_Calculos_Ordenes()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consultar_Calculos_Ordenes(this);
        }
        public DataTable Consultar_Calculos_Ordenes_Solo_Contrarecibo()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consultar_Calculos_Ordenes_Solo_Contrarecibo(this);
        }
        public DataTable Consulta_Detalles_Calculo()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consulta_Detalles_Calculo(this);
        }
        public DataTable Consulta_Detalles_Orden_Variacion()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consulta_Detalles_Orden_Variacion(this);
        }
        public DataTable Consultar_Ordenes_Variacion()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consultar_Ordenes_Variacion(this);
        }
        public DataTable Consultar_Datos_Cuenta_Predial()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consultar_Datos_Cuenta_Predial(this);
        }
        public int Actualizar_Calculo()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Actualizar_Calculo(this);
        }

        public int Actualizar_Estatus_Calculo()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Actualizar_Estatus_Calculo(this);
        }

        public int Actualizar_Estatus_Contrarecibo()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Actualizar_Estatus_Contrarecibo(this);
        }

        public DataTable Consulta_Folio_Orden_Contrarecibo(String No_Orden, Int32 Anio)
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consulta_Folio_Orden_Contrarecibo(No_Orden, Anio);
        }
        public int Alta_Pasivo()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Alta_Pasivo(this);
        }
        public int Eliminar_Referencias_Pasivo()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Eliminar_Referencias_Pasivo(this);
        }
        public int Cancelar_Descuentos_Caducados()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Cancelar_Descuentos_Caducados(this);
        }
        public int Modificar_Descuentos_Caducados()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Modificar_Descuentos_Caducados();
        }
        public DataTable Consultar_Descuentos_Traslado()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consultar_Descuentos_Traslado(this);
        }

        public DataTable Consulta_Calculos_Convenio()
        {
            return Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos.Consulta_Calculos_Convenio(this);
        }

#endregion (Metodos)

    }

}
