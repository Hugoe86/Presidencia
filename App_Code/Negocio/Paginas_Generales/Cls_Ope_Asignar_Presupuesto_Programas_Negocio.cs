using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Asignar_Presupuesto_Programas.Datos;

namespace Presidencia.Asignar_Presupuesto_Programas.Negocio
{
    public class Cls_Ope_Asignar_Presupuesto_Programas_Negocio
    {
        #region VARIABLES LOCALES
        private String Pres_Prog_Proy_ID;
        private String Nombre;
        private String Clave;
        private String Proyecto_Programa_ID;
        private String Monto_Presupuestal;
        private String Monto_Disponible;
        private String Monto_Comprometido;
        private String Anio_Presupuesto;
        private String Monto_Ejercido;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        #endregion
        #region VARIABLES GLOBALES
        public String P_Pres_Prog_Proy_ID
        {
            get { return Pres_Prog_Proy_ID; }
            set { Pres_Prog_Proy_ID = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }
        public String P_Proyecto_Programa_ID
        {
            get { return Proyecto_Programa_ID; }
            set { Proyecto_Programa_ID = value; }
        }
        public String P_Monto_Presupuestal
        {
            get { return Monto_Presupuestal; }
            set { Monto_Presupuestal = value; }
        }
        public String P_Monto_Disponible
        {
            get { return Monto_Disponible; }
            set { Monto_Disponible = value; }
        }
        public String P_Monto_Comprometido
        {
            get { return Monto_Comprometido; }
            set { Monto_Comprometido = value; }
        }
        public String P_Anio_Presupuesto
        {
            get { return Anio_Presupuesto; }
            set { Anio_Presupuesto = value; }
        }
        public String P_Monto_Ejercido
        {
            get { return Monto_Ejercido; }
            set { Monto_Ejercido = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        #endregion
        #region METODOS
        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Insertar_Presupuesto_Programas
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS: 
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 01/marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public Boolean Insertar_Presupuesto_Programas()
        {
            return Cls_Ope_Asignar_Presupuesto_Programas_Datos.Alta_Presupuesto_Programas(this);
        }
        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Presupuesto_Programas
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base de datos
        ///PARAMETROS:
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 01/marzo/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consultar_Presupuesto_Programas()
        {
            return Cls_Ope_Asignar_Presupuesto_Programas_Datos.Consulta_Presupuesto_Programas(this);
        }
        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Presupuesto_Programas
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS: 
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO:  01/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public Boolean Actualizar_Presupuesto_Programas()
        {
            return Cls_Ope_Asignar_Presupuesto_Programas_Datos.Modificar_Presupuesto_Programas(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_ID
        ///DESCRIPCIÓN: Metodo que regresa un ID
        ///PARAMETROS:   
        ///CREO: Leslie González Vázquez
        /// FECHA_CREO:  01/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Generar_ID()
        {
            return Cls_Ope_Asignar_Presupuesto_Programas_Datos.Consecutivo_ID();
        }// fin del Generar_ID

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Programa
        ///DESCRIPCIÓN: Metodo para consultar los programas existentes
        ///PARAMETROS:   
        ///CREO: Leslie González Vázquez
        /// FECHA_CREO:  01/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Programa()
        {
            return Cls_Ope_Asignar_Presupuesto_Programas_Datos.Consultar_Proyectos_Programas();
        }
        #endregion
    }
}