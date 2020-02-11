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
using Presidencia.Areas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Areas_Negocio
/// </summary>
namespace Presidencia.Areas.Negocios
{
    public class Cls_Cat_Areas_Negocio
    {
        public Cls_Cat_Areas_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Area_ID;
        private String Dependencia_ID;
        private String Nombre;
        private String Estatus;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
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
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        #endregion
        #region (Metodos)
        public void Alta_Area()
        {
            Cls_Cat_Areas_Datos.Alta_Area(this);
        }
        public void Modificar_Area()
        {
            Cls_Cat_Areas_Datos.Modificar_Area(this);
        }
        public void Elimina_Area()
        {
            Cls_Cat_Areas_Datos.Elimina_Area(this);
        }
        public DataTable Consulta_Areas()
        {
            return Cls_Cat_Areas_Datos.Consulta_Areas(this);
        }
        public DataTable Consulta_Datos_Areas()
        {
            return Cls_Cat_Areas_Datos.Consulta_Datos_Areas(this);
        }
        #endregion
    }
}