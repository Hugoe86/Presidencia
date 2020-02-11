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
using Presidencia.Operacion_Cierre_Definitivo_Dia.Datos;


/// <summary>
/// Summary description for Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio
/// </summary>
/// 
namespace Presidencia.Operacion_Cierre_Definitivo_Dia.Negocio
{
    public class Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio
    {

        #region Varibles Internas

            private String Modulo_ID;
            private String Caja_ID;
            private DataTable Dt_Cierre;

        #endregion

            #region Varibles Publicas


            public DataTable P_Dt_Cierre
            {
                get { return Dt_Cierre; }
                set { Dt_Cierre = value; }
            }
            public String P_Caja_ID
            {
                get { return Caja_ID; }
                set { Caja_ID = value; }
            }

            public String P_Modulo_ID
            {
                get { return Modulo_ID; }
                set { Modulo_ID = value; }
            }
            #endregion

            #region Metodos
            public DataTable Consultar_Impresion()
            {
                return Cls_Ope_Pre_Cierre_Definitivo_Dia_Datos.Consultar_Impresion(this);
            }

            public DataTable Consultar_Cajero()
            {
                return Cls_Ope_Pre_Cierre_Definitivo_Dia_Datos.Consultar_Cajero(this);
            }


            public DataTable Llenar_Combo_Caja()
            {
                return Cls_Ope_Pre_Cierre_Definitivo_Dia_Datos.Llenar_Combo_Caja(this);
            }

        public DataTable Llenar_Combo_Modulos()
        {
            return Cls_Ope_Pre_Cierre_Definitivo_Dia_Datos.Llenar_Combo_Modulos(this);
        }
        public DataTable Consultar_Cierres_Dia()
        {
            return Cls_Ope_Pre_Cierre_Definitivo_Dia_Datos.Consultar_Cierres_Dia(this);
        }

        #endregion

    }
}
