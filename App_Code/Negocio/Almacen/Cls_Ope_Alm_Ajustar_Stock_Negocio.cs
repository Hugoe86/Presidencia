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
using Presidencia.Ajustar_Stock.Datos;
/// <summary>
/// Summary description for Cls_Ope_Alm_Ajustar_Stock_Negocio
/// </summary>
/// 
namespace Presidencia.Ajustar_Stock.Negocio
{
    public class Cls_Ope_Alm_Ajustar_Stock_Negocio
    {
        public Cls_Ope_Alm_Ajustar_Stock_Negocio()
        {


        }
        #region VARIABLES LOCALES
        private String Producto;
        private String Existencia;
        private String Disponible;
        private String Clave;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String No_Ajuste;
        private String Motivo_Ajuste_Coordinador;
        private String Estatus;
        private DataTable Dt_Productos_Ajustados;
        #endregion

        #region VARIABLES PUBLICAS
        public DataTable P_Dt_Productos_Ajustados
        {
            get { return Dt_Productos_Ajustados; }
            set { Dt_Productos_Ajustados = value; }
        }

        public String P_Motivo_Ajuste_Coordinador
        {
            get { return Motivo_Ajuste_Coordinador; }
            set { Motivo_Ajuste_Coordinador = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_No_Ajuste
        {
            get { return No_Ajuste; }
            set { No_Ajuste = value; }
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
        public String P_Producto
        {
            get { return Producto; }
            set { Producto = value; }
        }
        public String P_Existencia
        {
            get { return Existencia; }
            set { Existencia = value; }
        }
        public String P_Disponible
        {
            get { return Disponible; }
            set { Disponible = value; }
        }
        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        #endregion

        #region
        public DataTable Consultar_Productos()
        {
            return Cls_Ope_Alm_Ajustar_Stock_Datos.Consultar_Productos(this);
        }
        public int Actualizar_Productos()
        {
            return Cls_Ope_Alm_Ajustar_Stock_Datos.Actualizar_Productos(this);
        }
        public DataTable Consultar_Ajustes_Inventario()
        {
            return Cls_Ope_Alm_Ajustar_Stock_Datos.Consultar_Ajustes_Inventario(this);
        }
        public int Guardar_Ajustes_Inventario()
        {
            return Cls_Ope_Alm_Ajustar_Stock_Datos.Guardar_Ajustes_Inventario(this);
        }
        public int Aplicar_Ajuste_Inventario()
        {
            return Cls_Ope_Alm_Ajustar_Stock_Datos.Aplicar_Ajuste_Inventario(this);
        }
        public DataTable Consultar_Productos_De_Ajuste()
        {
            return Cls_Ope_Alm_Ajustar_Stock_Datos.Consultar_Productos_De_Ajuste(this);
        }
        #endregion

    }
}
