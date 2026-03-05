package com.example.pharmaonecaja.service;

import com.example.pharmaonecaja.dto.abrirCajaDTO;
import com.google.gson.Gson;
import com.google.gson.JsonObject;

public class CajaService {

    public static int abrirCaja(double montoInicial, int usuarioId) {

        abrirCajaDTO dto = new abrirCajaDTO(montoInicial, usuarioId);

        String json = new Gson().toJson(dto);

        String response = Api.post("caja/abrir", json);

        JsonObject obj = new Gson().fromJson(response, JsonObject.class);

        return obj.get("cajaId").getAsInt();
    }

    public static String cerrarCaja(int cajaId) {

        String response = Api.post("caja/cerrar/" + cajaId, "");

        return response;
    }
}