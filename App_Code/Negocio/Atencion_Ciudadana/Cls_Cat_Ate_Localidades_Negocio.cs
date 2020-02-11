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
using Presidencia.Localidades.Datos;

namespace Presidencia.Localidades.Negocios
{
    /// <summary>
    /// Summary description for Cls_Cat_Ate_Localidades_Negocio
    /// </summary>
    public class Cls_Cat_Ate_Localidades_Negocio
    {
        #region Variables Internas
        private String Localidad_ID;
        private String Nombre;
        private String Descripcion;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        #endregion

        #region Variables Públicas

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Localidad_ID
        {
            get { return Localidad_ID; }
            set { Localidad_ID = value; }
        }

        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value.Trim(); }
        }

        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value.Trim(); }
        }

        public String P_Agrupar_Dinamico
        {
            get { return Agrupar_Dinamico; }
            set { Agrupar_Dinamico = value.Trim(); }
        }

        public String P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value.Trim(); }
        }
        #endregion

        #region Métodos
        public Cls_Cat_Ate_Localidades_Negocio()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Insertar_Localidad
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS: 1.-Usuario_Creo, Nombre del usuario logueado actualmente en el sistema
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public int Insertar_Localidad(String Usuario_Creo)
        {
            return Cls_Cat_Ate_Localidades_Datos.Insertar_Localidad(this, Usuario_Creo);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Localidad
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public int Eliminar_Localidad()
        {
            return Cls_Cat_Ate_Localidades_Datos.Eliminar_Localidad(this);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Localidad
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS: 1.-Usuario_Modifico, Nombre del usuario logueado actualmente en el sistema
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        public int Modificar_Localidad(String Usuario_Modifico)
        {
            return Cls_Cat_Ate_Localidades_Datos.Modificar_Localidad(this, Usuario_Modifico);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Localidad
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consultar_Localidad()
        {
            return Cls_Cat_Ate_Localidades_Datos.Consultar_Localidad(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Localidades
        ///DESCRIPCIÓN          : Ejecuta la consulta de Localidades de la capa de datos
        ///PARAMETROS:
        ///CREO                 : Antonio Slavador Benavides Guardado
        ///FECHA_CREO           : 20/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Localidades()
        {
            return Cls_Cat_Ate_Localidades_Datos.Consultar_Localidades(this);
        }
        #endregion
    }
}
