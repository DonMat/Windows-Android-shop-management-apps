package com.mo.MgRsklep.MySQL;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import com.mo.MgRsklep.MgR_App.R;

public class DbHelper extends AsyncTask<Connection, Void, Connection> {
	private final String dbName="chmielek_sklep";
	private final String dbUser="chmielek_sklep";
	private final String dbHost="baza.projekt-io.tk";
	private final String dbPass="nvh3dHZ5";
	private final String dbPort="3306";
	private ProgressDialog progres;
	private java.sql.Connection conn;
	private Context context;

	public DbHelper(Context c) {
		context = c;
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
					Log.i("1234", "RUN OK");
				} catch (SQLException e) {
					Log.e("1234", "Connection filed");
				}
			}	
			if(conn != null)
				return conn;
			else
				return null;
	}
	
	private DataBaseListener connectionListener;
	
	public void setConnectionListener(DataBaseListener connectionListener){
		this.connectionListener = connectionListener;
	}
	
	
	@Override
	protected void onPreExecute() {
	        progres = new ProgressDialog(context);
	        progres.setMessage(context.getString(R.string.testHas));
	        progres.setCancelable(false);
	        progres.show();
	}
	@Override
	protected void onPostExecute(Connection result) {
	    if (progres.isShowing())
	        progres.dismiss();

		connectionListener.getConnection(result);
	}

	public void setContext(Context context) {
		this.context = context;
	}
}
