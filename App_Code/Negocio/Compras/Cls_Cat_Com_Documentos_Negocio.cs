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
using Presidencia.Catalogo_Compras_Documentos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Documentos_Negocio
/// </summary>
namespace Presidencia.Catalogo_Compras_Documentos.Negocio
{
    public class Cls_Cat_Com_Documentos_Negocio
    {
        public Cls_Cat_Com_Documentos_Negocio()
        {
        }

        #region (Variables Locales)
        private String Documento_ID;
        private String Nombre;
        private String Comentarios;
        private String Catalogo;
        private String Usuario;

        #endregion

        #region (Variables Publicas)
        public String P_Documento_ID
        {
            get { return Documento_ID; }
            set { Documento_ID = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Catalogo
        {
            get { return Catalogo; }
            set { Catalogo = value; }
        }
        #endregion

        #region (Metodos)
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Documentos
        /// DESCRIPCION:            Dar de Alta un nuevo Documento a la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            04/Enero/2011 16:17
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Alta_Documentos()
        {
            Cls_Cat_Com_Documentos_Datos.Alta_Documentos(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Baja_Documentos
        /// DESCRIPCION:            Eliminar un Documento existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            04/Enero/2011 16:17 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Baja_Documentos()
        {
            Cls_Cat_Com_Documentos_Datos.Baja_Documentos(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Cambio_Documentos
        /// DESCRIPCION:            Modificar un Documento existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            04/Enero/2011 16:17
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Cambio_Documentos()
        {
            Cls_Cat_Com_Documentos_Datos.Cambio_Documentos(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Documentos
        /// DESCRIPCION:            Realizar la consulta de los Documentos por criterio de busqueda o por un ID
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            04/Enero/2011 16:18
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Documentos()
        {
            return Cls_Cat_Com_Documentos_Datos.Consulta_Documentos(this);
        }
        #endregion
    }
}