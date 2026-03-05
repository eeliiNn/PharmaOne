package com.example.pharmaonecaja.model;

public class MembresiaResponse {

    private boolean tieneMembresia;
    private double descuento;
    private String tipo;

    public boolean isTieneMembresia() {
        return tieneMembresia;
    }

    public double getDescuento() {
        return descuento;
    }

    public String getTipo() {
        return tipo;
    }
}