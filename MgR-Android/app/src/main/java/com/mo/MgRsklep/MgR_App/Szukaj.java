package com.mo.MgRsklep.MgR_App;

import android.app.Dialog;
import android.app.Fragment;
import android.os.Bundle;
import android.util.Log;
import android.view.ContextMenu;
import android.view.ContextMenu.ContextMenuInfo;
import android.view.LayoutInflater;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.AdapterContextMenuInfo;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.NumberPicker;
import android.widget.NumberPicker.OnValueChangeListener;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.mo.MgRsklep.Adapters.Produkt.Produkt;
import com.mo.MgRsklep.Adapters.Produkt.ProduktyAdapter;
import com.mo.MgRsklep.MySQL.GetKategorie;
import com.mo.MgRsklep.MySQL.GetProdukty;
import com.mo.MgRsklep.MySQL.KategoriaElem;
import com.mo.MgRsklep.MySQL.KategorieListener;
import com.mo.MgRsklep.MySQL.ProduktyListener;
import com.mo.MgRsklep.MySQL.SetZamowienie;

import java.util.ArrayList;

public class Szukaj extends Fragment implements OnItemSelectedListener,
        OnValueChangeListener {
    public static final String ITEM_NAME = "kategorie_lista";
    public static final String ITEM_id = "kategorie_id";
    public static final String LOGGED_USER = "userID";
    public ArrayList<String> lista_kat;
    public ArrayList<String> lista_kat_id;
    int userID;
    int ilosc;
    private ListView lista;
    private ProduktyAdapter adapterProdukt;
    private ArrayList<Produkt> produkt;
    private Button button1;

    private ArrayAdapter<String> adapter;
    private Spinner listKategori;
    private ProduktyListener asyncListener = new ProduktyListener() {

        @Override
        public void getProducts(ArrayList<Produkt> produktList) {
            produkt = produktList;
            adapterProdukt = new ProduktyAdapter(getActivity(),
                    R.layout.produkt_jeden, produkt);
            lista.setAdapter(adapterProdukt);
        }
    };
    private OnItemClickListener itemClick = new OnItemClickListener() {

        @Override
        public void onItemClick(AdapterView<?> parent, View view, int position,
                                long id) {
            show(position);
            Log.i("lista", "Item:" + position);
        }
    };
    private KategorieListener listener = new KategorieListener() {

        @Override
        public void getKategorie(ArrayList<KategoriaElem> kategorieElem) {

            lista_kat = new ArrayList<String>();
            lista_kat_id = new ArrayList<String>();

            for (KategoriaElem a : kategorieElem) {
                lista_kat.add(a.nazwa);
                lista_kat_id.add(a.id + "");
            }

            adapter = new ArrayAdapter<String>(getActivity(),
                    R.layout.spiner_item, lista_kat);
            listKategori.setAdapter(adapter);
        }
    };

    public Szukaj() {
        produkt = new ArrayList<Produkt>();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        userID = getArguments().getInt(LOGGED_USER);

        View view = inflater.inflate(R.layout.szukaj, container, false);

        Log.i("debSz", "szukaj " + (lista_kat == null));

        listKategori = (Spinner) view.findViewById(R.id.listaKat);
        listKategori.setOnItemSelectedListener(this);

        lista = (ListView) view.findViewById(R.id.listView_kategorie);
        lista.setOnItemClickListener(itemClick);
        pobierzKategorie();
        return view;
    }

    @Override
    public void onItemSelected(AdapterView<?> arg0, View arg1, int arg2,
                               long arg3) {
        TextView t = (TextView) arg1;
        String a = t.getText().toString();
        int p = lista_kat.indexOf(a);
        if (p > -1) {

            GetProdukty getProdukty = new GetProdukty(
                    Integer.parseInt(lista_kat_id.get(p)), getActivity());
            getProdukty.setListener(asyncListener);
            //getProdukty.execute();
        }
    }

    @Override
    public void onNothingSelected(AdapterView<?> arg0) {
    }

    @Override
    public void onCreateContextMenu(ContextMenu menu, View v,
                                    ContextMenuInfo menuInfo) {
        super.onCreateContextMenu(menu, v, menuInfo);
        MenuInflater infl = getActivity().getMenuInflater();
        infl.inflate(R.menu.kontekstowe, menu);

    }

    @Override
    public boolean onContextItemSelected(MenuItem item) {
        if (item.getItemId() == R.id.konZamow) {
            AdapterContextMenuInfo info = (AdapterContextMenuInfo) item
                    .getMenuInfo();
            show(info.position);
        }
        return super.onContextItemSelected(item);
    }

    public void show(int poz) {
        if (userID > 0) {
            final Produkt a = (Produkt) lista.getItemAtPosition(poz);

            final Dialog d = new Dialog(getActivity());
            d.setTitle(getActivity().getResources().getString(
                    R.string.podaj_liczb_sztuk));
            d.setContentView(R.layout.picker_dialog);
            button1 = (Button) d.findViewById(R.id.kup);
            Button b2 = (Button) d.findViewById(R.id.Anuluj);
            final NumberPicker np = (NumberPicker) d
                    .findViewById(R.id.numberPickerIlosc);
            np.setMaxValue(a.getIlosc());
            np.setMinValue(0);
            np.setWrapSelectorWheel(false);
            np.setOnValueChangedListener(this);
            button1.setEnabled(false);
            button1.setOnClickListener(new OnClickListener() {
                @Override
                public void onClick(View v) {

                    SetZamowienie t = new SetZamowienie(userID, ilosc, a
                            .getId(), getActivity());
                    a.setIlosc(ilosc);
                    adapterProdukt.notifyDataSetChanged();
                    //t.execute();

                    d.dismiss();
                }
            });
            b2.setOnClickListener(new OnClickListener() {
                @Override
                public void onClick(View v) {
                    Toast.makeText(
                            getActivity(),
                            getActivity().getResources().getString(
                                    R.string.anulowano), Toast.LENGTH_SHORT)
                            .show();
                    d.dismiss();
                }
            });
            d.show();
        } else {
            Toast.makeText(
                    getActivity(),
                    getActivity().getResources().getString(
                            R.string.zaloguj_si_aby_z_o_y_zam_wienie),
                    Toast.LENGTH_SHORT).show();
        }
    }

    @Override
    public void onValueChange(NumberPicker picker, int oldVal, int newVal) {
        ilosc = newVal;
        if (newVal == 0) {
            button1.setEnabled(false);
        } else {
            button1.setEnabled(true);
        }

    }

    public void pobierzKategorie() {
        GetKategorie getKat = new GetKategorie(getActivity());
        getKat.setKategorieListener(listener);
        //getKat.execute();
    }
}