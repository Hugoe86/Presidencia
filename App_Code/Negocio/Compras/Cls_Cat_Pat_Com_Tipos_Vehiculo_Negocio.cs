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
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Negocio {

    public class Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio {

        #region Variables Internas

            private String Tipo_Vehiculo_ID = null;
            private String Descripcion = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;

            private Int32 Vehiculo_Aseguradora_ID = 0;
            private String Aseguradora_ID = null;
            private String Descripcion_Seguro = null;
            private String Cobertura_Seguro = null;
            private String No_Poliza_Seguro = null;

            private String Descripcion_Archivo = null;
            private String Comentarios_Archivo = null;
            private DateTime Fecha ;
            private String Nombre_Fisico_Archivo = null;

            private DataTable Dt_Archivos = null;
            private DataTable Dt_Detalles = null;

        #endregion

        #region Variables Publicas

            public String P_Tipo_Vehiculo_ID {
                get { return Tipo_Vehiculo_ID; }
                set { Tipo_Vehiculo_ID = value; }
            }
            public String P_Descripcion {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public String P_Estatus {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Usuario {
                get { return Usuario; }
                set { Usuario = value; }
            }
            public String P_Tipo_DataTable {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }
            public Int32 P_Vehiculo_Aseguradora_ID
            {
                get { return Vehiculo_Aseguradora_ID; }
                set { Vehiculo_Aseguradora_ID = value; }
            }
            public String P_Aseguradora_ID
            {
                get { return Aseguradora_ID; }
                set { Aseguradora_ID = value; }
            }
            public String P_Descripcion_Seguro
            {
                get { return Descripcion_Seguro; }
                set { Descripcion_Seguro = value; }
            }
            public String P_Cobertura_Seguro
            {
                get { return Cobertura_Seguro; }
                set { Cobertura_Seguro = value; }
            }
            public String P_No_Poliza_Seguro
            {
                get { return No_Poliza_Seguro; }
                set { No_Poliza_Seguro = value; }
            }

            public String P_Descripcion_Archivo
            {
                get { return Descripcion_Archivo; }
                set { Descripcion_Archivo = value; }
            }
            public String P_Comentarios_Archivo
            {
                get { return Comentarios_Archivo; }
                set { Comentarios_Archivo = value; }
            }
            public DateTime P_Fecha
            {
                get { return Fecha; }
                set { Fecha = value; }
            }
            public String P_Nombre_Fisico_Archivo
            {
                get { return Nombre_Fisico_Archivo; }
                set { Nombre_Fisico_Archivo = value; }
            }
            public DataTable P_Dt_Archivos
            {
                get { return Dt_Archivos; }
                set { Dt_Archivos = value; }
            }
            public DataTable P_Dt_Detalles
            {
                get { return Dt_Detalles; }
                set { Dt_Detalles = value; }
            }

        #endregion

        #region Metodos

            public Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Alta_Tipo_Vehiculo() {
                return Cls_Cat_Pat_Com_Tipos_Vehiculo_Datos.Alta_Tipo_Vehiculo(this);
            }

            public Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Modificar_Tipo_Vehiculo() {
                return Cls_Cat_Pat_Com_Tipos_Vehiculo_Datos.Modificar_Tipo_Vehiculo(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Tipos_Vehiculo_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Tipo_Vehiculo() {
                Cls_Cat_Pat_Com_Tipos_Vehiculo_Datos.Eliminar_Tipo_Vehiculo(this);
            }

            public Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Consultar_Datos_Vehiculo() {
                return Cls_Cat_Pat_Com_Tipos_Vehiculo_Datos.Consultar_Datos_Tipos_Vehiculo(this);
            }

        #endregion

    }

}