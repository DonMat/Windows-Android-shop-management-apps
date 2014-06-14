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

import com.io.sklep.Adapters.Produkt.Zamowienie;
import com.io.sklep.klientapp.R;

public class GetZamowienia extends AsyncTask<Connection, Void, ArrayList<Zamowienie>>{
	ProgressDialog pDialog;
	Connection conn;
	Context cont;
	public int id;
	
	public GetZamowienia(int id, Context c) {
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
	protected ArrayList<Zamowienie> doInBackground(Connection... params) {
		ArrayList<Zamowienie> out = new ArrayList<Zamowienie>();
		if(conn != null)
		{
	    	Statement st;
			String query;
			ResultSet resoult;
			
				query = "CALL `zamowienia` ("+id+")";

			try {
				st = conn.createStatement();
				resoult = st.executeQuery(query);			
				while(resoult.next())
				{					
					out.add( new Zamowienie(resoult.getInt(1), resoult.getDate(2).toString(), resoult.getString(3),resoult.getInt(4), resoult.getDouble(5), resoult.getDouble(6), resoult.getString(7)+", "+resoult.getString(8)) );
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

	protected void onPreExecute() {
		super.onPreExecute();

	        pDialog = new ProgressDialog(cont);
	        pDialog.setMessage(cont.getString(R.string.produkty));
	        pDialog.setCancelable(false);
	        pDialog.show();
	}
	@Override
	protected void onPostExecute(ArrayList<Zamowienie> result) {
	    super.onPostExecute(result);
	    if (pDialog.isShowing())
	        pDialog.dismiss();
	}
}
