package com.io.sklep.klientapp;

import java.util.ArrayList;
import java.util.concurrent.ExecutionException;


import android.app.Activity;
import android.app.Fragment;
import android.content.Context;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.io.sklep.Adapters.Produkt.Produkt;
import com.io.sklep.Adapters.Produkt.ProduktyAdapter;
import com.io.sklep.MySQL.GetProdukty;
 
public class Produkty extends Fragment {
	 
		ListView list;
		ArrayList<Produkt> produkty;
		Context c;
	      public static final String IMAGE_RESOURCE_ID = "iconResourceID";
	      public static final String ITEM_NAME = "itemName";
	 
	      public Produkty() {
	    	  produkty = new ArrayList<Produkt>();
	      }
	      
		 @Override
		public void onAttach(Activity activity) {
				super.onAttach(activity);
			 c = activity.getApplicationContext();
		}
		 
	      @Override
	      public View onCreateView(LayoutInflater inflater, ViewGroup container,
	                  Bundle savedInstanceState) { 
	            View view = inflater.inflate(R.layout.produkty_lay, container,false);
	 
//	            produkty.add(new Produkt("Prod1",12.3,5,0,0));
//	            produkty.add(new Produkt("Prod2",22.2,1,0,0));
//	            produkty.add(new Produkt("Prod3",112.3,15,0,0));
//	            produkty.add(new Produkt("Prod11",112.3,15,0,0));
//	            produkty.add(new Produkt("Prod21",122.2,11,0,0));
//	            produkty.add(new Produkt("Prod31",1112.3,115,0,0));
//	            produkty.add(new Produkt("Prod12",212.3,25,0,0));
//	            produkty.add(new Produkt("Prod22",222.2,21,0,0));
//	            produkty.add(new Produkt("Prod32",2112.3,215,0,0));
//	            produkty.add(new Produkt("Prod13",312.3,35,0,0));
//	            produkty.add(new Produkt("Prod23",322.2,31,0,0));
//	            produkty.add(new Produkt("Prod33",3112.3,315,0,0));
	            
	            GetProdukty t = new GetProdukty(getActivity());
	            try {
					produkty = t.execute().get();
	    		} catch (InterruptedException e) {
	    			Log.i("1234", e.getMessage());
	    		} catch (ExecutionException e) {			
	    		}
	            
	            list = (ListView) view.findViewById(R.id.listView);
	            
	            ProduktyAdapter adapter = new ProduktyAdapter(c, R.layout.produkt_jeden, produkty);
	            list.setAdapter(adapter);
	            
	            return view;
	      }
 
}