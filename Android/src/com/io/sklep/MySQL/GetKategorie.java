package com.io.sklep.MySQL;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.concurrent.ExecutionException;

import com.io.sklep.klientapp.R;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

public class GetKategorie extends AsyncTask<Connection, Void, ArrayList<KategoriaElem>>{
	ProgressDialog pDialog;
	Connection conn;
	Context cont;

	
	public GetKategorie(Context c) {
		cont = c;

		DbHelper a = new DbHelper(c);
		try {
			conn = a.execute().get();
		} catch (InterruptedException e) {
			Log.i("1234", e.getMessage());
		} catch (ExecutionException e) {			
		}
	}
	
	@Override
	protected ArrayList<KategoriaElem> doInBackground(Connection... params) {
		ArrayList<KategoriaElem> out = new ArrayList<KategoriaElem>();
		if(conn != null)
		{
	    	Statement st;
			String query;
			ResultSet resoult;
			
			query = "SELECT * FROM `Category`";

			try {
				st = conn.createStatement();
				resoult = st.executeQuery(query);			
				while(resoult.next())
				{					
					out.add( new KategoriaElem(resoult.getString(2), resoult.getInt(1)) );
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
	        pDialog.setMessage(cont.getString(R.string.kategorie));
	        pDialog.setCancelable(false);
	        pDialog.show();
	}
	@Override
	protected void onPostExecute(ArrayList<KategoriaElem> result) {
	    super.onPostExecute(result);
	    if (pDialog.isShowing())
	        pDialog.dismiss();
	}
}
