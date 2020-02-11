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
using Presidencia.Catalogo_Cat_Factores_Cobro_Memorias_Descriptivas.Datos;
/// <summary>
/// Summary description for Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Factores_Cobro_Memorias_Descriptivas.Negocio
{
    public class Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio
    {
        #region Variables
        private string Factor_Cobro_Id;
        private string Anio;
        private string Cantidad_Cobro1;
        private string Cantidad_Cobro2;
        private DataTable Dt_Valores_Cobro;
               
        #endregion 

        #region Variables Publicas

        public string P_Factor_Cobro_Id
        {
            get { return Factor_Cobro_Id; }
            set { Factor_Cobro_Id = value; }
        }
        
        public string P_Anio
        { 
            get{ return Anio; }
            set{ Anio=value; }
        }

        public string P_Cantidad_Cobro1
        {
            get { return Cantidad_Cobro1; }
            set { Cantidad_Cobro1 = value; }
        }

        public string P_Cantidad_Cobro2
        {
            get { return Cantidad_Cobro2; }
            set { Cantidad_Cobro2 = value; }
        }

        public DataTable P_Dt_Valores_Cobro
        {
            get { return Dt_Valores_Cobro; }
            set { Dt_Valores_Cobro = value; }
        }  
 
        #endregion

        #region Metodos

        public Boolean Alta_Factores_Cobro_Memorias_Descriptivas()
        {
            return Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Datos.Alta_Factores_Cobro_Memorias_Descriptivas(this);
        }

        public DataTable Consulta_Factores_Cobro_Memorias_Descriptivas()
        {
            return Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Datos.Consulta_Factores_Cobro_Memorias_Descriptivas(this);
        }
        #endregion
    }
}