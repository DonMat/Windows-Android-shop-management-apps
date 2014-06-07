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
public class GetPassword extends AsyncTask<Connection, Void, String>{
	ProgressDialog pDialog;
	Connection conn;
	String user;
	Context cont;
	public GetPassword(String us, Context c) {
		user = us;
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
	protected String doInBackground(Connection... params) {
		String result = new String();
		if(conn != null)
		{
	    	Statement st;
			String query;
			ResultSet resoult;
			
			query = "Select `Haslo` from `Account` where `NazwaUzytkownika` like '"+user+"' limit 1";

			try {
				st = conn.createStatement();
				resoult = st.executeQuery(query);			
				while(resoult.next())
				{					
					result +=resoult.getString(1);
				}
				Log.i("1234", result);

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
	return result;
	}

	protected void onPreExecute() {
		super.onPreExecute();

	        pDialog = new ProgressDialog(cont);
	        pDialog.setMessage(cont.getString(R.string.testHas));
	        pDialog.setCancelable(false);
	        pDialog.show();
	}
	@Override
	protected void onPostExecute(String result) {
	    super.onPostExecute(result);
	    if (pDialog.isShowing())
	        pDialog.dismiss();
	}

}
