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

import com.mo.MgRsklep.Adapters.Produkt.Produkt;
import com.mo.MgRsklep.MgR_App.R;

public class GetProdukty extends
		AsyncTask<Connection, Void, ArrayList<Produkt>> {
	private ProgressDialog pDialog;
	private Connection connection;
	private Context context;
	public int id;
	private ProduktyListener listener;

	public void setListener(ProduktyListener listener) {
		this.listener = listener;
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

	public GetProdukty(Context c) {
		context = c;
		id = -1;
		DataBaseConnection.getDatabaseConnectionInstance(context).getConnection(connectionListener);
	}

	public GetProdukty(int id, Context c) {
		context = c;
		this.id = id;
		DataBaseConnection.getDatabaseConnectionInstance(context).getConnection(connectionListener);
	}

	@Override
	protected ArrayList<Produkt> doInBackground(Connection... params) {
		ArrayList<Produkt> out = new ArrayList<Produkt>();
		Log.i("deb", "1getConnAsync " + (connection != null));
			Log.i("deb", "getConnAsync " + (connection != null));
			if (connection != null) {
				Statement st;
				String query;
				ResultSet resoult;

				if (id == -1)
					query = "CALL `produkty` ()";

				else
					query = "CALL `produkty_kat` (" + id + ")";

				try {
					st = connection.createStatement();
					resoult = st.executeQuery(query);
					while (resoult.next()) {
						out.add(new Produkt(resoult.getString(2), resoult
								.getDouble(3), resoult.getInt(4), resoult
								.getInt(5), resoult.getInt(1)));
					}
					Log.i("1234", out.size() + " rozmiar");

					st.close();
					resoult.close();

				} catch (SQLException e) {
					Log.e("1234", "SQL error");
				} catch (Exception a) {
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

		pDialog = new ProgressDialog(context);
		pDialog.setMessage(context.getString(R.string.wczytaj));
		pDialog.setCancelable(false);
		pDialog.show();
	}

	@Override
	protected void onPostExecute(ArrayList<Produkt> result) {
		super.onPostExecute(result);

		Log.i("get", "get produkty " + (result == null));
		listener.getProducts(result);
		if (pDialog.isShowing())
			pDialog.dismiss();

	}
}
