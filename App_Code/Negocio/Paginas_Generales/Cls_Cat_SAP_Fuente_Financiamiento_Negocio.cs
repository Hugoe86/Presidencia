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
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Datos;

namespace Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio
{
    public class Cls_Cat_SAP_Fuente_Financiamiento_Negocio
    {
        //Propiedades
        public Cls_Cat_SAP_Fuente_Financiamiento_Negocio()
        {
        }

        /// --------------------------------------- Variables Internas ---------------------------------------
#region (Variables Internas)

        private String Fuente_Financiamiento_ID;
        private String Clave;
        private String Estatus;
        private String Descripcion;
        private String Nombre_Usuario;
        private String Dependencia_ID;
        private String Anio;
        private String Especiales_Ramo_33;
#endregion


        /// --------------------------------------- Propiedades públicas ---------------------------------------
 #region (Propiedades Publicas)
        public String P_Fuente_Financiamiento_ID
        {
            get { return Fuente_Financiamiento_ID; }
            set { Fuente_Financiamiento_ID = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public String P_Especiales_Ramo_33
        {
            get { return Especiales_Ramo_33; }
            set { Especiales_Ramo_33 = value; }
        }

#endregion

        /// --------------------------------------- Metodos ---------------------------------------
#region (Metodos)
        public void Alta_Fuente_Financiamiento()
        {
            Cls_Cat_SAP_Fuente_Financiamiento_Datos.Alta_Fuente_Financiamiento(this);
        }
        public void Modificar_Fuente_Financiamiento()
        {
            Cls_Cat_SAP_Fuente_Financiamiento_Datos.Modificar_Fuente_Financiamiento(this);
        }
        public DataTable Consulta_Fuente_Financiamiento()
        {
            return Cls_Cat_SAP_Fuente_Financiamiento_Datos.Consulta_Fuente_Financiamiento(this);
        }
        public DataTable Consulta_Datos_Fuente_Financiamiento()
        {
            return Cls_Cat_SAP_Fuente_Financiamiento_Datos.Consulta_Datos_Fuente_Financiamiento(this);
        }
        public DataTable Consulta_Fuente_Financiamiento_Especial()
        {
            return Cls_Cat_SAP_Fuente_Financiamiento_Datos.Consulta_Fuente_Financiamiento_Especial(this);
        }
#endregion

    }
}