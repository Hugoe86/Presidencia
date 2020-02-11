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
using Presidencia.Control_Patrimonial_Catalogo_Donadores.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Donadores_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Donadores.Negocio
{

    public class Cls_Cat_Pat_Com_Donadores_Negocio
    {

        #region Variables Internas

            private String Donador_ID = null;
            private String Nombre = null;
            private String Apellido_Paterno = null;
            private String Apellido_Materno = null;
            private String Direccion = null;
            private String Cuidad = null;
            private String Estado = null;
            private String Telefono = null;
            private String Celular = null;
            private String CURP = null;
            private String RFC = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Donador_ID
            {
                get { return Donador_ID; }
                set { Donador_ID = value; }
            }
            public String P_Nombre {
                get { return Nombre; }
                set { Nombre = value; }
            }
            public String P_Apellido_Paterno
            {
                get { return Apellido_Paterno; }
                set { Apellido_Paterno = value; }
            }
            public String P_Apellido_Materno
            {
                get { return Apellido_Materno; }
                set { Apellido_Materno = value; }
            }
            public String P_Direccion
            {
                get { return Direccion; }
                set { Direccion = value; }
            }
            public String P_Cuidad
            {
                get { return Cuidad; }
                set { Cuidad = value; }
            }
            public String P_Estado
            {
                get { return Estado; }
                set { Estado = value; }
            }
            public String P_Telefono
            {
                get { return Telefono; }
                set { Telefono = value; }
            }
            public String P_Celular
            {
                get { return Celular; }
                set { Celular = value; }
            }
            public String P_CURP
            {
                get { return CURP; }
                set { CURP = value; }
            }
            public String P_RFC
            {
                get { return RFC; }
                set { RFC = value; }
            }
            public String P_Usuario {
                get { return Usuario; }
                set { Usuario = value; }
            }
            public String P_Tipo_DataTable {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }

        #endregion

        #region Metodos

            public Cls_Cat_Pat_Com_Donadores_Negocio Alta_Donador() {
                return Cls_Cat_Pat_Com_Donadores_Datos.Alta_Donador(this);
            }

            public void Modificar_Donador() { 
                Cls_Cat_Pat_Com_Donadores_Datos.Modificar_Donador(this);
            }

            public DataTable Consultar_DataTable() { 
                return Cls_Cat_Pat_Com_Donadores_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Donador() {
                Cls_Cat_Pat_Com_Donadores_Datos.Eliminar_Donador(this);
            }

            public Cls_Cat_Pat_Com_Donadores_Negocio Consultar_Datos_Donador() {
                return Cls_Cat_Pat_Com_Donadores_Datos.Consultar_Datos_Donador(this);
            }

            public DataTable Consultar_Donadores() { 
                return Cls_Cat_Pat_Com_Donadores_Datos.Consultar_Donadores(this);
            }

            public DataTable Consultar_Productos_Donador(){
                return Cls_Cat_Pat_Com_Donadores_Datos.Consultar_Productos_Donador(this);
            }

        #endregion

    }

}