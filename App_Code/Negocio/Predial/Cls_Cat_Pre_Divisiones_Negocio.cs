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
using Presidencia.Catalogo_Divisiones.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Divisiones_Negocio
/// </summary>

namespace Presidencia.Catalogo_Divisiones.Negocio{
    public class Cls_Cat_Pre_Divisiones_Negocio{

        #region Variables Internas

            private String Division_ID;
            private String Identificador;
            private String Descripcion;
            private String Estatus;
            private int Anio;
            private String Usuario;
            private DataTable Divisiones_Impuestos;

        #endregion

        #region Variables Publicas

            public String P_Division_ID
            {
                get { return Division_ID; }
                set { Division_ID = value; }
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

            public int P_Anio
            {
                get { return Anio; }
                set { Anio = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public DataTable P_Divisiones_Impuestos
            {
                get { return Divisiones_Impuestos; }
                set { Divisiones_Impuestos = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Division() {
                Cls_Cat_Pre_Divisiones_Datos.Alta_Division(this);
            }

            public void Modificar_Division(){
                Cls_Cat_Pre_Divisiones_Datos.Modificar_Division(this);
            }

            public void Eliminar_Division(){
                Cls_Cat_Pre_Divisiones_Datos.Eliminar_Division(this);
            }

            public DataTable Consultar_Divisiones(){
                return Cls_Cat_Pre_Divisiones_Datos.Consultar_Divisiones(this);
            }

            public Cls_Cat_Pre_Divisiones_Negocio Consultar_Datos_Division(){
                return Cls_Cat_Pre_Divisiones_Datos.Consultar_Datos_Division(this);
            }

            public DataTable Consultar_Tasas_Divisines_Lotificaciones(){
                return Cls_Cat_Pre_Divisiones_Datos.Consultar_Tasas_Divisines_Lotificaciones(this);
            }

            public DataTable Consultar_Divisiones_ID(String Division_Lot_ID){
                return Cls_Cat_Pre_Divisiones_Datos.Consultar_Divisiones_ID(Division_Lot_ID);
            }

        #endregion

    }
}