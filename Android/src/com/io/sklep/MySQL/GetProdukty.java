package com.io.sklep.MySQL;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.concurrent.ExecutionException;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import com.io.sklep.Adapters.Produkt.Produkt;
import com.io.sklep.klientapp.R;

public class GetProdukty extends AsyncTask<Connection, Void, ArrayList<Produkt>>{
	ProgressDialog pDialog;
	Connection conn;
	Context cont;
	public int id;
	
	public GetProdukty(Context c) {
		cont = c;
		id = -1;
		DbHelper a = new DbHelper(c);
		try {
			conn = a.execute().get();
		} catch (InterruptedException e) {
			Log.i("1234", e.getMessage());
		} catch (ExecutionException e) {			
		}
	}
	public GetProdukty(int id, Context c) {
		cont = c;
		this.id = id;
		DbHelper a = new DbHelper(c);
		try {
			conn = a.execute().get();
		} catch (InterruptedException e) {
			Log.i("1234", e.getMessage());
		} catch (ExecutionException e) {			
		}
	}
	@Override
	protected ArrayList<Produkt> doInBackground(Connection... params) {
		ArrayList<Produkt> out = new ArrayList<Produkt>();
		if(conn != null)
		{
	    	Statement st;
			String query;
			ResultSet resoult;
			
			if(id == -1)
				query = "CALL `produkty` ()";
			
			else
				query = "CALL `produkty_kat` ("+id+")";

			try {
				st = conn.createStatement();
				resoult = st.executeQuery(query);			
				while(resoult.next())
				{					
					out.add( new Produkt(resoult.getString(2), resoult.getDouble(3),resoult.getInt(4), resoult.getInt(5), resoult.getInt(1)) );
				}
				Log.i("1234", out.size()+" rozmiar");

				st.close();
				resoult.close();
				
			} catch (SQLException e) {
				Log.e("1234", "SQL error");
			}
			catch(Exception a)
			{
				Log.e("1234", "Inny blad");
			}
			
			try {
                Thread.sleep(10);
            } catch (InterruptedException e) {
            	Log.i("1234", e.getMessage());
            }
		}
	return out;
	}

	@Override
	protected void onPreExecute() {
		super.onPreExecute();

	        pDialog = new ProgressDialog(cont);
	        pDialog.setMessage(cont.getString(R.string.produkty));
	        pDialog.setCancelable(false);
	        pDialog.show();
	}
	@Override
	protected void onPostExecute(ArrayList<Produkt> result) {
	    super.onPostExecute(result);
	    if (pDialog.isShowing())
	        pDialog.dismiss();
	}
}
