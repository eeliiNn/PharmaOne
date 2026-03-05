package com.example.pharmaonecaja.service;

import com.example.pharmaonecaja.model.ProductoDisponible;
import com.google.gson.Gson;

public class ProductoService {

    public static ProductoDisponible[] obtenerDisponibles() {

        String response = Api.get("venta/disponibles");

        return new Gson().fromJson(response, ProductoDisponible[].class);
    }
}