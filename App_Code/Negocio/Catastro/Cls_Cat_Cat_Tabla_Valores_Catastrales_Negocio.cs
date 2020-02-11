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
using Presidencia.Catalogo_Cat_Tabla_Valores_Catastrales.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio
/// </summary>
/// 

namespace Presidencia.Catalogo_Cat_Tabla_Valores_Catastrales.Negocio
{
    public class Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio
    {
        #region Variables Internas
        //Calidad
        private String Valor_Catastral_Id;
        private String Anio;
        private String Cantidad_1;
        private String Cantidad_2;
        private DataTable Dt_Tabla_Valores_Catastrales;

        #endregion

        #region Variables Publicas

        public String P_Valor_Catastral_Id
        {
            get { return Valor_Catastral_Id; }
            set { Valor_Catastral_Id = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Cantidad_1
        {
            get { return Cantidad_1; }
            set { Cantidad_1 = value; }
        }

        public String P_Cantidad_2
        {
            get { return Cantidad_2; }
            set { Cantidad_2 = value; }
        }

        public DataTable P_Dt_Tabla_Valores_Catastrales
        {
            get { return Dt_Tabla_Valores_Catastrales; }
            set { Dt_Tabla_Valores_Catastrales = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Valor_Catastral()
        {
            return Cls_Cat_Cat_Tabla_Valores_Catastrales_Datos.Alta_Valor_Catastral(this);
        }

        public Boolean Modificar_Valor_Catastral()
        {
            return Cls_Cat_Cat_Tabla_Valores_Catastrales_Datos.Modificar_Valor_Catastral(this);
        }

        public DataTable Consultar_Tabla_Valores_Catastrales()
        {
            return Cls_Cat_Cat_Tabla_Valores_Catastrales_Datos.Consultar_Tabla_Valores_Catastrales(this);
        }

        //public DataTable Consultar_Tipos_Construccion()
        //{
        //    return Cls_Cat_Cat_Tabla_Valores_Construccion_Datos.Consultar_Tipos_Construccion(this);
        //}

        //public DataTable Consultar_Calidad_Construccion()
        //{
        //    return Cls_Cat_Cat_Tabla_Valores_Construccion_Datos.Consultar_Calidad_Construccion(this);
        //}

        #endregion
    }
}