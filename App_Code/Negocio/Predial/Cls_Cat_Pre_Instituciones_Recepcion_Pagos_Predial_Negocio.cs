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
using System.Collections.Generic;
using Presidencia.Catalogo_Instituciones_Recepcion_Pago_Predial.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio
/// </summary>

namespace Presidencia.Catalogo_Instituciones_Recepcion_Pago_Predial.Negocio
{
    public class Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio
    {
        #region Variables Internas

        private String Institucion_Id;
        private String Institucion;
        private String Estatus;
        private String Caja_Id;
        private String Mes;
        private String Texto;
        private String Campos;
        private String Filtro;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private String Linea_Captura_Enero;
        private String Linea_Captura_Febrero;
        private String Convenio;

        #endregion

        #region Variables Publicas

        public String P_Institucion_Id
        {
            get { return Institucion_Id; }
            set { Institucion_Id = value; }
        }

        public String P_Institucion
        {
            get { return Institucion; }
            set { Institucion = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Caja_Id
        {
            get { return Caja_Id; }
            set { Caja_Id = value; }
        }

        public String P_Mes
        {
            get { return Mes; }
            set { Mes = value; }
        }

        public String P_Texto
        {
            get { return Texto; }
            set { Texto = value; }
        }

        public String P_Campos
        {
            get { return Campos; }
            set { Campos = value; }
        }

        public String P_Filtro
        {
            get { return Filtro; }
            set { Filtro = value; }
        }

        public String P_Agrupar_Dinamico
        {
            get { return Agrupar_Dinamico; }
            set { Agrupar_Dinamico = value; }
        }

        public String P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value; }
        }

        public String P_Linea_Captura_Enero
        {
            get { return Linea_Captura_Enero; }
            set { Linea_Captura_Enero = value; }
        }

        public String P_Linea_Captura_Febrero
        {
            get { return Linea_Captura_Febrero; }
            set { Linea_Captura_Febrero = value; }
        }

        public String P_Convenio
        {
            get { return Convenio; }
            set { Convenio = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Institucion()
        {
            Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos.Alta_Instituciones(this);
        }

        public void Modificar_Institucion()
        {
            Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos.Modificar_Institucion(this);
        }

        public void Eliminar_Institucion()
        {
            Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos.Eliminar_Institucion(this);
        }

        public Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Consultar_Datos_Institucion()
        {
            return Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos.Consultar_Datos_Instituciones(this);
        }

        public DataTable Consultar_Institucion()
        {
            return Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos.Consultar_Instituciones(this);
        }

        public DataTable Consultar_Instituciones_Reciben_Pago()
        {
            return Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos.Consultar_Instituciones_Reciben_Pago(this);
        }

        public DataTable Consultar_Cajas_Nombre_Id()
        {
            return Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos.Consultar_Cajas_Nombre_Id();
        }

        public DataTable Consultar_No_Repetir_Caja()
        {
            return Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos.Consultar_No_Repetir_Caja(this);
        }

        #endregion
    }
}