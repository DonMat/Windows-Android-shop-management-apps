package com.mo.MgRsklep.MySQL;

import com.mo.MgRsklep.Adapters.Produkt.Produkt;

import java.util.ArrayList;


public interface ProduktyListener {
    void getProducts(ArrayList<Produkt> produktList);
}
