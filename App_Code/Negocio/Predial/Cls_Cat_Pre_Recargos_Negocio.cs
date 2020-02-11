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
using Presidencia.Catalogo_Recargos.Datos;
/// <summary>
/// Summary description for Cls_Cat_Pre_Recargos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Recargos.Negocio{
    public class Cls_Cat_Pre_Recargos_Negocio{

        #region Variables Internas

            private String Recargo_ID;
            private String Identificador;
            private String Descripcion;
            private String Estatus;
            private String Usuario;
            private DataTable Recargos_Tasas;

        #endregion

        #region Variables Publicas

            public String P_Recargo_ID
            {
                get { return Recargo_ID; }
                set { Recargo_ID = value; }
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

            public DataTable P_Recargos_Tasas
            {
                get { return Recargos_Tasas; }
                set { Recargos_Tasas = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Recargo() {
                Cls_Cat_Pre_Recargos_Datos.Alta_Recargo(this);
            }

            public void Modificar_Recargo(){
                Cls_Cat_Pre_Recargos_Datos.Modificar_Recargo(this);
            }

            public void Eliminar_Recargo(){
                Cls_Cat_Pre_Recargos_Datos.Eliminar_Recargo(this);
            }

            public DataTable Consultar_Recargos(){
                return Cls_Cat_Pre_Recargos_Datos.Consultar_Recargos(this);
            }

            public Cls_Cat_Pre_Recargos_Negocio Consultar_Datos_Recargo(){
                return Cls_Cat_Pre_Recargos_Datos.Consultar_Datos_Recargo(this);
            }

        #endregion

    }
}