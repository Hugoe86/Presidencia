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
using Presidencia.Catalogo_Predial_Tipos_Documento.Datos;

namespace Presidencia.Catalogo_Predial_Tipos_Documento.Negocio
{
    public class Cls_Cat_Pre_Tipos_Documento_Negocio
    {
        public Cls_Cat_Pre_Tipos_Documento_Negocio()
        {
        }

/// --------------------------------------- PROPIEDADES ---------------------------------------
#region PROPIEDADES

        private String Documento_ID;
        private String Nombre_Documento;
        private String Descripcion;
        private String Estatus;
        private String Nombre_Usuario;

#endregion


/// --------------------------------------- Propiedades públicas ---------------------------------------
#region (Propiedades Publicas)
        public String P_Documento_ID
        {
            get { return Documento_ID; }
            set { Documento_ID = value; }
        }

        public String P_Nombre_Documento
        {
            get { return Nombre_Documento; }
            set { Nombre_Documento = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

#endregion


/// --------------------------------------- Metodos ---------------------------------------
#region (Metodos)
        public int Alta_Tipo_Documento()
        {
            return Cls_Cat_Pre_Tipos_Documento_Datos.Alta_Tipo_Documento(this);
        }
        public int Modificar_Tipo_Documento()
        {
            return Cls_Cat_Pre_Tipos_Documento_Datos.Modificar_Tipo_Documento(this);
        }
        public DataTable Consulta_Datos_Tipos_Documento()
        {
            return Cls_Cat_Pre_Tipos_Documento_Datos.Consulta_Datos_Tipos_Documento(this);
        }
        public DataTable Consulta_Tipos_Documento()
        {
            return Cls_Cat_Pre_Tipos_Documento_Datos.Consulta_Tipos_Documento(this);
        }

#endregion (Metodos)

    }
}