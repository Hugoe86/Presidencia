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
using Presidencia.Puestos.Datos;
/// <summary>
/// Summary description for Cls_Cat_Puestos_Negocio
/// </summary>
namespace Presidencia.Puestos.Negocios
{
    public class Cls_Cat_Puestos_Negocio
    {
        public Cls_Cat_Puestos_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Puesto_ID;
        private String Nombre;
        private String Estatus;
        private String Comentarios;
        private String Nombre_Usuario;
        private DataTable Perfiles;
        private DataTable Dt_Puestos;
        private Double Salario_Mensual;
        private String Tipo_DataTable;
        private String Plaza_ID;
        private String Dependencia_ID;
        private String Empleado_ID;
        private String Aplica_Fondo_Retiro;
        private String Aplica_PSM;
        #endregion
        #region (Variables Publicas)
        public String P_Puesto_ID
        {
            get { return Puesto_ID; }
            set { Puesto_ID = value; }
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

        public DataTable P_Perfiles
        {
            get { return Perfiles; }
            set { Perfiles = value; }
        }

        public Double P_Salario_Mensual
        {
            get { return Salario_Mensual; }
            set { Salario_Mensual = value; }
        }
        public String P_Tipo_DataTable
        {
            get { return Tipo_DataTable; }
            set { Tipo_DataTable = value; }
        }
        public String P_Plaza_ID
        {
            get { return Plaza_ID; }
            set { Plaza_ID = value; }
        }

        public DataTable P_Dt_Puestos {
            get { return Dt_Puestos; }
            set { Dt_Puestos = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Empleado_ID {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_Aplica_Fondo_Retiro {
            get { return Aplica_Fondo_Retiro; }
            set { Aplica_Fondo_Retiro = value; }
        }

        public string P_Aplica_Psm
        {
            get { return Aplica_PSM; }
            set { Aplica_PSM = value; }
        }

        #endregion
        #region (Metodos)
        public void Alta_Puesto() 
        {
            Cls_Cat_Puestos_Datos.Alta_Puesto(this);
        }
        public void Modificar_Puesto() 
        {
            Cls_Cat_Puestos_Datos.Modificar_Puesto(this);
        }
        public void Eliminar_Puesto()
        {
            Cls_Cat_Puestos_Datos.Eliminar_Puesto(this);
        }
        public Cls_Cat_Puestos_Negocio Consulta_Datos_Puestos() 
        {
            return Cls_Cat_Puestos_Datos.Consulta_Datos_Puestos(this);
        }
        public DataTable Consulta_DataTable() 
        {
            return Cls_Cat_Puestos_Datos.Consulta_DataTable(this);
        }

        public DataTable Consultar_Puestos() {
            return Cls_Cat_Puestos_Datos.Consultar_Puestos(this);
        }

        public DataTable Consultar_Puestos_Disponibles_Dependencia() {
            return Cls_Cat_Puestos_Datos.Consultar_Puestos_Disponibles_Dependencia(this);
        }

        public DataTable Consultar_Puestos_UR() { return Cls_Cat_Puestos_Datos.Consultar_Puestos_UR(this); }
        #endregion
    }
}