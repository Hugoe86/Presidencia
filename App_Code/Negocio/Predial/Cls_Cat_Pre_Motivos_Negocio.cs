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
using Presidencia.Catalogo_Motivos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Motivos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Motivos.Negocio
{
    public class Cls_Cat_Pre_Motivos_Negocio
    {

        #region Variables Internas

        private String Motivo_ID;
        private String Estatus;
        private String Nombre;
        private String filtro;

        #endregion 

        #region Variables Publicas

        public String P_Motivo_ID
        {
            get { return Motivo_ID; }
            set { Motivo_ID = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Filtro
        {
            get { return filtro; }
            set { filtro = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Motivo()
        {
            Cls_Cat_Pre_Motivos_Datos.Alta_Motivos(this);
        }

        public void Modificar_Motivo()
        {
            Cls_Cat_Pre_Motivos_Datos.Modificar_Motivo(this);
        }

        public void Eliminar_Motivo()
        {
            Cls_Cat_Pre_Motivos_Datos.Eliminar_Motivo(this);
        }

        public Cls_Cat_Pre_Motivos_Negocio Consultar_Datos_Motivo()
        {
            return Cls_Cat_Pre_Motivos_Datos.Consultar_Datos_Motivo(this);
        }

        public DataTable Consultar_Motivo()
        {
            return Cls_Cat_Pre_Motivos_Datos.Consultar_Motivos(this);
        }

        public DataTable Consultar_Motivo_Nombre_Id()
        {
            return Cls_Cat_Pre_Motivos_Datos.Consultar_Motivos_Nombre_Id();
        }

        #endregion

    }
}