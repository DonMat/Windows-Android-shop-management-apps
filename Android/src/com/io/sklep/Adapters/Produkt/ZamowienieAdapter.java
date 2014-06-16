package com.io.sklep.Adapters.Produkt;

import java.util.List;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import com.io.sklep.klientapp.R;

public class ZamowienieAdapter extends ArrayAdapter<Zamowienie>{
	Context context;
	List<Zamowienie> elementy;
	
	public ZamowienieAdapter(Context context, 
			int textViewResourceId, List<Zamowienie> objects) {
		super(context, textViewResourceId, objects);
		elementy = objects;
		this.context = context;
	}
	
	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		View wiersz = convertView;
		ZamowienieHolder holder = null;
		
		if(wiersz == null)
		{
			if(context == null) Log.i("111", "null");
			LayoutInflater inf = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
			wiersz = inf.inflate(R.layout.zamowienie_jeden, parent, false);
			holder = new ZamowienieHolder(wiersz);
			wiersz.setTag(holder);
		}
		else
		{
			holder = (ZamowienieHolder) wiersz.getTag();
		}
		
		holder.cena.setText( elementy.get(position).ilosc + context.getString(R.string.szt)+ " x " +elementy.get(position).cena+" = "+ elementy.get(position).wartosc + context.getString(R.string.zl));
		holder.adres.setText(context.getString(R.string.dostawa)+ elementy.get(position).adres + ( (elementy.get(position).dataRealizacji != null)?context.getResources().getString(R.string._zrealizowano_)+(elementy.get(position).dataRealizacji):context.getResources().getString(R.string.zamowiono)+(elementy.get(position).data)) );
		holder.idNazwa.setText(context.getString(R.string.zam_wienie_nr_)+elementy.get(position).idZam +" - " + elementy.get(position).nazwa);
		
		return wiersz;		
	}

}
