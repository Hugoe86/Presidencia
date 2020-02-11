using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Tipos_Construccion_Rustico.Datos;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


/// <summary>
/// Summary description for Cls_Cat_Cat_Tipos_Construccion_Rustico_Negocio
/// </summary>
/// 

namespace Presidencia.Catalogo_Cat_Tipos_Construccion_Rustico.Negocio
{
    public class Cls_Cat_Cat_Tipos_Construccion_Rustico_Negocio
    {
        #region Variables Internas
        private String Tipo_Constru_Rustico_ID;
        private String Estatus;
        private String Identificador;
        #endregion

        #region Variables Publicas

        public String P_Tipo_Constru_Rustico_ID
        {
            get { return Tipo_Constru_Rustico_ID; }
            set { Tipo_Constru_Rustico_ID = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Identificador
        {
            get { return Identificador;}
            set { Identificador = value; }
        }
        #endregion 

        
        #region metodos 
        public Boolean Alta_Tipo_Construccion_Rustico()
        {
            return Cls_Cat_Cat_Tipos_Construccion_Rustico_Datos.Alta_Tipo_Construccion_Rustico(this);
        }

        public Boolean Modificar_Tipo_Construccion_Rustico()
        {
            return Cls_Cat_Cat_Tipos_Construccion_Rustico_Datos.Modificar_Tipo_Construccion_Rustico(this);
        }

        public DataTable Consultar_Tipo_Construccion_Rustico()
        {

            return Cls_Cat_Cat_Tipos_Construccion_Rustico_Datos.Consultar_Tipos_Construccion_Rustico(this);

        }
        #endregion 


    }

    

}
