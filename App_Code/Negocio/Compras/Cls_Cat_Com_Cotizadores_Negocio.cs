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
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Constantes;
using Presidencia.Cotizadores.Datos;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Cotizadores.Negocio
{
    public class Cls_Cat_Com_Cotizadores_Negocio
    {
        #region (Variables Locales)
        private String Empleado_ID;
        private String No_Empleado;
        private String Nombre_Completo;
        private String Usuario;
        private String Giro_ID;
        private String Nombre;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Giro_Anterior;
        private String Correo;
        private String Password;
        private String Direccion_IP;
        DataTable Dt_Giros;

        #endregion

        #region Variables Internas
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_Password
        {
            get { return Password; }
            set { Password = value; }
        }

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_Nombre_Completo
        {
            get { return Nombre_Completo; }
            set { Nombre_Completo = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Giro_ID
        {
            get { return Giro_ID; }
            set { Giro_ID = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }

        }
        public DataTable P_Dt_Giros
        {
            get { return Dt_Giros; }
            set { Dt_Giros = value; }
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
        public String P_Giro_Anterior
        {
            get { return Giro_Anterior; }
            set { Giro_Anterior = value; }
        }

        public String P_Correo
        {
            get { return Correo; }
            set { Correo = value; }
        }
        public String P_Direccion_IP
        {
            get { return Direccion_IP; }
            set { Direccion_IP = value; }
        }


        #endregion

        #region Metodos
        public Cls_Cat_Com_Cotizadores_Negocio()
        {
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos
        ///DESCRIPCIÓN: Metodo que obtiene una dataset de acuerdo a los datos a modificar o modificados
        ///PARAMETROS: 
        ///CREO: Jacqueline Ramìrez Sierra
        ///FECHA_CREO: 11/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************         
        public DataTable Consulta_Datos()
        {
            return Cls_Cat_Com_Cotizadores_Datos.Consulta_Cotizadores(this);
        }

        public DataTable Consultar_Detalle_Cotizador()
        {
            return Cls_Cat_Com_Cotizadores_Datos.Consultar_Detalle_Cotizador(this);
        }

        public DataTable Consulta_Cotizadores()
        {
            return Cls_Cat_Com_Cotizadores_Datos.Consulta_Cotizadores(this);
        }
       
        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Empleado()
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
        ///de datos enviando un objeto de esta clase para sacar sus valores
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 15/febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Nombre_Empleado()
        {
            return Cls_Cat_Com_Cotizadores_Datos.Consultar_Nombre_Empleado(this);
        }
        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Cotizadores
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 14/febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public void Alta_Cotizadores()
        {
            Cls_Cat_Com_Cotizadores_Datos.Alta_Cotizadores(this);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Cotizadores
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 14/febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public void Eliminar_Cotizadores()
        { 
            Cls_Cat_Com_Cotizadores_Datos.Eliminar_Cotizadores(this);
        }
        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Cotizadores
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS:
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 14/febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public void Actualizar_Cotizadores()
        {
           Cls_Cat_Com_Cotizadores_Datos.Modificar_Cotizadores(this);
        }
        public void Eliminar_Detalles()
        {
            Cls_Cat_Com_Cotizadores_Datos.Eliminar_Detalles(this);
        }
        #endregion

        
    }
}