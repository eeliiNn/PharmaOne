package com.example.pharmaonecaja.model;

import javafx.beans.property.*;

public class ItemVenta {

    private final IntegerProperty productoId;
    private final StringProperty nombre;
    private final IntegerProperty cantidad;
    private final DoubleProperty precio;
    private final DoubleProperty subtotal;
    private final StringProperty foto;

    public ItemVenta(int productoId, String nombre, int cantidad, double precio, String foto) {
        this.productoId = new SimpleIntegerProperty(productoId);
        this.nombre = new SimpleStringProperty(nombre);
        this.cantidad = new SimpleIntegerProperty(cantidad);
        this.precio = new SimpleDoubleProperty(precio);
        this.subtotal = new SimpleDoubleProperty(cantidad * precio);
        this.foto = new SimpleStringProperty(foto);
    }

    public int getProductoId() { return productoId.get(); }
    public String getNombre() { return nombre.get(); }
    public int getCantidad() { return cantidad.get(); }
    public double getPrecio() { return precio.get(); }
    public double getSubtotal() { return subtotal.get(); }
    public String getFoto() { return foto.get(); }

    public void setCantidad(int nuevaCantidad) {
        cantidad.set(nuevaCantidad);
        subtotal.set(nuevaCantidad * precio.get());
    }

    public IntegerProperty productoIdProperty() { return productoId; }
    public StringProperty nombreProperty() { return nombre; }
    public IntegerProperty cantidadProperty() { return cantidad; }
    public DoubleProperty precioProperty() { return precio; }
    public DoubleProperty subtotalProperty() { return subtotal; }
    public StringProperty fotoProperty() { return foto; }
}