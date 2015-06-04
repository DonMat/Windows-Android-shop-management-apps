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

import com.mo.MgRsklep.MgR_App.R;

public class GetKategorie extends
		AsyncTask<Connection, Void, ArrayList<KategoriaElem>> {
	private ProgressDialog pDialog;
	private Connection connection;
	private Context context;
	private KategorieListener kategorieListener;

	public GetKategorie(Context c) {
		context = c;
		DataBaseConnection.getDatabaseConnectionInstance(context).getConnection(connectionListener);
	}

	public void setKategorieListener(KategorieListener kategorieListener) {
		this.kategorieListener = kategorieListener;
	}

	@Override
	protected ArrayList<KategoriaElem> doInBackground(Connection... params) {
		ArrayList<KategoriaElem> out = new ArrayList<KategoriaElem>();
			Log.i("debsz", "kategorie");
			if (connection != null) {
				Statement st;
				String query;
				ResultSet resoult;

				query = "call kategorie()";

				try {
					st = connection.createStatement();
					resoult = st.executeQuery(query);
					while (resoult.next()) {
						out.add(new KategoriaElem(resoult.getString(2), resoult
								.getInt(1)));
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
		pDialog.setMessage(context.getString(R.string.kategorie));
		pDialog.setCancelable(false);
		pDialog.show();
	}

	@Override
	protected void onPostExecute(ArrayList<KategoriaElem> result) {
		super.onPostExecute(result);
		if (pDialog.isShowing())
			pDialog.dismiss();

		kategorieListener.getKategorie(result);
	}

	private DataBaseListener connectionListener = new DataBaseListener() {

		@Override
		public void getConnection(Connection result) {
			connection = result;
			go();

		}

	};

	private void go() {
		this.execute();
	}
}
