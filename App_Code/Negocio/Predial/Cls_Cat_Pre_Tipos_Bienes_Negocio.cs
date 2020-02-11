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
using Presidencia.Catalogo_Tipos_Bienes.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Tipos_Bienes_Negocio
/// </summary>

namespace Presidencia.Catalogo_Tipos_Bienes.Negocio
{
    public class Cls_Cat_Pre_Tipos_Bienes_Negocio
    {

        #region Variables Internas

        private String Tipo_Bien_ID;
        private String Nombre;
        private String Estatus;
        private String Descripcion;
        private String filtro;

        #endregion

        #region Variables Publicas

        public String P_Tipo_Bien_ID
        {
            get { return Tipo_Bien_ID; }
            set { Tipo_Bien_ID = value; }
        }

        public String P_Filtro
        {
            get { return filtro; }
            set { filtro = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
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

        #endregion

        #region Metodos

        public void Alta_Bien()
        {
            Cls_Cat_Pre_Tipos_Bienes_Datos.Alta_Bien(this);
        }

        public void Modificar_Bien()
        {
            Cls_Cat_Pre_Tipos_Bienes_Datos.Modificar_Bien(this);
        }

        public Cls_Cat_Pre_Tipos_Bienes_Negocio Consultar_Datos_Bien()
        {
            return Cls_Cat_Pre_Tipos_Bienes_Datos.Consultar_Datos_Bienes(this);
        }

        public DataTable Consultar_Bien()
        {
            return Cls_Cat_Pre_Tipos_Bienes_Datos.Consultar_Bienes(this);
        }

        #endregion

    }
}