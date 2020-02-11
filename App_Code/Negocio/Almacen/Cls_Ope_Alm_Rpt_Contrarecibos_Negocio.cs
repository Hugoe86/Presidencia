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
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;
using Presidencia.Almacen_Reporte_Contrarecibos.Datos;

namespace Presidencia.Almacen_Reporte_Contrarecibos.Negocio
{
    public class Cls_Ope_Alm_Rpt_Contrarecibos_Negocio
    {
        #region Variables Locales
        private String Fecha_Inicial = null;
        private String Fecha_Final = null;
        private String Proveedor_ID = null;
        private String No_Contra_Recibo = null;
        private String No_Contra_Recibo_Inicial = null;
        private String No_Contra_Recibo_Final = null;
        private String Documento_ID = null;
        #endregion

        #region Variables Publicas
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
        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
        public String P_No_Contra_Recibo
        {
            get { return No_Contra_Recibo; }
            set { No_Contra_Recibo = value; }
        }
        public String P_No_Contra_Recibo_Inicial
        {
            get { return No_Contra_Recibo_Inicial; }
            set { No_Contra_Recibo_Inicial = value; }
        }
        public String P_No_Contra_Recibo_Final
        {
            get { return No_Contra_Recibo_Final; }
            set { No_Contra_Recibo_Final = value; }
        }
        public String P_Documento_ID
        {
            get { return Documento_ID; }
            set { Documento_ID = value; }
        }
        #endregion

        #region Metodos
        public DataTable Consultar_Contra_Recibos()
        {
            return Cls_Ope_Alm_Rpt_Contrarecibos_Datos.Consultar_ContraRecibos(this);
        }
        public DataTable Consultar_Nombre_Proveedor()
        {
            return Cls_Ope_Alm_Rpt_Contrarecibos_Datos.Consultar_Nombre_Proveedor(this);
        }
        public DataTable Consultar_Numero_Contra_Recibos()
        {
            return Cls_Ope_Alm_Rpt_Contrarecibos_Datos.Consultar_Numero_Contra_Recibos(this);
        }
        public DataTable Consultar_Documentos()
        {
            return Cls_Ope_Alm_Rpt_Contrarecibos_Datos.Consultar_Documentos(this);
        }
        public DataTable Consultar_Documentos_Soporte()
        {
            return Cls_Ope_Alm_Rpt_Contrarecibos_Datos.Consultar_Documentos_Soporte(this);
        }
        
        #endregion
        
    }
}
