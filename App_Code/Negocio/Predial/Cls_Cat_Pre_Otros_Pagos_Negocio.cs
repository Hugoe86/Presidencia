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
using Presidencia.Catalogo_Otros_Pagos.Datos;

namespace Presidencia.Catalogo_Otros_Pagos.Negocio{
    
    public class Cls_Cat_Pre_Otros_Pagos_Negocio {

        #region Variables Internas

            private String Pago_ID;
            private String Concepto;
            private String Descripcion;
            private String Estatus;
            private String Usuario;
            private DataTable Otros_Pagos;
            //private String Campos_Dinamicos;
            //private String Filtros_Dinamicos;
            //private String Agrupar_Dinamico;
            //private String Ordenar_Dinamico;
            //private Boolean Incluir_Campos_Foraneos;

        #endregion

        #region Variables Publicas

            public String P_Pago_ID
            {
                get { return Pago_ID; }
                set { Pago_ID = value; }
            }

            public String P_Concepto
            {
                get { return Concepto; }
                set { Concepto = value; }
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

            public DataTable P_Otros_Pagos
            {
                get { return Otros_Pagos; }
                set { Otros_Pagos = value; }
            }

           

        #endregion

        #region Metodos

            public void Alta_Otro_Pago()
            {
                Cls_Cat_Pre_Otros_Pagos_Datos.Alta_Otro_Pago(this);
            }

            public void Modificar_Otro_Pago()
            {
                Cls_Cat_Pre_Otros_Pagos_Datos.Modificar_Otro_Pago(this);
            }

            public void Eliminar_Otro_Pago()
            {
                Cls_Cat_Pre_Otros_Pagos_Datos.Eliminar_Otro_Pago(this);
            }

            public DataTable Consultar_Otro_Pago() //Busqueda
            {
                return Cls_Cat_Pre_Otros_Pagos_Datos.Consultar_Otro_Pago(this);
            }

            public DataTable Consultar_Otros_Pagos()
            {
                return Cls_Cat_Pre_Otros_Pagos_Datos.Consultar_Otros_Pagos();
            }

        #endregion

    }
}