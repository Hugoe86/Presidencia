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
using Presidencia.Autorizar_Ajuste.Datos;

/// <summary>
/// Summary description for Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio
/// </summary>
/// 
namespace Presidencia.Autorizar_Ajuste.Negocio
{
    public class Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        private String No_Ajuste;
        private String Empleado_Autorizo_ID;
        private String Fecha_Autorizo;
        private String Empleado_Rechazo_ID;
        private String Fecha_Rechazo;
        private String Motivo_Ajuste_Dir;
        private String Estatus;
        private String No_Ajuste_Busqueda;
        private String Fecha_Inicio;
        private String Fecha_Fin;

        #endregion


        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas
        public String P_No_Ajuste
        {
            get { return No_Ajuste; }
            set { No_Ajuste = value; }
        }

        public String P_Empleado_Autorizo_ID
        {
            get { return Empleado_Autorizo_ID; }
            set { Empleado_Autorizo_ID = value; }
        }

        public String P_Fecha_Autorizo
        {
            get { return Fecha_Autorizo; }
            set { Fecha_Autorizo = value; }
        }

        public String P_Empleado_Rechazo_ID
        {
            get { return Empleado_Rechazo_ID; }
            set { Empleado_Rechazo_ID = value; }
        }

        public String P_Fecha_Rechazo
        {
            get { return Fecha_Rechazo; }
            set { Fecha_Rechazo = value; }
        }
        public String P_Motivo_Ajuste_Dir
        {
            get { return Motivo_Ajuste_Dir; }
            set { Motivo_Ajuste_Dir = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_No_Ajuste_Busqueda
        {
            get { return No_Ajuste_Busqueda; }
            set { No_Ajuste_Busqueda = value; }
        }

        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }

        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }
        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consultar_Ajustes_Inventario()
        {
            return Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Datos.Consultar_Ajustes_Inventario(this);
        }

        public DataTable Consultar_Detalle_Ajustes()
        {
            return Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Datos.Consultar_Detalle_Ajustes(this);
        }

        public bool Modificar_Ajuste_Inventario()
        {
            return Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Datos.Modificar_Ajuste_Inventario(this);
        }
        #endregion

    }
}