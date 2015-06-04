package com.mo.MgRsklep.MySQL;


import android.content.Context;

public class DataBaseConnection {
    private DbHelper dbHelper;

    private DataBaseConnection(Context context){
        dbHelper = new DbHelper(context);
    }
    public static DataBaseConnection getDatabaseConnectionInstance(Context context){
        return new DataBaseConnection(context);
    }

    public void getConnection(DataBaseListener dataBaseListener){
        dbHelper.setConnectionListener(dataBaseListener);
        dbHelper.execute();
    }
}
