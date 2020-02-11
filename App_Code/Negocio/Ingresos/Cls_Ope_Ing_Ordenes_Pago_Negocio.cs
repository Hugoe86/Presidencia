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
using Presidencia.Cls_Ope_Ing_Ordenes_Pago.Datos;

namespace Presidencia.Cls_Ope_Ing_Ordenes_Pago.Negocio
{

    public class Cls_Ope_Ing_Ordenes_Pago_Negocio
    {
        #region Variables Internas

        private String No_Orden_Pago;
        private Int32 Año;
        private String Concepto_Orden_Pago_ID;
        private String Contribuyente_ID;
        private String Dependencia_ID;
        private String SubConcepto_Ing_ID;
        private String Tipo_Pago_ID;
        private String Garantia_Proceso_ID;
        private String Banco_ID;
        private String Fuente_Financiamiento_ID;
        private String Numero_Garantia;
        private String Clave_Padron;
        private String Estatus;
        private String Folio;
        private String Proteccion;
        private DateTime Fecha_Multa;
        private Decimal Unidades;
        private Decimal Importe;
        private Decimal Descuento_Importe;
        private Decimal Monto_Importe;
        private Decimal Honorarios;
        private Decimal Multas;
        private Decimal Moratorios;
        private Decimal Recargos;
        private Decimal Ajuste_Tarifario;
        private Decimal Total;
        private String Referencia;
        private String Observaciones;
        private String Usuario;

        private DataTable Dt_Conceptos_Ordenes_Pago;

        private Boolean Incluir_Campos_Vitacora;
        private Boolean Incluir_Campos_Foraneos;
        private String Incluir_Campos_Dinamicos;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private String Join;
        private String Unir_Tablas;

        private Boolean Actualiza_Conceptos = true;

        private OracleCommand Cmmd;
        #endregion

        #region Variables Publicas

        public String P_No_Orden_Pago
        {
            get { return No_Orden_Pago; }
            set { No_Orden_Pago = value; }
        }

        public Int32 P_Año
        {
            get { return Año; }
            set { Año = value; }
        }

        public String P_Concepto_Orden_Pago_ID
        {
            get { return Concepto_Orden_Pago_ID; }
            set { Concepto_Orden_Pago_ID = value; }
        }
        public String P_Contribuyente_ID
        {
            get { return Contribuyente_ID; }
            set { Contribuyente_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_SubConcepto_Ing_ID
        {
            get { return SubConcepto_Ing_ID; }
            set { SubConcepto_Ing_ID = value; }
        }

        public String P_Tipo_Pago_ID
        {
            get { return Tipo_Pago_ID; }
            set { Tipo_Pago_ID = value; }
        }

        public String P_Garantia_Proceso_ID
        {
            get { return Garantia_Proceso_ID; }
            set { Garantia_Proceso_ID = value; }
        }

        public String P_Banco_ID
        {
            get { return Banco_ID; }
            set { Banco_ID = value; }
        }

        public String P_Fuente_Financiamiento_ID
        {
            get { return Fuente_Financiamiento_ID; }
            set { Fuente_Financiamiento_ID = value; }
        }

        public String P_Numero_Garantia
        {
            get { return Numero_Garantia; }
            set { Numero_Garantia = value; }
        }

        public String P_Clave_Padron
        {
            get { return Clave_Padron; }
            set { Clave_Padron = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }

        public String P_Proteccion
        {
            get { return Proteccion; }
            set { Proteccion = value; }
        }

        public DateTime P_Fecha_Multa
        {
            get { return Fecha_Multa; }
            set { Fecha_Multa = value; }
        }

        public Decimal P_Unidades
        {
            get { return Unidades; }
            set { Unidades = value; }
        }

        public Decimal P_Importe
        {
            get { return Importe; }
            set { Importe = value; }
        }

        public Decimal P_Descuento_Importe
        {
            get { return Descuento_Importe; }
            set { Descuento_Importe = value; }
        }

        public Decimal P_Monto_Importe
        {
            get { return Monto_Importe; }
            set { Monto_Importe = value; }
        }

        public Decimal P_Honorarios
        {
            get { return Honorarios; }
            set { Honorarios = value; }
        }

        public Decimal P_Multas
        {
            get { return Multas; }
            set { Multas = value; }
        }

        public Decimal P_Moratorios
        {
            get { return Moratorios; }
            set { Moratorios = value; }
        }

        public Decimal P_Recargos
        {
            get { return Recargos; }
            set { Recargos = value; }
        }

        public Decimal P_Ajuste_Tarifario
        {
            get { return Ajuste_Tarifario; }
            set { Ajuste_Tarifario = value; }
        }

        public Decimal P_Total
        {
            get { return Total; }
            set { Total = value; }
        }

        public String P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }


        public DataTable P_Dt_Conceptos_Ordenes_Pago
        {
            get { return Dt_Conceptos_Ordenes_Pago; }
            set { Dt_Conceptos_Ordenes_Pago = value; }
        }



        public Boolean P_Incluir_Campos_Vitacora
        {
            get { return Incluir_Campos_Vitacora; }
            set { Incluir_Campos_Vitacora = value; }
        }

        public Boolean P_Incluir_Campos_Foraneos
        {
            get { return Incluir_Campos_Foraneos; }
            set { Incluir_Campos_Foraneos = value; }
        }

        public String P_Incluir_Campos_Dinamicos
        {
            get { return Incluir_Campos_Dinamicos; }
            set { Incluir_Campos_Dinamicos = value; }
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

        public String P_Join
        {
            get { return Join; }
            set { Join = value; }
        }

        public String P_Unir_Tablas
        {
            get { return Unir_Tablas; }
            set { Unir_Tablas = value; }
        }


        public Boolean P_Actualiza_Conceptos
        {
            get { return Actualiza_Conceptos; }
            set { Actualiza_Conceptos = value; }
        }


        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }
        #endregion

        #region Metodos
        public Boolean Alta_Orden_Pago()
        {
            return Cls_Ope_Ing_Ordenes_Pago_Datos.Alta_Orden_Pago(this);
        }

        public Boolean Modificar_Orden_Pago()
        {
            return Cls_Ope_Ing_Ordenes_Pago_Datos.Modificar_Orden_Pago(this);
        }

        public Boolean Eliminar_Orden_Pago()
        {
            return Cls_Ope_Ing_Ordenes_Pago_Datos.Eliminar_Orden_Pago(this);
        }

        public DataTable Consultar_Ordenes_Pago()
        {
            return Cls_Ope_Ing_Ordenes_Pago_Datos.Consultar_Ordenes_Pago(this);
        }

        public DataTable Consultar_Conceptos_Ordenes_Pago()
        {
            return Cls_Ope_Ing_Ordenes_Pago_Datos.Consultar_Conceptos_Ordenes_Pago(this);
        }
        #endregion

    }
}