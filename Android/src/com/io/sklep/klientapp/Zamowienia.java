package com.io.sklep.klientapp;

import java.util.ArrayList;
import java.util.concurrent.ExecutionException;

import android.app.Fragment;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import com.io.sklep.Adapters.Produkt.Zamowienie;
import com.io.sklep.Adapters.Produkt.ZamowienieAdapter;
import com.io.sklep.MySQL.GetZamowienia;
 
public class Zamowienia extends Fragment {
 
      ImageView icon;
      TextView nazwa;
      ListView listaZam;
      ArrayList<Zamowienie> zamow;
      ZamowienieAdapter adapter;
      int idU;
      boolean zal;
      
      public static final String IMAGE_RESOURCE_ID = "iconResourceID";
      public static final String ITEM_NAME = "itemName";
      public static final String LOGGED_IN = "zalogowany";
      public static final String LOGGED_USER = "userID";
      
      public Zamowienia()
      {
 
      }
 
      @Override
      public View onCreateView(LayoutInflater inflater, ViewGroup container,
                  Bundle savedInstanceState) {
 
            View view=inflater.inflate(R.layout.zamowienie,container, false);
 
            icon=(ImageView)view.findViewById(R.id.icon_zam);
            nazwa=(TextView)view.findViewById(R.id.text_zamow);
 
            nazwa.setText(getArguments().getString(ITEM_NAME));
            icon.setImageDrawable(view.getResources().getDrawable(
                        getArguments().getInt(IMAGE_RESOURCE_ID)));
            
            zal = getArguments().getBoolean(LOGGED_IN);
            
            if(zal == false)
            {
            	nazwa.setText(nazwa.getText().toString()+view.getResources().getString(R.string.nie_zalogowany));
            }
            else
            {
            	zamow = new ArrayList<Zamowienie>();            	
            	listaZam = (ListView) view.findViewById(R.id.listViewZamowienia);
            	
            	idU = getArguments().getInt(LOGGED_USER);
            	
            	GetZamowienia t = new GetZamowienia(idU, getActivity());
            	try {
					zamow = t.execute().get();
        		} catch (InterruptedException e) {
        			Log.i("1234", e.getMessage());
        		} catch (ExecutionException e) {
        			Log.i("1234", e.getMessage());
        		}
            	
            	if(zamow != null)
            	{
            		adapter = new ZamowienieAdapter(getActivity(), R.layout.zamowienie_jeden, zamow);
            		listaZam.setAdapter(adapter);
            	}
            	
            }
            
            return view;
      }
 
}