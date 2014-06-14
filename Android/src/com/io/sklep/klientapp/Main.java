package com.io.sklep.klientapp;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutionException;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Fragment;
import android.app.FragmentManager;
import android.content.Context;
import android.content.DialogInterface;
import android.content.res.Configuration;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.support.v4.app.ActionBarDrawerToggle;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Toast;


import com.io.sklep.MD5.MD5;
import com.io.sklep.MySQL.GetKategorie;
import com.io.sklep.MySQL.KategoriaElem;

public class Main extends Activity {
    private DrawerLayout mDrawerLayout;
    private ListView mDrawerList;
    private ActionBarDrawerToggle mDrawerToggle;

    private CharSequence mDrawerTitle;
    private CharSequence mTitle;
    private DrawerAdapter adapter;

    private List<Element> elementy;
    private boolean zalogowany;
    private int idUsera;
    
    private AlertDialog alertDialog;
    private ArrayList<String> kategorie;
    private ArrayList<String> kategorie_id;
    private ArrayList<KategoriaElem> kategorieElem;
    
@Override
protected void onCreate(Bundle savedInstanceState) {
	super.onCreate(savedInstanceState);
	setContentView(R.layout.activity_main);
	  
	getActionBar().setDisplayHomeAsUpEnabled(true);
	getActionBar().setHomeButtonEnabled(true);
	

	zalogowany = false;
	idUsera = -1;
	  
	elementy = new ArrayList<Element>();
	  mTitle = mDrawerTitle = getTitle();
	  mDrawerLayout = (DrawerLayout) findViewById(R.id.drawer_layout);
	  mDrawerList = (ListView) findViewById(R.id.left_drawer);
	
	  mDrawerLayout.setDrawerShadow(R.drawable.drawer_shadow,
	              GravityCompat.START);
	  
	  
	  elementy.add(new Element("Produkty",R.drawable.ic_action_collection));
	  elementy.add(new Element("Szukaj",R.drawable.ic_action_search));
	  elementy.add(new Element("Zamówienia",R.drawable.ic_action_go_to_today));
	  elementy.add(new Element("Zaloguj",R.drawable.ic_action_person));
	  
	  
	  
	  adapter = new DrawerAdapter(this, R.layout.drawer_item, elementy);
	  mDrawerList.setAdapter(adapter);
	  mDrawerList.setOnItemClickListener(new DrawerItemClickListener());
	  
	  mDrawerToggle = new ActionBarDrawerToggle(this, mDrawerLayout,
          R.drawable.ic_drawer, R.string.Otworz,
          R.string.Zamknij) {
    @Override
	public void onDrawerClosed(View view) {
          getActionBar().setTitle(mTitle);
          invalidateOptionsMenu(); // creates call to
                                                    // onPrepareOptionsMenu()
    }

    @Override
	public void onDrawerOpened(View drawerView) {
          getActionBar().setTitle(mDrawerTitle);
          invalidateOptionsMenu(); // creates call to
                                                    // onPrepareOptionsMenu()
    }
};

  mDrawerLayout.setDrawerListener(mDrawerToggle);
  
  if (savedInstanceState == null) {
        SelectItem(0);
  }
  else
  {
  	elementy.get(3).napis = savedInstanceState.getString("Login");
  	zalogowany = savedInstanceState.getBoolean("Log");
  	idUsera = savedInstanceState.getInt("UserID");
  }
}

@Override
public boolean onCreateOptionsMenu(Menu menu) {
	MenuInflater infl = getMenuInflater();
	infl.inflate(R.menu.main_action, menu);
	return super.onCreateOptionsMenu(menu);
}



public void SelectItem(int possition) {
	 
  Fragment fragment = null;
  Bundle args = new Bundle();
  
  if(internet() == false)
  {
  	polaczenie(possition);
  }
  
  switch (possition) {
  case 0:
        fragment = new Produkty();
        
        break;
  case 1:
          pobierzKategorie();
          fragment = new Szukaj();
                      
          
    	  	args.putStringArrayList(Szukaj.ITEM_NAME, kategorie);
    	  	args.putStringArrayList(Szukaj.ITEM_id, kategorie_id);
    	  	
          break;
  case 2:        	
	      fragment = new Zamowienia();
	      args.putString(Zamowienia.ITEM_NAME, elementy.get(possition)
	                  .getItemName());
	      args.putInt(Zamowienia.IMAGE_RESOURCE_ID, elementy.get(possition)
	                  .getImgResID());
	      args.putBoolean(Zamowienia.LOGGED_IN, zalogowany);
	      args.putInt(Zamowienia.LOGGED_USER, idUsera);
	      break;
  case 3:
  	if(zalogowany == false)
        fragment = new LoginForm();
  	else
  	{
  		fragment = new LogOut();
          args.putString(LogOut.ITEM_NAME, elementy.get(possition).getItemName());
  	}
        break;
  case 4:
      fragment = new Fragment1();
      args.putString(Fragment1.ITEM_NAME, elementy.get(possition)
                  .getItemName());
      args.putInt(Fragment1.IMAGE_RESOURCE_ID, elementy.get(possition)
                  .getImgResID());
      break;
  default:
        break;
  }

  fragment.setArguments(args);
  FragmentManager frgManager = getFragmentManager();
  frgManager.beginTransaction().replace(R.id.content_frame, fragment)
              .commit();

  mDrawerList.setItemChecked(possition, true);
  setTitle(elementy.get(possition).getItemName());
  mDrawerLayout.closeDrawer(mDrawerList);

}
@Override
public void setTitle(CharSequence title) {
    mTitle = title;
    getActionBar().setTitle(mTitle);
}

@Override
public boolean onOptionsItemSelected(MenuItem item) {
    // The action bar home/up action should open or close the drawer.
    // ActionBarDrawerToggle will take care of this.
    if (mDrawerToggle.onOptionsItemSelected(item)) {
          return true;
    }
    else
    {
  	  switch(item.getItemId())
  	  {
  	  	case R.id.szukaj : 
  	  		SelectItem(1);
  	  		break;
  	  		
  	  	default: return false;
  	  }
    }
    return false;
}

@Override
public void onConfigurationChanged(Configuration newConfig) {
    super.onConfigurationChanged(newConfig);
    // Pass any configuration change to the drawer toggles
    mDrawerToggle.onConfigurationChanged(newConfig);
}



private class DrawerItemClickListener implements ListView.OnItemClickListener {
@Override
public void onItemClick(AdapterView<?> parent, View view, int position,
        long id) {
  SelectItem(position);

}
}
public void Login(View w) {
	EditText pass = (EditText) findViewById(R.id.ePassword);
	EditText login =  (EditText) findViewById(R.id.eLogin);
	String pas= (pass.getText().toString());
	MD5 m = new MD5(pas);

	String nazwa = login.getText().toString();
	if(m.check(nazwa,Main.this) == true)
	{    		
		elementy.get(3).napis = getString(R.string.zalogowany)+nazwa;		  		
		zalogowany = true;
		idUsera = m.id;
		adapter.notifyDataSetChanged();
		SelectItem(3);
	}
	else
	{
		Toast.makeText(getApplicationContext(), R.string.zleHaslo, Toast.LENGTH_SHORT).show();
	}
}

public void LogOut(View w) {
	zalogowany = false;
	elementy.get(3).napis = getString(R.string.zaloguj);
	idUsera = -1;
	adapter.notifyDataSetChanged();
	SelectItem(3);
}
@Override
protected void onSaveInstanceState(Bundle outState) {
	outState.putString("Login", elementy.get(3).napis);
	outState.putBoolean("Log", zalogowany);
	outState.putInt("UserID", idUsera);
super.onSaveInstanceState(outState);

}

private boolean internet() {
  ConnectivityManager connectivityManager = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
  NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
  return (activeNetworkInfo != null && activeNetworkInfo.isConnected());
}

@SuppressWarnings("deprecation")
public void polaczenie(final int poz)
{
	alertDialog = new AlertDialog.Builder(this).create();
	alertDialog.setTitle(getString(R.string.brak_internetu_));
	alertDialog.setMessage(getString(R.string.sprawd_po_aczenie_z_sieci_));
	alertDialog.setIcon(R.drawable.ic_action_network_wifi);
	alertDialog.setCancelable(false);
	
	alertDialog.setButton("Ponów próbę", new DialogInterface.OnClickListener() {
	@Override
	public void onClick(DialogInterface dialog, int which) {
		if(internet())
		{
			alertDialog.dismiss();
			SelectItem(poz);
		}
		else
		{
			polaczenie(poz);
		}  		
	}
	});    	
	alertDialog.show();
}

public void pobierzKategorie()
{
	GetKategorie task = new GetKategorie(Main.this);
	try {
		kategorieElem = task.execute().get();
	} catch (InterruptedException e) {
		Log.i("1234", e.getMessage());
	} catch (ExecutionException e) {
		Log.i("1234", e.getMessage());
	}    	
	kategorie = new ArrayList<String>();
	kategorie_id = new ArrayList<String>();
	
	for(KategoriaElem a: kategorieElem)
	{
		kategorie.add(a.nazwa);
		kategorie_id.add(a.id+"");
	}
}
  }
