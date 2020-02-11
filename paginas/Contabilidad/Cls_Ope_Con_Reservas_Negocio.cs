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
using Presidencia.Generar_Reservas.Datos;

/// <summary>
/// Summary description for Cls_Ope_Con_Reservas_Negocio
/// </summary>
/// 

namespace Presidencia.Generar_Reservas.Negocio
{
    public class Cls_Ope_Con_Reservas_Negocio
    {
        #region Variables Privadas
        private String Beneficiario;
        private String Dependencia_ID;
        private String Area_ID;
        private String Proyecto_Programa_ID;
        private String Folio;
        private String Estatus;
        private String Comentarios;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String No_Reserva;
        private String Fuente_Financiamiento;
        private String Importe;
        private String Usuario_Modifico;
        private String Partida_ID;
        private String Anio_Presupuesto;
        #endregion 

        #region Variables Publicas
        public String P_Partida_ID
        {
            get{return Partida_ID;}
            set{Partida_ID = value;}
        }
        public String P_Anio_Presupuesto
        {
            get { return Anio_Presupuesto; }
            set { Anio_Presupuesto = value; }
        }

        public String P_Usuario_Modifico 
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        } 
        public String P_Importe
        {
            get { return Importe; }
            set { Importe = value; }
        }
        public String P_Beneficiario
        {
            get { return Beneficiario; }
            set { Beneficiario = value; }
        }
        public String P_No_Reserva
        {
            get { return No_Reserva; }
            set { No_Reserva = value; }
        }
        public String P_Fuente_Financiamiento
        {
            get { return Fuente_Financiamiento; }
            set { Fuente_Financiamiento = value; }
        }

        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
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

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Proyecto_Programa_ID
        {
            get { return Proyecto_Programa_ID; }
            set { Proyecto_Programa_ID = value; }
        }
        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        #endregion

       
        #region Metodos

        public Cls_Ope_Con_Reservas_Negocio()
        {
        }
        public DataTable Consultar_Proyectos_Programas() 
        {
            return Cls_Ope_Con_Reservas_Datos.Consultar_Proyectos_Programas(this);
        }
        //Se usa
        public DataTable Consultar_Partidas_De_Un_Programa() 
        {
            return Cls_Ope_Con_Reservas_Datos.Consultar_Partidas_De_Un_Programa(this);
        }
        public DataTable Consultar_Reservas()
        {
            return Cls_Ope_Con_Reservas_Datos.Consultar_Reservas(this);
        }

        public DataTable Consultar_Fuentes_Financiamiento()
        {
            return Cls_Ope_Con_Reservas_Datos.Consultar_Fuentes_Financiamiento(this);
        }
        public DataTable Consultar_Presupuesto_Partidas()
        {
            return Cls_Ope_Con_Reservas_Datos.Consultar_Presupuesto_Partidas(this);
        }
        public void Modificar_Reserva()
        {
            Cls_Ope_Con_Reservas_Datos.Modificar_Reserva(this);
        }
        #endregion 
    }
}
