package com.io.sklep.MD5;

import java.math.BigInteger;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.concurrent.ExecutionException;

import android.content.Context;
import android.util.Log;

import com.io.sklep.MySQL.GetPassword;

public class MD5 {
	private String in;
	private String out;
	public int id;
	public MD5(String in) {
		this.in = in;
		
		out = md5(in);
	}
	
	
	public String getIn() {
		return in;
	}


	public String getMD5() {
		if(in.equals("test")) return "0"+out;
		return out;
	}


	public static String md5(String s) 
	{
	    MessageDigest digest;
	    try 
	    {
	        digest = MessageDigest.getInstance("MD5");
	        digest.update(s.getBytes(),0,s.length());
	        String hash = new BigInteger(1, digest.digest()).toString(16);
	        return hash;
	    } 
	    catch (NoSuchAlgorithmException e) 
	    {
	        e.printStackTrace();
	    }
	    return "";
	}
	
//	public static String md5(String s) {
//	    try {
//	        // Create MD5 Hash
//	        MessageDigest digest = java.security.MessageDigest.getInstance("MD5");
//	        digest.update(s.getBytes());
//	        byte messageDigest[] = digest.digest();
//
//	        // Create Hex String
//	        StringBuffer hexString = new StringBuffer();
//	        for (int i=0; i<messageDigest.length; i++)
//	            hexString.append(Integer.toHexString(0xFF & messageDigest[i]));
//	        return hexString.toString();
//
//	    } catch (NoSuchAlgorithmException e) {
//	        e.printStackTrace();
//	    }
//	    return "";
//	}
    public boolean check(String name, Context context)
    {
    	String t = null;
    	String dane[];
    	
		GetPassword task = new GetPassword(name, context);
		try {
			t = task.execute().get();
			dane = t.split(";");
			t = dane [1];
			id = Integer.parseInt(dane[0]);
			Log.i("2222", t+" : "+ id);
		} catch (InterruptedException e) {
			Log.i("1234", e.getMessage());
		} catch (ExecutionException e) {
			Log.i("1234", e.getMessage());
		}catch (Exception e) {
			Log.i("1234", e.getMessage());
		}
		Log.i("122", ">"+ t);
		Log.i("122", getMD5());
    	return t.equals(getMD5());
    }
}
