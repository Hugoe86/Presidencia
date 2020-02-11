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
using Presidencia.Catalogo_Cat_Clasificacion_Zona.Datos;
/// <summary>
/// Summary description for Cls_Cat_Cat_Clasificacion_Zona_Negocio
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Clasificacion_Zona.Negocio
{
    public class Cls_Cat_Cat_Clasificacion_Zona_Negocio
    {
        #region Variables Internas

        private String Clasificacion_Zona_ID;
        private String Clasificacion_Zona;
        private String Estatus;

        #endregion

        #region Variables Publicas

        public String P_Clasificacion_Zona_ID
        {
            get { return Clasificacion_Zona_ID; }
            set { Clasificacion_Zona_ID = value; }
        }

        public String P_Clasificacion_Zona
        {
            get { return Clasificacion_Zona; }
            set { Clasificacion_Zona = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Clasificacion_Zona()
        {
            return Cls_Cat_Cat_Clasificacion_Zona_Datos.Alta_Clasificacion_Zona(this);
        }

        public Boolean Modificar_Clasificacion_Zona()
        {
            return Cls_Cat_Cat_Clasificacion_Zona_Datos.Modificar_Clasificacion_Zona(this);
        }

        public DataTable Consultar_Clasificacion_Zona()
        {
            return Cls_Cat_Cat_Clasificacion_Zona_Datos.Consultar_Clasificacion_Zona(this);
        }

        #endregion
    }
}