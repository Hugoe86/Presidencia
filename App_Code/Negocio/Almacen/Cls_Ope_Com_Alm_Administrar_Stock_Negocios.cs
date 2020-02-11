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
using Presidencia.Administrar_Stock.Datos;
/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Administrar_Stock_Negocios
/// </summary>
/// 
namespace Presidencia.Administrar_Stock.Negocios
{
    public class Cls_Ope_Com_Alm_Administrar_Stock_Negocios
    {
        #region Variables Locales
        private String Tipo_DataTable;
        private String Producto_ID;
        private String Familia_ID;
        private String Subfamilia_ID;
        private String Marca_ID;
        private String Letra_Inicio;
        private String Letra_Fin;
        private String No_Inventario;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Estatus;
        private String Tipo;
        private String Observaciones;
        private Int32 Contados_Usuario;
        private Int32 Marbete;
        private DataTable Inventario_Stock = null;
        private DataTable Datos_Productos = null;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String No_Empleado;
        private String Password;
        private String Tipo_Ajuste;
        private String Justificacion;
        private String Empleado_ID;
        #endregion

        #region Variables Publicas

        public String P_Tipo_DataTable
        {
            get { return Tipo_DataTable; }
            set { Tipo_DataTable = value; }
        }
        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
        }

        public String P_Familia_ID
        {
            get { return Familia_ID; }
            set { Familia_ID = value; }
        }

        public String P_Subfamilia_ID
        {
            get { return Subfamilia_ID; }
            set { Subfamilia_ID = value; }
        }

        public String P_Marca_ID
        {
            get { return Marca_ID; }
            set { Marca_ID = value; }
        }

        public String P_Letra_Inicio
        {
            get { return Letra_Inicio; }
            set { Letra_Inicio = value; }
        }

        public String P_Letra_Fin
        {
            get { return Letra_Fin; }
            set { Letra_Fin = value; }
        }

        public String P_No_Inventario
        {
            get { return No_Inventario; }
            set { No_Inventario = value; }
        }
        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public DataTable P_Inventario_Stock
        {
            get { return Inventario_Stock; }
            set { Inventario_Stock = value; }
        }

        public Int32 P_Contados_Usuario
        {
            get { return Contados_Usuario; }
            set { Contados_Usuario = value; }
        }
       
        public Int32 P_Marbete
        {
            get { return Marbete; }
            set { Marbete = value; }
        }

        public DataTable P_Datos_Productos
        {
            get { return Datos_Productos; }
            set { Datos_Productos = value; }
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
        public String P_Password
        {
            get { return Password; }
            set { Password = value; }
        }
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public String P_Tipo_Ajuste
        {
            get { return Tipo_Ajuste; }
            set { Tipo_Ajuste = value; }
        }
        public String P_Justificacion
        {
            get { return Justificacion; }
            set { Justificacion = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        #endregion

        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Inventario_Fisico
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo para consultar los productos y generar inventarios fisicos
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           05/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Inventario_Fisico()
        {
            return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Consulta_Inventario_Fisico(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Inventario_Selectivo
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo para consultar los productos y generar un inventarios selectivos
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           05/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Inventario_Selectivo()
        {
            return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Consulta_Inventario_Selectivo(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_DataTable
        ///DESCRIPCIÓN:          Método que manda llamar el metodo Consultar_DataTable para consultar informacion especifica
        ///PARAMETROS:   
        ///CREO:                Salvador Hernández Ramírez
        ///FECHA_CREO:         06/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_DataTable()
        {
            return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Consultar_DataTable(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Id_Consecutivo
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Obtener_Id_Consecutivo para consultar consultar el siguiente numero de inventario
        ///PARAMETROS:           Campo_ID: Nombre del campo "Identificador de la Tabla"
        ///                      Tabla: Nombre de la tabla a la que se le va aplicar el proceso
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           07/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Obtener_Id_Consecutivo(String Campo_ID, String Tabla)
        {
            return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Obtener_Id_Consecutivo(Campo_ID, Tabla);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Guardar_Inventario
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Guardar_Inventario para guardar el inventario generado
        ///PARAMETROS:           Campo_ID: Nombre del campo "Identificador de la Tabla"
        ///                      Tabla: Nombre de la tabla a la que se le va aplicar el proceso
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           08/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Guardar_Inventario()
        {
            return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Guardar_Inventario(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Inventarios_General
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Consulta_Inventarios_General para consultar los productos que pertenecen a los inventarios seleccionados 
        ///PARAMETROS:  
        ///              
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           10/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Inventarios_General()
        {
            return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Consulta_Inventarios_General(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Guardar_Inventarios_Capturado
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Guardar_Inventarios_Capturado para guardar los productos del inventario capturado
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           17/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Guardar_Inventarios_Capturado()
        {
             Cls_Ope_Com_Alm_Administrar_Stock_Datos.Guardar_Inventarios_Capturado(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Aplicar_Inventario
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Aplicar_Inventario para actualizar las cantidades de los productos que han sido capturados con las cantidades de los productos que han sido guardados
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           13/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Aplicar_Inventario()
        {
            Cls_Ope_Com_Alm_Administrar_Stock_Datos.Aplicar_Inventario(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_En_Inventarios
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Consulta_Productos_En_Inventarios para consultar los productos que ya forman parte de otros inventarios
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           15/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Productos_En_Inventarios()
        {
           return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Consulta_Productos_En_Inventarios(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Busqueda_Simple
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Busqueda_Simple para realizar la busqueda de inventarios en base a su estatus
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           18/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Busqueda_Simple()
        {
            return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Busqueda_Simple(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Busqueda_Avanzada_Inventarios
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Busqueda_Avanzada_Inventarios para realizar la busqueda de inventarios en base a su estatus y a su fecha
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           18/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Busqueda_Avanzada_Inventarios()
        {
            return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Busqueda_Avanzada_Inventarios(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cambiar_Estatus
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Cambiar_Estatus para realizar el cambio de estatus a cancelado al inventario seleccionado
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           20/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cambiar_Estatus()
        {
            Cls_Ope_Com_Alm_Administrar_Stock_Datos.Cambiar_Estatus(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Usuario
        ///DESCRIPCIÓN:          Metodo que manda llamar el metodo Consulta_Datos_Usuario para consultar los datos del usuario
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           26/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Datos_Usuario()
        {
            return Cls_Ope_Com_Alm_Administrar_Stock_Datos.Consulta_Datos_Usuario(this);
        }
        #endregion
    }
}