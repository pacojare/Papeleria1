//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Papeleria2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Ordenes
    {
        public int id { get; set; }
        [Display(Name = "Fecha de creación")]
        public System.DateTime fecha_creacion { get; set; }
        [Display(Name = "Total")]
        public decimal total { get; set; }
        [Display(Name = "Número de Guia")]
        public string num_guia { get; set; }
        [Display(Name = "Fecha de Entrega")]        
        public Nullable<System.DateTime> fecha_entrega { get; set; }
        [Display(Name = "Fecha de Envío")]
        public Nullable<System.DateTime> fecha_envio { get; set; }
        [Display(Name = "Cliente")]
        public int id_cliente { get; set; }
        [Display(Name = "Dirección de Entrega")]
        public string dir_entrega { get; set; }
        [Display(Name = "Paquetería")]
        public Nullable<int> id_paqueteria { get; set; }
        [Display(Name = "Número de confirmación")]
        public string status { get; set; }
    
        public virtual Clientes Clientes { get; set; }
        public virtual Paqueterias Paqueterias { get; set; }
    }
}
