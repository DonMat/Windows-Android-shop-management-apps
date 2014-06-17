package com.io.sklep.klientapp;

import java.util.ArrayList;
import java.util.concurrent.ExecutionException;

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
import android.widget.AdapterView.AdapterContextMenuInfo;
import android.widget.Button;
import android.widget.ListView;
import android.widget.NumberPicker;
import android.widget.NumberPicker.OnValueChangeListener;
import android.widget.Toast;

import com.io.sklep.Adapters.Produkt.Produkt;
import com.io.sklep.Adapters.Produkt.ProduktyAdapter;
import com.io.sklep.MySQL.GetProdukty;
import com.io.sklep.MySQL.SetZamowienie;

public class Produkty extends Fragment implements OnValueChangeListener {

	ListView list;
	ArrayList<Produkt> produkty;
	Context c;
	int userID;
	int ilosc;
	Button b1;
	ProduktyAdapter adapter;
	public static final String IMAGE_RESOURCE_ID = "iconResourceID";
	public static final String ITEM_NAME = "itemName";
	public static final String LOGGED_USER = "userID";

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
		View view = inflater.inflate(R.layout.produkty_lay, container, false);

		// produkty.add(new Produkt("Prod1",12.3,5,0,0));
		// produkty.add(new Produkt("Prod2",22.2,1,0,0));
		// produkty.add(new Produkt("Prod3",112.3,15,0,0));
		// produkty.add(new Produkt("Prod11",112.3,15,0,0));
		// produkty.add(new Produkt("Prod21",122.2,11,0,0));
		// produkty.add(new Produkt("Prod31",1112.3,115,0,0));
		// produkty.add(new Produkt("Prod12",212.3,25,0,0));
		// produkty.add(new Produkt("Prod22",222.2,21,0,0));
		// produkty.add(new Produkt("Prod32",2112.3,215,0,0));
		// produkty.add(new Produkt("Prod13",312.3,35,0,0));
		// produkty.add(new Produkt("Prod23",322.2,31,0,0));
		// produkty.add(new Produkt("Prod33",3112.3,315,0,0));
		userID = getArguments().getInt(LOGGED_USER);

		GetProdukty t = new GetProdukty(getActivity());
		try {
			produkty = t.execute().get();
		} catch (InterruptedException e) {
			Log.i("1234", e.getMessage());
		} catch (ExecutionException e) {
		}

		list = (ListView) view.findViewById(R.id.listView);

		adapter = new ProduktyAdapter(c, R.layout.produkt_jeden, produkty);
		list.setAdapter(adapter);

		registerForContextMenu(list);

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
			final Produkt a = (Produkt) list.getItemAtPosition(poz);

			final Dialog d = new Dialog(getActivity());
			d.setTitle(getActivity().getResources().getString(
					R.string.podaj_liczb_sztuk));
			d.setContentView(R.layout.picker_dialog);
			b1 = (Button) d.findViewById(R.id.kup);
			Button b2 = (Button) d.findViewById(R.id.Anuluj);
			final NumberPicker np = (NumberPicker) d
					.findViewById(R.id.numberPickerIlosc);
			np.setMaxValue(a.getIlosc());
			np.setMinValue(0);
			np.setWrapSelectorWheel(false);
			np.setOnValueChangedListener(this);
			b1.setEnabled(false);
			b1.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {

					SetZamowienie t = new SetZamowienie(userID, ilosc, a
							.getId(), getActivity());
					a.setIlosc(ilosc);
					adapter.notifyDataSetChanged();
					t.execute();

					d.dismiss();
					Toast.makeText(getActivity(),
							"Zamowiono: " + np.getValue(), Toast.LENGTH_SHORT)
							.show();
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
			b1.setEnabled(false);
		} else {
			b1.setEnabled(true);
		}
	}
}