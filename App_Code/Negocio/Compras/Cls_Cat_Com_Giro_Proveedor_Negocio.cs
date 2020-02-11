using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Giro_Proveedor.Datos;

namespace Presidencia.Catalogo_Compras_Giro_Proveedor.Negocio
{
    public class Cls_Cat_Com_Giro_Proveedor_Negocio
    {
        

        #region VARIABLES LOCALES
          private Cls_Cat_Com_Giro_Proveedor_Datos Datos_Giro_Proveedor;
          private string Giro_ID;
          private string Proveedor_ID;
          private string Usuario;
          private String Giro_Id_Anterior;

          public String P_Giro_Id_Anterior
          {
              get { return Giro_Id_Anterior; }
              set { Giro_Id_Anterior = value; }
          }

        #endregion

        #region VARIABLES PUBLICAS
          public String P_Giro_ID
          {
              get { return Giro_ID; }
              set { Giro_ID = value; }
          }

          public String P_Proveedor_ID
          {
              get { return Proveedor_ID; }
              set { Proveedor_ID = value; }
          }
          public String P_Usuario
          {
              get { return Usuario; }
              set { Usuario = value; }
          }
        #endregion

        #region METODOS
          public Cls_Cat_Com_Giro_Proveedor_Negocio()
          {
              Datos_Giro_Proveedor = new Cls_Cat_Com_Giro_Proveedor_Datos();
          }
          ///*******************************************************************************
          ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Modificados
          ///DESCRIPCIÓN: Metodo que obtiene una dataset de acuerdo a los datos a modificar o modificados
          ///PARAMETROS: 
          ///CREO: Leslie Gonzalez Vazquez
          ///FECHA_CREO: 04/Febrero/2011 
          ///MODIFICO:
          ///FECHA_MODIFICO:
          ///CAUSA_MODIFICACIÓN:
          ///*******************************************************************************         
          public DataTable Consulta_Datos()
          {
              return Datos_Giro_Proveedor.Consulta_Giro_Proveedor(this);                
          }

          ///*******************************************************************************
          ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Modificados
          ///DESCRIPCIÓN: Metodo que obtiene una dataset de acuerdo a los datos a modificar o modificados
          ///PARAMETROS: 
          ///CREO: Leslie Gonzalez Vazquez
          ///FECHA_CREO: 08/Febrero/2011 
          ///MODIFICO:
          ///FECHA_MODIFICO:
          ///CAUSA_MODIFICACIÓN:
          ///*******************************************************************************         
          public DataTable Consulta_Giro()
          {
              return Datos_Giro_Proveedor.Consultar_Giros();
          }

          ///*******************************************************************************
          ///NOMBRE DE LA FUNCIÓN: Consulta_Proveedor
          ///DESCRIPCIÓN: Metodo que obtiene una dataset de acuerdo a los datos a modificar o modificados
          ///PARAMETROS: 
          ///CREO: Leslie Gonzalez Vazquez
          ///FECHA_CREO: 08/Febrero/2011
          ///MODIFICO:
          ///FECHA_MODIFICO:
          ///CAUSA_MODIFICACIÓN:
          ///*******************************************************************************         
          public DataTable Consulta_Proveedor()
          {
              return Datos_Giro_Proveedor.Consultar_Proveedores();
          }

          ///*******************************************************************************
          ///NOMBRE DE LA FUNCIÓN: Eliminar_Giros_Proveedores
          ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para eliminar un giro proveedor
          ///PARAMETROS:
          ///CREO: Leslie Gonzalez Vazquez
          ///FECHA_CREO: 09/Febrero/2011
          ///MODIFICO:
          ///FECHA_MODIFICO:
          ///CAUSA_MODIFICACIÓN:
          ///*******************************************************************************

          public void Eliminar_Giros_Proveedores()
          {
              Datos_Giro_Proveedor.Eliminar_Giro_Proveedor(this);
          }

          ///*******************************************************************************
          ///NOMBRE DE LA FUNCIÓN: Eliminar_Giros_Proveedores
          ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para eliminar un giro proveedor
          ///PARAMETROS:
          ///CREO: Leslie Gonzalez Vazquez
          ///FECHA_CREO: 09/Febrero/2011
          ///MODIFICO:
          ///FECHA_MODIFICO:
          ///CAUSA_MODIFICACIÓN:
          ///*******************************************************************************

          public void Eliminar_Giros_Del_Proveedor()
          {
              Datos_Giro_Proveedor.Eliminar_Giros_Del_Proveedor(this);
          }

          ///*******************************************************************************
          ///NOMBRE DE LA FUNCIÓN: Alta_Giro_Proveedor
          ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para dar de alta un giro proveedor
          ///PARAMETROS:
          ///CREO: Leslie Gonzalez Vazquez
          ///FECHA_CREO: 09/Febrero/2011 
          ///MODIFICO:
          ///FECHA_MODIFICO:
          ///CAUSA_MODIFICACIÓN:
          ///*******************************************************************************/

          public void Alta_Giro_Proveedor()
          {
              Datos_Giro_Proveedor.Alta_Giro_Proveedor(this);
          }

          ///*******************************************************************************
          ///NOMBRE DE LA FUNCIÓN: Alta_Giro_Proveedor
          ///DESCRIPCIÓN: Sobrecarga para insertar fecha existente. Llama el metodo de la 
          ///       clase de Datos para dar de alta un giro proveedor
          ///PARAMETROS:
          ///           1. Fecha: cadena de texto con la fecha a insertar en el campo
          ///CREO: Roberto Gonzalez Oseguera
          ///FECHA_CREO: 16/Febrero/2011 
          ///MODIFICO:
          ///FECHA_MODIFICO:
          ///CAUSA_MODIFICACIÓN:
          ///*******************************************************************************/

          public void Alta_Giro_Proveedor(String Fecha)
          {
              Datos_Giro_Proveedor.Alta_Giro_Proveedor(this, Fecha);
          }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Giro_Proveedor
        ///DESCRIPCIÓN:Llama el metodo de la clase de Datos para modificar el giro de un proveedor
        ///PARAMETROS:
        ///CREO: Leslie Gonzalez Vazquez
        ///FECHA_CREO: 09/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************/
          public void Modificar_Giro_Proveedor()
          {
              Datos_Giro_Proveedor.Modificar_Giro_Proveedor(this);
          }

        #endregion
    }
}


