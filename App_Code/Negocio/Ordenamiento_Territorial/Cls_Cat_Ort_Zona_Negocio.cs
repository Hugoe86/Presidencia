using System;
using System.Data;
using Presidencia.Ordenamiento_Territorial_Zonas.Datos;

namespace Presidencia.Ordenamiento_Territorial_Zonas.Negocio
{
    public class Cls_Cat_Ort_Zona_Negocio
    {
        #region Variables internas
            
            private String Zona_ID;
            private String Nombre;
            private String Usuario;
            private String Responsable_Zona;
            private String Dependencia_ID;
            private String Empleado_ID;
            private String Area_ID;
            private String Area;
            
        #endregion

        #region Variables Publicas
        
            public String P_Zona_ID
            {
                get { return Zona_ID; }
                set { Zona_ID = value; }
            }
            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }
            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }
            public String P_Responsable_Zona
            {
                get { return Responsable_Zona; }
                set { Responsable_Zona = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Area_ID
            {
                get { return Area_ID; }
                set { Area_ID = value; }
            }
            public String P_Area
            {
                get { return Area; }
                set { Area = value; }
            }

        #endregion

        #region Consultas

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Tipos_Materiales
            ///DESCRIPCIÓN          : Metodo para consultar los datos
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 12/Junio/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Zonas()
            {
                return Cls_Cat_Ort_Zona_Datos.Consultar_Zonas(this);
            }
            public DataTable Consultar_Supervisores()
            {
                return Cls_Cat_Ort_Zona_Datos.Consultar_Supervisores(this);
            }
            public DataTable Consultar_Area_Id()
            {
                return Cls_Cat_Ort_Zona_Datos.Consultar_Area_Id(this);
            }
        #endregion

        #region Alta-Modificacion-Eliminar

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Condicion_Inmueble
            ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 12/Junio/2012 
            ///*********************************************************************************************************
            public Boolean Alta()
            {
                return Cls_Cat_Ort_Zona_Datos.Alta(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Condicion_Inmueble
            ///DESCRIPCIÓN          : Metodo para modificar los datos del usuario
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 12/Junio/2012 
            ///*********************************************************************************************************
            public Boolean Modificar()
            {
                return Cls_Cat_Ort_Zona_Datos.Modificar(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Condicion_Inmueble
            ///DESCRIPCIÓN          : Metodo para eliminar los datos del usuario
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 12/Junio/2012 
            ///*********************************************************************************************************
            public Boolean Eliminar()
            {
                return Cls_Cat_Ort_Zona_Datos.Eliminar(this);
            }

        #endregion
    }
}
