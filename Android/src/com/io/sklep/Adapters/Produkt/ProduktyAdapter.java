package com.io.sklep.Adapters.Produkt;

import java.util.List;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import com.io.sklep.klientapp.R;

public class ProduktyAdapter extends ArrayAdapter<Produkt>{
	Context context;
	List<Produkt> elementy;
	
	public ProduktyAdapter(Context context, 
			int textViewResourceId, List<Produkt> objects) {
		super(context, textViewResourceId, objects);
		elementy = objects;
		this.context = context;
	}
	
	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		View wiersz = convertView;
		ProduktyHolder holder = null;
		
		if(wiersz == null)
		{
			if(context == null) Log.i("111", "null");
			LayoutInflater inf = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
			wiersz = inf.inflate(R.layout.produkt_jeden, parent, false);
			holder = new ProduktyHolder(wiersz);
			wiersz.setTag(holder);
		}
		else
		{
			holder = (ProduktyHolder) wiersz.getTag();
		}
		
		holder.cena.setText(elementy.get(position).cena+""+context.getString(R.string.zl));
		holder.ilosc.setText(context.getString(R.string.na_stanie_)+""+elementy.get(position).ilosc+""+context.getString(R.string.szt));
		holder.nazwa.setText(elementy.get(position).nazwa);
		
		return wiersz;		
	}

}
