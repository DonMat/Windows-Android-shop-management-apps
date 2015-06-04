package com.mo.MgRsklep.MgR_App;

public class Element {
	 
    String napis;
    int imgID;

    public Element(String itemName, int imgResID) {
          super();
          napis = itemName;
          this.imgID = imgResID;
    }

    public String getItemName() {
          return napis;
    }
    public void setItemName(String itemName) {
          napis = itemName;
    }
    public int getImgResID() {
          return imgID;
    }
    public void setImgResID(int imgResID) {
          this.imgID = imgResID;
    }
}
