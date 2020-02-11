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
using Presidencia.Generar_Req_Listado.Datos;

/// <summary>
/// Summary description for Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio
/// </summary>
/// 
namespace Presidencia.Generar_Req_Listado.Negocio
{
    public class Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio
    {

        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        
        //Variable que almacena el ID del listado
        private String Listado_ID;
        private String Motivo_Borrado;
        private DataTable Dt_Productos;
        private String Estatus;

       


        #endregion

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

        public String P_Listado_ID
        {
            get { return Listado_ID; }
            set { Listado_ID = value; }
        }

        public String P_Motivo_Borrado
        {
            get { return Motivo_Borrado; }
            set { Motivo_Borrado = value; }
        }

        public DataTable P_Dt_Productos
        {
            get { return Dt_Productos; }
            set { Dt_Productos = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consulta_Listado_Almacen()
        {
            return Cls_Ope_Alm_Requisicion_Listado_Stock_Datos.Consulta_Listado_Almacen(this);
        }

        public DataTable Consulta_Listado_Detalle()
        {
            return Cls_Ope_Alm_Requisicion_Listado_Stock_Datos.Consulta_Listado_Detalle(this);
        }

        public bool Borrar_Productos_Listado()
        {
            return Cls_Ope_Alm_Requisicion_Listado_Stock_Datos.Borrar_Productos_Listado(this);

        }

        public String Convertir_Requisicion_Transitoria()
        {
            return Cls_Ope_Alm_Requisicion_Listado_Stock_Datos.Convertir_Requisicion_Transitoria(this);
        }

        public DataTable Consultar_Requisiciones_Listado()
        {
            return Cls_Ope_Alm_Requisicion_Listado_Stock_Datos.Consultar_Requisiciones_Listado(this);
        }

        public bool Modificar_Listado()
        {
            return Cls_Ope_Alm_Requisicion_Listado_Stock_Datos.Modificar_Listado(this);
        }
        #endregion


    }//fin del class
}//fin del namespace