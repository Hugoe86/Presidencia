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
using Presidencia.Distribuir_a_Proveedores.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio
/// </summary>
/// 
namespace Presidencia.Distribuir_a_Proveedores.Negocio
{
    public class Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        private String No_Requisicion;
        private DataTable Dt_Proveedores;
        private String Tipo_Articulo;
        private String Concepto_ID;
        private String Proveedor_ID;
        private String Cotizador_ID;



        #endregion

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas
        public String P_Cotizador_ID
        {
            get { return Cotizador_ID; }
            set { Cotizador_ID = value; }
        }
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }

        public DataTable P_Dt_Proveedores
        {
            get { return Dt_Proveedores; }
            set { Dt_Proveedores = value; }
        }

        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
        }

        public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }

        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consultar_Productos_Servicios()
        {
            return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Productos_Servicios(this);
        }

        public DataTable Consultar_Requisiciones()
        {
            return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Requisiciones(this);
        }

        public DataTable Consultar_Detalle_Requisicion()
        {
            return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Detalle_Requisicion(this);
        }

        public DataTable Consultar_Proveedores_Asignados()
        {
            return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Proveedores_Asignados(this);
        }

        public DataTable Consultar_Proveedores()
        {
            return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Proveedores(this);
        }
        public bool Alta_Proveedores_Asignados()
        {
            return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Alta_Proveedores_Asignados(this);
        }

        public void Eliminar_Proveedores()
        {
            Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Eliminar_Proveedores(this);
        }

        public DataTable Consulta_Parametros()
        {
            return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consulta_Parametros();
        }

        public DataTable Consultar_Email_Proveedores()
        {
            return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Email_Proveedores(this);
        }
         public DataTable Consultar_Datos_Cotizador()
        {
            return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Datos_Cotizador(this);
        }
         public DataTable Consultar_Comentarios()
         {
             return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Comentarios(this);
         }
         public int Marcar_Leida_Por_Cotizador()
         {
             return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Marcar_Leida_Por_Cotizador(this);
         }

         public DataTable Consultar_Req_Origen()
         {
             return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Req_Origen(this);
         }
         public DataTable Consultar_Parametro_Invitacion()
         {
             return Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos.Consultar_Parametro_Invitacion(this);
         }
        #endregion
    }
}//fin del namespace