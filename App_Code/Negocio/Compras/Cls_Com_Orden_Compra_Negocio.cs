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
using Presidencia.Rpt_Orden_Compra.Datos;


/// <summary>
/// Summary description for Cls_Com_Orden_Compra_Negocio
/// </summary>
namespace Presidencia.Rpt_Orden_Compra.Negocio
{

    public class Cls_Com_Orden_Compra_Negocio
    {
        ///*******************************************************************
        ///VARIABLES INTERNAS
        ///*******************************************************************
        #region Variables Internas
        private String Tipo;
        private String Estatus;
        private String Fecha_Inicial;
        private String Fecha_Final;
        #endregion

        ///*******************************************************************
        ///VARIABLES PUBLICAS
        ///*******************************************************************
        #region Variables Publicas
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

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

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        #endregion

        ///*******************************************************************
        ///VARIABLES METODOS
        ///*******************************************************************
        #region Metodos 

        public DataSet Consultar_Ordenes_Compra()
        {
            return Cls_Com_Orden_Compra_Datos.Consultar_Ordenes_Compra(this);
            
        }

        #endregion


    }

}