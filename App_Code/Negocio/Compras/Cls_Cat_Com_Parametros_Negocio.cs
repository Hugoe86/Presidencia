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
using Presidencia.Catalogo_Compras_Parametros.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Parametros_Negocio
/// </summary>
namespace Presidencia.Catalogo_Compras_Parametros.Negocio
{
    public class Cls_Cat_Com_Parametros_Negocio
    {
        public Cls_Cat_Com_Parametros_Negocio()
        {
        }

        #region (Variables Locales)
        private String Parametro_ID;
        private String Salario_Minimo_Resguardado;
        private String Plazo_Surtir_Orden_Compra;
        private String Usuario;
        private String Partida_Generica_ID;
        #endregion

        #region (Variables Publicas)
        public String P_Parametro_ID
        {
            get { return Parametro_ID; }
            set { Parametro_ID = value; }
        }

        public String P_Salario_Minimo_Resguardado
        {
            get { return Salario_Minimo_Resguardado; }
            set { Salario_Minimo_Resguardado = value; }
        }

        public String P_Plazo_Surtir_Orden_Compra
        {
            get { return Plazo_Surtir_Orden_Compra; }
            set { Plazo_Surtir_Orden_Compra = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Partida_Generica_ID
        {
            get { return Partida_Generica_ID; }
            set { Partida_Generica_ID = value; }
        }
        #endregion

        #region (Metodos)
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Parametros
        /// DESCRIPCION:            Dar de Alta un nuevo Parametro a la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 12:41 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Alta_Parametros()
        {
            Cls_Cat_Com_Parametros_Datos.Alta_Parametros(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Cambio_Parametros
        /// DESCRIPCION:            Modificar un Parametro existente de la base de datos
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 12:42 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Cambio_Parametros()
        {
            Cls_Cat_Com_Parametros_Datos.Cambio_Parametros(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Parametros
        /// DESCRIPCION:            Realizar la consulta de los Parametros por criterio de busqueda o por un ID
        /// PARAMETROS :            
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 12:42 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Parametros()
        {
            return Cls_Cat_Com_Parametros_Datos.Consulta_Parametros(this);
        }
        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Generico
        ///DESCRIPCIÓN: Consulta las partidas Genericas
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 12/marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Generico()
        {
            return Cls_Cat_Com_Parametros_Datos.Consultar_Generico(this);
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Especificas
        ///DESCRIPCIÓN: Consulta las partidas Especificas
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 12/marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Especificas()
        {
            return Cls_Cat_Com_Parametros_Datos.Consultar_Especificas(this);
        }
        #endregion
    }
}