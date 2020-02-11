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
using Presidencia.Operacion_Predial_Historial_Movimientos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pre_Historial_Movimientos_Negocio
/// </summary>
/// 


namespace Presidencia.Operacion_Predial_Historial_Movimientos.Negocio
{
    public class Cls_Ope_Pre_Historial_Movimientos_Negocio
    {
        #region Variables Internas
        private string Cuenta_Predial;
        private String Entre_Fecha;
        private String Y_Fecha;
        private String Entre_Movimiento;
        private String Y_Movimiento;
        private String Tipo;

        #endregion

        #region Variables Publicas
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
        public String P_Entre_Fecha
        {
            get { return Entre_Fecha; }
            set { Entre_Fecha = value; }
        }
        public String P_Y_Fecha
        {
            get { return Y_Fecha; }
            set { Y_Fecha = value; }
        }
        public String P_Entre_Movimiento
        {
            get { return Entre_Movimiento; }
            set { Entre_Movimiento = value; }
        }
        public String P_Y_Movimiento
        {
            get { return Y_Movimiento; }
            set { Y_Movimiento = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        #endregion
        public DataTable Consultar_Historial_Movimientos()
            {
                return Cls_Ope_Pre_Historial_Movimientos_Datos.Consultar_Historial_Movimientos(this);
            }
        
    }
}
