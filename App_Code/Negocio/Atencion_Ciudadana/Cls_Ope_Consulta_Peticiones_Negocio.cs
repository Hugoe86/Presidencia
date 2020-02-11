using System;
using System.Data;
using Presidencia.Consulta_Peticiones.Datos;

namespace Presidencia.Consulta_Peticiones.Negocios
{
    public class Cls_Ope_Consulta_Peticiones_Negocio
    {
        #region Variables Internas
        private String Folio;
        private String Area;
        private String Dependencia;
        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Nombre_Solicitante;
        private String Apellido_Paterno;
        private String Apellido_Materno;
        private String Telefono;
        private String Email;
        // consulta de dependencias
        private String Nombre;
        private String Clave;
        private String Comentarios;
        private String Estatus;
        private String Accion_ID;
        private int Programa_ID;
        private String Asunto_ID;
        private String Colonia_ID;

        
        #endregion

        #region Variables Públicas

        public String P_Telefono
        {
            get { return Telefono; }
            set { Telefono = value; }
        }

        public String P_Email
        {
            get { return Email; }
            set { Email = value; }
        }

        public String P_Apellido_Materno
        {
            get { return Apellido_Materno; }
            set { Apellido_Materno = value; }
        }

        public String P_Apellido_Paterno
        {
            get { return Apellido_Paterno; }
            set { Apellido_Paterno = value; }
        }

        public String P_Nombre_Solicitante
        {
            get { return Nombre_Solicitante; }
            set { Nombre_Solicitante = value; }
        }

        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }
        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }
        public String P_Dependencia
        {
            get { return Dependencia; }
            set { Dependencia = value; }
        }

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }

        public String P_Area
        {
            get { return Area; }
            set { Area = value; }
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
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Accion_ID
        {
            get { return Accion_ID; }
            set { Accion_ID = value; }
        }
        public int P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }
        public String P_Asunto_ID
        {
            get { return Asunto_ID; }
            set { Asunto_ID = value; }
        }
        public String P_Colonia_ID
        {
            get { return Colonia_ID; }
            set { Colonia_ID = value; }
        }
        #endregion

        #region Métodos
        public Cls_Ope_Consulta_Peticiones_Negocio()
        {

        }
        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Peticiones_General
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexión a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS: 
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consulta_Peticiones_General()
        {

            return Cls_Ope_Consulta_Peticiones_Datos.Consulta_Peticiones_General(this);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Peticion_Detallada
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexión a la base 
        ///de datos enviando 
        ///un objeto de esta clase para sacar sus valores
        ///PARAMETROS: 
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consulta_Peticion_Detallada()
        {
            return Cls_Ope_Consulta_Peticiones_Datos.Consulta_Peticion_Detallada(this);
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencias
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexión a la base 
        ///         de datos enviando una instancia de esta clase con filtros para la consulta
        ///PARAMETROS: 
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-may-2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Dependencias()
        {
            return Cls_Ope_Consulta_Peticiones_Datos.Consultar_Dependencias(this);
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitantes
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexión a la base 
        ///         de datos enviando una instancia de esta clase con filtros para la consulta
        ///PARAMETROS: 
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-may-2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Solicitantes()
        {
            return Cls_Ope_Consulta_Peticiones_Datos.Consultar_Solicitantes(this);
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Unidad_Responsable_De_Empleado
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de consultar la información
        ///PARAMETROS: 
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 29-oct-2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Unidad_Responsable_De_Empleado(String Empleado_ID, string Dependencia_ID_Empleado)
        {
            return Cls_Ope_Consulta_Peticiones_Datos.Consultar_Unidad_Responsable_De_Empleado(Empleado_ID, Dependencia_ID_Empleado);
        }

        #endregion
    }
}
