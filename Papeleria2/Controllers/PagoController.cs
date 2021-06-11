using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Papeleria2.Models;

namespace Papeleria2.Controllers
{
    public class PagoController : Controller
    {
        private PapeleriaContext db = new PapeleriaContext();
        private string NumConfirPago;

        // GET: Pago
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CrearOrden()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Session["CrearOrden"] = "pend";
                return RedirectToAction("Login", "Account");
            }
            var orden = new Ordenes();
            var db = new PapeleriaContext();
            string correo = User.Identity.Name;

            string fechaCreacion = DateTime.Today.ToShortDateString();
            string fechaProbEntrega = DateTime.Today.AddDays(3).ToShortDateString();
            var cliente = (from c in db.Clientes
                           where c.email == correo
                           select c).ToList().FirstOrDefault();

            Session["dirCliente"] = cliente.direccion + " " + cliente.colonia + " " + cliente.entidad_federativa;
            Session["fechaOrden"] = fechaCreacion;
            Session["fPEntreg"] = fechaProbEntrega;

            if (cliente.num_tarjeta.StartsWith("4"))
                Session["tTarj"] = "1";
            if (cliente.num_tarjeta.StartsWith("5"))
                Session["tTarj"] = "2";
            if (cliente.num_tarjeta.StartsWith("3"))
                Session["tTarj"] = "3";
            Session["tTarj"] = cliente.num_tarjeta;                       
            return View();
        }

        public ActionResult Pagar (string tipoPago)
        {
            string correo = User.Identity.Name;
         
            DateTime fechaCreacion = DateTime.Today;
            DateTime fechaProbEntrega = fechaCreacion.AddDays(3);
            var cliente = (from c in db.Clientes
                           where c.email == correo
                           select c).ToList().FirstOrDefault();
            int idClient = cliente.id;

            if (tipoPago.Equals("T"))
            {
                if (!validaPago(cliente))
                {
                    return RedirectToAction("pagoNoAceptado");
                }
                else
                {
                    var dirEnt = (from d in db.Clientes
                                  where d.id == cliente.id
                                  select d).ToList().FirstOrDefault();
                    String idDir = dirEnt.direccion;
                    return RedirectToAction("pagoAceptado", routeValues: new { idC = idClient, idD = idDir });
                }
            }
            if (tipoPago.Equals("P"))
            {
                
                    var dirEnt = (from d in db.Clientes
                                  where d.id == cliente.id
                                  select d).ToList().FirstOrDefault();
                    String idDir = dirEnt.direccion;
                    return RedirectToAction("pagoPaypal", routeValues: new { idC = idClient, idD = idDir });
                
            }
            return View();
            
        }
        public ActionResult pagoPaypal(int idC, String idD)
        {
            Session["idDir"] = idC;
            Session["idClient"] = idD;

            return View();
        }
        public ActionResult pagandoPaypal(int idC, String idD)
        {
            Session["idDir"] = idC;
            Session["idClient"] = idD;

            return View();
        }
        public ActionResult pagoNoAceptado()
        {
            return View();
        }

        private bool validaPago(Clientes cliente)
        {
            bool retorna = true;

            int randomvalue;

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] val = new byte[6];
                crypto.GetBytes(val);
                randomvalue = BitConverter.ToInt32(val, 1);
            }

            NumConfirPago = Math.Abs(randomvalue * 1000).ToString();
            Session["nConfirma"] = NumConfirPago;
            return retorna;
        }
         public ActionResult pagoAceptado(int idC, String idD)
        {
            Ordenes orden_cliente = new Ordenes();
            int idOrden = 0;
            if (!(db.Ordenes.Max(o => (int?)o.id) == null))
            {
                idOrden = db.Ordenes.Max(o => o.id);
            }
            else
            {
                idOrden = 0;
            }
            idOrden++;
            orden_cliente.id = idOrden;
            orden_cliente.fecha_creacion = DateTime.Today;
            var carro = Session["cart"] as List<Item>;
            var total = carro.Sum(item => item.Producto.precio * item.Cantidad);
            orden_cliente.total = total;
            orden_cliente.id_cliente = idC;
            orden_cliente.dir_entrega = idD;
            db.Ordenes.Add(orden_cliente);
            db.SaveChanges();

            Orden_productos ordenProd;
            List<Orden_productos> listaProdOrd = new List<Orden_productos>();
            foreach(Item linea in carro)
            {
                ordenProd = new Orden_productos();
                ordenProd.id_orden = orden_cliente.id;
                ordenProd.id_producto = linea.Producto.id;
                ordenProd.cantidad = linea.Cantidad;
                db.Orden_productos.Add(ordenProd);
            }
            db.SaveChanges();
            Session["cart"] = null;
            Session["nConfirma"] = null;
            Session["itemTotal"] = 0;
            return View();

        }
    }

}