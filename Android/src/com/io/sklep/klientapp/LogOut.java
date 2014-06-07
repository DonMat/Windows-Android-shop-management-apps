package com.io.sklep.klientapp;

import android.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

public class LogOut extends Fragment {
	      TextView tvItemName;
	 
	      public static final String ITEM_NAME = "itemName";
	 
	      public LogOut() {
	 
	      }
	 
	      @Override
	      public View onCreateView(LayoutInflater inflater, ViewGroup container,
	                  Bundle savedInstanceState) {
	 
	            View view = inflater.inflate(R.layout.log_out, container,
	                        false);
	 
	            tvItemName = (TextView) view.findViewById(R.id.frag1_text);
	 
	            tvItemName.setText(getString(R.string.ObZalogow)+getArguments().getString(ITEM_NAME));
	            
	            return view;
	      }
}
