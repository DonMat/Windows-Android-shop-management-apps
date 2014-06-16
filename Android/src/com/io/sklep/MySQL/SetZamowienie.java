package com.io.sklep.MySQL;

import java.sql.Connection;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.concurrent.ExecutionException;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import com.io.sklep.klientapp.R;

public class SetZamowienie extends AsyncTask<Connection, Void, String>{
	ProgressDialog pDialog;
	Connection conn;
	int userId;
	int ile;
	int produktId;
	Context cont;
	

	
	public SetZamowienie(int userId,int ile, int produktId, Context cont) 
	{
		super();
		this.userId = userId;
		this.ile = ile;
		this.produktId = produktId;
		this.cont = cont;
		
		DbHelper a = new DbHelper(cont);
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
	
			
			query = "call noweZamowienie(" + userId + ");";
			Log.i("1236", query);
			try {
				st = conn.createStatement();
				st.executeQuery(query);	
				query= "call noweZamowienie1(" + userId + "," + ile + "," + produktId + ");"; 
				Log.i("1236", query);

				st.executeQuery(query);	
				query = "call updateStore(" + produktId + ", " + ile + ");";
				Log.i("1236", query);

				st.executeQuery(query);	
				st.close();

				
			} catch (SQLException e) {
				Log.e("1235", "SQL error"+e.getMessage());
			}
			catch(Exception a)
			{
				Log.e("1235", "Inny blad");
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
	        pDialog.setMessage(cont.getString(R.string.skladanieZam));
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
