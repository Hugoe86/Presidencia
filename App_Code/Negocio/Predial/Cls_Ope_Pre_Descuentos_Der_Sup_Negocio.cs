using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Ope_Pre_Descuentos_Der_Sup.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Ope_Pre_Descuentos_Der_Sup_Negocio
/// </summary>

namespace Presidencia.Ope_Pre_Descuentos_Der_Sup.Negocio
{
    public class Cls_Ope_Pre_Descuentos_Der_Sup_Negocio
    {

        #region Variables Privadas

        private String No_Descuento;
        private String Cuenta_Predial_Id;
        private String Cuenta_Predial;
        private String Estatus;
        private DateTime Fecha;
        private Double Desc_Multa;
        private Double Desc_Recargo;
        private Double Total_Por_Pagar;
        private String Realizo;
        private DateTime Fecha_Vencimiento;
        private String Observaciones;
        private String Fundamento_Legal;
        private String No_Impuesto_Derecho_Supervision;
        private String Referencia;
        private Double Monto_Recargos;
        private Double Monto_Multas;
        private String Usuario_Creo;
        private DateTime Fecha_Creo;
        private String Usuario_Modifico;
        private DateTime Fecha_Modifico;
        private String Contribuyente_Id;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupadores_Dinamicos;
        private String Orden_Dinamico;

        #endregion

        #region Variables publicas

        public String P_No_Descuento
        {
            get { return No_Descuento; }
            set { No_Descuento = value; }
        }
        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public DateTime P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }
        public Double P_Desc_Multa
        {
            get { return Desc_Multa; }
            set { Desc_Multa = value; }
        }
        public Double P_Desc_Recargo
        {
            get { return Desc_Recargo; }
            set { Desc_Recargo = value; }
        }
        public Double P_Total_Por_Pagar
        {
            get { return Total_Por_Pagar; }
            set { Total_Por_Pagar = value; }
        }
        public String P_Realizo
        {
            get { return Realizo; }
            set { Realizo = value; }
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
        public String P_Fundamento_Legal
        {
            get { return Fundamento_Legal; }
            set { Fundamento_Legal = value; }
        }
        public String P_No_Impuesto_Derecho_Supervision
        {
            get { return No_Impuesto_Derecho_Supervision; }
            set { No_Impuesto_Derecho_Supervision = value; }
        }
        public String P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }
        public Double P_Monto_Recargos
        {
            get { return Monto_Recargos; }
            set { Monto_Recargos = value; }
        }
        public Double P_Monto_Multas
        {
            get { return Monto_Multas; }
            set { Monto_Multas = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public DateTime P_Fecha_Creo
        {
            get { return Fecha_Creo; }
            set { Fecha_Creo = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        public DateTime P_Fecha_Modifico
        {
            get { return Fecha_Modifico; }
            set { Fecha_Modifico = value; }
        }
        public String P_Contribuyente_Id
        {
            get { return Contribuyente_Id; }
            set { Contribuyente_Id = value; }
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
        public String P_Agrupadores_Dinamicos
        {
            get { return Agrupadores_Dinamicos; }
            set { Agrupadores_Dinamicos = value; }
        }
        public String P_Orden_Dinamico
        {
            get { return Orden_Dinamico; }
            set { Orden_Dinamico = value; }
        }

        #endregion

        #region Métodos

        public DataTable Consultar_Descuentos()
        {
            return Cls_Ope_Pre_Descuentos_Der_Sup_Datos.Consultar_Descuentos(this);
        }
        public DataTable Consultar_Descuento_Activo()
        {
            return Cls_Ope_Pre_Descuentos_Der_Sup_Datos.Consultar_Descuento_Activo(this);
        }
        public void Alta_Descuento()
        {
            Cls_Ope_Pre_Descuentos_Der_Sup_Datos.Alta_Descuento(this);
        }
        public void Modificar_Descuentos()
        {
            Cls_Ope_Pre_Descuentos_Der_Sup_Datos.Modificar_Descuento(this);
        }

        #endregion

    }
}