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
using Presidencia.Clasificacion_Gasto.Datos;

namespace Presidencia.Clasificacion_Gasto.Negocio 
{ 
    public class Cls_Ope_Psp_Clasificacion_Gasto_Negocio
    {
        #region VARIABLES INTERNAS
            private String Fuente_Financiamiento_ID;
            private String Area_Funcional_ID;
            private String Programa_ID;
            private String Dependencia_ID;
            private String Capitulo_ID;
            private String Partida__Generica_ID;
            private String Partida_Especifica_ID;
            private String Concepto_ID;
            private String Anio;
            private String Tipo_Descripcion;
        #endregion

        #region VARIABLES PUBLICAS

            //get y set de P_Fuente_Financiamiento_ID
            public String P_Fuente_Financiamiento_ID
            {
                get { return Fuente_Financiamiento_ID; }
                set { Fuente_Financiamiento_ID = value; }
            }

            //get y set de P_Area_Funcional_ID
            public String P_Area_Funcional_ID
            {
                get { return Area_Funcional_ID; }
                set { Area_Funcional_ID = value; }
            }

            //get y set de P_Programa_ID
            public String P_Programa_ID
            {
                get { return Programa_ID; }
                set { Programa_ID = value; }
            }

            //get y set de P_Dependencia_ID
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }

            //get y set de P_Capitulo_ID
            public String P_Capitulo_ID
            {
                get { return Capitulo_ID; }
                set { Capitulo_ID = value; }
            }

            //get y set de P_Partida__Generica_ID
            public String P_Partida__Generica_ID
            {
                get { return Partida__Generica_ID; }
                set { Partida__Generica_ID = value; }
            }

            //get y set de P_Partida_Especifica_ID
            public String P_Partida_Especifica_ID
            {
                get { return Partida_Especifica_ID ; }
                set { Partida_Especifica_ID = value; }
            }

            //get y set de P_Concepto_ID
            public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }

            //get y set de P_Anio
            public String P_Anio
            {
                get { return Anio; }
                set { Anio = value; }
            }

            //get y set de P_Tipo_Descripcion
            public String P_Tipo_Descripcion
            {
                get { return Tipo_Descripcion; }
                set { Tipo_Descripcion = value; }
            }
        #endregion

        #region MÉTODOS

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Fuente_Financiamiento
            ///DESCRIPCIÓN          : Metodo para obtener los datos de las fuentes de fiananciamiento
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Fuente_Financiamiento()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Fuente_Financiamiento(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Area_Funcional
            ///DESCRIPCIÓN          : Metodo para obtener los datos de las areas funcionales
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Area_Funcional()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Area_Funcional(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Programa
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los programas
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Programa()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Programas(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Dependencias
            ///DESCRIPCIÓN          : Metodo para obtener los datos de las dependencias
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Dependencias()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Dependencias(this);
            }
            
            
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Partidas_Especificas
            ///DESCRIPCIÓN          : Metodo para obtener los datos de las partidas especificas
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Partidas_Especificas()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Partida_Especifica(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Anios
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los años presupuestados
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Anios()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Anios();
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los detalles de las clasificaciones
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 03/Diciembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Detalles()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Detalles(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Capitulos
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los capitulos
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Capitulos()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Capitulos(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Conceptos
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los conceptos
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Conceptos()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Conceptos(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Partida_Generica
            ///DESCRIPCIÓN          : Metodo para obtener los datos de las partidas genericas
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Partida_Generica()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Partida_Generica(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Partidas
            ///DESCRIPCIÓN          : Metodo para obtener los datos de las partidas especificas
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011 
            ///*********************************************************************************************************
            public DataTable Consultar_Partidas()
            {
                return Cls_Ope_Psp_Clasificacion_Gasto_Datos.Consultar_Partida(this);
            }
        #endregion
    }
}
