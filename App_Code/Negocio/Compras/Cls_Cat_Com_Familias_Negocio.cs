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
using Presidencia.Catalogo_Compras_Familias.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Familias_Negocio
/// </summary>
namespace Presidencia.Catalogo_Compras_Familias.Negocio
{
    public class Cls_Cat_Com_Familias_Negocio
    {
        public Cls_Cat_Com_Familias_Negocio()
        {
        }

        #region (Variables Locales)
        private String Familia_ID;
        private String Abreviatura;
        private String Estatus;
        private String Nombre;
        private String Comentarios;
        private String Usuario;
        #endregion

        #region (Variables Publicas)
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
        /// NOMBRE DE LA CLASE:     Alta_Familias
        /// DESCRIPCION:            Dar de Alta un nuevo Familia a la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            05/Enero/2011 12:30
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Alta_Familias()
        {
            Cls_Cat_Com_Familias_Datos.Alta_Familias(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Baja_Familias
        /// DESCRIPCION:            Eliminar un Familia existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            05/Enero/2011 12:30 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Baja_Familias()
        {
            Cls_Cat_Com_Familias_Datos.Baja_Familias(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Cambio_Familias
        /// DESCRIPCION:            Modificar un Familia existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            05/Enero/2011 12:30
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Cambio_Familias()
        {
            Cls_Cat_Com_Familias_Datos.Cambio_Familias(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Familias
        /// DESCRIPCION:            Realizar la consulta de los Familias por criterio de busqueda o por un ID
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            05/Enero/2011 12:30
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Familias()
        {
            return Cls_Cat_Com_Familias_Datos.Consulta_Familias(this);
        }
        #endregion
    }
}