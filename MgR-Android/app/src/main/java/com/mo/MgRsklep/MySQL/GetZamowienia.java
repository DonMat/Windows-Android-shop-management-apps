package com.mo.MgRsklep.MySQL;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import com.mo.MgRsklep.Adapters.Produkt.Zamowienie;
import com.mo.MgRsklep.MgR_App.R;

public class GetZamowienia extends
		AsyncTask<Connection, Void, ArrayList<Zamowienie>> {
	private ProgressDialog pDialog;
	private Connection connection;
	private Context context;
	public int id;
	private ZamowieniaListener listener;
	
	public void setListener(ZamowieniaListener listener) {
		this.listener = listener;
	}

	public GetZamowienia(int id, Context c) {
		context = c;
		this.id = id;
		DataBaseConnection.getDatabaseConnectionInstance(context).getConnection(connectionListener);
	}

	private boolean stateRun = true;
	private DataBaseListener connectionListener = new DataBaseListener() {

		@Override
		public void getConnection(Connection result) {
			connection = result;
			go();
			Log.i("deb", "getConn success");
		}

	};

	private void go() {
		this.execute();
	}

	@Override
	protected ArrayList<Zamowienie> doInBackground(Connection... params) {
		ArrayList<Zamowienie> out = new ArrayList<Zamowienie>();
			if (connection != null) {
				Statement st;
				String query;
				ResultSet resoult;

				query = "CALL `zamowienia` (" + id + ")";

				try {
					st = connection.createStatement();
					resoult = st.executeQuery(query);
					while (resoult.next()) {
						out.add(new Zamowienie(resoult.getInt(1), resoult
								.getDate(2).toString(), resoult.getString(3),
								resoult.getInt(4), resoult.getDouble(5),
								resoult.getDouble(6), resoult.getString(7)
										+ ", " + resoult.getString(8), resoult
										.getDate(9)));
					}
					Log.i("1234", out.size() + " rozmiar");
					st.close();
					resoult.close();

				} catch (SQLException e) {
					Log.e("1234", "SQL error");
				} catch (Exception a) {
					Log.e("1234", "Inny blad" + a.getMessage());
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

		pDialog = new ProgressDialog(context);
		pDialog.setMessage(context.getString(R.string.wczytaj));
		pDialog.setCancelable(false);
		pDialog.show();
	}

	@Override
	protected void onPostExecute(ArrayList<Zamowienie> result) {
		super.onPostExecute(result);
		if (pDialog.isShowing())
			pDialog.dismiss();
		
		listener.getZamowienia(result);
	}
}
