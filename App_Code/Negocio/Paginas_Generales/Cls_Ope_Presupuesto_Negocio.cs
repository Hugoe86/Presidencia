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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;
using Presidencia.SCG_Presupuestos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Presupuesto_Negocio
/// </summary>
/// 
namespace Presidencia.SCG_Presupuestos.Negocio
{

    public class Cls_Ope_Presupuesto_Negocio
    {
        public Cls_Ope_Presupuesto_Negocio()
        {

        }
        #region Variables Privadas

        private String Fte_Financiamiento_ID;
        private String Dependencia_ID;
        private String Proyecto_Programa_ID;
        private String Partida_ID;
                
        #endregion

        #region Variables Publicas

        public String P_Fte_Financiamiento_ID
        {
            get { return Fte_Financiamiento_ID; }
            set { Fte_Financiamiento_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Proyecto_Programa_ID
        {
            get { return Proyecto_Programa_ID; }
            set { Proyecto_Programa_ID = value; }
        }

        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }
        #endregion
        #region Metodos
        
        public DataTable Consultar_Presupuesto_UR() 
        {
            return Cls_Ope_Presupuesto_Datos.Consultar_Presupuesto(this);
        }
        #endregion

    }
}