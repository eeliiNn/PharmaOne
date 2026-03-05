package com.example.pharmaonecaja.dto;

public class DetalleVentaDTO {

    private int productoId;
    private int cantidad;

    public DetalleVentaDTO() {}

    public DetalleVentaDTO(int productoId, int cantidad) {
        this.productoId = productoId;
        this.cantidad = cantidad;
    }

    public int getProductoId() {
        return productoId;
    }

    public void setProductoId(int productoId) {
        this.productoId = productoId;
    }

    public int getCantidad() {
        return cantidad;
    }

    public void setCantidad(int cantidad) {
        this.cantidad = cantidad;
    }
}