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
using Presidencia.Operacion_Cat_Digitalizacion_Avaluos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio
/// </summary>
/// 

namespace Presidencia.Operacion_Cat_Digitalizacion_Avaluos.Negocio
{
    public class Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio
    {
        #region Variables


        private String Cuenta_Predial_Id;
        private String Cuenta_Predial;

        private String Digitalizacion_Avaluo_Id;
        private String Ruta_Documento;
        private DataTable Dt_Archivos;


        #endregion

        #region Variables_Publicas

        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Digitalizacion_Avaluo_Id
        {
            get { return Digitalizacion_Avaluo_Id; }
            set { Digitalizacion_Avaluo_Id = value; }
        }
      
        public String P_Ruta_Documento
        {
            get { return Ruta_Documento; }
            set { Ruta_Documento = value; }
        }
        public DataTable P_Dt_Archivos
        {
            get { return Dt_Archivos; }
            set { Dt_Archivos = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Documentos_Cuenta_Predial()
        {
            return Cls_Ope_Cat_Digitalizacion_Avaluos_Datos.Alta_Documentos_Cuenta_Predial(this);
        }

        public DataTable Consultar_Documentos_Cuenta_Predial()
        {
            return Cls_Ope_Cat_Digitalizacion_Avaluos_Datos.Consultar_Documentos_Cuenta_Predial(this);
        }



        #endregion
    }
}