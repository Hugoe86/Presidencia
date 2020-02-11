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
using Presidencia.Catalogo_Contribuyentes.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Contribuyentes_Negocio
/// </summary>

namespace Presidencia.Catalogo_Contribuyentes.Negocio {
    public class Cls_Cat_Pre_Contribuyentes_Negocio {

        #region Variables Internas

            private String Contribuyente_ID;
            private String Apellido_Paterno;
            private String Apellido_Materno;
            private String Nombre;
            private String Sexo;
            private String Estado_Civil;
            private DateTime Fecha_Nacimiento;
            
            private String RFC;
            private String CURP;
            private String IFE;
            private String Estatus;
            private String Tipo_Persona;
            private String Representante_Legal;
            private String Tipo_Propietario;
            private String Domicilio;
            private String Interior;
            private String Exterior;
            private String Colonia;
            private String Ciudad;
            private String Codigo_Postal;
            private String Estado;
            private String Usuario;
            private String Campos_Dinamicos;
            private String Filtros_Dinamicos;
            private String Agrupar_Dinamico;
            private String Ordenar_Dinamico;

        #endregion

        #region Variables Publicas

            public String P_Contribuyente_ID
            {
                get { return Contribuyente_ID; }
                set { Contribuyente_ID = value; }
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

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public String P_Sexo
            {
                get { return Sexo; }
                set { Sexo = value; }
            }

            public String P_Estado_Civil
            {
                get { return Estado_Civil; }
                set { Estado_Civil = value; }
            }

            public DateTime P_Fecha_Nacimiento
            {
                get { return Fecha_Nacimiento; }
                set { Fecha_Nacimiento = value; }
            }

            public String P_RFC
            {
                get { return RFC; }
                set { RFC = value; }
            }

            public String P_CURP
            {
                get { return CURP; }
                set { CURP = value; }
            }

            public String P_IFE
            {
                get { return IFE; }
                set { IFE = value; }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

            public String P_Tipo_Persona
            {
                get { return Tipo_Persona; }
                set { Tipo_Persona = value; }
            }

            public String P_Representante_Legal
            {
                get { return Representante_Legal; }
                set { Representante_Legal = value; }
            }

            public String P_Tipo_Propietario
            {
                get { return Tipo_Propietario; }
                set { Tipo_Propietario = value; }
            }

            public String P_Domicilio
            {
                get { return Domicilio; }
                set { Domicilio = value; }
            }

            public String P_Interior
            {
                get { return Interior; }
                set { Interior = value; }
            }

            public String P_Exterior
            {
                get { return Exterior; }
                set { Exterior = value; }
            }

            public String P_Colonia
            {
                get { return Colonia; }
                set { Colonia = value; }
            }

            public String P_Ciudad
            {
                get { return Ciudad; }
                set { Ciudad = value; }
            }

            public String P_Codigo_Postal
            {
                get { return Codigo_Postal; }
                set { Codigo_Postal = value; }
            }

            public String P_Estado
            {
                get { return Estado; }
                set { Estado = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public String P_Campos_Dinamicos
            {
                get { return Campos_Dinamicos; }
                set { Campos_Dinamicos = value; }
            }

            public String P_Filtros_Dinamicos
            {
                get { return Filtros_Dinamicos; }
                set { Filtros_Dinamicos = value; }
            }

            public String P_Agrupar_Dinamico
            {
                get { return Agrupar_Dinamico; }
                set { Agrupar_Dinamico = value; }
            }

            public String P_Ordenar_Dinamico
            {
                get { return Ordenar_Dinamico; }
                set { Ordenar_Dinamico = value; }
            }

        #endregion

        #region Metodos

            public bool Alta_Contribuyente_Orden_Variacion()
            {
                return Cls_Cat_Pre_Contribuyentes_Datos.Alta_Contribuyente_Orden_Variacion(this);
            }
            public void Alta_Contribuyente()
            {
                Cls_Cat_Pre_Contribuyentes_Datos.Alta_Contribuyente(this);
            }

            public void Modificar_Contribuyente()
            {
                Cls_Cat_Pre_Contribuyentes_Datos.Modificar_Contribuyente(this);
            }

            public void Eliminar_Contribuyente()
            {
                Cls_Cat_Pre_Contribuyentes_Datos.Eliminar_Contribuyente(this);
            }

            public Cls_Cat_Pre_Contribuyentes_Negocio Consultar_Datos_Contribuyente()
            {
                return Cls_Cat_Pre_Contribuyentes_Datos.Consultar_Datos_Contribuyente(this);
            }
            public DataTable Consultar_Contribuyentes_Popup()
            {
                return Cls_Cat_Pre_Contribuyentes_Datos.Consultar_Contribuyentes_Popup(this);
            }
            public DataTable Consultar_Contribuyentes()
            {
                return Cls_Cat_Pre_Contribuyentes_Datos.Consultar_Contribuyentes(this);
            }
            ///******************************************************************************* 
            ///NOMBRE DE LA FUNCIÓN: Consultar_Menu_Contribuyentes
            ///DESCRIPCIÓN: consultar contribuyentes por nombre o Rfc o fecha de nacimiento
            ///PARAMETROS: 
            ///CREO: jtoledo
            ///FECHA_CREO: 08/18/2011 03:31:53 p.m.
            ///MODIFICO: 
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************            
            public DataTable Consultar_Menu_Contribuyentes()
            {
                return Cls_Cat_Pre_Contribuyentes_Datos.Consultar_Menu_Contribuyentes(this);
            }

        #endregion


            
    }
}