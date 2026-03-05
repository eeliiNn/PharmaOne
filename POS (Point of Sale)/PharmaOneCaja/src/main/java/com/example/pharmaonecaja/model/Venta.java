package com.example.pharmaonecaja.model;

import java.time.LocalDateTime;

public class Venta {

    private int ventaId;
    private String fechaVenta;
    private String tipoPago;
    private double total;
    private double descuentoAplicado;

    public int getVentaId() { return ventaId; }
    public String getFechaVenta() { return fechaVenta; }
    public String getTipoPago() { return tipoPago; }
    public double getTotal() { return total; }
    public double getDescuentoAplicado() { return descuentoAplicado; }
}