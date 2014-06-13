package com.io.sklep.MySQL;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.concurrent.ExecutionException;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import com.io.sklep.klientapp.R;

public class GetUserInfo extends AsyncTask<Connection, Void, String[]>{
	ProgressDialog pDialog;
	Connection conn;
	Context cont;
	public String id;
	
	public GetUserInfo(String id, Context c) {
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
	protected String[] doInBackground(Connection... params) {
		String[] out = new String[5];
		if(conn != null)
		{
	    	Statement st;
			String query;
			ResultSet resoult;
			
				query = "CALL `infoUser` ('"+id+"')";
			
			try {
				st = conn.createStatement();
				resoult = st.executeQuery(query);			
				while(resoult.next())
				{		
					out[0] = resoult.getString(2)+" "+ resoult.getString(3);
					out[1] = resoult.getString(4)+" "+ resoult.getString(5);
					out[2] = resoult.getString(6)+" "+ resoult.getString(7);
					out[3] = resoult.getString(8);
					out[4] = resoult.getString(9);					
				}
				Log.i("1234", "5 rozmiar");

				st.close();
				resoult.close();
				
			} catch (SQLException e) {
				Log.e("1234", "SQL error"+e.getMessage());
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
	protected void onPostExecute(String[] result) {
	    super.onPostExecute(result);
	    if (pDialog.isShowing())
	        pDialog.dismiss();
	}
}
