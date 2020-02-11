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
using Presidencia.Constantes;
using Presidencia.Registro_Peticion.Datos;
using System.Data.OracleClient;
using System.Collections.Generic;
namespace Presidencia.Registro_Peticion.Negocios
{
    public class Cls_Cat_Ate_Peticiones_Negocio
    {
        #region Variables Locales
        //Propiedades de la tabla Ope_Ate_Peticiones
        private String No_Peticion;
        private int Anio_Peticion;
        private string Programa_ID;
        private String Asunto_ID;
        private String Accion_ID;
        private String Usuario_ID;
        private String Folio;
        private String Estatus;
        private String Estatus_Archivo;
        private String Peticion;
        private String Fecha_Peticion;
        private String Fecha_Solucion_Probable;
        private String Fecha_Solucion_Real;
        private String Nivel_Importancia;
        private String Nombre;
        private String Apellido_Paterno;
        private String Apellido_Materno;
        private String Colonia_ID;
        private String Calle_ID;
        private String Numero_Exterior;
        private String Numero_Interior;
        private String Referencia;
        private int Edad;
        private DateTime Fecha_Nacimiento;
        private String Sexo;
        private String Telefono;
        private String Email;
        private String Descripcion_Solucion;
        private String Genera_Noticia;
        private String Codigo_Postal;
        private String Origen;
        private String Por_Validar;
        private String Programa_Empleado_ID;
        private String Tipo_Consecutivo;
        private int Cantidad_Peticiones_Consultar = 0;
        
        private String Dependencia_ID;
        private String Descripcion_Cambio;
        private String Fecha_Asignacion_Cambio;
        private String Area_ID;
        private String Asignado;
        private String Tipo_Solucion;
        private String Nombre_Atendio;

        private String Usuario_Creo_Modifico;
        
        private String Fecha_Inicio;
        private String Fecha_Final;
        private OracleCommand Comando_Oracle;
        private String Filtros_Dinamicos;
        private String Orden_Dinamico;
        private DataTable Dt_Archivos;

        private List<string> Lista_Archivos_Eliminar;
        #endregion

        #region Variables Publicas
        public String P_Asignado
        {
            get { return Asignado; }
            set { Asignado = value; }
        }
        public String P_No_Peticion
        {
            get { return No_Peticion; }
            set { No_Peticion = value; }
        }

        public int P_Anio_Peticion
        {
            get { return Anio_Peticion; }
            set { Anio_Peticion = value; }
        }

        public string P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }

        public String P_Asunto_ID
        {
            get { return Asunto_ID; }
            set { Asunto_ID = value; }
        }

        public String P_Accion_ID
        {
            get { return Accion_ID; }
            set { Accion_ID = value; }
        }

        public String P_Usuario_ID
        {
            get { return Usuario_ID; }
            set { Usuario_ID = value; }
        }

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Filtro_Estatus_Archivo
        {
            get { return Estatus_Archivo; }
            set { Estatus_Archivo = value; }
        }

        public String P_Peticion
        {
            get { return Peticion; }
            set { Peticion = value; }
        }

        public String P_Fecha_Peticion
        {
            get { return Fecha_Peticion; }
            set { Fecha_Peticion = value; }
        }

        public String P_Fecha_Solucion_Probable
        {
            get { return Fecha_Solucion_Probable; }
            set { Fecha_Solucion_Probable = value; }
        }

        public String P_Fecha_Solucion_Real
        {
            get { return Fecha_Solucion_Real; }
            set { Fecha_Solucion_Real = value; }
        }

        public String P_Nivel_Importancia
        {
            get { return Nivel_Importancia; }
            set { Nivel_Importancia = value; }
        }
        public String P_Nombre
        {
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
        public String P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }
        public String P_Colonia_ID
        {
            get { return Colonia_ID; }
            set { Colonia_ID = value; }
        }
        public String P_Calle_ID
        {
            get { return Calle_ID; }
            set { Calle_ID = value; }
        }
        public String P_Numero_Exterior
        {
            get { return Numero_Exterior; }
            set { Numero_Exterior = value; }
        }
        public String P_Numero_Interior
        {
            get { return Numero_Interior; }
            set { Numero_Interior = value; }
        }
        public int P_Edad
        {
            get { return Edad; }
            set { Edad = value; }
        }
        public DateTime P_Fecha_Nacimiento
        {
            get { return Fecha_Nacimiento; }
            set { Fecha_Nacimiento = value; }
        }
        public String P_Sexo
        {
            get { return Sexo; }
            set { Sexo = value; }
        }
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
        public String P_Descripcion_Solucion
        {
            get { return Descripcion_Solucion; }
            set { Descripcion_Solucion = value; }
        }
        public String P_Genera_Noticia
        {
            get { return Genera_Noticia; }
            set { Genera_Noticia = value; }
        }

        public String P_Codigo_Postal
        {
            get { return Codigo_Postal; }
            set { Codigo_Postal = value; }
        }

        public String P_Origen
        {
            get { return Origen; }
            set { Origen = value; }
        }

        public String P_Por_Validar
        {
            get { return Por_Validar; }
            set { Por_Validar = value; }
        }

        public String P_Programa_Empleado_ID
        {
            get { return Programa_Empleado_ID; }
            set { Programa_Empleado_ID = value; }
        }

        public String P_Tipo_Consecutivo
        {
            get { return Tipo_Consecutivo; }
            set { Tipo_Consecutivo = value; }
        }

        public int P_Cantidad_Peticiones_Consultar
        {
            get { return Cantidad_Peticiones_Consultar; }
            set { Cantidad_Peticiones_Consultar = value; }
        }

        //
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Descripcion_Cambio
        {
            get { return Descripcion_Cambio; }
            set { Descripcion_Cambio = value; }
        }

        public String P_Fecha_Asignacion_Cambio
        {
            get { return Fecha_Asignacion_Cambio; }
            set { Fecha_Asignacion_Cambio = value; }
        }

        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }
        public String P_Tipo_Solucion
        {
            get { return Tipo_Solucion; }
            set { Tipo_Solucion = value; }
        }
        public String P_Nombre_Atendio
        {
            get { return Nombre_Atendio; }
            set { Nombre_Atendio = value; }
        }
        public String P_Usuario_Creo_Modifico
        {
            get { return Usuario_Creo_Modifico; }
            set { Usuario_Creo_Modifico = value; }
        }
        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }

        public OracleCommand P_Comando_Oracle
        {
            get { return Comando_Oracle; }
            set { Comando_Oracle = value; }
        }
        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value; }
        }
        public String P_Orden_Dinamico
        {
            get { return Orden_Dinamico; }
            set { Orden_Dinamico = value; }
        }

        public DataTable P_Dt_Archivos
        {
            get { return Dt_Archivos; }
            set { Dt_Archivos = value; }
        }

        public List<string> P_Lista_Archivos_Eliminar
        {
            get { return Lista_Archivos_Eliminar; }
            set { Lista_Archivos_Eliminar = value; }
        }

        #endregion

        #region Metodos

        public int Alta_Peticion()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Alta_Peticion(this);
        }
        public int Modificar_Peticion()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Modificar_Peticion(this);
        }
        public DataTable Consulta_Peticion()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Peticion(this);
        }
        public DataTable Consulta_Peticion_Folio()
        {
            return null;
        }
        public DataTable Consulta_Peticion_Bandeja()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Peticion_Bandeja(this);
        }
        public DataTable Consulta_Peticion_Bandeja_No_Asignados()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consultar_Peticion_Bandeja_No_Asignados(this);
        } 
        public int Modificar_Peticion_Reasignacion()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Modificar_Peticion_Reasignacion(this);
        }
        public int Modificar_Peticion_Solucion()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Modificar_Peticion_Solucion(this);
        }
        public int Modificar_Validacion_Solucion()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Modificar_Validacion_Solucion(this);
        }
        public DataTable Consulta_Peticion_Respuesta()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Peticion_Respuesta(this);
        }
        public DataTable Consulta_Peticion_Correo_Solucion()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Peticion_Correo_Solucion(this);
        }
        public DataTable Consulta_Peticion_Seguimiento()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Peticion_Seguimiento(this);
        }
        public DataTable Consulta_Observaciones_Peticion()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Observaciones_Peticion(this);
        }
        public DataTable Consulta_Archivos_Peticion()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Archivos_Peticion(this);
        }
        public string Consulta_Folio()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Folio();
        }
        public DataTable Consulta_Directores_Dependencia()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Directores_Dependencia(this);
        }
        public DataTable Consulta_Dependencias_Con_Asunto()
        {
            return Cls_Cat_Ate_Peticiones_Datos.Consulta_Dependencias_Con_Asunto(this);
        }
#endregion            
        
    }
}
