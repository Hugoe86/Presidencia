using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Peritos_Externos.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Peritos_Externos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Peritos_Externos.Negocio
{
    public class Cls_Cat_Cat_Peritos_Externos_Negocio
    {
        #region variables privadas

        private String Perito_Externo_Id;
        private String Nombre;
        private String Apellido_Paterno;
        private String Apellido_Materno;
        private String Calle;
        private String Colonia;
        private String Estado;
        private String Ciudad;
        private String Telefono;
        private String Celular;
        private String Usuario;
        private String Password;
        private String Estatus;
        private String Observaciones;
        private String Fecha;

        #endregion

        #region Variables Publicas

        public String P_Perito_Externo_Id
        {
            get { return Perito_Externo_Id; }
            set { Perito_Externo_Id = value; }
        }

        public String P_Nombre
        {
            get { return Nombre;; }
            set { Nombre = value; }
        }

        public String P_Apellido_Paterno
        {
            get { return Apellido_Paterno; }
            set { Apellido_Paterno = value; }
        }

        public String P_Apellido_Materno
        {
            get { return Apellido_Materno; }
            set { Apellido_Materno = value; }
        }

        public String P_Calle
        {
            get { return Calle; }
            set { Calle = value; }
        }

        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }

        public String P_Estado
        {
            get { return Estado; }
            set { Estado = value; }
        }

        public String P_Ciudad
        {
            get { return Ciudad; }
            set { Ciudad = value; }
        }

        public String P_Telefono
        {
            get { return Telefono; }
            set { Telefono = value; }
        }

        public String P_Celular
        {
            get { return Celular; }
            set { Celular = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Password
        {
            get { return Password; }
            set { Password = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Perito_Externo()
        {
            return Cls_Cat_Cat_Peritos_Externos_Datos.Alta_Perito_Externo(this);
        }

        public Boolean Modificar_Perito_Externo()
        {
            return Cls_Cat_Cat_Peritos_Externos_Datos.Modificar_Perito_Externo(this);
        }
        public Boolean Modificar_Perito_Externo_Est()
        {
            return Cls_Cat_Cat_Peritos_Externos_Datos.Modificar_Perito_Externo_Est(this);
        }

        public Boolean Baja_Perito_Externo()
        {
            return Cls_Cat_Cat_Peritos_Externos_Datos.Baja_Perito_Externo(this);
        }

        public DataTable Consultar_Peritos_Externos()
        {
            return Cls_Cat_Cat_Peritos_Externos_Datos.Consultar_Peritos_Externos(this);
        }

        #endregion
    }
}