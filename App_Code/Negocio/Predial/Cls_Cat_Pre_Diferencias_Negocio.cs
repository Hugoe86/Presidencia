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
using System.Collections.Generic;
using Presidencia.Catalogo_Diferencias.Datos;
/// <summary>
/// Summary description for Cls_Cat_Pre_Diferencias_Negocio
/// </summary>

namespace Presidencia.Catalogo_Diferencias.Negocio{
    public class Cls_Cat_Pre_Diferencias_Negocio{
        
        #region Variables Internas

            private String Tasa_Predial_ID;
            private String Identificador;
            private String Descripcion;
            private String Estatus;
            private String Usuario;
            private DataTable Diferencias_Tasas;

        #endregion

        #region Variables Publicas

            public String P_Tasa_Predial_ID
            {
                get { return Tasa_Predial_ID; }
                set { Tasa_Predial_ID = value; }
            }

            public String P_Identificador
            {
                get { return Identificador; }
                set { Identificador = value; }
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

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public DataTable P_Diferencias_Tasas
            {
                get { return Diferencias_Tasas; }
                set { Diferencias_Tasas = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Diferencia() {
                Cls_Cat_Pre_Diferencias_Datos.Alta_Diferencia(this);
            }

            public void Modificar_Diferencia(){
                Cls_Cat_Pre_Diferencias_Datos.Modificar_Diferencia(this);
            }

            public void Eliminar_Diferencia(){
                Cls_Cat_Pre_Diferencias_Datos.Eliminar_Diferencia(this);
            }

            public DataTable Consultar_Diferencias(){
                return Cls_Cat_Pre_Diferencias_Datos.Consultar_Diferencias(this);
            }

            public Cls_Cat_Pre_Diferencias_Negocio Consultar_Datos_Diferencia(){
                return Cls_Cat_Pre_Diferencias_Datos.Consultar_Datos_Diferencia(this);
            }

        #endregion
    }
}