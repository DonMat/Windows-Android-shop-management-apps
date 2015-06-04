package com.mo.MgRsklep.MgR_App;

import android.app.Fragment;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.mo.MgRsklep.MySQL.GetUserInfo;
import com.mo.MgRsklep.MySQL.UserInfoListener;

public class LogOut extends Fragment {
    public static final String ITEM_NAME = "itemName";
    String nazwaU;
    TextView imieNazw;
    TextView ulica;
    TextView miasto;
    TextView mail;
    TextView tel;

    public LogOut() {

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        View view = inflater.inflate(R.layout.log_out, container, false);
        nazwaU = getArguments().getString(ITEM_NAME).substring(1);

        imieNazw = (TextView) view.findViewById(R.id.lImienazw);
        ulica = (TextView) view.findViewById(R.id.ulicadom);
        miasto = (TextView) view.findViewById(R.id.kodmiasto);
        mail = (TextView) view.findViewById(R.id.mail);
        tel = (TextView) view.findViewById(R.id.telef);

        pobierz();

        return view;
    }

    protected void pobierz() {
        GetUserInfo user = new GetUserInfo(nazwaU, getActivity());
        user.setUserInfoListener(new UserInfoListener() {
            @Override
            public void getUserInfo(String[] a) {
                Log.i("info", "user info List " + a[0]);

                if (a[0] == null)
                    Toast.makeText(getActivity(),
                            getString(R.string.retryInfo), Toast.LENGTH_SHORT)
                            .show();

                imieNazw.setText(a[0]);
                ulica.setText(a[1]);
                miasto.setText(a[2]);
                mail.setText(a[3]);
                tel.setText(a[4]);

            }
        });

       // user.execute();
    }
}
