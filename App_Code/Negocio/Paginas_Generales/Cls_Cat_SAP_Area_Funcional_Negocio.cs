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
using Presidencia.Area_Funcional.Datos;

namespace Presidencia.Area_Funcional.Negocio
{
    public class Cls_Cat_SAP_Area_Funcional_Negocio
    {
        #region VARIABLES LOCALES
            private String Area_Funcional_ID;
            private String Clave;
            private String Descripcion;
            private String Estatus;
            private String Usuario_Creo;
            private String Usuario_Modifico;
            private String Anio;
        #endregion
        #region VARIABLES GLOBALES
            public String P_Area_Funcional_ID
            {
                get { return Area_Funcional_ID; }
                set { Area_Funcional_ID = value; }
            }
            public String P_Clave
            {
                get { return Clave; }
                set { Clave = value; }
            }
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public String P_Usuario_Modifico
            {
                get { return Usuario_Modifico; }
                set { Usuario_Modifico = value; }
            }
            public String P_Anio
            {
                get { return Anio; }
                set { Anio = value; }
            }
        #endregion
        #region METODOS
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Insertar_Area_Funcional
            ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
            ///de datos enviando 
            ///un objeto de esta clase para sacar sus valores
            ///PARAMETROS: 
            ///CREO: Leslie González Vázquez
            ///FECHA_CREO: 25/febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public Boolean Alta_Area_Funcional()
            {
                return Cls_Cat_SAP_Area_Funcional_Datos.Alta_Area_Funcional(this);
            }
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Area_Funcional
            ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base de datos
            ///PARAMETROS:
            ///CREO: Leslie González Vázquez
            ///FECHA_CREO: 25/febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public DataSet Consultar_Area_Funcional()
            {
                return Cls_Cat_SAP_Area_Funcional_Datos.Consulta_Area_Funcional(this);
            }
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Actualizar_Area_Funcional
            ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
            ///de datos enviando 
            ///un objeto de esta clase para sacar sus valores
            ///PARAMETROS: Clave-para tener la referencia de la clave antes hacer los cambios el usuario 
            ///CREO: Leslie González Vázquez
            ///FECHA_CREO: 25/febrero/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public Boolean Modificar_Area_Funcional(String Clave)
            {
                String Clave_Actual = Clave;
                return Cls_Cat_SAP_Area_Funcional_Datos.Modificar_Area_Funcional(this, Clave_Actual);
            }

            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Eliminar_Area_Funcional
            ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
            ///de datos enviando 
            ///un objeto de esta clase para sacar sus valores
            ///PARAMETROS:
            ///CREO: Leslie González Vázquez
            ///FECHA_CREO: 25/febrero/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public Boolean Eliminar_Area_Funcional()
            {
                return Cls_Cat_SAP_Area_Funcional_Datos.Eliminar_Area_Funcional(this);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Generar_ID
            ///DESCRIPCIÓN: Metodo que regresa un ID
            ///PARAMETROS:   
            ///CREO: Leslie González Vázquez
            /// FECHA_CREO:  28/febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public String Generar_ID()
            {
                return Cls_Cat_SAP_Area_Funcional_Datos.Consecutivo_ID();
            }// fin del Generar_ID
            public DataTable Consulta_Area_Funcional_Especial()
            {
                return Cls_Cat_SAP_Area_Funcional_Datos.Consulta_Area_Funcional_Especial(this);
            }
        #endregion
    }
}