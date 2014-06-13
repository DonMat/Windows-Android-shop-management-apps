package com.io.sklep.klientapp;

import java.util.ArrayList;
import java.util.List;

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
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Toast;

import com.io.sklep.MD5.MD5;

public class Main extends Activity {
	      private DrawerLayout mDrawerLayout;
	      private ListView mDrawerList;
	      private ActionBarDrawerToggle mDrawerToggle;
	 
	      private CharSequence mDrawerTitle;
	      private CharSequence mTitle;
	      private DrawerAdapter adapter;
	 
	      private List<Element> elementy;
	      private boolean zalogowany;
	      
	      private AlertDialog alertDialog;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		  
		getActionBar().setDisplayHomeAsUpEnabled(true);
		getActionBar().setHomeButtonEnabled(true);
		zalogowany = false;
		  
		elementy = new ArrayList<Element>();
        mTitle = mDrawerTitle = getTitle();
        mDrawerLayout = (DrawerLayout) findViewById(R.id.drawer_layout);
        mDrawerList = (ListView) findViewById(R.id.left_drawer);

        mDrawerLayout.setDrawerShadow(R.drawable.drawer_shadow,
                    GravityCompat.START);
        
        
        elementy.add(new Element("Produkty",R.drawable.ic_launcher));
        elementy.add(new Element("Zamówienia",R.drawable.ic_launcher));
        elementy.add(new Element("Zaloguj",R.drawable.ic_action_person));
        
        
        adapter = new DrawerAdapter(this, R.layout.drawer_item, elementy);
        mDrawerList.setAdapter(adapter);
        mDrawerList.setOnItemClickListener(new DrawerItemClickListener());
        
        mDrawerToggle = new ActionBarDrawerToggle(this, mDrawerLayout,
                R.drawable.ic_drawer, R.string.Otworz,
                R.string.Zamknij) {
          public void onDrawerClosed(View view) {
                getActionBar().setTitle(mTitle);
                invalidateOptionsMenu(); // creates call to
                                                          // onPrepareOptionsMenu()
          }
     
          public void onDrawerOpened(View drawerView) {
                getActionBar().setTitle(mDrawerTitle);
                invalidateOptionsMenu(); // creates call to
                                                          // onPrepareOptionsMenu()
          }
    };
    
	    mDrawerLayout.setDrawerListener(mDrawerToggle);
	    
	    if (savedInstanceState == null) {
	          SelectItem(1);
	    }
	    else
	    {
	    	elementy.get(2).napis = savedInstanceState.getString("Login");
	    	zalogowany = savedInstanceState.getBoolean("Log");
	    }
    
	    if(internet() == false)
	    {
	    	polaczenie();
	    }

	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}
	
	
	
	public void SelectItem(int possition) {
		 
        Fragment fragment = null;
        Bundle args = new Bundle();
        switch (possition) {
        case 0:
              fragment = new Produkty();
              
              break;
        case 1:
              fragment = new Fragment2();
              args.putString(Fragment2.ITEM_NAME, elementy.get(possition)
                          .getItemName());
              args.putInt(Fragment2.IMAGE_RESOURCE_ID, elementy.get(possition)
                          .getImgResID());
              break;
        case 2:
        	if(zalogowany == false)
              fragment = new LoginForm();
        	else
        	{
        		fragment = new LogOut();
                args.putString(LogOut.ITEM_NAME, elementy.get(possition).getItemName());
        	}
              break;
        case 3:
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
    		elementy.get(2).napis = getString(R.string.zalogowany)+nazwa;		  		
    		zalogowany = true;
    		adapter.notifyDataSetChanged();
    		SelectItem(2);
    	}
    	else
    	{
    		Toast.makeText(getApplicationContext(), R.string.zleHaslo, Toast.LENGTH_SHORT).show();
    	}
	}
    
    public void LogOut(View w) {
		zalogowany = false;
		elementy.get(2).napis = getString(R.string.zaloguj);
		adapter.notifyDataSetChanged();
		SelectItem(2);
	}
    @Override
    protected void onSaveInstanceState(Bundle outState) {
    	outState.putString("Login", elementy.get(2).napis);
    	outState.putBoolean("Log", zalogowany);
    super.onSaveInstanceState(outState);
    
    }
    
    private boolean internet() {
        ConnectivityManager connectivityManager = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
        return (activeNetworkInfo != null && activeNetworkInfo.isConnected());
    }
    
    @SuppressWarnings("deprecation")
	public void polaczenie()
    {
    	alertDialog = new AlertDialog.Builder(this).create();
    	alertDialog.setTitle(getString(R.string.brak_internetu_));
    	alertDialog.setMessage(getString(R.string.sprawd_po_aczenie_z_sieci_));
    	alertDialog.setCancelable(false);
    	
    	alertDialog.setButton("Ponów próbę", new DialogInterface.OnClickListener() {
    	public void onClick(DialogInterface dialog, int which) {
    		if(internet())
    		{
    			alertDialog.dismiss();
    		}
    		else
    		{
    			polaczenie();
    		}  		
    	}
    	});    	
    	alertDialog.show();
    }
  }
