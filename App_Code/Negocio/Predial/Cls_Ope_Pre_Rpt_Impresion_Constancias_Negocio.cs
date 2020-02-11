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
using Presidencia.Ope_Pre_Rpt_Impresion_Constancias.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pre_Rpt_Impresion_Constancias_Negocio
/// </summary>

namespace Presidencia.Ope_Pre_Rpt_Impresion_Constancias.Negocio
{
    public class Cls_Ope_Pre_Rpt_Impresion_Constancias_Negocio
    {
        #region Varibles Internas

        String Fecha_Inicial;
        String Fecha_Final;
        String Estatus;
        String Tipo;

        #endregion

        #region Varibles Publicas

        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }

        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        #endregion

        #region Métodos

        public DataTable Consultar_Clave_Gasto_Ejecucion()
        {
            return Cls_Ope_Pre_Rpt_Impresion_Constancias_Datos.Consultar_Constancias_No_Propiedad(this);
        }

        public DataTable Consultar_Constancias_Y_Certificaciones()
        {
            return Cls_Ope_Pre_Rpt_Impresion_Constancias_Datos.Consultar_Constancias_Y_Certificaciones(this);
        }

        #endregion

    }
}