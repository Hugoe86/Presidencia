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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;
using Presidencia.Reportes_Productos.Datos;


/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos
/// </summary>
/// 

namespace Presidencia.Reportes_Productos.Negocio
{
    public class Cls_Ope_Com_Alm_Rpts_Productos_Negocio
    {


        #region Variables Locales

        private String Partida_Especifica_ID = null;
        private String Modelo_ID = null;
        private String Marca_ID = null;
        private String Proveedor_ID = null;
        private String Letra_Inicial = null;
        private String Letra_Final = null;
        private String Nombre_Tabla = null;
        private String Tipo_Producto = null;
        #endregion

        #region Variables Publicas

        public String P_Partida_Especifica_ID
        {
            get { return Partida_Especifica_ID; }
            set { Partida_Especifica_ID = value; }
        }
        public String P_Modelo_ID
        {
            get { return Modelo_ID; }
            set { Modelo_ID = value; }
        }
        public String P_Marca_ID
        {
            get { return Marca_ID; }
            set { Marca_ID = value; }
        }
        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }

        public String P_Letra_Inicial
        {
            get { return Letra_Inicial; }
            set { Letra_Inicial = value; }
        }
        public String P_Letra_Final
        {
            get { return Letra_Final; }
            set { Letra_Final = value; }
        }
        public String P_Nombre_Tabla
        {
            get { return Nombre_Tabla; }
            set { Nombre_Tabla = value; }
        }
        public String P_Tipo_Producto
        {
            get { return Tipo_Producto; }
            set { Tipo_Producto = value; }
        }
        #endregion


        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Productos
        ///DESCRIPCIÓN:          Método que instancia al método de la clase de datos, 
        ///                      el cual consulta los productos
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           05/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public  DataTable Consultar_Productos()
        {
            return Cls_Ope_Com_Alm_Rpts_Productos_Datos.Consultar_Productos(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tablas
        ///DESCRIPCIÓN:          Método que instancia al método de la clase de datos, 
        ///                      que consulta los Proveedores, Modelos, Marcas y Partidas Especificas
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           05/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public  DataTable Consultar_Tablas()
        {
            return Cls_Ope_Com_Alm_Rpts_Productos_Datos.Consultar_Tablas(this);
        }

        #endregion
    }

}
