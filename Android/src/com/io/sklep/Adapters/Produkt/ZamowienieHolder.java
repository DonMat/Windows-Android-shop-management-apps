package com.io.sklep.Adapters.Produkt;

import android.view.View;
import android.widget.TextView;

import com.io.sklep.klientapp.R;


public class ZamowienieHolder {
	TextView idNazwa;
	TextView cena;
	TextView adres;
	
	public ZamowienieHolder(View w) {
		idNazwa = (TextView) w.findViewById(R.id.idData);
		cena = (TextView) w.findViewById(R.id.nazwIloscWartos);
		adres = (TextView) w.findViewById(R.id.adres);
	}
}
