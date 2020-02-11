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
using Presidencia.Constantes;
using Presidencia.Operacion_Atencion_Ciudadana_Reiniciar_Folios.Datos;
using System.Data.OracleClient;

namespace Presidencia.Operacion_Atencion_Ciudadana_Reiniciar_Folios.Negocios
{
    public class Cls_Ope_Ate_Reiniciar_Folios_Negocio
    {
        #region Variables Locales

        private string Prefijo_Folio;
        private int Anio_Actual;
        private string Programa_ID;
        private string Usuario_Modifico;

        #endregion Variables Locales

        #region Variables Publicas

        public string P_Prefijo_Folio
        {
            get { return Prefijo_Folio; }
            set { Prefijo_Folio = value; }
        }
        public int P_Anio_Actual
        {
            get { return Anio_Actual; }
            set { Anio_Actual = value; }
        }
        public string P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }

        public string P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        #endregion Variables Publicas

        #region Metodos

        public int Reiniciar_Folios()
        {
            return Cls_Ope_Ate_Reiniciar_Folios_Datos.Reiniciar_Folios(this);
        }
        #endregion

    }
}
