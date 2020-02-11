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
using Presidencia.Kardex_Productos.Datos;
/// <summary>
/// Summary description for Cls_Ope_Alm_Kardex_Productos_Negocio
/// </summary>
/// 

namespace Presidencia.Kardex_Productos.Negocio
{
    public class Cls_Ope_Alm_Kardex_Productos_Negocio
    {
        public Cls_Ope_Alm_Kardex_Productos_Negocio()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        #region (Variables Locales)
        private String Fecha_Inicio_B;
        private String Fecha_Fin_B;
        private String Clave_B;

        #endregion


        #region (Variables Publicas)
        public String P_Fecha_Inicio_B
        {
            get { return Fecha_Inicio_B; }
            set { Fecha_Inicio_B = value; }
        }

        public String P_Fecha_Fin_B
        {
            get { return Fecha_Fin_B; }
            set { Fecha_Fin_B = value; }
        }

        public String P_Clave_B
        {
            get { return Clave_B; }
            set { Clave_B = value; }
        }

        #endregion


        #region (Metodos)

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Entradas_Productos
        /// DESCRIPCION:            Consultar las entradas de los productos seleccionados por el usuario
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            13/Agosto/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consultar_Entradas_Productos()
        {
            return Cls_Ope_Alm_Kardex_Productos_Datos.Consultar_Entradas_Productos(this);
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Salidas_Productos
        /// DESCRIPCION:            Consultar las salidas de los productos seleccionados por el usuario
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            13/Agosto/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consultar_Salidas_Productos()
        {
            return Cls_Ope_Alm_Kardex_Productos_Datos.Consultar_Salidas_Productos(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Datos_Producto
        /// DESCRIPCION:            Consultar los datos generales del producto seleccionado por el usuario
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            13/Agosto/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consultar_Datos_Producto()
        {
            return Cls_Ope_Alm_Kardex_Productos_Datos.Consultar_Datos_Producto(this);
        }
        #endregion
    }
}