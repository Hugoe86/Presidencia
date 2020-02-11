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
using Presidencia.Predial_Pae_Bienes.Datos;
using System.Data.OracleClient;
using System.Collections.Generic;

namespace Presidencia.Predial_Pae_Bienes.Negocio
{
    public class Cls_Ope_Pre_Pae_Bienes_Negocio
    {
        #region Variables Internas
        //Para la tabla Bienes
        private String No_Bien;
        private String No_Detalle_Etapa;
        private String Tipo_Bien_ID;
        private String Descripcion;
        private String Valor_Bien;
        private String Nombre_Usuario;

        //Para la tabla de Peritajes
        private String No_Peritaje;
        private String Avaluo;
        private DateTime Fecha_Peritaje;
        private String Perito;
        private String Valor_Peritaje;
        private String Observaciones;
        private String Lugar_Almacenamiento = "";
        private String Costo_Metro_Cuadrado = "";
        private String Dimensiones = "";
        private DateTime Fecha_Ingreso;
        private String Tiempo_Transcurrido;
        private String Costo_Almacenamiento;

        //Para la tabla de Depositario
        private String No_Cambio_Depositario;
        private String Figura;
        private String Nombre_Depositario;
        private String Domicilio_Depositario;
        private DateTime Fecha_Remocion;

        private String Filtro;
        private String Campos_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private String Proceso_Actual;
        private String Despacho_ID;
        private String Estatus_Etapa = "";
        private String Cuenta_Predial;
        private String Contribuyente_ID;
        private String Etapa_Omitida;
        private int Folio_Inicial = 0;
        private int Folio_Final = 0;
        private DateTime Fecha_Inicial;
        private DateTime Fecha_Final;

        DataTable Dt_Bienes;
        DataTable Dt_Fotografias;
        List<string> Bienes_Eliminar;

        private OracleCommand Comando_Transaccion;
        #endregion

        #region Variables Publicas

        public String P_No_Bien
        {
            get { return No_Bien; }
            set { No_Bien = value; }
        }
        public String P_No_Detalle_Etapa
        {
            get { return No_Detalle_Etapa; }
            set { No_Detalle_Etapa = value; }
        }
        public String P_Tipo_Bien_ID
        {
            get { return Tipo_Bien_ID; }
            set { Tipo_Bien_ID = value; }
        }
        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }
        public String P_Valor_Bien
        {
            get { return Valor_Bien; }
            set { Valor_Bien = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        public String P_Lugar_Almacenamiento
        {
            get { return Lugar_Almacenamiento; }
            set { Lugar_Almacenamiento = value; }
        }
        public String P_Costo_Metro_Cuadrado
        {
            get { return Costo_Metro_Cuadrado; }
            set { Costo_Metro_Cuadrado = value; }
        }
        public String P_Dimensiones
        {
            get { return Dimensiones; }
            set { Dimensiones = value; }
        }
        public DateTime P_Fecha_Ingreso
        {
            get { return Fecha_Ingreso; }
            set { Fecha_Ingreso = value; }
        }
        public String P_Tiempo_Transcurrido
        {
            get { return Tiempo_Transcurrido; }
            set { Tiempo_Transcurrido = value; }
        }
        public String P_Costo_Almacenamiento
        {
            get { return Costo_Almacenamiento; }
            set { Costo_Almacenamiento = value; }
        }
        public String P_No_Peritaje
        {
            get { return No_Peritaje; }
            set { No_Peritaje = value; }
        }
        public String P_Avaluo
        {
            get { return Avaluo; }
            set { Avaluo = value; }
        }
        public DateTime P_Fecha_Peritaje
        {
            get { return Fecha_Peritaje; }
            set { Fecha_Peritaje = value; }
        }
        public String P_Perito
        {
            get { return Perito; }
            set { Perito = value; }
        }
        public String P_Valor_Peritaje
        {
            get { return Valor_Peritaje; }
            set { Valor_Peritaje = value; }
        }
        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }
        public String P_Filtro
        {
            get { return Filtro; }
            set { Filtro = value; }
        }
        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value; }
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
        public String P_Proceso_Actual
        {
            get { return Proceso_Actual; }
            set { Proceso_Actual = value; }
        }
        public String P_Despacho_ID
        {
            get { return Despacho_ID; }
            set { Despacho_ID = value; }
        }
        public String P_Estatus_Etapa
        {
            get { return Estatus_Etapa; }
            set { Estatus_Etapa = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Contribuyente_ID
        {
            get { return Contribuyente_ID; }
            set { Contribuyente_ID = value; }
        }
        public String P_Etapa_Omitida
        {
            get { return Etapa_Omitida; }
            set { Etapa_Omitida = value; }
        }
        public int P_Folio_Inicial
        {
            get { return Folio_Inicial; }
            set { Folio_Inicial = value; }
        }
        public int P_Folio_Final
        {
            get { return Folio_Final; }
            set { Folio_Final = value; }
        }
        public DateTime P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public DateTime P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }

        public String P_No_Depositario
        {
            get { return No_Cambio_Depositario; }
            set { No_Cambio_Depositario = value; }
        }
        public String P_Figura
        {
            get { return Figura; }
            set { Figura = value; }
        }
        public String P_Nombre_Depositario
        {
            get { return Nombre_Depositario; }
            set { Nombre_Depositario = value; }
        }
        public String P_Domicilio_Depositario
        {
            get { return Domicilio_Depositario; }
            set { Domicilio_Depositario = value; }
        }
        public DateTime P_Fecha_Remocion
        {
            get { return Fecha_Remocion; }
            set { Fecha_Remocion = value; }
        }
        public DataTable P_Dt_Bienes
        {
            get { return Dt_Bienes; }
            set { Dt_Bienes = value; }
        }
        public DataTable P_Dt_Fotografias
        {
            get { return Dt_Fotografias; }
            set { Dt_Fotografias = value; }
        }
        public List<string> P_Bienes_Eliminar
        {
            get { return Bienes_Eliminar; }
            set { Bienes_Eliminar = value; }
        }
        public OracleCommand P_Comando_Transaccion
        {
            get { return Comando_Transaccion; }
            set { Comando_Transaccion = value; }
        }
        #endregion
        #region Metodos
        public void Alta_Pae_Bienes()
        {
            Cls_Ope_Pre_Pae_Bienes_Datos.Alta_Pae_Bienes(this);
        }
        public int Alta_Pae_Peritajes()
        {
            return Cls_Ope_Pre_Pae_Bienes_Datos.Alta_Pae_Peritajes(this);
        }
        public int Modificar_Peritajes()
        {
            return Cls_Ope_Pre_Pae_Bienes_Datos.Modificar_Peritajes(this);
        }
        public void Alta_Depositario()
        {
            Cls_Ope_Pre_Pae_Bienes_Datos.Alta_Pae_Det_Etapas_Depositario(this);
        }
        public DataTable Consulta_Bienes_Peritajes()
        {
            return Cls_Ope_Pre_Pae_Bienes_Datos.Consulta_Bienes_Peritajes(this);
        }
        public DataTable Consulta_Depositarios()
        {
            return Cls_Ope_Pre_Pae_Bienes_Datos.Consulta_Depositarios(this);
        }
        public DataTable Consulta_Bienes()
        {
            return Cls_Ope_Pre_Pae_Bienes_Datos.Consulta_Bienes(this);
        }
        public DataTable Consulta_Peritajes()
        {
            return Cls_Ope_Pre_Pae_Bienes_Datos.Consulta_Peritajes(this);
        }
        public DataTable Consultar_Detalles_Etapas()
        {
            return Cls_Ope_Pre_Pae_Bienes_Datos.Consultar_Detalles_Etapas(this);
        }
        #endregion
    }
}
