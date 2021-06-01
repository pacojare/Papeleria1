using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Papeleria2.Models;

namespace Papeleria2.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {

        private PapeleriaContext db = new PapeleriaContext();
        

        
        // GET: Usuario
        public ActionResult Index(string email)
        {
            if (User.Identity.IsAuthenticated)
            {
                string correo = email;
                string departamento = "";

                using (db)
                {
                    var query = from st in db.Empleados
                                where st.email == correo
                                select st;
                    var lista = query.ToList();
                    if (lista.Count > 0)
                    {
                        var empleado = query.FirstOrDefault<Empleados>();
                        string[] nombres = empleado.nombre.ToString().Split(' ');
                        Session["name"] = nombres[0];
                        Session["usr"] = empleado.nombre;
                        departamento = empleado.departamento.ToString().TrimEnd();
                    }
                    else
                    {
                        var query1 = from st in db.Clientes
                                     where st.email == correo
                                     select st;
                        var lista1 = query1.ToList();
                        if (lista1.Count > 0)
                        {
                            var cliente = query1.FirstOrDefault<Clientes>();
                            string[] nombres = cliente.nombre.ToString().Split(' ');
                            Session["name"] = nombres[0];
                            Session["usr"] = cliente.nombre;
                            departamento = "cliente";
                        }
                    }

                }
                if (departamento == "Compras")
                {
                    return RedirectToAction("Index", "Compras");
                }
                if (departamento == "Almacen")
                {
                    return RedirectToAction("Index", "Envios");
                }
                if (departamento == "Servicio al cliente")
                {
                    return RedirectToAction("Index", "Chat");
                }
                if (departamento == "cliente")
                {
                    return RedirectToAction("Index", "Home");
                }
                if (departamento == "Administracion")
                {
                    return RedirectToAction("Index", "Empleado");
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
    }
}