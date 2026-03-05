package com.example.pharmaonecaja.model;

public class ProductoDisponible {

    private int productoId;
    private String nombre;
    private double precioVenta;
    private String foto;
    private int stock;

    public int getProductoId() { return productoId; }
    public String getNombre() { return nombre; }
    public double getPrecioVenta() { return precioVenta; }
    public String getFoto() { return foto; }
    public int getStock() { return stock; }
}