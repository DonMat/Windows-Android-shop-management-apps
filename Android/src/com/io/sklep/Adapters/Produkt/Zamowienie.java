package com.io.sklep.Adapters.Produkt;

import java.sql.Date;


public class Zamowienie {
	int idZam;	
	String data;
	String nazwa;		
	int ilosc;
	double cena;
	double wartosc;
	String adres;
	Date dataRealizacji;
	
	public Zamowienie(int idZam, String data, String nazwa, int ilosc,
			double cena, double wartosc, String adres, Date a) {
		super();
		this.idZam = idZam;
		this.data = data;
		this.nazwa = nazwa;
		this.ilosc = ilosc;
		this.cena = cena;
		this.wartosc = wartosc;
		this.adres = adres;
		this.dataRealizacji = a;
	}
}
