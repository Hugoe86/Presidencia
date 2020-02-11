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
using Presidencia.Catalogo_Compras_Proyectos_Programas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Proyectos_Programas_Negocio
/// </summary>
namespace Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio
{
    public class Cls_Cat_Com_Proyectos_Programas_Negocio
    {
        public Cls_Cat_Com_Proyectos_Programas_Negocio()
        {
        }

        #region (Variables Locales)
            private String Proyecto_Programa_ID;
            private String Nombre;
            private String Estatus;
            private String Comentarios;
            private String Clave;
            private String Elemento_Pep;            
            private String Usuario;
        #endregion

        #region (Variables Publicas)
            public String P_Proyecto_Programa_ID
            {
                get { return Proyecto_Programa_ID; }
                set { Proyecto_Programa_ID = value; }
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

            public String P_Clave
            {
                get { return Clave; }
                set { Clave = value; }
            }
            public String P_Elemento_Pep
            {
                get { return Elemento_Pep; }
                set { Elemento_Pep = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }        
        #endregion

        #region (Metodos)
            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Alta_Proyectos_Programas
            /// DESCRIPCION:            Dar de Alta un nuevo proyecto o programa a la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            05/Noviembre/2010 13:21 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Alta_Programas_Proyectos()
            {
                Cls_Cat_Com_Proyectos_Programas_Datos.Alta_Proyectos_Programas(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Baja_Programas_Proyectos
            /// DESCRIPCION:            Eliminar un proyecto o programa existente de la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            05/Noviembre/2010 13:29 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Baja_Programas_Proyectos()
            {
                Cls_Cat_Com_Proyectos_Programas_Datos.Baja_Programas_Proyectos(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Cambio_Programas_Proyectos
            /// DESCRIPCION:            Modificar un programa o proyecto existente de la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            05/Noviembre/2010 13:42 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Cambio_Programas_Proyectos()
            {
                Cls_Cat_Com_Proyectos_Programas_Datos.Cambio_Programas_Proyectos(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Programas_Proyectos
            /// DESCRIPCION:            Realizar la consulta de los programas o proyectos por criterio de busqueda o por un ID
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            05/Noviembre/2010 13:51 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Programas_Proyectos()
            {
                return Cls_Cat_Com_Proyectos_Programas_Datos.Consulta_Programas_Proyectos(this);
            }
            ///******************************************************************************* 
            ///NOMBRE DE LA FUNCIÓN: Consulta_Partidas_Genericas
            ///DESCRIPCIÓN: consultar las partidas genericas almacenadas en la base de datos
            ///PARAMETROS: 
            ///CREO: jtoledo
            ///FECHA_CREO: 03/03/2011 12:47:37 p.m.
            ///MODIFICO: 
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************            
            public DataTable Consulta_Partidas_Genericas(String Concepto_ID)
            {
                return Cls_Cat_Com_Proyectos_Programas_Datos.Consulta_Partidas_Genericas(Concepto_ID);
            }
            ///******************************************************************************* 
            ///NOMBRE DE LA FUNCIÓN: Consulta_Partidas_Especificas
            ///DESCRIPCIÓN: consultar las partidas especificas almacenadas en la base de datos
            ///PARAMETROS: 
            ///CREO: jtoledo
            ///FECHA_CREO: 03/03/2011 12:48:26 p.m.
            ///MODIFICO: 
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************            
            public DataTable Consulta_Partidas_Especificas(String ID)
            {
                return Cls_Cat_Com_Proyectos_Programas_Datos.Consulta_Partidas_Especificas(ID);
            }
            ///******************************************************************************* 
            ///NOMBRE DE LA FUNCIÓN: Consulta_Conceptos
            ///DESCRIPCIÓN: consultar los conceptos almacenados en la base de datos
            ///PARAMETROS: 
            ///CREO: jtoledo
            ///FECHA_CREO: 03/03/2011 12:48:57 p.m.
            ///MODIFICO: 
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************            
            public DataTable Consulta_Conceptos(String ID)
            {
                return Cls_Cat_Com_Proyectos_Programas_Datos.Consulta_Conceptos(ID);
            }
            ///******************************************************************************* 
            ///NOMBRE DE LA FUNCIÓN: Consulta_Programas_Partidas
            ///DESCRIPCIÓN: consultar las partidas asignadas a un proyecto
            ///PARAMETROS: 
            ///CREO: jtoledo
            ///FECHA_CREO: 03/03/2011 12:49:35 p.m.
            ///MODIFICO: 
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************            
            public DataTable Consulta_Programas_Partidas()
            {
                return Cls_Cat_Com_Proyectos_Programas_Datos.Consulta_Partidas(this);
            }
            ///******************************************************************************* 
            ///NOMBRE DE LA FUNCIÓN: Consulta_Capitulos
            ///DESCRIPCIÓN: consultar los capitulos existentes en la base de datos
            ///PARAMETROS: 
            ///CREO: jtoledo
            ///FECHA_CREO: 03/03/2011 12:50:08 p.m.
            ///MODIFICO: 
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************            
            public DataTable Consulta_Capitulos()
            {
                return Cls_Cat_Com_Proyectos_Programas_Datos.Consulta_Capitulos(this);
            }

            public DataTable Consulta_Programas_Especial()
            {
                return Cls_Cat_Com_Proyectos_Programas_Datos.Consulta_Programas_Especial(this);
            }
        #endregion

            
    }
}