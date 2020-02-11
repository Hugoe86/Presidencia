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
using Presidencia.Catalogo_Cat_Cuotas_Perito.Datos;
//using Presidencia.Catalogo_Cat_Factores_Cobro_Memorias_Descriptivas.Datos;
/// <summary>
/// Summary description for Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Cuotas_Perito.Negocio
{
    public class Cls_Cat_Cat_Cuotas_Perito_Negocio
    {
        #region Variables
        private string Cuota_Perito_Id;
        private string Perito_Interno_Id;
        private string Anio;
        private string Primera_Entrega;
        private string Segunda_Entrega;
        private string Tercera_Entrega;
        private string Cuarta_Entrega;
        private string Quinta_Entrega;
        private string Sexta_Entrega;
        private string Septima_Entrega;
        
        private DataTable Dt_Cuotas_Peritos;

        #endregion

        #region Variables Publicas

        public string P_Cuota_Perito_Id
        {
            get { return Cuota_Perito_Id; }
            set { Cuota_Perito_Id = value; }
        }
        public string P_Perito_Interno_Id
        {
            get { return Perito_Interno_Id; }
            set { Perito_Interno_Id = value; }
        }
        public string P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public string P_Primera_Entrega
        {
            get { return Primera_Entrega; }
            set { Primera_Entrega = value; }
        }

        public string P_Segunda_Entrega
        {
            get { return Segunda_Entrega; }
            set { Segunda_Entrega = value; }
        }
        public string P_Tercera_Entrega
        {
            get { return Tercera_Entrega; }
            set { Tercera_Entrega = value; }
        }
        public string P_Cuarta_Entrega
        {
            get { return Cuarta_Entrega; }
            set { Cuarta_Entrega = value; }
        }
        public string P_Quinta_Entrega
        {
            get { return Quinta_Entrega; }
            set { Quinta_Entrega = value; }
        }
        public string P_Sexta_Entrega
        {
            get { return Sexta_Entrega; }
            set { Sexta_Entrega = value; }
        }
        public string P_Septima_Entrega
        {
            get { return Septima_Entrega; }
            set { Septima_Entrega = value; }
        }
        public DataTable P_Dt_Coutas_Peritos
        {
            get { return Dt_Cuotas_Peritos; }
            set { Dt_Cuotas_Peritos = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Cuota_Peritos()
        {
            return Cls_Cat_Cat_Cuotas_Perito_Datos.Alta_Cuotas_Peritos(this);
        }

        public DataTable Consultar_Tabla_Cuotas_Perito()
        {
            return Cls_Cat_Cat_Cuotas_Perito_Datos.Consultar_Tabla_Cuotas_Perito(this);
        }
        public Boolean Modificar_Cuotas_Peritos()
        {
            return Cls_Cat_Cat_Cuotas_Perito_Datos.Modificar_Cuotas_Peritos(this);
        }
        #endregion
    }
}