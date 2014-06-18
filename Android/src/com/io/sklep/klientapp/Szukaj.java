package com.io.sklep.klientapp;

import java.util.ArrayList;
import java.util.concurrent.ExecutionException;

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
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.NumberPicker;
import android.widget.NumberPicker.OnValueChangeListener;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.io.sklep.Adapters.Produkt.Produkt;
import com.io.sklep.Adapters.Produkt.ProduktyAdapter;
import com.io.sklep.MySQL.GetProdukty;
import com.io.sklep.MySQL.SetZamowienie;

public class Szukaj extends Fragment implements OnItemSelectedListener,
		OnValueChangeListener {
	ListView lista;
	ProduktyAdapter adapterProdukt;
	public ArrayList<String> lista_kat;
	public ArrayList<String> lista_kat_id;
	public static final String ITEM_NAME = "kategorie_lista";
	public static final String ITEM_id = "kategorie_id";
	public static final String LOGGED_USER = "userID";
	ArrayList<Produkt> produkt;
	int userID;
	int ilosc;
	Button b1;

	public Szukaj() {
		produkt = new ArrayList<Produkt>();
	}

	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		userID = getArguments().getInt(LOGGED_USER);

		View view = inflater.inflate(R.layout.szukaj, container, false);
		lista_kat = getArguments().getStringArrayList(ITEM_NAME);
		lista_kat_id = getArguments().getStringArrayList(ITEM_id);

		ArrayAdapter<String> adapter = new ArrayAdapter<String>(getActivity(),
				R.layout.spiner_item, lista_kat);
		Spinner list = (Spinner) view.findViewById(R.id.listaKat);
		list.setAdapter(adapter);
		list.setOnItemSelectedListener(this);

		lista = (ListView) view.findViewById(R.id.listView_kategorie);

		return view;
	}

	@Override
	public void onItemSelected(AdapterView<?> arg0, View arg1, int arg2,
			long arg3) {
		TextView t = (TextView) arg1;
		String a = t.getText().toString();
		int p = lista_kat.indexOf(a);
		if (p >-1) {
			GetProdukty con = new GetProdukty(Integer.parseInt(lista_kat_id.get(p)), getActivity());
			try {
				produkt = con.execute().get();
			} catch (InterruptedException e) {
				Log.i("1234", e.getMessage());
			} catch (ExecutionException e) {
			} finally {
				adapterProdukt = new ProduktyAdapter(getActivity(),
						R.layout.produkt_jeden, produkt);
				lista.setAdapter(adapterProdukt);
				registerForContextMenu(lista);
			}
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
					adapterProdukt.notifyDataSetChanged();
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