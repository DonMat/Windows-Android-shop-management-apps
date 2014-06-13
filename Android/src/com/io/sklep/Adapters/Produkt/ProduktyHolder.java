package com.io.sklep.Adapters.Produkt;


import android.view.View;
import android.widget.TextView;

import com.io.sklep.klientapp.R;


public class ProduktyHolder {
	TextView nazwa;
	TextView cena;
	TextView ilosc;
	
	public ProduktyHolder(View w) {
		nazwa = (TextView) w.findViewById(R.id.nazwa);
		cena = (TextView) w.findViewById(R.id.cena);
		ilosc = (TextView) w.findViewById(R.id.ilosc);
	}
	
	
}
