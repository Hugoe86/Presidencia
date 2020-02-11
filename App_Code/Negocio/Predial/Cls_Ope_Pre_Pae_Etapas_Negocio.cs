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
using Presidencia.Predial_Pae_Etapas.Datos;

namespace Presidencia.Predial_Pae_Etapas.Negocio
{
    public class Cls_Ope_Pre_Pae_Etapas_Negocio
    {
        #region Variables Internas

        //Para la tabla Etapas
        private String No_Etapa;
        private String Despacho_Id;
        private String Numero_Entrega;
        private String Total_Etapa;
        private String Modo_Generacion;
        private String Nombre_Archivo;
        private String Comentario;

        //Detalle etapas
        private String No_Detalle_Etapa;
        private String Cuenta_Predial_Id;
        private String Periodo_Corriente;
        private String Adeudo_Corriente;
        private String Periodo_Rezago;
        private String Adeudo_Rezago;
        private String Adeudo_Recargos_Ordinarios;
        private String Adeudo_Recargos_Moratorios;
        private String Adeudo_Honorarios;
        private String Multas;
        private String Adeudo_Total;
        private String Proceso_Actual;
        private String Estatus;
        private String Omitida;
        private String Motivo_Omision;
        private String Folio;
        private String Impresa;
        private String Fecha_Hora_Notificacion;
        private String Motivo_Cambio_Estatus;
        private String Resolucion;
        private String Fecha_Creo_Ini;
        private String Fecha_Creo_Fin;


        //Detalles cuentas
        private String No_Detalle_Cuenta;
        private String Fecha_Proceso_Cambio;
        private String Proceso_Anterior;
        private String No_Almoneda;

        //Impresiones
        private String No_Impresion;
        private String Proceso;
        private String Total_Proceso;

        private String Filtro;
        private String Campos_Dinamicos;
        private String Agrupar_Dinamico;

        private DataTable Dt_Omitidas;
        private DataTable Dt_Generadas;

        //Este campo no esta en las tablas pero sirver para cuando se unen las tablas consultar la cuenta predial
        private String Cuenta_Predial;
        private String Contribuyente_Id;
        private String Folio_Inicial;
        private String Folio_Final;
        private String Tipo_Predio;
        private String Colonia_ID;
        private String Calle_ID;
        private String Colonia_ID_Notificacion;
        private String Calle_ID_Notificacion;

        #endregion

        #region Variables Publicas

        public String P_No_Etapa
        {
            get { return No_Etapa; }
            set { No_Etapa = value; }
        }

        public String P_Despacho_Id
        {
            get { return Despacho_Id; }
            set { Despacho_Id = value; }
        }

        public String P_Numero_Entrega
        {
            get { return Numero_Entrega; }
            set { Numero_Entrega = value; }
        }
        public String P_Total_Etapa
        {
            get { return Total_Etapa; }
            set { Total_Etapa = value; }
        }

        public String P_Modo_Generacion
        {
            get { return Modo_Generacion; }
            set { Modo_Generacion = value; }
        }
        public String P_Nombre_Archivo
        {
            get { return Nombre_Archivo; }
            set { Nombre_Archivo = value; }
        }

        public String P_Comentario
        {
            get { return Comentario; }
            set { Comentario = value; }
        }
        public String P_No_Detalle_Etapa
        {
            get { return No_Detalle_Etapa; }
            set { No_Detalle_Etapa = value; }
        }

        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }
        public String P_Periodo_Corriente
        {
            get { return Periodo_Corriente; }
            set { Periodo_Corriente = value; }
        }

        public String P_Adeudo_Corriente
        {
            get { return Adeudo_Corriente; }
            set { Adeudo_Corriente = value; }
        }
        public String P_Periodo_Rezago
        {
            get { return Periodo_Rezago; }
            set { Periodo_Rezago = value; }
        }
        public String P_Adeudo_Rezago
        {
            get { return Adeudo_Rezago; }
            set { Adeudo_Rezago = value; }
        }

        public String P_Adeudo_Recargos_Ordinarios
        {
            get { return Adeudo_Recargos_Ordinarios; }
            set { Adeudo_Recargos_Ordinarios = value; }
        }
        public String P_Adeudo_Recargos_Moratorios
        {
            get { return Adeudo_Recargos_Moratorios; }
            set { Adeudo_Recargos_Moratorios = value; }
        }

        public String P_Adeudo_Honorarios
        {
            get { return Adeudo_Honorarios; }
            set { Adeudo_Honorarios = value; }
        }
        public String P_Multas
        {
            get { return Multas; }
            set { Multas = value; }
        }

        public String P_Adeudo_Total
        {
            get { return Adeudo_Total; }
            set { Adeudo_Total = value; }
        }
        public String P_Proceso_Actual
        {
            get { return Proceso_Actual; }
            set { Proceso_Actual = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Omitida
        {
            get { return Omitida; }
            set { Omitida = value; }
        }

        public String P_Motivo_Omision
        {
            get { return Motivo_Omision; }
            set { Motivo_Omision = value; }
        }
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }

        public String P_Impresa
        {
            get { return Impresa; }
            set { Impresa = value; }
        }
        public String P_No_Detalle_Cuenta
        {
            get { return No_Detalle_Cuenta; }
            set { No_Detalle_Cuenta = value; }
        }
        public String P_Fecha_Proceso_Cambio
        {
            get { return Fecha_Proceso_Cambio; }
            set { Fecha_Proceso_Cambio = value; }
        }

        public String P_Proceso_Anterior
        {
            get { return Proceso_Anterior; }
            set { Proceso_Anterior = value; }
        }
        public String P_No_Impresion
        {
            get { return No_Impresion; }
            set { No_Impresion = value; }
        }

        public String P_Proceso
        {
            get { return Proceso; }
            set { Proceso = value; }
        }
        public String P_Total_Proceso
        {
            get { return Total_Proceso; }
            set { Total_Proceso = value; }
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
        public DataTable P_Dt_Omitidas
        {
            get { return Dt_Omitidas; }
            set { Dt_Omitidas = value; }
        }
        public DataTable P_Dt_Generadas
        {
            get { return Dt_Generadas; }
            set { Dt_Generadas = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }

        public String P_No_Almoneda
        {
            get { return No_Almoneda; }
            set { No_Almoneda = value; }
        }
        public String P_Contribuyente_Id
        {
            get { return Contribuyente_Id; }
            set { Contribuyente_Id = value; }
        }
        public String P_Fecha_Hora_Notificacion
        {
            get { return Fecha_Hora_Notificacion; }
            set { Fecha_Hora_Notificacion = value; }
        }
        public String P_Folio_Inicial
        {
            get { return Folio_Inicial; }
            set { Folio_Inicial = value; }
        }
        public String P_Folio_Final
        {
            get { return Folio_Final; }
            set { Folio_Final = value; }
        }
        public String P_Motivo_Cambio_Estatus
        {
            get { return Motivo_Cambio_Estatus; }
            set { Motivo_Cambio_Estatus = value; }
        }
        public String P_Resolucion
        {
            get { return Resolucion; }
            set { Resolucion = value; }
        }
        public String P_Tipo_Predio
        {
            get { return Tipo_Predio; }
            set { Tipo_Predio = value; }
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
        public String P_Colonia_ID_Notificacion
        {
            get { return Colonia_ID_Notificacion; }
            set { Colonia_ID_Notificacion = value; }
        }
        public String P_Calle_ID_Notificacion
        {
            get { return Calle_ID_Notificacion; }
            set { Calle_ID_Notificacion = value; }
        }
        public String P_Fecha_Creo_Ini
        {
            get { return Fecha_Creo_Ini; }
            set { Fecha_Creo_Ini = value; }
        }
        public String P_Fecha_Creo_Fin
        {
            get { return Fecha_Creo_Fin; }
            set { Fecha_Creo_Fin = value; }
        }
        
        #endregion

        #region Metodos
        public void Alta_Pae_Etapas()
        {
            Cls_Ope_Pre_Pae_Etapas_Datos.Alta_Pae_Etapas(this);
        }
        public void Alta_Pae_Detalles_Cuentas(string No_Detalle_Etapa)
        {
            Cls_Ope_Pre_Pae_Etapas_Datos.Alta_Pae_Detalles_Cuentas(No_Detalle_Etapa, this);
        }
        public String Consultar_No_Entrega(String Año)
        {
            return Cls_Ope_Pre_Pae_Etapas_Datos.Consultar_No_Entrega(this, Año);
        }
        public DataTable Consultar_Pae_Det_Etapas()
        {
            return Cls_Ope_Pre_Pae_Etapas_Datos.Consultar_Pae_Det_Etapas(this);
        }
        public DataTable Consultar_Pae_Etapas()
        {
            return Cls_Ope_Pre_Pae_Etapas_Datos.Consultar_Pae_Etapas(this);
        }
        public Boolean Actualiza_Pae_Det_Etapas()
        {
            return Cls_Ope_Pre_Pae_Etapas_Datos.Actualiza_Pae_Det_Etapas(this);
        }
        public Boolean Actualiza_Pae_Detalle_Cuenta()
        {
            return Cls_Ope_Pre_Pae_Etapas_Datos.Actualiza_Pae_Detalle_Cuentas(this);
        }
        public DataTable Consultar_Contribuyente_Etapas_Pae()
        {
            return Cls_Ope_Pre_Pae_Etapas_Datos.Consultar_Contribuyente_Etapas_Pae(this);
        }
        public DataTable Consulta_Cuentas_Honorarios()
        {
            return Cls_Ope_Pre_Pae_Etapas_Datos.Consulta_Cuentas_Honorarios(this);
        }
        public DataTable Consulta_Reporte_Impresas()
        {
            return Cls_Ope_Pre_Pae_Etapas_Datos.Consulta_Reporte_Impresas(this);
        }
        #endregion
    }
}