using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Constantes;
using Presidencia.Capitulos.Datos;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Capitulos.Negocio
{
    public class Cls_Cat_SAP_Capitulos_Negocio
    {
        public Cls_Cat_SAP_Capitulos_Negocio()
        {
        }

    #region (Variables Internas)
        private String Capitulo_ID;
        private String Clave;
        private String Estatus;
        private String Descripcion;
        private String Nombre_Usuario;
#endregion
    #region (Propiedades Publicas)
        public String P_Capitulo_ID
        {
            get { return Capitulo_ID; }
            set { Capitulo_ID = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
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

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

#endregion
    #region (Metodos)
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Capitulos()
        ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para dar de alta un capitulo nuevo 
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 28/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************/
        public void Alta_Capitulos()
        {
            Cls_Cat_SAP_Capitulos_Datos.Alta_Capitulos(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Capitulos()
        ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para modificar 
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 28/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************/
        public void Modificar_Capitulos()
        {
            Cls_Cat_SAP_Capitulos_Datos.Modificar_Capitulos(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Capitulos()
        ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para consultar 
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 28/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************/
        public DataTable Consulta_Capitulos()
        {
            return Cls_Cat_SAP_Capitulos_Datos.Consulta_Capitulos(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Capitulos()
        ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para consultar los capitulos existentes
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 28/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************/
        public DataTable Consulta_Datos_Capitulos()
        {
            return Cls_Cat_SAP_Capitulos_Datos.Consulta_Datos_Capitulos(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Capitulos()
        ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para modificar el estado del combo a Inactivo 
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 28/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************/
        public Boolean Eliminar_Capitulos()
        {
            return Cls_Cat_SAP_Capitulos_Datos.Eliminar_Capitulos(this);
        }
#endregion

    }
}