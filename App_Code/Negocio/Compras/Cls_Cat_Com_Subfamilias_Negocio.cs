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
using Presidencia.Catalogo_Compras_Subfamilias.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Subfamilias_Negocio
/// </summary>
namespace Presidencia.Catalogo_Compras_Subfamilias.Negocio
{
    public class Cls_Cat_Com_Subfamilias_Negocio
    {
        public Cls_Cat_Com_Subfamilias_Negocio()
        {
        }

        #region (Variables Locales)
        private String Subfamilia_ID;
        private String Familia_ID;
        private String Abreviatura;
        private String Estatus;
        private String Nombre;
        private String Comentarios;
        private String Usuario;
        #endregion

        #region (Variables Publicas)
        public String P_Subfamilia_ID
        {
            get { return Subfamilia_ID; }
            set { Subfamilia_ID = value; }
        }

        public String P_Familia_ID
        {
            get { return Familia_ID; }
            set { Familia_ID = value; }
        }

        public String P_Abreviatura
        {
            get { return Abreviatura; }
            set { Abreviatura = value; }
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
        #endregion

        #region (Metodos)
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Subfamilias
        /// DESCRIPCION:            Dar de Alta un nuevo Subfamilia a la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            05/Enero/2011 17:44
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Alta_Subfamilias()
        {
            Cls_Cat_Com_Subfamilias_Datos.Alta_Subfamilias(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Baja_Subfamilias
        /// DESCRIPCION:            Eliminar un Subfamilia existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            05/Enero/2011 17:44 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Baja_Subfamilias()
        {
            Cls_Cat_Com_Subfamilias_Datos.Baja_Subfamilias(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Cambio_Subfamilias
        /// DESCRIPCION:            Modificar un Subfamilia existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            05/Enero/2011 17:44
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Cambio_Subfamilias()
        {
            Cls_Cat_Com_Subfamilias_Datos.Cambio_Subfamilias(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Subfamilias
        /// DESCRIPCION:            Realizar la consulta de los Subfamilias por criterio de busqueda o por un ID
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            05/Enero/2011 17:44
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Subfamilias()
        {
            return Cls_Cat_Com_Subfamilias_Datos.Consulta_Subfamilias(this);
        }
        #endregion
    }
}