package com.io.sklep.klientapp;

import java.util.ArrayList;
import java.util.concurrent.ExecutionException;


import android.app.Fragment;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.TextView;

import com.io.sklep.Adapters.Produkt.Produkt;
import com.io.sklep.Adapters.Produkt.ProduktyAdapter;
import com.io.sklep.MySQL.GetProdukty;
 
public class Szukaj extends Fragment implements OnItemSelectedListener{
	  ListView lista;
	  ProduktyAdapter adapterProdukt;
	  public ArrayList<String> lista_kat;
	  public ArrayList<String> lista_kat_id;
	  public static final String ITEM_NAME = "kategorie_lista";
	  public static final String ITEM_id = "kategorie_id";
	  ArrayList<Produkt> produkt;
      public Szukaj() {
    	  produkt = new ArrayList<Produkt>();
      }
 
      @Override
      public View onCreateView(LayoutInflater inflater, ViewGroup container,
                  Bundle savedInstanceState) {
 
            View view = inflater.inflate(R.layout.szukaj, container,
                        false);        
            lista_kat = getArguments().getStringArrayList(ITEM_NAME);
            lista_kat_id = getArguments().getStringArrayList(ITEM_id);
            
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(getActivity(), R.layout.spiner_item, lista_kat);
            Spinner list = (Spinner) view.findViewById(R.id.listaKat);
            list.setAdapter(adapter);           
            list.setOnItemSelectedListener(this);
            
            lista = (ListView) view.findViewById(R.id.listView_kategorie);
            
            return view;
      }

	@Override
	public void onItemSelected(AdapterView<?> arg0, View arg1, int arg2,
			long arg3) {
		TextView t = (TextView)arg1;
		String a = t.getText().toString();
		int p = lista_kat.indexOf(a)+1;
		if(p!= 0)
		{
			GetProdukty con = new GetProdukty(p, getActivity());
			try {
				produkt = con.execute().get();
			} catch (InterruptedException e) {
				Log.i("1234", e.getMessage());
			} catch (ExecutionException e) {			
			}
			finally
			{
				adapterProdukt = new ProduktyAdapter(getActivity(), R.layout.produkt_jeden, produkt);
				lista.setAdapter(adapterProdukt);
			}
		}		
	}

	@Override
	public void onNothingSelected(AdapterView<?> arg0) {		
	}
 
}