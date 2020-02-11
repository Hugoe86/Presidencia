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
using Presidencia.Catalogo_Cat_Tabla_Factores.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tabla_Factores_Negocio
/// </summary>
/// 

namespace Presidencia.Catalogo_Cat_Tabla_Factores.Negocio
{
    public class Cls_Cat_Cat_Tabla_Factores_Negocio
    {
        #region Variables Internas
        //cliente
        private string Factor_Cobro_Id;
        private string Anio;
        private string Factor_Cobro_2;
        private string Base_Cobro;
        private string Factor_Menor_1_Ha;
        private string Factor_Mayor_1_Ha;
        private string Porcentaje_Perito_Externo;
        private DataTable Dt_Tabla_Factores;

        #endregion

        #region Variables Publicas

        public String P_Factor_Cobro_Id
        {
            get { return Factor_Cobro_Id; }
            set { Factor_Cobro_Id = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public String P_Factor_Cobro_2
        {
            get { return Factor_Cobro_2; }
            set { Factor_Cobro_2 = value; }
        }
        public String P_Base_Cobro
        {
            get { return Base_Cobro; }
            set { Base_Cobro = value; }
        }
        public String P_Factor_Menor_1_Ha
        {
            get { return Factor_Menor_1_Ha; }
            set { Factor_Menor_1_Ha = value; }
        }
        public String P_Factor_Mayor_1_Ha
        {
            get { return Factor_Mayor_1_Ha; }
            set { Factor_Mayor_1_Ha = value; }
        }
        public String P_Porcentaje_Perito_Externo
        {
            get { return Porcentaje_Perito_Externo; }
            set { Porcentaje_Perito_Externo = value; }
        }
        public DataTable P_Dt_Tabla_Factores
        {
            get { return Dt_Tabla_Factores; }
            set { Dt_Tabla_Factores = value; }
        }
        #endregion

        #region Metodos

        public Boolean Alta_Tabla_Factores_Cobro_Avaluos()
        {
            return Cls_Cat_Cat_Tabla_Factores_Datos.Alta_Tabla_Factores_Cobro_Avaluos(this);
        }


        public DataTable Consultar_Tabla_Factores_Cobro_Avaluos()
        {
            return Cls_Cat_Cat_Tabla_Factores_Datos.Consultar_Tabla_Factores_Cobro_Avaluos(this);
        }

        #endregion
    }
}