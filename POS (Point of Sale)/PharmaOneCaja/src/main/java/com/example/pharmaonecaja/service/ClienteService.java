package com.example.pharmaonecaja.service;

import com.example.pharmaonecaja.model.Cliente;
import com.example.pharmaonecaja.service.Api;
import com.google.gson.Gson;
import com.google.gson.JsonObject;

public class ClienteService {

    public static Cliente obtenerCliente(int id){

        String response = Api.get("clientes/" + id);

        return new Gson().fromJson(response, Cliente.class);
    }

    public static int registrar(String usuario,
                                String password,
                                String nombre,
                                String apellidos,
                                String telefono,
                                String direccion) {

        String json = """
        {
          "usuarioNombre":"%s",
          "password":"%s",
          "rolId":3,
          "nombre":"%s",
          "apellidos":"%s",
          "telefono":"%s",
          "direccion":"%s"
        }
        """.formatted(usuario,password,nombre,apellidos,telefono,direccion);

        String response = Api.post("clientes/register", json);

        JsonObject obj = new Gson().fromJson(response, JsonObject.class);

        return obj.get("clienteId").getAsInt();
    }

}