package com.io.sklep.MySQL;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import com.io.sklep.klientapp.R;

public class DbHelper extends AsyncTask<Connection, Void, Connection> {
	private final String dbName="chmielek_test";
	private final String dbUser="chmielek_Android";
	private final String dbHost="baza.projekt-io.tk";
	private final String dbPass="test";
	private final String dbPort="3306";
	ProgressDialog progres;
	private java.sql.Connection conn;
	Context cont;
	
	public DbHelper(Context c) {
		cont = c;
	}
	@Override
	protected Connection doInBackground(Connection... params) {
		
    	try {
			Class.forName("com.mysql.jdbc.Driver");
		} catch (ClassNotFoundException e) {
			Log.e("1234", "Class not found");
		}    	
			Log.i("1234", "RUN");
			
			if(conn == null)
			{
				try {
					conn = (Connection) DriverManager.getConnection("jdbc:mysql://"+dbHost+":"+dbPort+"/"+dbName,dbUser,dbPass);
				} catch (SQLException e) {
					Log.e("1234", "Connection filed");
				}
			}	
			if(conn != null)
				return conn;
			else
				return null;
	}
	@Override
	protected void onPreExecute() {
		super.onPreExecute();

	        progres = new ProgressDialog(cont);
	        progres.setMessage(cont.getString(R.string.testHas));
	        progres.setCancelable(false);
	        progres.show();
	}
	@Override
	protected void onPostExecute(Connection result) {
	    super.onPostExecute(result);
	    if (progres.isShowing())
	        progres.dismiss();
	}
}
