package com.io.sklep.Adapters.Produkt;


public class Zamowienie {
		int idZam;	
		String data;
		String nazwa;		
		int ilosc;
		double cena;
		double wartosc;
		String adres;
		
		public Zamowienie(int idZam, String data, String nazwa, int ilosc,
				double cena, double wartosc, String adres) {
			super();
			this.idZam = idZam;
			this.data = data;
			this.nazwa = nazwa;
			this.ilosc = ilosc;
			this.cena = cena;
			this.wartosc = wartosc;
			this.adres = adres;
		}
}
