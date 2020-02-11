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
using System.Data.OleDb;
using System.Text;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Xml;
using Presidencia.Sessiones;
using Presidencia.Plantillas_Word;
using System.IO;
using Presidencia.Constantes;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.IO.Packaging;
using System.Text.RegularExpressions;

namespace Presidencia.Ayudante_Plantilla_Word
{
    public class Cls_Ayudante_Llenado_Plantilla_Word
    {
        /// *****************************************************************************************
        /// Nombre: Obtener_Etiquetas_Word
        /// Descripción: Método que obtendra las etiquetas de la plantilla de word
        /// Parámetros: Ruta_Archivo .- Ruta física del archivo.
        /// Usuario Creó: Hugo Enrique Ramírez Aguilera
        /// Fecha Creó: 09/Septiembre/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        public List<Tag> Obtener_Etiquetas_Word(String Ruta_Archivo)
        {
            List<Tag> Lista_Marcadores;
            try
            {
                WordprocessingDocument Documento = WordprocessingDocument.Open(Ruta_Archivo, false);
                Lista_Marcadores = Documento.MainDocumentPart.Document.Descendants<Tag>().ToList();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al leer un archivo de excel. Error: " + Ex.Message + "]");
            }
            return Lista_Marcadores;
        }

        /// *****************************************************************************************
        /// Nombre: Llenar_Etiquetas_Word
        /// Descripción: Método que llenara las etiquetas de la plantilla de word 
        ///              (Dt_Datos datos utilizados:
        ///               NOMBRE_DATO:sera el nombre de la etiqueta,
        ///               VALOR: sera el contenido que se agregara a la etiqueta)
        ///               
        /// Parámetros: Lista_Marcadores .- Lista de marcadores que tiene el documento de word
        ///             Dt_Datos.- La informacion que se ingresara a las etiquetas
        ///             
        /// Usuario Creó: Hugo Enrique Ramírez Aguilera
        /// Fecha Creó: 09/Septiembre/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        public XmlDocument Llenar_Etiquetas_Word(List<Tag> Lista_Marcadores, DataTable Dt_Datos)
        {
            XmlDocument _Xml = new XmlDocument();
            XmlElement _Marcador = null;
            XmlElement _Root = _Xml.CreateElement("root");
            String Nombre_Marcador = "";
            String Nombre = "";
            int Contador = 0;
            try
            {
                for (int Contador_For = 0; Contador_For < Lista_Marcadores.Count; Contador_For++)
                {
                    Nombre = Lista_Marcadores.ElementAt<Tag>(Contador_For).Val;//   se obtiene el nombre del marcador
                    Nombre_Marcador = Nombre.ToUpper();//Nombre del maracador

                    //if (Contador == Dt_Datos.Rows.Count)
                    //{
                    //    break;
                    //}// fin del if contador

                    if (Dt_Datos is DataTable)
                    {
                        if (Dt_Datos != null && Dt_Datos.Rows.Count > 0)
                        {
                            foreach (DataRow Registro in Dt_Datos.Rows)
                            {
                                if (!String.IsNullOrEmpty(Registro["NOMBRE_DATO"].ToString()))
                                {
                                    //if (Contador == Dt_Datos.Rows.Count)
                                    //{
                                    //    break;
                                    //}

                                    if (Nombre_Marcador.ToUpper().Trim().Equals(Registro["NOMBRE_DATO"].ToString().ToUpper().Trim()))
                                    {
                                        _Marcador = _Xml.CreateElement(Nombre);//Creamos elemento
                                        _Marcador.InnerText = Registro["VALOR"].ToString().ToUpper().Trim();
                                        _Root.AppendChild(_Marcador);
                                        Contador++;
                                        break;
                                    }

                                }// fin del if!String.IsNullOrEmpty(Registro["NOMBRE_DATO"].ToString())
                            }// fin del foreach
                        }// fin del if Dt_Datos != null && Dt_Datos.Rows.Count > 0
                    }// fin del if (Dt_Datos is DataTable
                }// fin del for

                _Xml.AppendChild(_Root);//  se carga la estructura del documento

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al leer un archivo de excel. Error: " + Ex.Message + "]");
            }
            return _Xml;
        }

        /// *****************************************************************************************
        /// Nombre: Generar_Documento_Word
        /// Descripción: Método que genera el documento a partir de la plantilla
        /// Parámetros: Ruta_Archivo .- Ruta física del archivo.
        /// Usuario Creó: Hugo Enrique Ramírez Aguilera
        /// Fecha Creó: 09/Septiembre/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************
        public Boolean Generar_Documento_Word(String Ruta_Plantilla, String Documento_Salida, String Carpeta_Principal, XmlDocument _Xml)
        {
            Boolean Estatus = false;
            MainDocumentPart main;
            CustomXmlPart CustomXml;
            try
            {
                //eliminamos el documento si es que existe
                if (System.IO.File.Exists(Documento_Salida))
                {
                    System.IO.File.Delete(Documento_Salida);
                }

                //verifica si existe un directorio llamado con el nombre de la Carpeta_Principal de donde se genera el documento
                if (!Directory.Exists(Carpeta_Principal))
                {
                    Directory.CreateDirectory(Carpeta_Principal);
                }

                //copiamos la plantilla
                File.Copy(Ruta_Plantilla, Documento_Salida);

                using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
                {
                    main = doc.MainDocumentPart;
                    main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);
                    CustomXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                    using (StreamWriter ts = new StreamWriter(CustomXml.GetStream()))
                    {
                        ts.Write(_Xml.InnerXml);
                    }
                    // guardar los cambios en el documento
                    main.Document.Save();
                    Estatus = true;

                }// fin del using WordprocessingDocument
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el archivo de word. Error: " + Ex.Message + "]");
            }
            return Estatus;
        }
    }
}
