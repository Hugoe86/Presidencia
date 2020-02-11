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
using Microsoft.Office.Interop.Word;
using System.IO;

/// <summary>
/// Summary description for Interaccion_Word
/// </summary>

/*******************************************************************************
 NOMBRE DE LA CLASE: Interaccion_Word
 DESCRIPCIÓN: Clase que maneja la conexion a Word.
 PARÁMETROS :
 CREO       : Francisco Antonio Gallardo Castañeda
 FECHA_CREO : 27/Septiembre/2010
 MODIFICO          :
 FECHA_MODIFICO    :
 CAUSA_MODIFICACIÓN:
*******************************************************************************/
namespace Presidencia.Plantillas_Word {
    public class Cls_Interaccion_Word {

        #region Variables Internas

            private Application Aplicacion = null;
            private Document Documento = null;
            private Object Documento_Origen = null;
            private Object Documento_Destino = null;
            private Object Nueva_Plantilla = false;
            private Object Tipo_Documento = 0;
            private Object Visible = false;
            private Object Guardar_Cambios = false;
            private Object Numero_Copias = 1;
            private Object Paginas = "";
            private Object Rango_Imprimir = WdPrintOutRange.wdPrintAllDocument;
            private Object Tipo_Pagina = WdPrintOutPages.wdPrintAllPages;
            private Object Detalle = WdPrintOutItem.wdPrintDocumentContent;
            private Object Verdadero = true;
            private Object Falso = false;
            private Object Dato_Faltante = System.Reflection.Missing.Value;

        #endregion
        
        #region Variables Publicas

            public Object P_Documento_Origen
            {
                get { return Documento_Origen; }
                set { Documento_Origen = value; }
            }

            public Object P_Documento_Destino
            {
                get { return Documento_Destino; }
                set { Documento_Destino = value; }
            }

            public Object P_Nueva_Plantilla
            {
                get { return Nueva_Plantilla; }
                set { Nueva_Plantilla = value; }
            }

            public Object P_Visible
            {
                get { return Visible; }
                set { Visible = value; }
            }

            public Object P_Guardar_Cambios
            {
                get { return Guardar_Cambios; }
                set { Guardar_Cambios = value; }
            }

            public Object P_Numero_Copias
            {
                get { return Numero_Copias; }
                set { Numero_Copias = value; }
            }

            public Object P_Paginas
            {
                get { return Paginas; }
                set { Paginas = value; }
            }

            public Object P_Rango_Imprimir
            {
                get { return Rango_Imprimir; }
                set { Rango_Imprimir = value; }
            }

            public Object P_Tipo_Pagina
            {
                get { return Tipo_Pagina; }
                set { Tipo_Pagina = value; }
            }

            public Object P_Detalle
            {
                get { return Detalle; }
                set { Detalle = value; }
            }
            
        #endregion
        
        #region Metodos
            ///*******************************************************************************************************
            ///NOMBRE_FUNCIÓN: Iniciar_Aplicacion
            ///DESCRIPCIÓN: 
            ///PARÁMETROS:
            ///CREO:
            ///FECHA_CREO: 
            ///MODIFICÓ: 
            ///FECHA_MODIFICÓ: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public void Iniciar_Aplicacion()
            {
                try
                {
                    if (Documento_Origen != null)
                    {
                        if (File.Exists(Documento_Origen.ToString()))
                        {
                            Aplicacion = new Application();
                            Documento = Aplicacion.Documents.Add(ref Documento_Origen, ref Nueva_Plantilla, ref Tipo_Documento, ref Visible);
                            Documento.Activate();
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Iniciar_Aplicacion. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************************************
            ///NOMBRE_FUNCIÓN: Escribir_Sobre_Marcador
            ///DESCRIPCIÓN: 
            ///PARÁMETROS:
            ///CREO:
            ///FECHA_CREO: 
            ///MODIFICÓ: 
            ///FECHA_MODIFICÓ: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public void Escribir_Sobre_Marcador(String Nombre_Marcador, String Texto)
            {
                try
                {
                    if (Aplicacion != null && Documento != null)
                    {
                        for (int cnt = 0; cnt < Documento.Bookmarks.Count; cnt++)
                        {
                            Object indice = cnt + 1;
                            Object marcador = Documento.Bookmarks.get_Item(ref indice).Name;
                            if (marcador.ToString().Equals(Nombre_Marcador)) { Documento.Bookmarks.get_Item(ref indice).Range.Text = Texto; }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Escribir_Sobre_Marcador. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************************************
            ///NOMBRE_FUNCIÓN: Escribir_Directamente_Sobre_Documento
            ///DESCRIPCIÓN: 
            ///PARÁMETROS:
            ///CREO:
            ///FECHA_CREO: 
            ///MODIFICÓ: 
            ///FECHA_MODIFICÓ: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public void Escribir_Directamente_Sobre_Documento(String Texto)
            {
                try
                {
                    if (Aplicacion != null && Documento != null)
                    {
                        Aplicacion.Selection.TypeText(Texto);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Escribir_Directamente_Sobre_Documento. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************************************
            ///NOMBRE_FUNCIÓN: Guardar_Nuevo_Documento
            ///DESCRIPCIÓN: 
            ///PARÁMETROS:
            ///CREO:
            ///FECHA_CREO: 
            ///MODIFICÓ: 
            ///FECHA_MODIFICÓ: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public void Guardar_Nuevo_Documento()
            {
                try
                {
                    if (Aplicacion != null && Documento != null)
                    {
                        String Raiz = Path.GetDirectoryName(Documento_Destino.ToString());

                        if (!Directory.Exists(Raiz)) 
                        { 
                            Directory.CreateDirectory(Raiz); 
                        }

                        Documento.SaveAs(ref Documento_Destino, ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante,
                                         ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante,
                                         ref Dato_Faltante, ref Dato_Faltante);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Guardar_Nuevo_Documento. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************************************
            ///NOMBRE_FUNCIÓN: Imprimir_Documento
            ///DESCRIPCIÓN: 
            ///PARÁMETROS:
            ///CREO:
            ///FECHA_CREO: 
            ///MODIFICÓ: 
            ///FECHA_MODIFICÓ: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public void Imprimir_Documento()
            {
                try
                {
                    if (Aplicacion != null && Documento != null)
                    {
                        Documento.PrintOut(ref Verdadero, ref Falso, ref Rango_Imprimir, ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante, ref Detalle,
                                           ref Numero_Copias, ref Paginas, ref Tipo_Pagina, ref Falso, ref Verdadero, ref Dato_Faltante, ref Falso, ref Dato_Faltante,
                                           ref Dato_Faltante, ref Dato_Faltante, ref Dato_Faltante);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Imprimir_Documento. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************************************
            ///NOMBRE_FUNCIÓN: Cerrar_Aplicacion
            ///DESCRIPCIÓN: 
            ///PARÁMETROS:
            ///CREO:
            ///FECHA_CREO: 
            ///MODIFICÓ: 
            ///FECHA_MODIFICÓ: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public void Cerrar_Aplicacion()
            {
                try
                {
                    if (Documento != null)
                    {
                        Documento.Close(ref Guardar_Cambios, ref Dato_Faltante, ref Dato_Faltante);
                    }
                    if (Aplicacion != null)
                    {
                        Aplicacion.Quit(ref Guardar_Cambios, ref Dato_Faltante, ref Dato_Faltante);
                    }
                    Restaurar_Propiedades();
                }
                catch (Exception Ex)
                {
                    throw new Exception("Cerrar_Aplicacion. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************************************
            ///NOMBRE_FUNCIÓN: Obtener_Marcadores
            ///DESCRIPCIÓN: 
            ///PARÁMETROS:
            ///CREO:
            ///FECHA_CREO: 
            ///MODIFICÓ: 
            ///FECHA_MODIFICÓ: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public System.Data.DataTable Obtener_Marcadores()
            { 
                System.Data.DataTable Tabla = new System.Data.DataTable();
                try
                {
                    Tabla.Columns.Add("MARCADOR_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE_MARCADOR", Type.GetType("System.String"));

                    if (Aplicacion != null && Documento != null)
                    {
                        for (int cnt = 0; cnt < Documento.Bookmarks.Count; cnt++)
                        {
                            Object indice = cnt + 1;
                            Object marcador = Documento.Bookmarks.get_Item(ref indice).Name;
                            DataRow Fila = Tabla.NewRow();
                            Fila["MARCADOR_ID"] = marcador.ToString();
                            Fila["NOMBRE_MARCADOR"] = marcador.ToString().Replace("_", " ");
                            Tabla.Rows.Add(Fila);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Obtener_Marcadores. Error: [" + Ex.Message + "]");
                }
                return Tabla;
            }
            ///*******************************************************************************************************
            ///NOMBRE_FUNCIÓN: Restaurar_Propiedades
            ///DESCRIPCIÓN: 
            ///PARÁMETROS:
            ///CREO:
            ///FECHA_CREO: 
            ///MODIFICÓ: 
            ///FECHA_MODIFICÓ: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            private void Restaurar_Propiedades()
            {
                try
                {
                    Aplicacion = null;
                    Documento = null;
                    Documento_Origen = null;
                    Nueva_Plantilla = false;
                    Tipo_Documento = 0;
                    Visible = false;
                    Guardar_Cambios = false;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Restaurar_Propiedades. Error: [" + Ex.Message + "]");
                }
            }

        #endregion

    }
}