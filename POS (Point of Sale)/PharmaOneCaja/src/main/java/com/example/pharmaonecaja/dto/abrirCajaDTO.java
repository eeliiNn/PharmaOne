package com.example.pharmaonecaja.dto;


public class abrirCajaDTO {

    private double montoInicial;
    private int usuarioId;

    public abrirCajaDTO(double montoInicial, int usuarioId) {
        this.montoInicial = montoInicial;
        this.usuarioId = usuarioId;
    }

    public double getMontoInicial() {
        return montoInicial;
    }

    public int getUsuarioId() {
        return usuarioId;
    }
}