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
using Presidencia.Almacen_Resguardos.Datos;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;

/// <summary>
/// Summary description for Cls_Alm_Com_Resguardos_Negocio
/// </summary>
namespace Presidencia.Almacen_Resguardos.Negocio
{
    public class Cls_Alm_Com_Resguardos_Negocio
    {
        public Cls_Alm_Com_Resguardos_Negocio()
        {
        }

        #region (Variables Locales)

            private String Operacion;

        #endregion

        #region (Variables Publicas)

            public String P_Operacion
            {
                get { return Operacion; }
                set { Operacion = value; }
            }

        #endregion

        #region (Metodos)

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Resguardos_Bienes
        ///DESCRIPCIÓN:             Llama a la clase de datos que se encarga de la conexion a la base 
        ///                         de datos enviando un objeto de esta clase para obtener sus valores
        ///PARAMETROS:              Id_Bien: Identificador del Bien Mueble el cual se vana a consultar sus datos
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              17/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consulta_Resguardos_Bienes(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Id_Bien)
        {
            return Cls_Alm_Com_Resguardos_Datos.Consulta_Resguardos_Bienes(this, Id_Bien);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Resguardos_Bienes
        ///DESCRIPCIÓN:             Llama a la clase de datos que se encarga de la conexion a la base 
        ///                         de datos enviando un objeto de esta clase para obtener sus valores
        ///PARAMETROS:              Id_Bien: Identificador del Bien Mueble el cual se vana a consultar sus datos
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              17/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consulta_Resguardos_Bienes2(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Id_Bien)
        {
            return Cls_Alm_Com_Resguardos_Datos.Consulta_Resguardos_Bienes2(this, Id_Bien);
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Resguardos_Vehiculos
        ///DESCRIPCIÓN:             Llama a la clase de datos que se encarga de la conexion a la base 
        ///                         de datos enviando un objeto de esta clase para obtener sus valores
        ///PARAMETROS:              Id_Vehiculo: Identificador del vehiculo que se va a consultar
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              23/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consulta_Resguardos_Vehiculos(Cls_Ope_Pat_Com_Vehiculos_Negocio Id_Vehiculo)
        {
            return Cls_Alm_Com_Resguardos_Datos.Consulta_Resguardos_Vehiculos(this, Id_Vehiculo);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Vehiculos_Asegurados
        ///DESCRIPCIÓN:             Llama a la clase de datos que se encarga de la conexion a la base de datos 
        ///                         enviando un objeto de esta clase para obtener sus valores
        ///PARAMETROS: 
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              23/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consulta_Vehiculos_Asegurados(Cls_Ope_Pat_Com_Vehiculos_Negocio Id_Vehiculo)
        {
            return Cls_Alm_Com_Resguardos_Datos.Consulta_Vehiculos_Asegurados(this, Id_Vehiculo);
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Datos_Producto
        ///DESCRIPCIÓN:             Llama a la clase de datos que se encarga de la conexion a la base 
        ///                         de datos enviando el Id_Producto y el No_Requisicion
        ///PARAMETROS:              1.- Id_Producto, Es el identificador del producto que se va a resguardar
        ///                         2.- No_Requisicion, es el numero de requisiciòn que solicito el producto
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              29/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consulta_Datos_Producto(String Id_Producto, String No_Requisicion)
        {
            return Cls_Alm_Com_Resguardos_Datos.Consulta_Datos_Producto(Id_Producto, No_Requisicion);
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Resguardos_Cemovientes
        ///DESCRIPCIÓN:             Llama a la clase de datos que se encarga de la conexion a la base 
        ///                         de datos enviando un objeto de esta clase para obtener sus valores
        ///PARAMETROS:              Id_Cemoviente: identificador del Cemoviente que se va a consultar
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              29/Diciembre/2010 
        ///MODIFICO:                
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consulta_Resguardos_Cemovientes(Cls_Ope_Pat_Com_Cemovientes_Negocio Id_Cemoviente)
        {
            return Cls_Alm_Com_Resguardos_Datos.Consulta_Resguardos_Cemovientes(this, Id_Cemoviente);
        }

        #endregion
    }
}