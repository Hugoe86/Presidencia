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
using Presidencia.Turnos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Turnos_Negocio
/// </summary>
namespace Presidencia.Turnos.Negocios
{
    public class Cls_Cat_Turnos_Negocio
    {
        public Cls_Cat_Turnos_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Turno_ID;
        private String DESCRIPCION;
        private DateTime Hora_Entrada;
        private DateTime Hora_Salida;
        private String Estatus;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_Turno_ID
        {
            get { return Turno_ID; }
            set { Turno_ID = value; }
        }

        public String P_Descripcion
        {
            get { return DESCRIPCION; }
            set { DESCRIPCION = value; }
        }

        public DateTime P_Hora_Entrada
        {
            get { return Hora_Entrada; }
            set { Hora_Entrada = value; }
        }

        public DateTime P_Hora_Salida
        {
            get { return Hora_Salida; }
            set { Hora_Salida = value; }
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
        #endregion
        #region (Metodos)
        public void Alta_Turnos()
        {
            Cls_Cat_Turnos_Datos.Alta_Turnos(this);
        }
        public void Modificar_Turno()
        {
            Cls_Cat_Turnos_Datos.Modificar_Turnos(this);
        }
        public void Eliminar_Turno()
        {
            Cls_Cat_Turnos_Datos.Elimina_Turno(this);
        }
        public DataTable Consulta_Datos_Turnos()
        {
            return Cls_Cat_Turnos_Datos.Consulta_Datos_Turnos(this);
        }
        public DataTable Consulta_Turnos()
        {
            return Cls_Cat_Turnos_Datos.Consulta_Turnos(this);
        }
        #endregion
    }
}