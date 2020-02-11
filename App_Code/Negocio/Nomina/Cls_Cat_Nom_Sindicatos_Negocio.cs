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
using Presidencia.Sindicatos.Datos;

namespace Presidencia.Sindicatos.Negocios
{
    public class Cls_Cat_Nom_Sindicatos_Negocio
    {
        #region (Variables Internas)
        private String Sindicato_ID;
        private String Nombre;
        private String Responsable;
        private String Estatus;        
        private String Tipo_Percepcion;
        private String Comentarios;
        private String Nombre_Usuario;
        private String Antiguedad_Sindicato;
        private String Monto;
        private DataTable Dt_Percepciones;
        private DataTable Dt_Deducciones;
        private DataTable Dt_Antiguedad_Sindicatos;
        #endregion

        #region (Variables Publicas)
        public String P_Sindicato_ID
        {
            get { return Sindicato_ID; }
            set { Sindicato_ID = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Responsable
        {
            get { return Responsable; }
            set { Responsable = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public DataTable P_Dt_Percepciones
        {
            get { return Dt_Percepciones; }
            set { Dt_Percepciones = value; }
        }

        public DataTable P_Dt_Deducciones
        {
            get { return Dt_Deducciones; }
            set { Dt_Deducciones = value; }
        }

        public String P_Tipo_Percepcion
        {
            get { return Tipo_Percepcion; }
            set { Tipo_Percepcion = value; }
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
        public String P_Antiguedad_Sindicato
        {
            get { return Antiguedad_Sindicato; }
            set { Antiguedad_Sindicato = value; }
        }
        public String P_Monto
        {
            get { return Monto; }
            set { Monto = value; }
        }
        public DataTable P_Dt_Antiguedad_Sindicatos
        {
            get { return Dt_Antiguedad_Sindicatos; }
            set { Dt_Antiguedad_Sindicatos = value; }
        }
        #endregion

        #region (Metodos)
        public void Alta_Sindicato()
        {
            Cls_Cat_Nom_Sindicatos_Datos.Alta_Sindicato(this);
        }
        public void Modificar_Sindicato()
        {
            Cls_Cat_Nom_Sindicatos_Datos.Modificar_Sindicato(this);
        }
        public void Eliminar_Sindicato()
        {
            Cls_Cat_Nom_Sindicatos_Datos.Eliminar_Sindicato(this);
        }
        public DataTable Consulta_Datos_Sindicato()
        {
            return Cls_Cat_Nom_Sindicatos_Datos.Consulta_Datos_Sindicato(this);
        }
        public DataTable Consulta_Sindicato()
        {
            return Cls_Cat_Nom_Sindicatos_Datos.Consulta_Sindicato(this);
        }
        public DataTable Consultar_Percepciones_Deducciones()
        {
            return Cls_Cat_Nom_Sindicatos_Datos.Consultar_Percepciones_Deducciones(this);
        }

        public DataTable Consultar_Percepciones_Deducciones_Generales()
        {
            return Cls_Cat_Nom_Sindicatos_Datos.Consultar_Percepciones_Deducciones_Generales(this);
        }
        public DataTable Consultar_Antiguedades_Sindicales() {
            return Cls_Cat_Nom_Sindicatos_Datos.Consultar_Antiguedades_Sindicales(this);
        }
        #endregion
    }
}