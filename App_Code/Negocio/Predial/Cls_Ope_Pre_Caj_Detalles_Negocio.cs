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
using Presidencia.Operacion_Pre_Caj_Detalles.Datos;

/// <summary>
/// Summary description for Cls_Ope_Caj_Cierre_Dia_Negocio
/// </summary>

namespace Presidencia.Operacion_Pre_Caj_Detalles.Negocio
{
    public class Cls_Ope_Pre_Caj_Detalles_Negocio
    {

        #region Variables Internas
        private String Modulo_id;
        private String Caja_id;
        private String Empleado_ID;
        private String Fecha;
        private String Fecha_Final;
        private String Estatus;
        private DataTable Dt_datos;
        #region Variables Publicas

        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }
        public DataTable P_Dt_Datos
        {
            get { return Dt_datos ;}
            set { Dt_datos = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Modulo_Id
        {
            get { return Modulo_id; }
            set { Modulo_id = value; }
        }
        public String P_Caja_Id
        {
            get { return Caja_id; }
            set { Caja_id = value; }
        }

        #endregion

        #region Metodos

        public DataTable Consulta_Pagos()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Consultar_Pagos(this);
        }
        public DataSet Consulta_Caja()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Consultar_Caja(this);
        }
        public DataTable Consulta_Pagos_General()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Consultar_Pagos_General(this);
        }
        public DataTable Reporte_Desglosado_Tarjeta_Bancaria()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Reporte_Desglosado_Tarjeta_Bancaria(this);
        }
        public DataTable Reporte_Concentracion_Monetarea()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Reporte_Concentracion_Monetarea(this);
        }
        public DataTable Reporte_Pagos_tarjetas()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Reporte_Pagos_tarjetas(this);
        }
        public DataTable Reporte_Detallado_Pagos_Tarjeta()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Reporte_Detallado_Pagos_Tarjeta(this);
        }
        public DataTable Reporte_Detallado_Pagos_Cheque()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Reporte_Detallado_Pagos_Cheque(this);
        }
        public DataTable Reporte_Detallado_Pagos_Transferencia()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Reporte_Detallado_Pagos_Transferencia(this);
        }
        public DataTable Reporte_Concentracion_Ingreso()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Reporte_Concentracion_Ingreso(this);
        }
        public DataTable Consultar_Resumen_Diario_Ingresos()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Consultar_Resumen_Diario_Ingresos();
        }
        public DataTable Consultar_Corte_Caja()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Consultar_Corte_Caja( this);
        }
        public DataTable Consultar_Analisis_Entrega_Dia()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Consultar_Analisis_Entrega_Dia(this);
        }
        public DataTable Reporte_Recibos_Cancelados_Empleado()
        {
            return Cls_Ope_Pre_Caj_Detalles_Datos.Reporte_Recibos_Cancelados_Empleado(this);
        }
        #endregion

    }
        #endregion
}