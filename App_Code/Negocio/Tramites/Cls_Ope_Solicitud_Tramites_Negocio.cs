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
using Presidencia.Solicitud_Tramites.Datos;


namespace Presidencia.Solicitud_Tramites.Negocios
{
    public class Cls_Ope_Solicitud_Tramites_Negocio
    {
        #region Variables Internas

            private String Estatus;
            private String Tramite_ID;
            private String Clave_Solicitud;
            private String Porcentaje;
            private String Solicitud_ID;
            private String[,] Datos;
            private String[,] Documentos;
            private String Nombre_Solicitante;
            private String Apellido_Paterno;
            private String Apellido_Materno;
            private String Subproceso_ID;
            private String E_Mail;
            private DateTime Fecha_Entrega;
            private String Perito_ID;
            private String Comentarios;
            private String Cuenta_Predial;
            private String Inspector_ID;
            private DataTable Dt_Datos;
            private String Usuario;
            private String Zona_ID;
            private String Empleado_ID;
            private String Folio;
            private String Costo_Base;
            private String Cantidad;
            private String Costo_Total;
            private String Tipo_Dato;
            private String Contribuyente_ID;
            private String Direccion_Predio;
            private String Propietario_Predio;
            private String Calle_Predio;
            private String Nuemro_Predio;
            private String Manzana_Predio;
            private String Lote_Predio;
            private String Otros_Predio;
            private String Complemento;
            private String Consecutivo;

        #endregion

        #region Variables Publicas

            public String P_E_Mail
            {
                get { return E_Mail; }
                set { E_Mail = value; }
            }
            public String P_Subproceso_ID
            {
                get { return Subproceso_ID; }
                set { Subproceso_ID = value; }
            }
            public String P_Apellido_Materno
            {
                get { return Apellido_Materno; }
                set { Apellido_Materno = value; }
            }
            public String P_Apellido_Paterno
            {
                get { return Apellido_Paterno; }
                set { Apellido_Paterno = value; }
            }
            public String P_Nombre_Solicitante
            {
                get { return Nombre_Solicitante; }
                set { Nombre_Solicitante = value; }
            }
            public String[,] P_Documentos
            {
                get { return Documentos; }
                set { Documentos = value; }
            }
            public String[,] P_Datos
            {
                get { return Datos; }
                set { Datos = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Tramite_ID
            {
                get { return Tramite_ID; }
                set { Tramite_ID = value; }
            }
            public String P_Clave_Solicitud
            {
                get { return Clave_Solicitud; }
                set { Clave_Solicitud = value; }
            }
            public String P_Porcentaje
            {
                get { return Porcentaje; }
                set { Porcentaje = value; }
            }
            public String P_Solicitud_ID
            {
                get { return Solicitud_ID; }
                set { Solicitud_ID = value; }
            }
            public DateTime P_Fecha_Entrega
            {
                get { return Fecha_Entrega; }
                set { Fecha_Entrega = value; }
            }
            public String P_Perito_ID
            {
                get { return Perito_ID; }
                set { Perito_ID = value; }
            }
            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }
            public String P_Cuenta_Predial
            {
                get { return Cuenta_Predial; }
                set { Cuenta_Predial = value; }
            }
            public String P_Inspector_ID
            {
                get { return Inspector_ID; }
                set { Inspector_ID = value; }
            }
            public DataTable P_Dt_Datos
            {
                get { return Dt_Datos; }
                set { Dt_Datos = value; }
            }
            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }
            public String P_Zona_ID
            {
                get { return Zona_ID; }
                set { Zona_ID = value; }
            }
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Folio
            {
                get { return Folio; }
                set { Folio = value; }
            }
            public String P_Costo_Base
            {
                get { return Costo_Base; }
                set { Costo_Base = value; }
            }
            public String P_Cantidad
            {
                get { return Cantidad; }
                set { Cantidad = value; }
            }
            public String P_Costo_Total
            {
                get { return Costo_Total; }
                set { Costo_Total = value; }
            }
            public String P_Tipo_Dato
            {
                get { return Tipo_Dato; }
                set { Tipo_Dato = value; }
            }
            public String P_Contribuyente_ID
            {
                get { return Contribuyente_ID; }
                set { Contribuyente_ID = value; }
            }
            public String P_Direccion_Predio
            {
                get { return Direccion_Predio; }
                set { Direccion_Predio = value; }
            }
            public String P_Propietario_Predio
            {
                get { return Propietario_Predio; }
                set { Propietario_Predio = value; }
            }
            public String P_Calle_Predio
            {
                get { return Calle_Predio; }
                set { Calle_Predio = value; }
            }
            public String P_Nuemro_Predio
            {
                get { return Nuemro_Predio; }
                set { Nuemro_Predio = value; }
            }
            public String P_Manzana_Predio
            {
                get { return Manzana_Predio; }
                set { Manzana_Predio = value; }
            }
            public String P_Lote_Predio
            {
                get { return Lote_Predio; }
                set { Lote_Predio = value; }
            }
            public String P_Otros_Predio
            {
                get { return Otros_Predio; }
                set { Otros_Predio = value; }
            }
            public String P_Complemento
            {
                get { return Complemento; }
                set { Complemento = value; }
            }
            public String P_Consecutivo
            {
                get { return Consecutivo; }
                set { Consecutivo = value; }
            }
        #endregion

        #region Métodos
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Alta_Solicitud
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 12/Octubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public String Alta_Solicitud(String Usuario_Creo) 
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Alta_Solicitud(this, Usuario_Creo);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Alta_Solicitud_Empleado
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 12/Octubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public int Alta_Solicitud_Empleado(String Usuario_Creo)
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Alta_Solicitud_Empleado(this, Usuario_Creo);
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_solicitud
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 12/Octubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public void Modificar_solicitud(String Usuario_Modifico) 
            {
                Cls_Ope_Solicitud_Tramites_Datos.Modificar_Solicitud(this, Usuario_Modifico);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_solicitud_Estatus_Pendiente
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la modificacion de la solicitud
            ///PARAMETROS:
            ///CREO: Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO: 16/Julio/2012 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public Boolean Modificar_Solicitud_Estatus_Pendiente()
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Modificar_Solicitud_Estatus_Pendiente(this);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Tramite
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 12/Octubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public DataSet Consultar_Datos_Tramite() 
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Datos_Tramite(this);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos_Tramite
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 12/Octubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public DataSet Consultar_Documentos_Tramite() 
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Documentos_Tramite(this);
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Tramites
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 17/OCtubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public DataSet Consultar_Tramites() 
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Tramites(this);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitud
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 17/OCtubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public DataSet Consultar_Solicitud() 
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Cosultar_Solicitud(this);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Solicitud
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 17/OCtubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public DataSet Consultar_Datos_Solicitud() 
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Datos_Solicitud(this);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos_Solicitud
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 17/OCtubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public DataSet Consultar_Documentos_Solicitud() 
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Documentos_Solicitud(this);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Subproceso
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 18/OCtubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public DataSet Consultar_Subproceso() 
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Subproceso(this);
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
            ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
            ///PARAMETROS:
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 18/OCtubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public String Obtener_Consecutivo(String Tabla, String Campo) 
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Obtener_Id_Consecutivo(Campo, Tabla);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Inspectores
            ///DESCRIPCIÓN          : Metodo que consultara  los inspectores que se encuentran registrados
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 10/Julio/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Inspectores()
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Inspectores(this);
            }
            
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Obra
            ///DESCRIPCIÓN          : Metodo que consultara la informacion de la obra
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 10/Julio/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Datos_Obra()
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Datos_Obra(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Obra
            ///DESCRIPCIÓN          : Metodo que consultara la informacion de la obra
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 10/Noviembre/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Datos_Finales_Tramite()
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Datos_Finales_Tramite(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Actividad_Solicitud_Hija
            ///DESCRIPCIÓN          : Metodo que consultara la informacion de la obra
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 07/Noviembre/2012 
            ///*********************************************************************************************************
            public Boolean Modificar_Actividad_Solicitud_Hija()
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Modificar_Actividad_Solicitud_Hija(this);
            }

            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Subproceso
            ///DESCRIPCIÓN         : Consulta los Subprocesos
            ///PARAMETROS          : 1.- Solicitud. Filtros para realizar la busqueda
            ///CREO                : Salvador Vazquez Camacho
            ///FECHA_CREO          : 10/Agosto/2012
            ///MODIFICO            :
            ///FECHA_MODIFICO      :
            ///CAUSA_MODIFICACIÓN  :
            ///*******************************************************************************
            public DataTable Consultar_Tabla_Subproceso()
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Tabla_Subproceso(this);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Tramites
            ///DESCRIPCIÓN         : Consulta los Subprocesos
            ///PARAMETROS          : 1.- Solicitud. Filtros para realizar la busqueda
            ///CREO                : Salvador Vazquez Camacho
            ///FECHA_CREO          : 10/Agosto/2012
            ///MODIFICO            :
            ///FECHA_MODIFICO      :
            ///CAUSA_MODIFICACIÓN  :
            ///*******************************************************************************
            public DataTable Consultar_Tabla_Tramites()
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Tabla_Tramites(this);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Cuenta_Predial
            ///DESCRIPCIÓN         : Consulta los datos de la cuenta predial
            ///PARAMETROS          : 1.- Solicitud. Filtros para realizar la busqueda
            ///CREO                : Salvador Vazquez Camacho
            ///FECHA_CREO          : 10/Agosto/2012
            ///MODIFICO            :
            ///FECHA_MODIFICO      :
            ///CAUSA_MODIFICACIÓN  :
            ///*******************************************************************************
            public DataTable Consultar_Cuenta_Predial()
            {
                return Cls_Ope_Solicitud_Tramites_Datos.Consultar_Cuenta_Predial(this);
            }

        #endregion
    }
        
}
