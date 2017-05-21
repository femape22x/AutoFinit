using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFinit.Pojos
{
    public class Transicion
    {
        public String estado { get; set; }
        public String simbolo { get; set; }
        public String proximoEstado { get; set; }

        public Transicion()
        {
        }

        public Transicion(String estado, String simbolo, String proximoEstado)
        {
            this.estado = estado;
            this.simbolo = simbolo;
            this.proximoEstado = proximoEstado;
        }
    }
}