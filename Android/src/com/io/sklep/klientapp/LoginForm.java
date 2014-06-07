package com.io.sklep.klientapp;

import android.app.Fragment;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;

public class LoginForm extends Fragment {

    public Button button;
    public EditText edit;
    final String match = "[A-Za-z\\d]+";

    public LoginForm() {

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                Bundle savedInstanceState) {

          View view = inflater.inflate(R.layout.login_form, container,
                      false);
          button = (Button) view.findViewById(R.id.bLogi);
          edit = (EditText) view.findViewById(R.id.eLogin);
          
          edit.addTextChangedListener(new TextWatcher() {
			
			@Override
			public void onTextChanged(CharSequence s, int start, int before, int count) {
				// TODO Auto-generated method stub
				
				String a = edit.getText().toString();
				if(! a.matches(match))
					button.setEnabled(false);
				else
					button.setEnabled(true);
			}
			
			@Override
			public void beforeTextChanged(CharSequence s, int start, int count,
					int after) {
				// TODO Auto-generated method stub
				
			}
			
			@Override
			public void afterTextChanged(Editable s) {
				// TODO Auto-generated method stub
				
			}
		});
                   
          return view;
    }

}
