package com.example.pharmaonecaja.model;

public class LoginRequest {

    private String usuarioNombre;
    private String password;

    public LoginRequest(String usuarioNombre, String password) {
        this.usuarioNombre = usuarioNombre;
        this.password = password;
    }

    public String getUsuarioNombre() {
        return usuarioNombre;
    }

    public String getPassword() {
        return password;
    }
}