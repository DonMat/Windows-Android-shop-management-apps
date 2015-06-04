package com.mo.MgRsklep.MySQL;

import java.util.ArrayList;

import com.mo.MgRsklep.Adapters.Produkt.Zamowienie;

public interface ZamowieniaListener {
	void getZamowienia(ArrayList<Zamowienie> zamownienie);
}
