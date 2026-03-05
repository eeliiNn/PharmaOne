package com.example.pharmaonecaja.service;

import com.example.pharmaonecaja.model.MembresiaResponse;
import com.example.pharmaonecaja.service.Api;
import com.google.gson.Gson;

import java.time.LocalDate;

public class MembresiaService {

    public static MembresiaResponse obtener(int clienteId){

        String response = Api.get("membresias/cliente/" + clienteId);

        return new Gson().fromJson(response, MembresiaResponse.class);
    }

    public static void crear(int clienteId, String tipo, LocalDate fechaFin){

        String json = """
    {
       "clienteId": %d,
       "tipoMembresia": "%s",
       "fechaFin": "%s"
    }
    """.formatted(clienteId, tipo, fechaFin.toString());

        String response = Api.post("membresias", json);

        System.out.println("Respuesta API membresia: " + response);
    }
}