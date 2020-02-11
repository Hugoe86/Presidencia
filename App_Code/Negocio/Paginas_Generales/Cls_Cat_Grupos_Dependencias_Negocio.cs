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
using Presidencia.Grupos_Dependencias.Datos;

/// <summary>
/// Summary description for Cls_Cat_Grupos_Dependencias_Negocio
/// </summary>
/// 
namespace Presidencia.Grupos_Dependencias.Negocio
{
    public class Cls_Cat_Grupos_Dependencias_Negocio
    {
        ///*******************************************************************************
        ///VARIABLES INTERNAS
        ///*******************************************************************************
        #region Variables_Internas 
        private String Grupo_Dependencia_ID;
        private String Nombre;
        private String Clave;
        private String Comentarios;
        private String Estatus;

        #endregion 
        ///*******************************************************************************
        ///VARIABLES EXTERNAS
        ///*******************************************************************************
        #region Variables_Externas

        public String P_Grupo_Dependencia_ID
        {
            get { return Grupo_Dependencia_ID; }
            set { Grupo_Dependencia_ID = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        #endregion

        ///*******************************************************************************
        ///METODOS
        ///*******************************************************************************
        #region Metodos 

        public DataTable Consultar_Grupos_Dependencias()
        {
            return Cls_Cat_Grupos_Dependencias_Datos.Consultar_Grupos_Dependencias(this);
        }

        public String Alta_Grupo_Dependencia()
        {
            return Cls_Cat_Grupos_Dependencias_Datos.Alta_Grupo_Dependencia(this);
        }

        public String Modificar_Grupo_Dependencia()
        {
            return Cls_Cat_Grupos_Dependencias_Datos.Modificar_Grupo_Dependencia(this);
        }

        public String Eliminar_Grupo_Dependencia()
        {
            return Cls_Cat_Grupos_Dependencias_Datos.Eliminar_Grupo_Dependencia(this);
        }

        #endregion

    }
}