package com.mo.MgRsklep.MgR_App;

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
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Toast;

import com.mo.MgRsklep.EncryptionMethods.MD5;
import com.mo.MgRsklep.EncryptionMethods.PasswordChecker;
import com.mo.MgRsklep.EncryptionMethods.TestAccount;
import com.mo.MgRsklep.MySQL.PasswordListener;

import java.util.ArrayList;
import java.util.List;

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
    private PasswordChecker passwordChecker;
    private Fragment fragment = null;
    private Bundle args = new Bundle();
    private PasswordListener passwordListener = new PasswordListener() {
        @Override
        public void getPass(String[] password) {
            boolean check = (password[1].equals(password[3]));
            if (check == true) {
                elementy.get(3).napis = getString(R.string.zalogowany) + password[2];
                zalogowany = true;
                idUsera = Integer.parseInt(password[0]);
                adapter.notifyDataSetChanged();
                Toast.makeText(Main.this, getString(R.string.zalogowano),
                        Toast.LENGTH_SHORT).show();
                SelectItem(3);
            } else {
                Toast.makeText(getApplicationContext(), R.string.zleHaslo,
                        Toast.LENGTH_SHORT).show();
            }
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        getActionBar().setDisplayHomeAsUpEnabled(true);
        getActionBar().setHomeButtonEnabled(true);

        zalogowany = false;
        idUsera = -1;

        initMenuElems();

        initDrawer();

        initLogin(savedInstanceState);

        passwordChecker = new PasswordChecker(Main.this, new MD5());
    }

    private void initLogin(Bundle savedInstanceState) {
        if (savedInstanceState == null) {
            SelectItem(3);
        } else {
            elementy.get(3).napis = savedInstanceState.getString("Login");
            zalogowany = savedInstanceState.getBoolean("Log");
            idUsera = savedInstanceState.getInt("UserID");
        }
    }

    private void initDrawer() {
        mDrawerList.setAdapter(adapter);
        mDrawerList.setOnItemClickListener(new DrawerItemClickListener());

        mDrawerToggle = new ActionBarDrawerToggle(this, mDrawerLayout,
                R.drawable.ic_drawer, R.string.Otworz, R.string.Zamknij) {
            public void onDrawerClosed(View view) {
                getActionBar().setTitle(mTitle);
                invalidateOptionsMenu();
            }

            public void onDrawerOpened(View drawerView) {
                getActionBar().setTitle(mDrawerTitle);
                invalidateOptionsMenu();
            }
        };

        mDrawerLayout.setDrawerListener(mDrawerToggle);
    }

    private void initMenuElems() {
        elementy = new ArrayList<Element>();
        mTitle = mDrawerTitle = getTitle();
        mDrawerLayout = (DrawerLayout) findViewById(R.id.drawer_layout);
        mDrawerList = (ListView) findViewById(R.id.left_drawer);

        mDrawerLayout.setDrawerShadow(R.drawable.drawer_shadow,
                GravityCompat.START);

        elementy.add(new Element(getResources().getString(R.string.produkty),
                R.drawable.ic_action_collection));
        elementy.add(new Element(getResources().getString(R.string.szukaj),
                R.drawable.ic_action_search));
        elementy.add(new Element(getResources().getString(R.string.zam_wienia),
                R.drawable.ic_action_go_to_today));
        elementy.add(new Element(getResources().getString(R.string.zaloguj),
                R.drawable.ic_action_person));

        adapter = new DrawerAdapter(this, R.layout.drawer_item, elementy);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        MenuInflater infl = getMenuInflater();
        infl.inflate(R.menu.main_action, menu);
        return super.onCreateOptionsMenu(menu);
    }

    public void SelectItem(int possition) {

        if (internet() == false) {
            polaczenie(possition);
        }

        switch (possition) {
            case 0:
                fragment = new Produkty();
                args.putInt(Produkty.LOGGED_USER, idUsera);
                break;
            case 1:
                fragment = new Szukaj();
                args.putInt(Zamowienia.LOGGED_USER, idUsera);

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
                if (zalogowany == false)
                    fragment = new LoginForm();
                else {
                    fragment = new LogOut();
                    args.putString(LogOut.ITEM_NAME, elementy.get(possition)
                            .getItemName());
                }
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
        if (mDrawerToggle.onOptionsItemSelected(item)) {
            return true;
        } else {
            switch (item.getItemId()) {
                case R.id.szukaj:
                    SelectItem(1);
                    break;

                default:
                    return false;
            }
        }
        return false;
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        mDrawerToggle.onConfigurationChanged(newConfig);
    }

    public void Login(View w) {
        EditText pass = (EditText) findViewById(R.id.ePassword);
        EditText login = (EditText) findViewById(R.id.eLogin);
        String userName = login.getText().toString();
        String password = (pass.getText().toString());

        passwordChecker.setInputPassword(password);
        passwordChecker.isValidPasswordFor(userName, passwordListener);
        passwordChecker.setEncryptionMethod(new MD5());
    }

    public void Login1(View v) {
        passwordChecker.setEncryptionMethod(new TestAccount());
        EditText login = (EditText) findViewById(R.id.eLogin);
        login.setText("kot");
        Login(v);
    }

    public void LogOut(View w) {
        zalogowany = false;
        idUsera = -1;
        elementy.get(3).napis = getString(R.string.zaloguj);
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

    public boolean internet() {
        ConnectivityManager connectivityManager = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetworkInfo = connectivityManager
                .getActiveNetworkInfo();
        return (activeNetworkInfo != null && activeNetworkInfo.isConnected());
    }

    @SuppressWarnings("deprecation")
    public void polaczenie(int pozy) {
        final int pos = pozy;
        alertDialog = new AlertDialog.Builder(this).create();
        alertDialog.setTitle(getString(R.string.brak_internetu_));
        alertDialog.setMessage(getString(R.string.sprawd_po_aczenie_z_sieci_));
        alertDialog.setIcon(R.drawable.ic_action_network_wifi);
        alertDialog.setCancelable(false);

        String btnName = getString(R.string.retry);

        alertDialog.setButton(btnName, new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int which) {
                if (internet()) {
                    alertDialog.dismiss();
                    SelectItem(pos);
                } else {
                    polaczenie(pos);
                }
            }
        });
        alertDialog.show();
    }

    private class DrawerItemClickListener implements
            ListView.OnItemClickListener {
        @Override
        public void onItemClick(AdapterView<?> parent, View view, int position,
                                long id) {
            SelectItem(position);

        }
    }

}
