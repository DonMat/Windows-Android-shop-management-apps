package com.mo.MgRsklep.MgR_App;

import android.app.Activity;
import android.app.Dialog;
import android.app.Fragment;
import android.content.Context;
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
import android.widget.Button;
import android.widget.ListView;
import android.widget.NumberPicker;
import android.widget.NumberPicker.OnValueChangeListener;
import android.widget.Toast;

import com.mo.MgRsklep.Adapters.Produkt.Produkt;
import com.mo.MgRsklep.Adapters.Produkt.ProduktyAdapter;
import com.mo.MgRsklep.MySQL.GetProdukty;
import com.mo.MgRsklep.MySQL.ProduktyListener;
import com.mo.MgRsklep.MySQL.SetZamowienie;

import java.util.ArrayList;

public class Produkty extends Fragment implements OnValueChangeListener {

    public static final String IMAGE_RESOURCE_ID = "iconResourceID";
    public static final String ITEM_NAME = "itemName";
    public static final String LOGGED_USER = "userID";
    ListView list;
    ArrayList<Produkt> produkty;
    Context context;
    int userID;
    int ilosc;
    Button b1;
    ProduktyAdapter adapter;
    private OnItemClickListener itemClick = new OnItemClickListener() {

        @Override
        public void onItemClick(AdapterView<?> parent, View view, int position,
                                long id) {
            show(position);
            Log.i("lista", "Item:" + position);
        }
    };
    private ProduktyListener asyncListener = new ProduktyListener() {

        @Override
        public void getProducts(ArrayList<Produkt> produktList) {
            produkty = produktList;
            adapter = new ProduktyAdapter(context, R.layout.produkt_jeden, produkty);
            list.setAdapter(adapter);
            list.setOnItemClickListener(itemClick);

        }
    };

    public Produkty() {
        produkty = new ArrayList<Produkt>();
    }

    @Override
    public void onAttach(Activity activity) {
        super.onAttach(activity);
        context = activity.getApplicationContext();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.produkty_lay, container, false);

        userID = getArguments().getInt(LOGGED_USER);

        list = (ListView) view.findViewById(R.id.listView);

        GetProdukty getProdukty = new GetProdukty(getActivity());
        getProdukty.setListener(asyncListener);

        return view;
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
            final Produkt produktZam = (Produkt) list.getItemAtPosition(poz);

            final Dialog dialog = new Dialog(getActivity());
            dialog.setTitle(getActivity().getResources().getString(
                    R.string.podaj_liczb_sztuk));
            dialog.setContentView(R.layout.picker_dialog);
            b1 = (Button) dialog.findViewById(R.id.kup);
            Button b2 = (Button) dialog.findViewById(R.id.Anuluj);
            final NumberPicker np = (NumberPicker) dialog
                    .findViewById(R.id.numberPickerIlosc);
            np.setMaxValue(produktZam.getIlosc());
            np.setMinValue(0);
            np.setWrapSelectorWheel(true); // false
            np.setOnValueChangedListener(this);
            b1.setEnabled(false);
            b1.setOnClickListener(new OnClickListener() {
                @Override
                public void onClick(View v) {

                    SetZamowienie t = new SetZamowienie(userID, ilosc,
                            produktZam.getId(), getActivity());
                    produktZam.setIlosc(ilosc);
                    adapter.notifyDataSetChanged();

                    dialog.dismiss();
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
                    dialog.dismiss();
                }
            });
            dialog.show();
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
            b1.setEnabled(false);
        } else {
            b1.setEnabled(true);
        }
    }
}