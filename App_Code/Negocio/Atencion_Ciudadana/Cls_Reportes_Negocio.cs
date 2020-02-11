using System;
using System.Data;
using Presidencia.Reportes_Atencion_Ciudadana.Datos;

namespace Presidencia.Reportes_Atencion_Ciudadana.Negocios
{
    public class Cls_Rpt_Ate_Reportes_Negocio
    {
        #region Variables Internas

        private String Dependencia;
        private String Estatus;
        private String Colonia;
        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Genera_Noticia;
        private String Folio_Vencido;
        private String Asunto_ID;
        private String Programa_ID;
        private String Tipo_Solucion;
        private String Sexo;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Ordenamiento_Dinamico;
        #endregion

        #region Variables Públicas
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

        public String P_Ordenamiento_Dinamico
        {
            get { return Ordenamiento_Dinamico; }
            set { Ordenamiento_Dinamico = value; }
        }

        public String P_Sexo
        {
            get { return Sexo; }
            set { Sexo = value; }
        }

        public String P_Tipo_Solucion
        {
            get { return Tipo_Solucion; }
            set { Tipo_Solucion = value; }
        }

        public String P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }

        public String P_Asunto_ID
        {
            get { return Asunto_ID; }
            set { Asunto_ID = value; }
        }

        public String P_Folio_Vencido
        {
            get { return Folio_Vencido; }
            set { Folio_Vencido = value; }
        }

        public String P_Genera_Noticia
        {
            get { return Genera_Noticia; }
            set { Genera_Noticia = value; }
        }

        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }

        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }

        public String P_Dependencia
        {
            get { return Dependencia; }
            set { Dependencia = value; }
        }
        #endregion

        #region Métodos
        public Cls_Rpt_Ate_Reportes_Negocio()
        {
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Detallado_Peticiones
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 7/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Reporte_Detallado_Peticiones()
        {
            return Cls_Reportes_Atencion_Ciudadana_Datos.Consulta_Reporte_Detallado_Peticiones(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Estadistica_Peticiones
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexión a la bd
        ///PARAMETROS:
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 22-jun-2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Estadistica_Peticiones()
        {
            return Cls_Reportes_Atencion_Ciudadana_Datos.Consulta_Estadistica_Peticiones(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Estadistica_Peticiones_Por_Usuario
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexión a la bd
        ///PARAMETROS:
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 22-jun-2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Estadistica_Peticiones_Por_Usuario()
        {
            return Cls_Reportes_Atencion_Ciudadana_Datos.Consulta_Estadistica_Peticiones_Por_Usuario(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Tiempo_Promedio_Respuesta_Peticion
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexión a la bd
        ///PARAMETROS:
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 23-jun-2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Tiempo_Promedio_Respuesta_Peticion()
        {
            return Cls_Reportes_Atencion_Ciudadana_Datos.Consulta_Tiempo_Promedio_Respuesta_Peticion(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Acumulado_Peticiones
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexión a la bd
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 7/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Reporte_Acumulado_Peticiones()
        {
            return Cls_Reportes_Atencion_Ciudadana_Datos.Consulta_Reporte_Acumulado_Peticiones(this);
        }
        
        #endregion

    }
}
