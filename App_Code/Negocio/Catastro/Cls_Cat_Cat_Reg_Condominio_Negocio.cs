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
using Presidencia.Catalogo_Cat_Reg_Condominio.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Reg_Condominio_Negocios
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Reg_Condominio.Negocio
{
    public class Cls_Cat_Cat_Reg_Condominio_Negocio
    {
        #region Variables Internas
        private String Regimen_Condominio_ID;
        private String Estatus;
        private String Nombre_Documento;
        private String Tipo;
        
        #endregion Variables Internas

        #region Variables Publicas
        public String P_Regimen_Condominio_ID
        {
            get { return Regimen_Condominio_ID; }
            set { Regimen_Condominio_ID = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Nombre_Documento
        {
            get { return Nombre_Documento; }
            set { Nombre_Documento = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        #endregion Variables Publicas


        #region Metodos
        public Boolean Alta_Regimen_Condiminio()
        {
            return Cls_Cat_Cat_Reg_Condominio_Datos.Alta_Regimen_Condominio(this);
        }
        public Boolean Modificar_Regimen_Condominio()
        {
            return Cls_Cat_Cat_Reg_Condominio_Datos.Modificar_Regimen_Condiminio(this);
        }
        public DataTable Consultar_Regimen_Condominio()
        {
            return Cls_Cat_Cat_Reg_Condominio_Datos.Consultar_Regimen_Condominio(this);

        }

        #endregion Metodos
    }



}