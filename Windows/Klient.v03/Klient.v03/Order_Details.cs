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
    
    public partial class Order_Details
    {
        public int Zamowienie_id { get; set; }
        public int Produkt_id { get; set; }
        public int Ilosc { get; set; }
        public string Miasto_dostawy { get; set; }
        public string Adres_dostawy { get; set; }
        public string Kod_pocztowy_dostawy { get; set; }
    
        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }
}