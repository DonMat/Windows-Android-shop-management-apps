//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Klient.v03
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customers
    {
        public Customers()
        {
            this.Orders = new HashSet<Orders>();
        }
    
        public int Id { get; set; }
        public int Id_konta { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Ulica { get; set; }
        public string Nr_domu { get; set; }
        public string Miasto { get; set; }
        public string Kod_pocztowy { get; set; }
        public string Mail { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
    
        public virtual Accounts Accounts { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}