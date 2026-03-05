package com.example.pharmaonecaja.controller;

import com.example.pharmaonecaja.model.DetalleVenta;
import com.example.pharmaonecaja.service.VentaService;
import javafx.collections.FXCollections;
import javafx.fxml.FXML;
import javafx.scene.control.Label;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.cell.PropertyValueFactory;

import java.util.List;

public class DetalleController {

    @FXML
    private TableView<DetalleVenta> tablaDetalle;

    @FXML
    private TableColumn<DetalleVenta, String> colProducto;

    @FXML
    private TableColumn<DetalleVenta, Integer> colCantidad;

    @FXML
    private TableColumn<DetalleVenta, Double> colPrecio;

    @FXML
    private TableColumn<DetalleVenta, Double> colSubtotal;

    @FXML
    private Label lblTotal;

    @FXML
    public void initialize() {

        colProducto.setCellValueFactory(new PropertyValueFactory<>("nombre"));
        colCantidad.setCellValueFactory(new PropertyValueFactory<>("cantidad"));
        colPrecio.setCellValueFactory(new PropertyValueFactory<>("precioUnitario"));
        colSubtotal.setCellValueFactory(new PropertyValueFactory<>("subTotal"));

    }

    public void cargarVenta(int ventaId) {

        try {

            List<DetalleVenta> detalles = VentaService.obtenerDetalle(ventaId);

            tablaDetalle.setItems(
                    FXCollections.observableArrayList(detalles)
            );

            double total = detalles.stream()
                    .mapToDouble(DetalleVenta::getSubTotal)
                    .sum();

            lblTotal.setText("Total: $" + total);

        } catch (Exception e) {
            e.printStackTrace();
        }

    }
}