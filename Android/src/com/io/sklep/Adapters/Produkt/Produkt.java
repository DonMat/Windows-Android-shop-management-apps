package com.io.sklep.Adapters.Produkt;


public class Produkt {
	String nazwa;
	double cena;
	int ilosc;
	int kategoria;
	int id;
	public Produkt(String nazwa, double cena, int ilosc, int kategoria, int id) {
		super();
		this.nazwa = nazwa;
		this.cena = cena;
		this.ilosc = ilosc;
		this.kategoria = kategoria;
		this.id = id;
	}
	public String getNazwa() {
		return nazwa;
	}
	public double getCena() {
		return cena;
	}
	public int getIlosc() {
		return ilosc;
	}
	public int getKategoria() {
		return kategoria;
	}
	public int getId() {
		return id;
	}
	public void setIlosc(int ilosc) {
		this.ilosc -=ilosc ;
	}
}
