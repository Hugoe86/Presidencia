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
using Presidencia.Cls_Ope_Ing_Descuentos.Datos;
using System.Data.OracleClient;

namespace Presidencia.Cls_Ope_Ing_Descuentos.Negocio
{

    public class Cls_Ope_Ing_Descuentos_Negocio
    {

        #region Variables Internas

        private String No_Descuento;
        private String Concepto_Orden_Pago_ID;
        private String Referencia;
        private Decimal Unidades;
        private Decimal Monto_Honorarios;
        private Decimal Monto_Multas;
        private Decimal Monto_Moratorios;
        private Decimal Monto_Recargos;
        private Decimal Descuento_Honorarios;
        private Decimal Descuento_Multas;
        private Decimal Descuento_Moratorios;
        private Decimal Descuento_Recargos;
        private Decimal Total_Pagar;
        private String Estatus;
        private DateTime Fecha_Descuento;
        private DateTime Fecha_Vencimiento;
        private String Fundamento_Legal;
        private String Observaciones;
        private String Realizo;
        private String Usuario;
        private DateTime Fecha_Creo;

        private DataTable Dt_Descuentos;

        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private String Join;
        private String Unir_Tablas;

        private OracleCommand Cmmd;

        #endregion

        #region Variables Publicas
        public String P_No_Descuento
        {
            get { return No_Descuento; }
            set { No_Descuento = value; }
        }

        public String P_Concepto_Orden_Pago_ID
        {
            get { return Concepto_Orden_Pago_ID; }
            set { Concepto_Orden_Pago_ID = value; }
        }

        public String P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }

        public Decimal P_Unidades
        {
            get { return Unidades; }
            set { Unidades = value; }
        }

        public Decimal P_Monto_Honorarios
        {
            get { return Monto_Honorarios; }
            set { Monto_Honorarios = value; }
        }

        public Decimal P_Monto_Multas
        {
            get { return Monto_Multas; }
            set { Monto_Multas = value; }
        }

        public Decimal P_Monto_Moratorios
        {
            get { return Monto_Moratorios; }
            set { Monto_Moratorios = value; }
        }

        public Decimal P_Monto_Recargos
        {
            get { return Monto_Recargos; }
            set { Monto_Recargos = value; }
        }

        public Decimal P_Descuento_Honorarios
        {
            get { return Descuento_Honorarios; }
            set { Descuento_Honorarios = value; }
        }

        public Decimal P_Descuento_Multas
        {
            get { return Descuento_Multas; }
            set { Descuento_Multas = value; }
        }

        public Decimal P_Descuento_Moratorios
        {
            get { return Descuento_Moratorios; }
            set { Descuento_Moratorios = value; }
        }

        public Decimal P_Descuento_Recargos
        {
            get { return Descuento_Recargos; }
            set { Descuento_Recargos = value; }
        }

        public Decimal P_Total_Pagar
        {
            get { return Total_Pagar; }
            set { Total_Pagar = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public DateTime P_Fecha_Descuento
        {
            get { return Fecha_Descuento; }
            set { Fecha_Descuento = value; }
        }

        public DateTime P_Fecha_Vencimiento
        {
            get { return Fecha_Vencimiento; }
            set { Fecha_Vencimiento = value; }
        }

        public String P_Fundamento_Legal
        {
            get { return Fundamento_Legal; }
            set { Fundamento_Legal = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_Realizo
        {
            get { return Realizo; }
            set { Realizo = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public DateTime P_Fecha_Creo
        {
            get { return Fecha_Creo; }
            set { Fecha_Creo = value; }
        }

        public DataTable P_Dt_Descuentos
        {
            get { return Dt_Descuentos; }
            set { Dt_Descuentos = value; }
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

        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Descuentos()
        {
            return Cls_Ope_Ing_Descuentos_Datos.Alta_Descuentos(this);
        }

        public Boolean Modificar_Descuentos()
        {
            return Cls_Ope_Ing_Descuentos_Datos.Modificar_Descuentos(this);
        }

        public DataTable Consultar_Descuentos()
        {
            return Cls_Ope_Ing_Descuentos_Datos.Consultar_Descuentos(this);
        }
        #endregion

    }
}