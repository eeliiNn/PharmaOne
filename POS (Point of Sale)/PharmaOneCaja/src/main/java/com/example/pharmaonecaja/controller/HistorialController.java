package com.example.pharmaonecaja.controller;

import com.example.pharmaonecaja.model.Venta;
import com.example.pharmaonecaja.service.CajaService;
import com.example.pharmaonecaja.service.VentaService;
import com.example.pharmaonecaja.util.Session;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.stage.Stage;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

public class HistorialController {

    @FXML
    private TableView<Venta> tablaVentas;

    @FXML
    private TableColumn<Venta, Integer> colId;

    @FXML
    private TableColumn<Venta, String> colFecha;

    @FXML
    private TableColumn<Venta, String> colTipoPago;

    @FXML
    private TableColumn<Venta, Double> colTotal;

    @FXML
    private TableColumn<Venta, Double> colDescuento;

    @FXML
    private TextField txtBuscarId;

    @FXML
    private DatePicker dpFecha;

    @FXML
    private Label lblTotalVentas;

    private ObservableList<Venta> listaVentas = FXCollections.observableArrayList();

    @FXML
    public void initialize() {

        colId.setCellValueFactory(new PropertyValueFactory<>("ventaId"));
        colFecha.setCellValueFactory(new PropertyValueFactory<>("fechaVenta"));
        colTipoPago.setCellValueFactory(new PropertyValueFactory<>("tipoPago"));
        colTotal.setCellValueFactory(new PropertyValueFactory<>("total"));
        colDescuento.setCellValueFactory(new PropertyValueFactory<>("descuentoAplicado"));

        cargarVentas();

        tablaVentas.setRowFactory(tv -> {
            TableRow<Venta> row = new TableRow<>();

            row.setOnMouseClicked(event -> {

                if (event.getClickCount() == 2 && !row.isEmpty()) {

                    Venta venta = row.getItem();
                    abrirDetalleVenta(venta);

                }

            });

            return row;
        });
    }

    @FXML
    public void cargarVentas() {

        List<Venta> ventas = VentaService.obtenerVentas();

        listaVentas.setAll(ventas);

        tablaVentas.setItems(listaVentas);

        calcularTotal();
    }

    @FXML
    public void filtrarVentas() {

        List<Venta> ventas = VentaService.obtenerVentas();

        ObservableList<Venta> filtradas = FXCollections.observableArrayList();

        String idTexto = txtBuscarId.getText();
        LocalDate fecha = dpFecha.getValue();

        for (Venta v : ventas) {

            boolean coincide = true;

            // FILTRO POR ID
            if (!idTexto.isEmpty()) {

                try {

                    int id = Integer.parseInt(idTexto);
                    coincide = v.getVentaId() == id;

                } catch (NumberFormatException e) {

                    coincide = false;

                }
            }

            // FILTRO POR FECHA
            if (fecha != null) {

                LocalDate fechaVenta = LocalDateTime
                        .parse(v.getFechaVenta())
                        .toLocalDate();

                coincide = coincide && fechaVenta.equals(fecha);

            }

            if (coincide) {
                filtradas.add(v);
            }
        }

        tablaVentas.setItems(filtradas);

        calcularTotal();
    }

    private void calcularTotal() {

        double total = 0;

        for (Venta v : tablaVentas.getItems()) {

            total += v.getTotal();

        }

        lblTotalVentas.setText(String.format("$%.2f", total));
    }

    @FXML
    private void abrirVentas() {

        try {

            FXMLLoader loader = new FXMLLoader(
                    getClass().getResource("/com/example/pharmaonecaja/views/ventas-view.fxml")
            );

            Scene scene = new Scene(loader.load(), 1200, 700);

            Stage stage = (Stage) tablaVentas.getScene().getWindow();
            stage.setScene(scene);

        } catch (Exception e) {

            e.printStackTrace();

        }
    }

    @FXML
    private void cerrarCaja() {

        Alert alert = new Alert(Alert.AlertType.CONFIRMATION);
        alert.setTitle("Cerrar Caja");
        alert.setHeaderText("¿Desea cerrar la caja?");
        alert.setContentText("Se cerrará el turno actual.");

        if (alert.showAndWait().get() == ButtonType.OK) {

            try {

                int cajaId = Session.getCajaId();

                CajaService.cerrarCaja(cajaId);

                Session.clear();

                FXMLLoader loader = new FXMLLoader(
                        getClass().getResource("/com/example/pharmaonecaja/login-view.fxml")
                );

                Scene scene = new Scene(loader.load(), 700, 400);

                Stage stage = (Stage) tablaVentas.getScene().getWindow();
                stage.setScene(scene);

            } catch (Exception e) {

                e.printStackTrace();

            }
        }
    }

    private void abrirDetalleVenta(Venta venta) {

        try {

            FXMLLoader loader = new FXMLLoader(
                    getClass().getResource("/com/example/pharmaonecaja/views/detalle-venta-view.fxml")
            );

            Scene scene = new Scene(loader.load(), 600, 500);

            DetalleController controller = loader.getController();
            controller.cargarVenta(venta.getVentaId());

            Stage stage = new Stage();
            stage.setTitle("Detalle de Venta");
            stage.setScene(scene);
            stage.show();

        } catch (Exception e) {

            e.printStackTrace();

        }
    }

}