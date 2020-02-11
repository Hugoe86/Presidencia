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
using Presidencia.Colonias.Datos;


/// <summary>
///Summary description for cat_ate_colonias
///</summary>

namespace Presidencia.Colonias.Negocios
{
    public class Cls_Cat_Ate_Colonias_Negocio
    {
        #region Variables Internas

        //atributo de clase - Nombre de la Colonia
        private String Nombre;
        //atributo de clase - Tipo de Colonia
        private String Tipo_Colonia;
        //atributo de clase - Descripcion de la Colonia
        private String Descripcion;
        //atributo de clase - Id de la Colonia
        private String Colonia_Id;
        //atributo de clase - Usuario logeado
        private String Usuario;
        //atributo de clase - Costo de construcción por colonia
        private String Costo_Construccion;
        //atributo de clase - Campos a mostrar
        private String Campos_Dinamicos;
        //atributo de clase - Filtros a usar
        private String Filtros_Dinamicos;
        //atributo de clase - Agrupaciones a aplicar
        private String Agrupar_Dinamico;
        //atributo de clase - Ordenación a aplicar
        private String Ordenar_Dinamico;


        //Variable que servira para llamar a la clase de Datos 

        private Cls_Cat_Ate_Colonias_Datos Datos_Colonia;

        #endregion

        #region Variables Publicas

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        
        public String P_Colonia_Id
        {
            get { return Colonia_Id; }
            set { Colonia_Id = value; }
        }

        public String P_Tipo_Colonia
        {
            get { return Tipo_Colonia; }
            set { Tipo_Colonia = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Costo_Construccion
        {
            get { return Costo_Construccion; }
            set { Costo_Construccion = value; }
        }
        private Cls_Ate_Colonias_Datos colonia_dato;

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

        #region Metodos

        public Cls_Cat_Ate_Colonias_Negocio()
        {
            Datos_Colonia = new Cls_Cat_Ate_Colonias_Datos();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Colonia
        ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para dar de alta una colonia
        ///PARAMETROS:
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************/

        public void Alta_Colonia()
        {
            Datos_Colonia.Alta_Colonia(this);
        }


        /*******************************************************************************
        NOMBRE DE LA FUNCIÓN: Modificar_Colonia
        DESCRIPCIÓN:Llama el metodo de la clase de Datos para modificar una colonia
        PARAMETROS:
        CREO: Susana Trigueros Armenta
        FECHA_CREO: 23/Agosto/2010 
        MODIFICO:
        FECHA_MODIFICO:
        CAUSA_MODIFICACIÓN:
        *******************************************************************************/

        public void Modificar_Colonia()
        {
            Datos_Colonia.Modificar_Colonia(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Colonia
        ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para eliminar una colonia
        ///PARAMETROS:
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public void Eliminar_Colonia()
        {
            Datos_Colonia.Eliminar_Colonia(this);
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_ID
        ///DESCRIPCIÓN: Metodo que regresa un ID
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        /// FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Generar_ID()
        {
            //colonia_dato = new cat_ate_colonias_datos();
            String Id_generado = Datos_Colonia.Consecutivo();
            return Id_generado;
        }// fin del Generar_ID

        
        

        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN: Consulta_Datos_Colonia
        /////DESCRIPCIÓN: Metodo que llena el GridView de acuerdo a una busqueda
        /////PARAMETROS: GridView que se llenara
        /////CREO: Susana Trigueros Armenta
        /////FECHA_CREO: 24/Agosto/2010 
        /////MODIFICO:
        /////FECHA_MODIFICO:
        /////CAUSA_MODIFICACIÓN:
        /////*******************************************************************************     
        //public void Consulta_Datos_Colonia(GridView Grid_colonias)
        //{
        //    DataSet Data_Set = new DataSet();
        //    Data_Set = Datos_Colonia.Consulta_Colonia(this);
        //    if (Data_Set != null)
        //    {
        //        Grid_colonias.DataSource = Data_Set;
        //        Grid_colonias.DataBind();
        //    }
        //}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Modificados
        ///DESCRIPCIÓN: Metodo que obtiene una dataset de acuerdo a los datos a modificar o modificados
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 01/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************         
        public DataSet Consulta_Datos()
        {
            DataSet Data_Set = new DataSet();
            Data_Set = Datos_Colonia.Consulta_Colonia(this);
            return Data_Set;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Colonias
        ///DESCRIPCIÓN          : Método para ejecutar la consulta de Colonias de la capa de datos
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guarado
        ///FECHA_CREO           : 16/Diciembre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************            
        public DataTable Consultar_Colonias()
        {
            return Cls_Cat_Ate_Colonias_Datos.Consultar_Colonias(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Modificados
        ///DESCRIPCIÓN: Metodo que obtiene una dataset de acuerdo a los datos a modificar o modificados
        ///PARAMETROS: 
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 04/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************         
        public DataTable Consulta_Tipos_Colonias(){
            return Datos_Colonia.Consultar_Tipo_Colonias();
        }

        #endregion

    }
}