using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Papeleria2.Models;

namespace Papeleria2.Controllers
{
    public class ClientesController : Controller
    {
        private PapeleriaContext db = new PapeleriaContext();

        // GET: Clientes
        public ActionResult Index()
        {
            return View(db.Clientes.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public ActionResult Create([Bind(Include = "id,nombre,email,contrasenia,cp,entidad_federativa,ciudad,colonia,direccion,num_tarjeta,mes_expiracion,anio_expiracion,cvv,tipo_tarjeta")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(clientes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clientes);
        }*/
        public ActionResult Create(string nombre, string email, string contrasenia,
        string cp, string estado, string entidad_federativa, string ciudad, string colonia, string direccion, string num_tarjeta,
        string mes_expiracion, string anio_expiracion, string cvv, string tipo_tarjeta)
        {
            Clientes clientes = new Clientes();
            int id = 0;
            if (!(db.Clientes.Max(c => (int?)c.id) == null))
            {
                id = db.Clientes.Max(c => c.id);
            }
            else
            {
                id = 0;
            }
            id++;
            if (Tarjeta(num_tarjeta, tipo_tarjeta, mes_expiracion, anio_expiracion, cvv))
            {
                //comunicarse con el sistema de pago
                if (validaPago(nombre, entidad_federativa, cp, ciudad, colonia, direccion))
                {
                    clientes.nombre = nombre;
                    clientes.email = Session["correo"].ToString();
                    clientes.entidad_federativa = entidad_federativa;
                    clientes.cp = cp;
                    clientes.ciudad = ciudad;
                    clientes.colonia = colonia;
                    clientes.direccion = direccion;
                    clientes.num_tarjeta = num_tarjeta;
                    db.Clientes.Add(clientes);
                    db.SaveChanges();
                    string[] nombres = clientes.nombre.ToString().Split(' ');
                    Session["name"] = nombres[0];
                    Session["urs"] = clientes.nombre;

                    if (Session["CrearOrden"] != null)
                    {
                        if (Session["CrearOrden"].Equals("pend"))
                        {
                            return RedirectToAction("CrearOrden", "Pago");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Invalida");
                }

            }
            else
            {
                return RedirectToAction("Invalida");
            }
            return View(clientes);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,email,contrasenia,cp,entidad_federativa,ciudad,colonia,direccion,num_tarjeta,mes_expiracion,anio_expiracion,cvv,tipo_tarjeta")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientes).State = EntityState.Modified;
                db.SaveChanges();  
            }
            return RedirectToAction("Index");
        }
        public ActionResult Invalida()
        {
            return View();
        }

        public ActionResult BorraUser()
        {
            string idUser = User.Identity.AuthenticationType;
            return RedirectToAction("Delete", "Account", routeValues: new {id = idUser});
        }


        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clientes clientes = db.Clientes.Find(id);
            db.Clientes.Remove(clientes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool Tarjeta (string tarj,string tipo, string mes, string anio, string cvv)
        {
            //llamar al metodo luhn para verificar que es un numero valido de tarjeta
            bool retorna = validaTarj(tarj);
            if (retorna)
            {
                if ((tarj.StartsWith("4")) && (tipo.Equals("Visa")))
                {
                    retorna = true;
                }
                else
                {
                    if ((tarj.StartsWith("5")) && (tipo.Equals("Mastercard")))
                    {
                        retorna = true;
                    }
                    else
                    {
                        if ((tarj.StartsWith("3")) && (tipo.Equals("American")))
                        {
                            retorna = true;
                        }
                        else
                        {
                            return false;
                        }

                    }

                }
                DateTime hoy = new DateTime();
                if(Convert.ToInt32(anio) >= hoy.Year)
                {
                    if (Convert.ToInt32(mes) > hoy.Month)
                    {
                        retorna = true;
                    }
                    else
                    {
                        retorna = false;
                    }   
                }
                else
                {
                    retorna = false;
                }
            }
            return retorna;
        }

        private bool validaTarj(string tarj)
        {
            bool retorna = true;
            StringBuilder digitOnly = new StringBuilder();
            foreach (Char c in tarj)
            {
                if (Char.IsDigit(c)) digitOnly.Append(c);               
            };
            if (digitOnly.Length > 18 || digitOnly.Length < 15) return false;
            int sum = 0;
            int digit = 0;
            int addend = 0;
            bool timesTwo = false;

            for(int i = digitOnly.Length -1; i >= 0; i--) 
            {
                digit = Int32.Parse(digitOnly.ToString(i, 1));
                if (timesTwo)
                {
                    addend = digit * 2;
                    if (addend > 9)
                        addend -= 9;
                }
                else
                    addend = digit;
                sum += addend;
                timesTwo = !timesTwo;
            }
            retorna = ((sum % 10) == 0);
            return retorna;

        }
        private bool validaPago(string nombre, string entidad_federativa, string cp, string ciudad, string colonia, string direccion)
        {
            bool retorna = true;
            return retorna;
        }
    }
    
}
