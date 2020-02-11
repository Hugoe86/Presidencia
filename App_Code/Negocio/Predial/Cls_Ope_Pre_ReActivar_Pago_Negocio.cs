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
using Presidencia.Operacion_ReActivar_Pago.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Cancelacion_Pago_Negocio
/// </summary>

namespace Presidencia.Operacion_ReActivar_Pago.Negocio
{
    public class Cls_Ope_Pre_ReActivar_Pago_Negocio
    {
        #region Variables Internas

            private String Motivo_Cancelacion_Id;
            private String Estatus;
            private String Comentarios;
            private Int32 No_Recibo;
            private String No_Pago;
            private Int32 No_Operacion;
            private String Filtro;
            private String Cajero;
            private String Modulo;
            private String Caja;
            private String Fecha;
            private String Monto;
            private String Empleado_ID;
            private String No_Turno;
            private String Usuario;
        #endregion

        #region Variables Publicas

        public String P_Motivo_Cancelacion_Id
        {
            get { return Motivo_Cancelacion_Id; }
            set { Motivo_Cancelacion_Id = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public Int32 P_No_Recibo
        {
            get { return No_Recibo; }
            set { No_Recibo = value; }
        }

        public String P_No_Pago
        {
            get { return No_Pago; }
            set { No_Pago = value; }
        }

        public Int32 P_No_Operacion
        {
            get { return No_Operacion; }
            set { No_Operacion = value; }
        }

        public String P_Filtro
        {
            get { return Filtro; }
            set { Filtro = value; }
        }

        public String P_Cajero
        {
            get { return Cajero; }
            set { Cajero = value; }
        }

        public String P_Caja
        {
            get { return Caja; }
            set { Caja = value; }
        }

        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public String P_Monto
        {
            get { return Monto; }
            set { Monto = value; }
        }

        public String P_Modulo
        {
            get { return Modulo; }
            set { Modulo = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_No_Turno
        {
            get { return No_Turno; }
            set { No_Turno = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region Metodos

        public void Reactivar_Pago()
        {
            Cls_Ope_Pre_ReActivar_Pago_Datos.Reactivar_Pago(this);
        }

        public DataTable Consultar_Cancelacion_Pagos()
        {
            return Cls_Ope_Pre_ReActivar_Pago_Datos.Consultar_Cancelaciones(this);
        }

        public Cls_Ope_Pre_ReActivar_Pago_Negocio Consultar_Datos_cancelacion_Pagos()
        {
            return Cls_Ope_Pre_ReActivar_Pago_Datos.Consultar_Datos_Cancelaciones(this);
        }

        public DataTable Consulta_Caja_Empleado()
        {
            return Cls_Ope_Pre_ReActivar_Pago_Datos.Consulta_Caja_Empleado(this);
        }
        #endregion
    }
}