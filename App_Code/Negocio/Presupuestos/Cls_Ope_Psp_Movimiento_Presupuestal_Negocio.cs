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
using Presidencia.Movimiento_Presupuestal.Datos;

namespace Presidencia.Movimiento_Presupuestal.Negocio
{
    public class Cls_Ope_Psp_Movimiento_Presupuestal_Negocio
    {
        #region(Variables Privadas)
            private String No_Solicitud;
            private String Codigo_Programatico_De;//es el codigo Origen
            private String Codigo_Programatico_Al;//Es el codigo Destino
            private String Fuente_Financiera;
            private String Area_Funcional;
            private String Programa;
            private String Responsable;
            private String Partida;
            private String Monto;
            private String Justificacion;
            private String Estatus;
            private String Usuario_Creo;
            private String Tipo_Operacion;
            private String Origen_Fuente_Financiamiento_Id;
            private String Destino_Fuente_Financiamiento_Id;
            private String Origen_Area_Funcional_Id;
            private String Destino_Area_Funcional_Id;
            private String Origen_Programa_Id;
            private String Destino_Programa_Id;
            private String Origen_Partida_Id;
            private String Destino_Partida_Id;
            private String Origen_Dependencia_Id;
            private String Destino_Dependencia_Id;
            private String Comentario;
            private String Fecha_Inicio;
            private String Fecha_Final;
           
        #endregion

        #region(Variables Publicas)
            public String P_No_Solicitud
            {
                get { return No_Solicitud; }
                set { No_Solicitud = value; }
            }
            public String P_Codigo_Programatico_De
            {
                get { return Codigo_Programatico_De; }
                set { Codigo_Programatico_De = value; }
            }
            public String P_Codigo_Programatico_Al
            {
                get { return Codigo_Programatico_Al; }
                set { Codigo_Programatico_Al = value; }
            }
            public String P_Fuente_Financiera
            {
                get { return Fuente_Financiera; }
                set { Fuente_Financiera = value; }
            }
            public String P_Area_Funcional
            {
                get { return Area_Funcional; }
                set { Area_Funcional = value; }
            }
            public String P_Programa
            {
                get { return Programa; }
                set { Programa = value; }
            }
            public String P_Responsable
            {
                get { return Responsable; }
                set { Responsable = value; }
            }
            public String P_Partida
            {
                get { return Partida; }
                set { Partida = value; }
            }
            public String P_Monto
            {
                get { return Monto; }
                set { Monto = value; }
            }
            public String P_Justificacion
            {
                get { return Justificacion; }
                set { Justificacion = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public String P_Tipo_Operacion
            {
                get { return Tipo_Operacion; }
                set { Tipo_Operacion = value; }
            }
            public String P_Origen_Fuente_Financiamiento_Id
            {
                get { return Origen_Fuente_Financiamiento_Id; }
                set { Origen_Fuente_Financiamiento_Id = value; }
            }
            public String P_Destino_Fuente_Financiamiento_Id
            {
                get { return Destino_Fuente_Financiamiento_Id; }
                set { Destino_Fuente_Financiamiento_Id = value; }
            }
            public String P_Destino_Area_Funcional_Id
            {
                get { return Destino_Area_Funcional_Id; }
                set { Destino_Area_Funcional_Id = value; }
            }
            public String P_Origen_Area_Funcional_Id
            {
                get { return Origen_Area_Funcional_Id; }
                set { Origen_Area_Funcional_Id = value; }
            }
            public String P_Origen_Programa_Id
            {
                get { return Origen_Programa_Id; }
                set { Origen_Programa_Id = value; }
            }
            public String P_Destino_Programa_Id
            {
                get { return Destino_Programa_Id; }
                set { Destino_Programa_Id = value; }
            }
            public String P_Origen_Partida_Id
            {
                get { return Origen_Partida_Id; }
                set { Origen_Partida_Id = value; }
            }
            public String P_Destino_Partida_Id
            {
                get { return Destino_Partida_Id; }
                set { Destino_Partida_Id = value; }
            }
            public String P_Origen_Dependencia_Id
            {
                get { return Origen_Dependencia_Id; }
                set { Origen_Dependencia_Id = value; }
            }
            public String P_Destino_Dependencia_Id
            {
                get { return Destino_Dependencia_Id; }
                set { Destino_Dependencia_Id = value; }
            }
            public String P_Comentario
            {
                get { return Comentario; }
                set { Comentario = value; }
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
        #endregion

        #region(Metodos)
            public Boolean Alta_Movimiento()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Alta_Movimiento(this);
            }
            public Boolean Alta_Comentario()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Alta_Comentario(this);
            }
            public Boolean Modificar_Movimiento()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Modificar_Movimiento(this);
            }
            
            public Boolean Eliminar_Movimiento()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Eliminar_Movimiento(this);
            }
            public DataTable Consulta_Movimiento()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Consultar_Movimiento(this);
            }
            public DataTable Consulta_Movimiento_Fecha()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Consulta_Movimiento_Fecha(this);
            }
            public DataTable Consultar_Programa()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Consultar_Programa(this);
            }
            public DataTable Consultar_Area_Funciona()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Consultar_Area_Funcional(this);
            }

            public DataTable Consultar_Like_Movimiento()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Consultar_Like_Movimiento(this);
            }
            public DataTable Consultar_Dependencia_Ordenada()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Consultar_Dependencia_Ordenada(this);
            }
            public DataTable Consulta_Datos_Partidas()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Consulta_Datos_Partidas(this);
            }
            public DataTable Consulta_Datos_Comentarios()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Consulta_Datos_Comentarios(this);
            }

            public DataTable Consulta_Movimiento_Btn_Busqueda()
            {
                return Cls_Ope_Psp_Movimiento_Presupuestal_Datos.Consulta_Movimiento_Btn_Busqueda(this);
            }
        #endregion


    }
}