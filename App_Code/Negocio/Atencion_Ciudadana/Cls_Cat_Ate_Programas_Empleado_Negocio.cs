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
using Presidencia.Catalogo_Atencion_Ciudadana_Programas_Empleado.Datos;

namespace Presidencia.Catalogo_Atencion_Ciudadana_Programas_Empleado.Negocio
{
    public class Cls_Cat_Ate_Programas_Empleado_Negocio
    {
        #region Variables Privadas
        private String Programa_Empleado_ID;
        private String Emplead_ID;
        private int Programa_ID;
        private String Estatus;
        private String Unidad_Responsable_ID;
        private String Numero_Empleado;
        private String Nombre_Empleado;
        private String Nombre_Usuario;
        #endregion Variables Privadas

        #region Variables publicas
        public String P_Programa_Empleado_ID
        {
            get { return Programa_Empleado_ID; }
            set { Programa_Empleado_ID = value; }
        }
        public String P_Empleado_ID
        {
            get { return Emplead_ID; }
            set { Emplead_ID = value; }
        }
        public int P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Unidad_Responsable_ID
        {
            get { return Unidad_Responsable_ID; }
            set { Unidad_Responsable_ID = value; }
        }
        public String P_Numero_Empleado
        {
            get { return Numero_Empleado; }
            set { Numero_Empleado = value; }
        }
        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        
        #endregion Variables publicas

        #region Metodos Alta_Modificar_Eliminar

        public int Alta_Programa_Empleado()
        {
            return Cls_Cat_Ate_Programas_Empleado_Datos.Alta_Programa_Empleado(this);
        }

        public int Modificar_Programa_Empleado()
        {
            return Cls_Cat_Ate_Programas_Empleado_Datos.Modificar_Programa_Empleado(this);
        }

        public int Eliminar_Programa_Empleado()
        {
            return Cls_Cat_Ate_Programas_Empleado_Datos.Eliminar_Programa_Empleado(this);
        }

        #endregion

        #region Metodos Consulta

        public DataTable Consultar_Empleado()
        {
            return Cls_Cat_Ate_Programas_Empleado_Datos.Consultar_Empleado(this);
        }

        public DataTable Consultar_Programas_Empleados()
        {
            return Cls_Cat_Ate_Programas_Empleado_Datos.Consultar_Programas_Empleados(this);
        }

        #endregion

    }
}
