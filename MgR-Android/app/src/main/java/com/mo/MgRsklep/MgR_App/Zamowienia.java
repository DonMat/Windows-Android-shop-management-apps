package com.mo.MgRsklep.MgR_App;

import android.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import com.mo.MgRsklep.Adapters.Produkt.Zamowienie;
import com.mo.MgRsklep.Adapters.Produkt.ZamowienieAdapter;
import com.mo.MgRsklep.MySQL.GetZamowienia;
import com.mo.MgRsklep.MySQL.ZamowieniaListener;

import java.util.ArrayList;

public class Zamowienia extends Fragment {

    public static final String IMAGE_RESOURCE_ID = "iconResourceID";
    public static final String ITEM_NAME = "itemName";
    public static final String LOGGED_IN = "zalogowany";
    public static final String LOGGED_USER = "userID";
    ImageView icon;
    TextView nazwa;
    ListView listaZam;
    ArrayList<Zamowienie> zamow;
    ZamowienieAdapter adapter;
    int idU;
    boolean zal;

    public Zamowienia() {

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        View view = inflater.inflate(R.layout.zamowienie, container, false);

        listaZam = (ListView) view.findViewById(R.id.listViewZamowienia);

        icon = (ImageView) view.findViewById(R.id.icon_zam);
        nazwa = (TextView) view.findViewById(R.id.text_zamow);

        nazwa.setText(getArguments().getString(ITEM_NAME));
        icon.setImageDrawable(view.getResources().getDrawable(
                getArguments().getInt(IMAGE_RESOURCE_ID)));

        zal = getArguments().getBoolean(LOGGED_IN);

        if (zal == false) {
            nazwa.setText(nazwa.getText().toString()
                    + view.getResources().getString(R.string.nie_zalogowany));
        } else {
            idU = getArguments().getInt(LOGGED_USER);
            GetZamowienia zamowienia = new GetZamowienia(idU, getActivity());
            zamowienia.setListener(new ZamowieniaListener() {

                @Override
                public void getZamowienia(ArrayList<Zamowienie> zamownienie) {
                    zamow = zamownienie;
                    if (zamow != null) {
                        adapter = new ZamowienieAdapter(getActivity(),
                                R.layout.zamowienie_jeden, zamow);
                        listaZam.setAdapter(adapter);
                    }
                }
            });
        }

        return view;
    }

}