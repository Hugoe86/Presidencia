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
using Presidencia.Catalogo_Compras_Modelos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Modelos_Negocio
/// </summary>
namespace Presidencia.Catalogo_Compras_Modelos.Negocio
{
    public class Cls_Cat_Com_Modelos_Negocio
    {
        public Cls_Cat_Com_Modelos_Negocio()
        {
        }

        #region (Variables Locales)
        private String Modelo_ID;
        private String Subfamilia_ID;
        private String Nombre;
        private String Estatus;
        private String Comentarios;
        private String Usuario;
        #endregion

        #region (Variables Publicas)
        public String P_Modelo_ID
        {
            get { return Modelo_ID; }
            set { Modelo_ID = value; }
        }

        public String P_Subfamilia_ID
        {
            get { return Subfamilia_ID; }
            set { Subfamilia_ID = value; }
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

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region (Metodos)
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Modelos
        /// DESCRIPCION:            Dar de Alta un nuevo Modelo a la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 13:37
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Alta_Modelos()
        {
            Cls_Cat_Com_Modelos_Datos.Alta_Modelos(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Baja_Modelos
        /// DESCRIPCION:            Eliminar un Modelo existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 13:37 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Baja_Modelos()
        {
            Cls_Cat_Com_Modelos_Datos.Baja_Modelos(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Cambio_Modelos
        /// DESCRIPCION:            Modificar un Modelo existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 13:37 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Cambio_Modelos()
        {
            Cls_Cat_Com_Modelos_Datos.Cambio_Modelos(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Modelos
        /// DESCRIPCION:            Realizar la consulta de los Modelos por criterio de busqueda o por un ID
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 13:38 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Modelos()
        {
            return Cls_Cat_Com_Modelos_Datos.Consulta_Modelos(this);
        }
        #endregion
    }
}
