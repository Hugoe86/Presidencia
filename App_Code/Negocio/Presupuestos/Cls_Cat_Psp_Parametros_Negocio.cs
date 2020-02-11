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
using Presidencia.Paramentros_Presupuestos.Datos;
/// <summary>
/// Summary description for Cls_Ope_Psp_Parametros_Negocio
/// </summary>

namespace Presidencia.Paramentros_Presupuestos.Negocio { 
    public class Cls_Cat_Psp_Parametros_Negocio {
    
        #region Variables Internas

            private String Parametro_ID = null;
            private DateTime Fecha_Apertura;
            private DateTime Fecha_Cierre;
            private Int32 Anio_Presupuestar = (-1);
            private DataTable Dt_Partidas_Stock = null;
            private String Usuario = null;
            private String Capitulo_ID = null;
            private String Estatus = null;
            private String Fuente_Financiamiento_ID = null;
             
        #endregion

        #region Variables Publicas

            public String P_Parametro_ID
            {
                get { return Parametro_ID; }
                set { Parametro_ID = value; }
            }
            public DateTime P_Fecha_Apertura
            {
                get { return Fecha_Apertura; }
                set { Fecha_Apertura = value; }
            }
            public DateTime P_Fecha_Cierre
            {
                get { return Fecha_Cierre; }
                set { Fecha_Cierre = value; }
            }
            public Int32 P_Anio_Presupuestar
            {
                get { return Anio_Presupuestar; }
                set { Anio_Presupuestar = value; }
            }
            public DataTable P_Dt_Partidas_Stock
            {
                get { return Dt_Partidas_Stock; }
                set { Dt_Partidas_Stock = value; }
            }
            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }
            public String P_Capitulo_Id {
                get { return Capitulo_ID; }
                set { Capitulo_ID = value; } 
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Fuente_Financiamiento_ID
            {
                get { return Fuente_Financiamiento_ID; }
                set { Fuente_Financiamiento_ID = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Parametro() {
                Cls_Cat_Psp_Parametros_Datos.Alta_Parametro(this);
            }

            public void Modificar_Parametro() {
                Cls_Cat_Psp_Parametros_Datos.Modificar_Parametro(this);
            }

            public DataTable Consultar_Parametros() {
                return Cls_Cat_Psp_Parametros_Datos.Consultar_Parametros(this);
            }

            public Cls_Cat_Psp_Parametros_Negocio Consultar_Detalles_Parametro() {
                return Cls_Cat_Psp_Parametros_Datos.Consultar_Detalles_Parametro(this);
            }

            public void Eliminar_Parametro() {
                Cls_Cat_Psp_Parametros_Datos.Eliminar_Parametro(this);
            }

            public DataTable Consultar_Capitulos() {
               return Cls_Cat_Psp_Parametros_Datos.Consultar_Capitulos();
            }

            public DataTable Consultar_Programas()
            {
                return Cls_Cat_Psp_Parametros_Datos.Consultar_Programas();
            }

            public DataTable Consultar_Partida_Especifica()
            {
                return Cls_Cat_Psp_Parametros_Datos.Consultar_Partida_Especifica(this);
            }

            public DataTable Consultar_Fuente_Financiamiento()
            {
                return Cls_Cat_Psp_Parametros_Datos.Consultar_Fuente_Financiamiento();
            }
        #endregion

    }  
}

