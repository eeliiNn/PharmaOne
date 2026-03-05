package com.example.pharmaonecaja.service;

import com.example.pharmaonecaja.dto.VentaDTO;
import com.example.pharmaonecaja.model.DetalleVenta;
import com.example.pharmaonecaja.model.Venta;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.lang.reflect.Type;
import java.util.Arrays;
import java.util.List;

public class VentaService {

    public static String crearVenta(VentaDTO venta) {

        String json = new Gson().toJson(venta);

        String response = Api.post("venta/crear", json);

        return response;
    }

    public static Venta[] obtenerHistorial() {

        String response = Api.get("venta/historial");

        return new Gson().fromJson(response, Venta[].class);
    }

    public static List<Venta> obtenerVentas() {

        String response = Api.get("venta/listar");

        Venta[] ventas = new Gson().fromJson(response, Venta[].class);

        return Arrays.asList(ventas);
    }
    public static List<DetalleVenta> obtenerDetalle(int ventaId) {

        String response = Api.get("venta/detalle/" + ventaId);

        Gson gson = new Gson();

        Type listType = new TypeToken<List<DetalleVenta>>(){}.getType();

        return gson.fromJson(response, listType);
    }
}