using Papeleria2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Papeleria2.Controllers

{
    [Authorize]
    public class PedidosController : Controller
    {
        PapeleriaContext db = new PapeleriaContext();
        // GET: Pedidos
        public ActionResult Index()
        {
            string correo = User.Identity.Name;
            Clientes cl = (from c in db.Clientes
                           where c.email == correo
                           select c
                          ).ToList().FirstOrDefault();

            int id = cl.id;
            var query = from o in db.Ordenes
                        where o.id_cliente == id
                        orderby o.fecha_creacion ascending
                        select o;

            List<Ordenes> ordenes = query.ToList();

            List<PedidoCliente> pedidos = new List<PedidoCliente>();
            PedidoCliente pedido;
            List<Orden_productos> ordPed;
            List<ItemPedido> itemPed = new List<ItemPedido>();
            ItemPedido iPed;

            foreach (Ordenes o in ordenes)
            {
                pedido = new PedidoCliente();
                pedido.Orden = o;
                pedido.Fecha = o.fecha_creacion.ToShortDateString();

                if (o.fecha_envio.HasValue)
                {
                    pedido.envio = o.fecha_envio.GetValueOrDefault().ToShortDateString();

                }
                else
                {
                    pedido.envio = "Proximamente";
                }
                if (o.fecha_entrega.HasValue)
                {
                    pedido.status = o.fecha_entrega.GetValueOrDefault().ToShortDateString();

                }
                else
                {
                    pedido.status = "Sin entregar";
                }
                pedido.Total = o.total.ToString();
                pedidos.Add(pedido);
                ordPed = (from oP in db.Orden_productos
                          where oP.id_orden == o.id
                          select oP).ToList();
                pedido.ordenProductos = ordPed;

                foreach (Orden_productos op in ordPed)
                {
                    iPed = new ItemPedido();
                    iPed.idOrd = op.id_orden;
                    iPed.Product = db.Productos.First(p => p.id == op.id_producto);
                    iPed.cantidad = op.cantidad;
                    itemPed.Add(iPed);
                }
            }
            Session["misPedidos"] = pedidos;
            Session["Pedido"] = itemPed;

            return View();
        }
    }
}