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
using Presidencia.Reportes_Contrarecibos.Datos;


/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos
/// </summary>
/// 

namespace Presidencia.Reportes_Contrarecibos.Negocio
{
    public class Cls_Ope_Com_Alm_Rpts_Contrarecibos_Negocio
    {


        #region Variables Locales

        private String No_ContraRecibo = null;
        private String Proveedor_ID = null;
        private String Usuario_Genero = null;
        private String Fecha_Inicial = null;
        private String Fecha_Final = null;
        private String Nombre_Tabla = null;

        #endregion

        #region Variables Publicas

        public String P_No_ContraRecibo
        {
            get { return No_ContraRecibo; }
            set { No_ContraRecibo = value; }
        }
        public String P_Usuario_Genero
        {
            get { return Usuario_Genero; }
            set { Usuario_Genero = value; }
        }
        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }
        public String P_Nombre_Tabla
        {
            get { return Nombre_Tabla; }
            set { Nombre_Tabla = value; }
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
        public  DataTable Consultar_Contra_Recibos()
        {
            return Cls_Ope_Com_Alm_Rpts_Contrarecibos_Datos.Consultar_Contrarecibos(this);
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
            return Cls_Ope_Com_Alm_Rpts_Contrarecibos_Datos.Consultar_Tablas(this);
        }

        #endregion
    }

}
