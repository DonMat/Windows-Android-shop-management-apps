package com.mo.MgRsklep.MySQL;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.Toast;

import com.mo.MgRsklep.MgR_App.R;

import java.sql.Connection;
import java.sql.SQLException;
import java.sql.Statement;

public class SetZamowienie extends AsyncTask<Connection, Void, String> {
    ProgressDialog pDialog;
    Connection connection;
    int userId;
    int ile;
    int produktId;
    Context context;

    public SetZamowienie(int userId, int ile, int produktId, Context cont) {
        super();
        this.userId = userId;
        this.ile = ile;
        this.produktId = produktId;
        this.context = cont;
        DataBaseConnection.getDatabaseConnectionInstance(context).getConnection(connectionListener);
    }

    @Override
    protected String doInBackground(Connection... params) {
        String result = new String();
        if (connection != null) {
            Statement st;
            String query;

            query = "call noweZamowienie(" + userId + ");";
            Log.i("1236", query);
            try {
                st = connection.createStatement();
                st.executeQuery(query);
                query = "call noweZamowienie1(" + userId + "," + ile + ","
                        + produktId + ");";
                Log.i("1236", query);

                st.executeQuery(query);
                query = "call updateStore(" + produktId + ", " + ile + ");";
                Log.i("1236", query);

                st.executeQuery(query);
                st.close();
                result = "OK";

            } catch (SQLException e) {
                Log.e("1235", "SQL error" + e.getMessage());
            } catch (Exception a) {
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

        pDialog = new ProgressDialog(context);
        pDialog.setMessage(context.getString(R.string.skladanieZam));
        pDialog.setCancelable(false);
        pDialog.show();
    }

    @Override
    protected void onPostExecute(String result) {
        super.onPostExecute(result);
        if (pDialog.isShowing())
            pDialog.dismiss();
        if (result.equals("OK"))
            Toast.makeText(context, "Zamowienie zakonczone sukcesem!",
                    Toast.LENGTH_SHORT).show();
        else
            Toast.makeText(context, "Niepowodzenie!", Toast.LENGTH_SHORT)
                    .show();
    }

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

}
