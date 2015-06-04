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

public class GetUserInfo extends AsyncTask<Connection, Void, String[]> {
    private ProgressDialog pDialog;
    private Connection connection;
    private Context context;
    public String id;
    private UserInfoListener userInfoListener;

    public void setUserInfoListener(UserInfoListener userInfoListener) {
        this.userInfoListener = userInfoListener;
    }

    public GetUserInfo(String id, Context c) {
        context = c;
        this.id = id;

        DataBaseConnection.getDatabaseConnectionInstance(context).getConnection(connectionListener);
    }

    @Override
    protected String[] doInBackground(Connection... params) {
        String[] out = new String[5];
            if (connection != null) {
                Statement st;
                String query;
                ResultSet resoult;

                query = "CALL `infoUser` ('" + id + "')";

                try {
                    st = connection.createStatement();
                    resoult = st.executeQuery(query);
                    while (resoult.next()) {
                        out[0] = resoult.getString(2) + " "
                                + resoult.getString(3);
                        out[1] = resoult.getString(4) + " "
                                + resoult.getString(5);
                        out[2] = resoult.getString(6) + " "
                                + resoult.getString(7);
                        out[3] = resoult.getString(8);
                        out[4] = resoult.getString(9);
                    }
                    Log.i("1234", "5 rozmiar");

                    st.close();
                    resoult.close();

                } catch (SQLException e) {
                    Log.e("1234", "SQL error" + e.getMessage());
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
    protected void onPostExecute(String[] result) {
        super.onPostExecute(result);
        if (pDialog.isShowing())
            pDialog.dismiss();

        userInfoListener.getUserInfo(result);
    }

    private void go(){
        this.execute();
    }
    private boolean stateRun = true;
    private DataBaseListener
            connectionListener = new DataBaseListener() {

        @Override
        public void getConnection(Connection result) {
            connection = result;
            stateRun = false;

            go();
            Log.i("deb", "getConn success");
        }

    };
}
