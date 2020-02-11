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
using Presidencia.Catalogo_Notarios.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Notarios_Negocio
/// </summary>

namespace Presidencia.Catalogo_Notarios.Negocio{

    public class Cls_Cat_Pre_Notarios_Negocio
    {

        #region Variables Internas

            private String Notario_ID;
            private String Apellido_Paterno;
            private String Apellido_Materno;
            private String Nombre;
            private String Numero_Notaria;
            private String Sexo;
            private String Estado;
            private String Ciudad;
            private String Colonia;
            private String Calle;
            private String Numero_Interior;
            private String Numero_Exterior;
            private String Codigo_Postal;
            private String RFC;
            private String CURP;
            private String Estatus;
            private String Telefono;
            private String Fax;
            private String Celular;
            private String E_Mail;
            private String Usuario;
            private String Filtro_Dinamico;

        #endregion

        #region Variables Publicas

            public String P_Notario_ID
            {
                get { return Notario_ID; }
                set { Notario_ID = value.Trim(); }
            }

            public String P_Apellido_Paterno
            {
                get { return Apellido_Paterno; }
                set { Apellido_Paterno = value.Trim(); }
            }

            public String P_Apellido_Materno
            {
                get { return Apellido_Materno; }
                set { Apellido_Materno = value.Trim(); }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value.Trim(); }
            }

            public String P_Numero_Notaria
            {
                get { return Numero_Notaria; }
                set { Numero_Notaria = value.Trim(); }
            }

            public String P_Sexo
            {
                get { return Sexo; }
                set { Sexo = value.Trim(); }
            }

            public String P_Estado
            {
                get { return Estado; }
                set { Estado = value.Trim(); }
            }

            public String P_Ciudad
            {
                get { return Ciudad; }
                set { Ciudad = value.Trim(); }
            }

            public String P_Colonia
            {
                get { return Colonia; }
                set { Colonia = value.Trim(); }
            }

            public String P_Calle
            {
                get { return Calle; }
                set { Calle = value.Trim(); }
            }

            public String P_Numero_Exterior
            {
                get { return Numero_Exterior; }
                set { Numero_Exterior = value.Trim(); }
            }

            public String P_Numero_Interior
            {
                get { return Numero_Interior; }
                set { Numero_Interior = value.Trim(); }
            }

            public String P_Codigo_Postal
            {
                get { return Codigo_Postal; }
                set { Codigo_Postal = value; }
            }

            public String P_RFC
            {
                get { return RFC; }
                set { RFC = value.Trim(); }
            }

            public String P_CURP
            {
                get { return CURP; }
                set { CURP = value.Trim(); }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value.Trim(); }
            }

            public String P_Telefono
            {
                get { return Telefono; }
                set { Telefono = value.Trim(); }
            }

            public String P_Fax
            {
                get { return Fax; }
                set { Fax = value.Trim(); }
            }

            public String P_Celular
            {
                get { return Celular; }
                set { Celular = value.Trim(); }
            }

            public String P_E_Mail
            {
                get { return E_Mail; }
                set { E_Mail = value.Trim(); }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value.Trim(); }
            }

            public String P_Filtro_Dinamico
            {
                get { return Filtro_Dinamico; }
                set { Filtro_Dinamico = value; }
            }

        #endregion

        #region Metodos

            public Boolean Alta_Notario()
            {
                return Cls_Cat_Pre_Notarios_Datos.Alta_Notario(this);
            }

            public Boolean Modificar_Notario()
            {
                return Cls_Cat_Pre_Notarios_Datos.Modificar_Notario(this);
            }

            public Boolean Eliminar_Notario()
            {
                return Cls_Cat_Pre_Notarios_Datos.Eliminar_Notario(this);
            }

            public Cls_Cat_Pre_Notarios_Negocio Consultar_Datos_Notario()
            {
                return Cls_Cat_Pre_Notarios_Datos.Consultar_Datos_Notario(this);
            }

            public DataTable Consultar_Notarios()
            {
                return Cls_Cat_Pre_Notarios_Datos.Consultar_Notarios(this);
            }

        #endregion

    }
}
