package com.io.sklep.klientapp;

import java.util.ArrayList;
import java.util.List;

import android.app.Activity;
import android.app.Fragment;
import android.app.FragmentManager;
import android.content.res.Configuration;
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

public class Main extends Activity {		 
	      private DrawerLayout mDrawerLayout;
	      private ListView mDrawerList;
	      private ActionBarDrawerToggle mDrawerToggle;
	 
	      private CharSequence mDrawerTitle;
	      private CharSequence mTitle;
	      private DrawerAdapter adapter;
	 
	      private List<Element> elementy;


	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		  
		getActionBar().setDisplayHomeAsUpEnabled(true);
		getActionBar().setHomeButtonEnabled(true);
		  
		  
		elementy = new ArrayList<Element>();
        mTitle = mDrawerTitle = getTitle();
        mDrawerLayout = (DrawerLayout) findViewById(R.id.drawer_layout);
        mDrawerList = (ListView) findViewById(R.id.left_drawer);

        mDrawerLayout.setDrawerShadow(R.drawable.drawer_shadow,
                    GravityCompat.START);
        
        
        elementy.add(new Element("Produkty",R.drawable.ic_launcher));
        elementy.add(new Element("Zamówienia",R.drawable.ic_launcher));
        elementy.add(new Element("Zaloguj",R.drawable.ic_launcher));
        
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
              fragment = new Fragment1();
              args.putString(Fragment1.ITEM_NAME, elementy.get(possition)
                          .getItemName());
              args.putInt(Fragment1.IMAGE_RESOURCE_ID, elementy.get(possition)
                          .getImgResID());
              break;
        case 1:
              fragment = new Fragment1();
              args.putString(Fragment2.ITEM_NAME, elementy.get(possition)
                          .getItemName());
              args.putInt(Fragment1.IMAGE_RESOURCE_ID, elementy.get(possition)
                          .getImgResID());
              break;
        case 2:
              fragment = new LoginForm();
//              args.putString(Fragment1.ITEM_NAME, elementy.get(possition)
//                          .getItemName());
//              args.putInt(Fragment1.IMAGE_RESOURCE_ID, elementy.get(possition)
//                          .getImgResID());
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
		Toast.makeText(getApplicationContext(), "Main", Toast.LENGTH_SHORT).show();
		Element a = elementy.remove(2);
		EditText login = (EditText) findViewById(R.id.eLogin);
		a.napis = login.getText().toString();
		
		elementy.add(2, a);
	}
}
