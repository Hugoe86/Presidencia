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
using Presidencia.Nomina_Reporte_Cantidad_Recibos.Datos;

namespace Presidencia.Nomina_Reporte_Cantidad_Recibos.Negocio
{
    public class Cls_Rpt_Nom_Cantidad_Recibos_Negocio
    {
        #region Variables Privadas
        private String Nomina_id;
        private String No_Nomina;
        private String Banco_ID;
        private String Tipo_Nomina_ID;
        #endregion

        #region Variables Publicas

        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        public String P_Nomina_id
        {
            get { return Nomina_id; }
            set { Nomina_id = value; }
        }
        public String P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
        public String P_Banco_ID
        {
            get { return Banco_ID; }
            set { Banco_ID = value; }
        }
        #endregion

        #region(Metodos)
        public DataTable Consultar_Cantidad_Recibos_Impresos()
        {
            return Cls_Rpt_Nom_Cantidad_Recibos_Datos.Consultar_Cantidad_Recibos_Impresos(this);
        }
        #endregion

    }
}