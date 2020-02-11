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
using Presidencia.Catalogo_Cat_Tabla_Descrip_Rustico.Datos;
using Presidencia.Catalogo_Cat_Descripcion_Construccion_Rustico.Datos;


/// <summary>
/// Summary description for Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Tabla_Descrip_Rustico.Negocio
{
    public class Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio
    {
        #region Variables Internas

        private String Descripcion_Rustico_Id;
        private String Anio;
        private String Valor_Indice;
        private String Indicador_A;
        private String Indicador_B;
        private String Indicador_C;
        private String Indicador_D;
        private DataTable Dt_Tabla_Descrip_Rustico;
        //detalle
        private String Desc_Constru_Rustico_Id;
        private String Identificador;
        private String Estatus;
        private DataTable Dt_Tabla_Des_Constru_Rustico;

        #endregion

        #region Variables Publicas



        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Valor_Indice
        {
            get { return Valor_Indice; }
            set { Valor_Indice = value; }
        }

        public String P_Descripcion_Rustico_Id
        {
            get { return Descripcion_Rustico_Id; }
            set { Descripcion_Rustico_Id = value; }
        }
        public String P_Indicador_A
        {
            get { return Indicador_A; }
            set { Indicador_A = value; }
        }
        public String P_Indicador_B
        {
            get { return Indicador_B; }
            set { Indicador_B = value; }
        }
        public String P_Indicador_C
        {
            get { return Indicador_C; }
            set { Indicador_C = value; }
        }
        public String P_Indicador_D
        {
            get { return Indicador_D; }
            set { Indicador_D = value; }
        }

        public String P_Des_Constru_Rustico_Id
        {
            get { return Desc_Constru_Rustico_Id; }
            set { Desc_Constru_Rustico_Id = value; }
        }

        public String P_Identificador
        {
            get { return Identificador; }
            set { Identificador = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public DataTable P_Dt_Tabla_Descrip_Rustico
        {
            get { return Dt_Tabla_Descrip_Rustico; }
            set { Dt_Tabla_Descrip_Rustico = value; }
        }


        public DataTable P_Dt_Tabla_Des_Constru_Rustico
        {
            get { return Dt_Tabla_Des_Constru_Rustico; }
            set { Dt_Tabla_Des_Constru_Rustico = value; }
        }


        #endregion

        #region Metodos

        public Boolean Alta_Valor_Rustico()
        {
            return Cls_Cat_Cat_Tabla_Descrip_Rustico_Datos.Alta_Valor_Rustico(this);
        }

        public Boolean Modificar_Valor_Rustico()
        {
            return Cls_Cat_Cat_Tabla_Descrip_Rustico_Datos.Modificar_Valor_Rustico(this);
        }

        public DataTable Consultar_Tabla_Valores_Rustico()
        {
            return Cls_Cat_Cat_Tabla_Descrip_Rustico_Datos.Consultar_Tabla_Valores_Rustico(this);
        }



        #endregion
    }
}