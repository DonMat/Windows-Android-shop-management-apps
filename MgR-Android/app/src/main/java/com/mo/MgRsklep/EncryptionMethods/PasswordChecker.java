package com.mo.MgRsklep.EncryptionMethods;


import android.content.Context;

import com.mo.MgRsklep.MySQL.GetPassword;
import com.mo.MgRsklep.MySQL.PasswordListener;

public class PasswordChecker {
    private Encryption encryptionMethod;
    private String password;
    private String encryptedPassword;
    private Context context;

    public PasswordChecker(Context context, Encryption encryptionMethod) {
        this.encryptionMethod = encryptionMethod;
        this.context = context;
    }

    public PasswordChecker(Context context, Encryption encryptionMethod, String password) {
        this.encryptionMethod = encryptionMethod;
        this.password = password;
        encryptedPassword = encryptionMethod.encrypt(password);
    }

    public void setEncryptionMethod(Encryption encryptionMethod) {
        this.encryptionMethod = encryptionMethod;
        encryptedPassword = null;
    }

    public void setInputPassword(String password) {
        this.password = password;
        encryptedPassword = encryptionMethod.encrypt(password);
    }

    public void isValidPasswordFor(String userName, PasswordListener passwordListener) {
        if (encryptedPassword == null) {
            encryptedPassword = encryptionMethod.encrypt(password);
        }

        GetPassword getUserPassword = new GetPassword(userName, context, passwordListener);
        getUserPassword.setPassword(encryptedPassword);
    }

}
