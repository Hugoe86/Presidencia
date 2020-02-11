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
using Presidencia.Dependencias.Datos;

namespace Presidencia.Dependencias.Negocios
{
    public class Cls_Cat_Dependencias_Negocio
    {
        #region (Variables Internas)
        private String Dependencia_ID;
        private String Nombre;
        private String Estatus;
        private String Comentarios;
        private String Nombre_Usuario;
        private String Area_Funcional_ID;
        private String Clave;
        private String Grupo_Dependencia_ID;
        private DataTable Dt_Fuentes_Financiamiento;
        private DataTable Dt_Programas;
        private DataTable Dt_Puestos;
        private String Tipo_Plaza;
        #endregion

        #region (Variables Públicas)
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

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
        public String P_Grupo_Dependencia_ID
        {
            get { return Grupo_Dependencia_ID; }
            set { Grupo_Dependencia_ID = value; }
        }
        
        public DataTable P_Dt_Fuentes_Financiamiento
        {
            get { return Dt_Fuentes_Financiamiento; }
            set { Dt_Fuentes_Financiamiento = value; }
        }

        public DataTable P_Dt_Programas
        {
            get { return Dt_Programas; }
            set { Dt_Programas = value; }
        }

        public DataTable P_Dt_Puestos {
            get { return Dt_Puestos; }
            set { Dt_Puestos = value; }
        }

        public String P_Tipo_Plaza {
            get { return Tipo_Plaza; }
            set { Tipo_Plaza = value; }
        }
        #endregion

        #region (Métodos)
        public void Alta_Dependencia()
        {
            Cls_Cat_Dependencias_Datos.Alta_Dependencia(this);
        }
        public void Modificar_Dependencia()
        {
            Cls_Cat_Dependencias_Datos.Modificar_Dependencia(this);
        }
        public DataTable Consulta_Dependencias()
        {
            return Cls_Cat_Dependencias_Datos.Consulta_Dependencias(this);
        }
        public void Elimina_Dependencia()
        {
            Cls_Cat_Dependencias_Datos.Elimina_Dependencia(this);
        }
        public DataTable Consulta_Area_Funcional() {
            return Cls_Cat_Dependencias_Datos.Consulta_Area_Funcional();
        }
        public DataTable Consultar_Sap_Det_Fte_Dependencia() {
            return Cls_Cat_Dependencias_Datos.Consultar_Sap_Det_Fte_Dependencia(this);
        }
        public DataTable Consultar_Sap_Det_Prog_Dependencia()
        {
            return Cls_Cat_Dependencias_Datos.Consultar_Sap_Det_Prog_Dependencia(this);
        }

        public DataTable Consultar_Puestos_Dependencia() {
            return Cls_Cat_Dependencias_Datos.Consultar_Puestos_Dependencia(this);
        }

        public  DataTable Consultar_Puestos_UR()
        {
            return Cls_Cat_Dependencias_Datos.Consultar_Puestos_UR(this);
        }

        public DataTable Consultar_Plazas_UR() { return Cls_Cat_Dependencias_Datos.Consultar_Plazas_UR(this); }
        #endregion
    }
}