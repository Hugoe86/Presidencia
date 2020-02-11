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
using Presidencia.SAP_Partidas_Especificas.Datos;


namespace Presidencia.SAP_Partidas_Especificas.Negocio
{
    public class Cls_Cat_SAP_Partidas_Especificas_Negocio
    {
        #region (Variables Internas)
            private string Partida_ID;
            private string Giro_ID;
            private string Estatus;
            private string Nombre;
            private string Partida_Generica_ID;
            private string Clave;
            private string Descripcion;
            private string Cuenta;
            private string Usuario_Creo;
            private string Fecha_Creo;
            private string Usuario_Modifico;
            private string Fecha_Modifico;
        #endregion

        #region (Variables Publicas)
            public string P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public string P_Partida_ID
            {
                get { return Partida_ID; }
                set { Partida_ID = value; }
            }
            public string P_Giro_ID
            {
                get { return Giro_ID; }
                set { Giro_ID = value; }
            }
            public string P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public string P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }
            public string P_Cuenta
            {
                get { return Cuenta; }
                set { Cuenta = value; }
            }
            public string P_Clave
            {
                get { return Clave; }
                set { Clave = value; }
            }
            public string P_Partida_Generica_ID
            {
                get { return Partida_Generica_ID; }
                set { Partida_Generica_ID = value; }
            }
            public string P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public string P_Fecha_Creo
            {
                get { return Fecha_Creo; }
                set { Fecha_Creo = value; }
            }
            public string P_Usuario_Modifico
            {
                get { return Usuario_Modifico; }
                set { Usuario_Modifico = value; }
            }
            public string P_Fecha_Modifico
        {
            get { return Fecha_Modifico; }
            set { Fecha_Modifico = value; }
        }
        #endregion

        #region (Metodos)
            public DataTable Consulta_Partida_Especifica()
            {
                return Cls_Cat_SAP_Partidas_Especificas_Datos.Consulta_Partida_Especifica(this);
            }
        #endregion
    }
}