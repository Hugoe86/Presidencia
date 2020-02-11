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
using Presidencia.Ope_Psp_Asignar_Partida.Datos;

namespace Presidencia.Ope_Psp_Asignar_Partida.Negocio 
{ 
    public class Cls_Ope_Psp_Asignar_Partida_Negocio
    {
        #region VARIABLES INTERNAS
        private String Dependencia_ID;
        private String Capitulo_ID;
        private String Partida_ID;
        private String Producto_ID;
        private DataTable Dt_Datos;
        private String Fuente_Financiamiento_ID;
        private String Programa_ID;
        private String Anio_Presupuesto;
        private String Total;
        private String Clave_Producto;
        private String Nombre_Producto;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Estatus;
        private String Comentario;

        #endregion

        #region VARIABLES PUBLICAS

        //get y set de P_Dependencia_ID
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        //get y set de P_Capitulo_ID
        public String P_Capitulo_ID
        {
            get { return Capitulo_ID; }
            set { Capitulo_ID = value; }
        }

        //get y set de P_Partida_ID
        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }

        //get y set de P_Dt_Datos
        public DataTable P_Dt_Datos
        {
            get { return Dt_Datos; }
            set { Dt_Datos = value; }
        }

        //get y set de P_Producto_ID
        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
        }

        //get y set de P_Fuente_Financiamiento_ID
        public String P_Fuente_Financiamiento_ID
        {
            get { return Fuente_Financiamiento_ID; }
            set { Fuente_Financiamiento_ID = value; }
        }

        //get y set de P_Programa_ID
        public String P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }

        //get y set de P_Anio_Presupuesto
        public String P_Anio_Presupuesto
        {
            get { return Anio_Presupuesto; }
            set { Anio_Presupuesto = value; }
        }

        //get y set de P_Total
        public String P_Total
        {
            get { return Total; }
            set { Total = value; }
        }

        //get y set de P_Clave_Producto
        public String P_Clave_Producto
        {
            get { return Clave_Producto; }
            set { Clave_Producto = value; }
        }

        //get y set de P_Nombre_Producto
        public String P_Nombre_Producto
        {
            get { return Nombre_Producto; }
            set { Nombre_Producto = value; }
        }

        //get y set de P_Estatus
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        //get y set de P_Usuario_Creo
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        //get y set de P_Usuario_Modifico
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        //get y set de P_Comentario
        public String P_Comentario
        {
            get { return Comentario; }
            set { Comentario = value; }
        }
        #endregion

        #region MÉTODOS
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Unidad_Responsable
        ///DESCRIPCIÓN          : Metodo para obtener los datos de las unidades responsables
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011 
        ///*********************************************************************************************************
        public DataTable Consultar_Unidad_Responsable()
        {
            return Cls_Ope_Psp_Asignar_Partida_Datos.Consultar_Unidad_Responsable();
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Partidas
        ///DESCRIPCIÓN          : Metodo para obtener los datos de los capitulos
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011 
        ///*********************************************************************************************************
        public DataTable Consultar_Capitulos()
        {
            return Cls_Ope_Psp_Asignar_Partida_Datos.Consultar_Capitulos();
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Estatus_Partidas
        ///DESCRIPCIÓN          : Metodo para obtener el estatus del las partidas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011 
        ///*********************************************************************************************************
        public DataTable Consultar_Estatus_Partidas()
        {
            return Cls_Ope_Psp_Asignar_Partida_Datos.Consultar_Estatus_Partidas(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Partidas_Asignadas
        ///DESCRIPCIÓN          : Metodo para guardar los datos de las partidas asignadas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011 
        ///*********************************************************************************************************
        public Boolean Guardar_Partidas_Asignadas()
        {
            return Cls_Ope_Psp_Asignar_Partida_Datos.Guardar_Partidas_Asignadas(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Partidas_Asignadas
        ///DESCRIPCIÓN          : Metodo para obtener los datos de las partidas asignadas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011 
        ///*********************************************************************************************************
        public DataTable Consultar_Partidas_Asignadas()
        {
            return Cls_Ope_Psp_Asignar_Partida_Datos.Consultar_Partida_Asignadas(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Partidas_Asignadas
        ///DESCRIPCIÓN          : Metodo para modificar los datos de las partidas asignadas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011 
        ///*********************************************************************************************************
        public Boolean Modificar_Partidas_Asignadas()
        {
            return Cls_Ope_Psp_Asignar_Partida_Datos.Modificar_Partida_Asignadas(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Partidas_Autorizadas
        ///DESCRIPCIÓN          : Metodo para obtener los datos de las partidas autorizadas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 25/Noviembre/2011 
        ///*********************************************************************************************************
        public DataTable Consultar_Partidas_Autorizadas()
        {
            return Cls_Ope_Psp_Asignar_Partida_Datos.Consultar_Partida_Autorizadas(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Dependencias_Presupuestadas
        ///DESCRIPCIÓN          : Metodo para obtener los datos de las dependencias presupuestadas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 28/Noviembre/2011 
        ///*********************************************************************************************************
        public DataTable Consultar_Dependencias_Presupuestadas()
        {
            return Cls_Ope_Psp_Asignar_Partida_Datos.Consultar_Dependencias_Presupuestadas();
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Historial_Calendario
        ///DESCRIPCIÓN          : Metodo para guardar los comentarios del presupuesto
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 29/Noviembre/2011 
        ///*********************************************************************************************************
        public Boolean Guardar_Historial_Calendario()
        {
            return Cls_Ope_Psp_Asignar_Partida_Datos.Guardar_Historial_Calendario(this);
        }
        #endregion
    }
}
