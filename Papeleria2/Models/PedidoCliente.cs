using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Papeleria2.Models
{
    public class PedidoCliente
    {

        private PapeleriaContext db = new PapeleriaContext();
        private List<Orden_productos> detalle_orden;

        public PedidoCliente()
        {
            detalle_orden = db.Orden_productos.ToList();
        }

        public Ordenes Orden
        {
            get;
            set;
        }

        public string Fecha
        {
            get;
            set;
        }

        public string envio
        {
            get;
            set;
        }

        public string status
        {
            get;
            set;
        }

        public string Total
        {
            get;
            set;
        }

        public List<Orden_productos> ordenProductos
        {
            get;
            set;
        }
    }
}