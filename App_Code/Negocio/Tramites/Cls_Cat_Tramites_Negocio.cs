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
using Presidencia.Catalogo_Tramites.Datos;

/// <summary>
/// Summary description for Cls_Cat_Tramites_Negocio
/// </summary>

namespace Presidencia.Catalogo_Tramites.Negocio
{

    public class Cls_Cat_Tramites_Negocio
    {

        #region   Variables Internas

            private String Tramite_ID;
            private String Dependencia_ID;
            private String Cuenta_ID;
            private String Clave_Tramite;
            private String Nombre;
            private String Tipo;
            private String Descripcion;
            private Int32 Tiempo_Estimado;
            private String Solicitar_Intenet;
            private Double Costo;
            private String Usuario;
            private DataTable Perfiles_Autorizar;
            private DataTable Datos_Tramite;
            private DataTable Documentacion_Tramite;
            private DataTable SubProcesos_Tramite;
            private DataTable Dt_Detalle_Plantilla;
            private DataTable Dt_Detalle_Formato; 
            private DataTable Matriz_Costo;
            private String Tipo_DataTable;
            private String Tipo_Actividad;
            private String Sub_Proceso_ID;
            private String Plantilla_ID;
            private String Cuenta; 
            private String Nombre_Cuenta; 
            private String Cuenta_Contable_ID;
            private String Cuenta_Contable_Clave;
            private String Area_Dependencia;
            private String Estatus_Tramite;

            private String Parametro1;
            private String Parametro2;
            private String Parametro3;
            private String Operador1;
            private String Operador2;
            private String Operador3;

        #endregion

        #region   Variables Publicas

            public String P_Tramite_ID
            {
                get { return Tramite_ID; }
                set { Tramite_ID = value; }
            }

            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }

            public String P_Cuenta_ID
            {
                get { return Cuenta_ID; }
                set { Cuenta_ID = value; }
            }

            public String P_Clave_Tramite
            {
                get { return Clave_Tramite; }
                set { Clave_Tramite = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public String P_Tipo
            {
                get { return Tipo; }
                set { Tipo = value; }
            }

            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }

            public Int32 P_Tiempo_Estimado
            {
                get { return Tiempo_Estimado; }
                set { Tiempo_Estimado = value; }
            }

            public Double P_Costo
            {
                get { return Costo; }
                set { Costo = value; }
            }

            public String P_Solicitar_Intenet
            {
                get { return Solicitar_Intenet; }
                set { Solicitar_Intenet = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public DataTable P_Perfiles_Autorizar
            {
                get { return Perfiles_Autorizar; }
                set { Perfiles_Autorizar = value; }
            }

            public DataTable P_Datos_Tramite
            {
                get { return Datos_Tramite; }
                set { Datos_Tramite = value; }
            }
            public DataTable P_Matriz_Costo
            {
                get { return Matriz_Costo; }
                set { Matriz_Costo = value; }
            }
            public DataTable P_Documentacion_Tramite
            {
                get { return Documentacion_Tramite; }
                set { Documentacion_Tramite = value; }
            }

            public DataTable P_SubProcesos_Tramite
            {
                get { return SubProcesos_Tramite; }
                set { SubProcesos_Tramite = value; }
            }

            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }

            public DataTable P_Dt_Detalle_Plantilla
            {
                get { return Dt_Detalle_Plantilla; }
                set { Dt_Detalle_Plantilla = value; }
            }

            public DataTable P_Dt_Detalle_Formato
            {
                get { return Dt_Detalle_Formato; }
                set { Dt_Detalle_Formato = value; }
            }

            public String P_Tipo_Actividad
            {
                get { return Tipo_Actividad; }
                set { Tipo_Actividad = value; }
            }
            public String P_Sub_Proceso_ID
            {
                get { return Sub_Proceso_ID; }
                set { Sub_Proceso_ID = value; }
            }

            public String P_Plantilla_ID
            {
                get { return Plantilla_ID; }
                set { Plantilla_ID = value; }
            }

            public String P_Cuenta
            {
                get { return Cuenta; }
                set { Cuenta = value; }
            }
            public String P_Nombre_Cuenta
            {
                get { return Nombre_Cuenta; }
                set { Nombre_Cuenta = value; }
            }
            public String P_Cuenta_Contable_ID
            {
                get { return Cuenta_Contable_ID; }
                set { Cuenta_Contable_ID = value; }
            }

            public String P_Cuenta_Contable_Clave
            {
                get { return Cuenta_Contable_Clave; }
                set { Cuenta_Contable_Clave = value; }
            }
            public String P_Area_Dependencia
            {
                get { return Area_Dependencia; }
                set { Area_Dependencia = value; }
            }
            public String P_Estatus_Tramite
            {
                get { return Estatus_Tramite; }
                set { Estatus_Tramite = value; }
            }

            public String P_Parametro1
            {
                get { return Parametro1; }
                set { Parametro1 = value; }
            }
            public String P_Parametro2
            {
                get { return Parametro2; }
                set { Parametro2 = value; }
            }
            public String P_Parametro3
            {
                get { return Parametro3; }
                set { Parametro3 = value; }
            }
            public String P_Operador1
            {
                get { return Operador1; }
                set { Operador1 = value; }
            }
            public String P_Operador2
            {
                get { return Operador2; }
                set { Operador2 = value; }
            }
            public String P_Operador3
            {
                get { return Operador3; }
                set { Operador3 = value; }
            }

        #endregion

        #region   Metodos

            public void Alta_Tramite()
            {
                Cls_Cat_Tramites_Datos.Alta_Tramite(this);
            }

            public void Modificar_Tramite()
            {
                Cls_Cat_Tramites_Datos.Modificar_Tramite(this);
            }

            public void Eliminar_Tramite()
            {
                Cls_Cat_Tramites_Datos.Eliminar_Tramite(this);
            }

            public DataTable Consultar_DataTable()
            {
                return Cls_Cat_Tramites_Datos.Consultar_DataTable(this);
            }

            public Cls_Cat_Tramites_Negocio Consultar_Datos_Tramite()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Datos_Tramite(this);
            }

            public DataTable Consultar_Tabla_Tramite()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Tabla_Tramite(this);
            }

            public void Dar_Baja_Tramite(String Tramite_ID)
            {
                Cls_Cat_Tramites_Datos.Dar_Baja_Tramite(Tramite_ID);
            }    

            public DataTable Consultar_Avance()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Avance(this);
            }

            public DataTable Consultar_Clave_Repetida()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Clave_Repetida(this);
            }

            public DataTable Consultar_Detalles_Plantilla()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Detalles_Plantilla(this);
            }


            public DataTable Consultar_Detalles_Formato()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Detalles_Formato(this);
            }


            public DataTable Consultar_Tipo_Actividad()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Tipo_Actividad(this);
            }

            public DataTable Consultar_Subprocesos_Tramite()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Subprocesos_Tramite(this);
            }


            public void Eliminar_Detalles_Plantillas_Formato()
            {
                Cls_Cat_Tramites_Datos.Eliminar_Detalles_Plantillas_Formato(this);
            }

            public void Modificar_Detalles_Plantillas_Formato()
            {
                Cls_Cat_Tramites_Datos.Modificar_Detalles_Plantillas_Formato(this);
            }
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Cuenta_Contable
            ///DESCRIPCIÓN          : Metodo para consultar los cuentas de ingresos
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 31/Mayo/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Cuenta_Contable()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Cuenta(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Plantillas
            ///DESCRIPCIÓN          : Metodo para consultar los cuentas de ingresos
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 19/Junio/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Plantillas()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Plantillas(this);
            }
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Formatos
            ///DESCRIPCIÓN          : Metodo para consultar los cuentas de ingresos
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 19/Junio/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Formatos()
            {
                return Cls_Cat_Tramites_Datos.Consultar_Formatos(this);
            }


        #endregion

    }
}