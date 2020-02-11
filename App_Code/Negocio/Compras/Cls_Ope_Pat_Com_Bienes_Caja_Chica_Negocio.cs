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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Caja_Chica.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Operacion_Bienes_Caja_Chica.Negocio {

    public class Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio {

        #region Variables Internas

            private String Bien_ID = null;
            private String Nombre = null;
            private String Dependencia_ID = null;
            private String Material_ID = null;
            private String Color_ID = null;
            private String Marca_ID = null;
            private String Modelo_ID = null;
            private String Numero_Inventario = null;
            private Double Costo = 0.0;
            private String Estatus = null;
            private String Motivo_Baja = null;
            private String Estado = null;
            private String Comentarios = null;
            private DateTime Fecha_Adquisicion;
            private Int32 Cantidad = -1;

            private DataTable Resguardantes = null;
            private String RFC_Resguardante = null;
            private String Resguardante_ID = null;
            private String Usuario_ID = null;
            private String Usuario_Nombre = null;
            private String Tipo_DataTable = null;
            private Boolean Buscar_Numero_Inventario = false;
            private Boolean Buscar_Fecha_Adquisicion = false;
            private DataTable Historial_Resguardos = null;
            private String Tipo_Filtro_Busqueda = null;

            private String Archivo = null;
            private DataTable Dt_Historial_Archivos = null;
        #endregion

        #region Variables Publicas

            public String P_Bien_ID
            {
                get { return Bien_ID; }
                set { Bien_ID = value; }
            }
            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Material_ID
            {
                get { return Material_ID; }
                set { Material_ID = value; }
            }
            public String P_Color_ID
            {
                get { return Color_ID; }
                set { Color_ID = value; }
            }
            public String P_Marca_ID
            {
                get { return Marca_ID; }
                set { Marca_ID = value; }
            }
            public String P_Modelo_ID
            {
                get { return Modelo_ID; }
                set { Modelo_ID = value; }
            }
            public String P_Numero_Inventario
            {
                get { return Numero_Inventario; }
                set { Numero_Inventario = value; }
            }
            public Double P_Costo
            {
                get { return Costo; }
                set { Costo = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Motivo_Baja
            {
                get { return Motivo_Baja; }
                set { Motivo_Baja = value; }
            }
            public String P_Estado
            {
                get { return Estado; }
                set { Estado = value; }
            }
            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }
            public DataTable P_Resguardantes
            {
                get { return Resguardantes; }
                set { Resguardantes = value; }
            }
            public String P_Usuario_Nombre
            {
                get { return Usuario_Nombre; }
                set { Usuario_Nombre = value; }
            }
            public String P_Usuario_ID
            {
                get { return Usuario_ID; }
                set { Usuario_ID = value; }
            }
            public Int32 P_Cantidad
            {
                get { return Cantidad; }
                set { Cantidad = value; }
            }
            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }
            public DateTime P_Fecha_Adquisicion
            {
                get { return Fecha_Adquisicion; }
                set { Fecha_Adquisicion = value; }
            }
            public Boolean P_Buscar_Numero_Inventario
            {
                get { return Buscar_Numero_Inventario; }
                set { Buscar_Numero_Inventario = value; }
            }
            public Boolean P_Buscar_Fecha_Adquisicion
            {
                get { return Buscar_Fecha_Adquisicion; }
                set { Buscar_Fecha_Adquisicion = value; }
            }
            public DataTable P_Historial_Resguardos
            {
                get { return Historial_Resguardos; }
                set { Historial_Resguardos = value; }
            }
            public String P_Tipo_Filtro_Busqueda
            {
                get { return Tipo_Filtro_Busqueda; }
                set { Tipo_Filtro_Busqueda = value; }
            }
            public String P_RFC_Resguardante
            {
                get { return RFC_Resguardante; }
                set { RFC_Resguardante = value; }
            }
            public String P_Resguardante_ID
            {
                get { return Resguardante_ID; }
                set { Resguardante_ID = value; }
            }

            public String P_Archivo
            {
                get { return Archivo; }
                set { Archivo = value; }
            }

            public DataTable P_Dt_Historial_Archivos
            {
                get { return Dt_Historial_Archivos; }
                set { Dt_Historial_Archivos = value; }
            }

        #endregion

        #region Metodos

            public Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Alta_Bien_Caja_Chica() {
                return Cls_Ope_Pat_Com_Bienes_Caja_Chica_Datos.Alta_Bien_Caja_Chica(this);
            }

            public void Modificar_Bien_Caja_Chica() {
                Cls_Ope_Pat_Com_Bienes_Caja_Chica_Datos.Modificar_Bien_Caja_Chica(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Ope_Pat_Com_Bienes_Caja_Chica_Datos.Consultar_DataTable(this);
            }

            public Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Consultar_Datos_Bien_Caja_Chica() {
                return Cls_Ope_Pat_Com_Bienes_Caja_Chica_Datos.Consultar_Datos_Bien(this);
            }

        #endregion

    }

}