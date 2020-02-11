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
using Presidencia.Catalogo_Compras_Marcas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Marcas_Negocio
/// </summary>
namespace Presidencia.Catalogo_Compras_Marcas.Negocio
{
    public class Cls_Cat_Com_Marcas_Negocio
    {
        public Cls_Cat_Com_Marcas_Negocio()
        {
        }

        #region (Variables Locales)
        private String Marca_ID;
        private String Nombre;
        private String Estatus;
        private String Comentarios;
        private String Usuario;
        #endregion

        #region (Variables Publicas)
        public String P_Marca_ID
        {
            get { return Marca_ID; }
            set { Marca_ID = value; }
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
        /// NOMBRE DE LA CLASE:     Alta_Marcas
        /// DESCRIPCION:            Dar de Alta un nuevo Marca a la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 17:27 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Alta_Marcas()
        {
            Cls_Cat_Com_Marcas_Datos.Alta_Marcas(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Baja_Marcas
        /// DESCRIPCION:            Eliminar un Marca existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 15:27 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Baja_Marcas()
        {
            Cls_Cat_Com_Marcas_Datos.Baja_Marcas(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Cambio_Marcas
        /// DESCRIPCION:            Modificar un Marca existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 17:27 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Cambio_Marcas()
        {
            Cls_Cat_Com_Marcas_Datos.Cambio_Marcas(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Marcas
        /// DESCRIPCION:            Realizar la consulta de los Marcas por criterio de busqueda o por un ID
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 17:28 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Marcas()
        {
            return Cls_Cat_Com_Marcas_Datos.Consulta_Marcas(this);
        }
        #endregion
    }
}
