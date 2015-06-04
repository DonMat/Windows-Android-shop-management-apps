package com.mo.MgRsklep.MySQL;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import com.mo.MgRsklep.MgR_App.R;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class GetPassword extends AsyncTask<Connection, Void, String[]> {
    private ProgressDialog pDialog;
    private Connection connection;
    private String user;
    private Context context;
    private PasswordListener passwordListener;
    private String password;

    public void setPasswordListener(PasswordListener passListener) {
        this.passwordListener = passListener;
    }

    public GetPassword(String us, Context c, PasswordListener passwordListener) {
        user = us;
        context = c;
        this.passwordListener = passwordListener;
        DataBaseConnection.getDatabaseConnectionInstance(context).getConnection(connectionListener);
    }

    @Override
    protected String[] doInBackground(Connection... params) {
        String result[] = new String[4];
        if (connection != null) {
            Statement st;
            String query;
            ResultSet resoult;

            query = "call haslo('" + user + "')";

            try {
                st = connection.createStatement();
                resoult = st.executeQuery(query);
                while (resoult.next()) {
                    result[0] = resoult.getString(1);
                    result[1] = resoult.getString(2);
                    result[2] = resoult.getString(3);
                }

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
        return result;
    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();

        pDialog = new ProgressDialog(context);
        pDialog.setMessage(context.getString(R.string.testHas));
        pDialog.setCancelable(false);
        pDialog.show();
    }

    @Override
    protected void onPostExecute(String[] result) {
        super.onPostExecute(result);
        if (pDialog.isShowing())
            pDialog.dismiss();
        result[3] = password;
        passwordListener.getPass(result);
    }

    private DataBaseListener connectionListener = new DataBaseListener() {

        @Override
        public void getConnection(Connection result) {

            Log.i("deb", "getConn - listener");
            connection = result;
            go();
            Log.i("deb", "getConn success");
        }

    };

    private void go() {
        this.execute();
    }


    public void setPassword(String password) {
        this.password = password;
    }
}
