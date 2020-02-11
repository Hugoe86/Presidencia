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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Montos_Proceso_Compra.Datos;

namespace Presidencia.Montos_Proceso_Compra.Negocio
{
    public class Cls_Cat_Com_Montos_Proceso_Compra_Negocio
    {
        #region VARIABLES LOCALES
            private String Parametro_ID;
            private String Tipo;
            private String Compra_Directa_Inicio;
            private String Compra_Directa_Fin;
            private String Cotizacion_Inicio;
            private String Cotizacion_Fin;
            private String Comite_Inicio;
            private String Comite_Fin;
            private String Licitacion_Restringida_Inicio;
            private String Licitacion_Restringida_Fin;
            private String Licitacion_Publica_Inicio;
            private String Licitacion_Publica_Fin;
            private String Fondo_Fijo_Inicio;
            private String Fondo_Fijo_Fin;
            private String Usuario;
        #endregion
        #region VARIABLES PUBLICAS
            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }
            public String P_Parametro_ID
            {
                get { return Parametro_ID;}
                set { Parametro_ID = value; }
            }
            public String P_Tipo
            {
                get { return Tipo;}
                set { Tipo = value; }
            }
            public String P_Compra_Directa_Inicio
            {
                get { return Compra_Directa_Inicio; }
                set { Compra_Directa_Inicio = value; }
            }
            public String P_Compra_Directa_Fin
            {
                get { return Compra_Directa_Fin; }
                set { Compra_Directa_Fin = value; }
            }
            public String P_Cotizacion_Inicio
            {
                get { return Cotizacion_Inicio; }
                set { Cotizacion_Inicio = value; }
            }
            public String P_Cotizacion_Fin
            {
                get { return Cotizacion_Fin; }
                set { Cotizacion_Fin = value; }
            }
            public String P_Comite_Inicio
            {
                get { return Comite_Inicio;}
                set { Comite_Inicio = value;}
            }
            public String P_Comite_Fin
            {
                get { return Comite_Fin; }
                set { Comite_Fin = value; }
            }
            public String P_Licitacion_Restringida_Inicio
            {
                get { return Licitacion_Restringida_Inicio;}
                set { Licitacion_Restringida_Inicio = value;}
            }
            public String P_Licitacion_Restringida_Fin
            {
                get { return Licitacion_Restringida_Fin; }
                set { Licitacion_Restringida_Fin = value; }
            }
            public String P_Licitacion_Publica_Inicio
            {
                get { return Licitacion_Publica_Inicio;}
                set { Licitacion_Publica_Inicio = value; }
            }
            public String P_Licitacion_Publica_Fin
            {
                get { return Licitacion_Publica_Fin; }
                set { Licitacion_Publica_Fin = value; }
            }
            public String P_Fondo_Fijo_Inicio
            {
                get { return Fondo_Fijo_Inicio; }
                set { Fondo_Fijo_Inicio = value; }
            }
            public String P_Fondo_Fijo_Fin
            {
                get { return Fondo_Fijo_Fin; }
                set { Fondo_Fijo_Fin = value; }
            }   
        #endregion
        #region METODOS
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Monto_Proceso_Compra
            ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para consultar 
            ///PARAMETROS:
            ///CREO: Leslie Gonzalez Vazquez
            ///FECHA_CREO: 15/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************/
            public DataTable Consultar_Monto_Proceso_Compra()
            {
                Cls_Cat_Com_Montos_Proceso_Compra_Datos Montos_Proceso_Compra_Datos = new Cls_Cat_Com_Montos_Proceso_Compra_Datos();
                return Montos_Proceso_Compra_Datos.Consultar_Montos_Proceso_Compra(this);
            }    

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_Monto_Proceso_Compra
            ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para modificar los montos del proceso de compra
            ///PARAMETROS:
            ///CREO: Leslie Gonzalez Vazquez
            ///FECHA_CREO: 15/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************/
            public Boolean Modificar_Monto_Proceso_Compra()
            {
                //Cls_Cat_Com_Montos_Proceso_Compra_Datos Montos_Proceso_Compra_Datos = new Cls_Cat_Com_Montos_Proceso_Compra_Datos();
                return Cls_Cat_Com_Montos_Proceso_Compra_Datos.Modificar_Montos_Proceso_Compra(this);
            }
        #endregion
    }
}