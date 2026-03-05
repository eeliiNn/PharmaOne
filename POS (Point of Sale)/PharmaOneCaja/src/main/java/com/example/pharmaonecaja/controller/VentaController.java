package com.example.pharmaonecaja.controller;

import com.example.pharmaonecaja.dto.DetalleVentaDTO;
import com.example.pharmaonecaja.dto.VentaDTO;
import com.example.pharmaonecaja.model.Cliente;
import com.example.pharmaonecaja.model.ItemVenta;
import com.example.pharmaonecaja.model.ProductoDisponible;
import com.example.pharmaonecaja.service.CajaService;
import com.example.pharmaonecaja.service.ProductoService;
import com.example.pharmaonecaja.service.VentaService;
import com.example.pharmaonecaja.util.Session;

import com.example.pharmaonecaja.service.ClienteService;
import com.example.pharmaonecaja.service.MembresiaService;
import com.example.pharmaonecaja.model.MembresiaResponse;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.stage.Stage;

import java.util.ArrayList;
import java.util.List;

public class VentaController {

    @FXML private TextField txtBuscar;
    @FXML private TableView<ItemVenta> tablaVenta;

    @FXML private TableColumn<ItemVenta, String> colImagen;
    @FXML private TableColumn<ItemVenta, String> colProducto;
    @FXML private TableColumn<ItemVenta, Integer> colCantidad;
    @FXML private TableColumn<ItemVenta, Double> colPrecio;
    @FXML private TableColumn<ItemVenta, Double> colSubtotal;

    @FXML private Label lblTotal;
    @FXML private ComboBox<String> cmbTipoPago;

    @FXML private TextField txtCliente;
    @FXML private Button btnNuevaMembresia;

    @FXML private Label lblCliente;
    @FXML private Label lblMembresia;
    @FXML private Label lblDescuento;

    private Integer clienteId = null;
    private double descuento = 0;

    private ObservableList<ItemVenta> lista = FXCollections.observableArrayList();
    private ProductoDisponible[] productosDisponibles;

    @FXML
    public void initialize() {

        colProducto.setCellValueFactory(data -> data.getValue().nombreProperty());
        colCantidad.setCellValueFactory(data -> data.getValue().cantidadProperty().asObject());
        colPrecio.setCellValueFactory(data -> data.getValue().precioProperty().asObject());
        colSubtotal.setCellValueFactory(data -> data.getValue().subtotalProperty().asObject());

        tablaVenta.setItems(lista);

        cmbTipoPago.getItems().addAll("Efectivo", "Tarjeta");
        cmbTipoPago.getSelectionModel().selectFirst();

        productosDisponibles = ProductoService.obtenerDisponibles();

        configurarColumnaImagen();

        actualizarTotal();
    }

    private void configurarColumnaImagen() {

        colImagen.setCellValueFactory(data ->
                new javafx.beans.property.SimpleStringProperty(data.getValue().getFoto()));

        colImagen.setCellFactory(col -> new TableCell<ItemVenta, String>() {

            private final ImageView imageView = new ImageView();

            {
                imageView.setFitHeight(40);
                imageView.setFitWidth(40);
                imageView.setPreserveRatio(true);
            }

            @Override
            protected void updateItem(String foto, boolean empty) {

                super.updateItem(foto, empty);

                if (empty || foto == null || foto.isBlank()) {
                    setGraphic(null);
                    setText(null);
                } else {

                    Image image = new Image(foto, 40, 40, true, true);
                    imageView.setImage(image);

                    setGraphic(imageView);
                    setText(null);
                }
            }
        });
    }

    @FXML
    private void buscarCliente(){

        try{

            int id = Integer.parseInt(txtCliente.getText());

            Cliente cliente = ClienteService.obtenerCliente(id);

            if(cliente == null){
                mostrarAlerta("Cliente no encontrado");
                btnNuevaMembresia.setVisible(true);
                return;
            }

            clienteId = cliente.getClienteId();

            lblCliente.setText("Cliente: " + cliente.getNombre());

            btnNuevaMembresia.setVisible(false);

            verificarMembresia(clienteId);

        }catch(Exception e){
            mostrarAlerta("Cliente inválido");
        }
    }

    @FXML
    private void agregarProducto() {

        String texto = txtBuscar.getText().toLowerCase().trim();

        if (texto.isEmpty()) {
            mostrarAlerta("Ingrese un producto");
            return;
        }

        for (ProductoDisponible p : productosDisponibles) {

            if (p.getNombre().toLowerCase().contains(texto)) {

                for (ItemVenta item : lista) {

                    if (item.getProductoId() == p.getProductoId()) {

                        if (item.getCantidad() + 1 > p.getStock()) {
                            mostrarAlerta("Stock insuficiente");
                            return;
                        }

                        item.setCantidad(item.getCantidad() + 1);
                        tablaVenta.refresh();
                        actualizarTotal();
                        txtBuscar.clear();
                        return;
                    }
                }

                if (p.getStock() <= 0) {
                    mostrarAlerta("Sin stock disponible");
                    return;
                }

                lista.add(new ItemVenta(
                        p.getProductoId(),
                        p.getNombre(),
                        1,
                        p.getPrecioVenta(),
                        p.getFoto()     // IMPORTANTE para mostrar imagen
                ));

                actualizarTotal();
                txtBuscar.clear();
                return;
            }
        }

        mostrarAlerta("Producto no encontrado");
    }

    @FXML
    private void finalizarVenta() {

        if (lista.isEmpty()) {
            mostrarAlerta("No hay productos en la venta");
            return;
        }

        if(clienteId == null){
            mostrarAlerta("Debe seleccionar un cliente");
            return;
        }

        try {

            VentaDTO venta = new VentaDTO();
            venta.setCajaId(Session.getCajaId());
            venta.setTipoPago(cmbTipoPago.getValue());
            venta.setClienteId(clienteId); // CORREGIDO

            List<DetalleVentaDTO> detalles = new ArrayList<>();

            for (ItemVenta item : lista) {
                detalles.add(new DetalleVentaDTO(
                        item.getProductoId(),
                        item.getCantidad()
                ));
            }

            venta.setDetalles(detalles);

            VentaService.crearVenta(venta);

            mostrarAlerta("Venta realizada correctamente");

            limpiarVenta();

        } catch (Exception e) {
            e.printStackTrace();
            mostrarAlerta("Error al registrar venta");
        }
    }

    @FXML
    private void abrirHistorial() {

        try {

            FXMLLoader loader = new FXMLLoader(
                    getClass().getResource("/com/example/pharmaonecaja/views/historial-view.fxml")
            );

            Scene scene = new Scene(loader.load(), 1200, 700);

            Stage stage = (Stage) tablaVenta.getScene().getWindow();
            stage.setScene(scene);

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @FXML
    private void cerrarCaja() {

        try {

            int cajaId = Session.getCajaId();
            CajaService.cerrarCaja(cajaId);

            FXMLLoader loader = new FXMLLoader(
                    getClass().getResource("/com/example/pharmaonecaja/views/login-view.fxml")
            );

            Scene scene = new Scene(loader.load(), 700, 400);

            Stage stage = (Stage) tablaVenta.getScene().getWindow();
            stage.setScene(scene);

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void verificarMembresia(int clienteId){

        try{

            MembresiaResponse m = MembresiaService.obtener(clienteId);

            if(m == null || !m.isTieneMembresia()){

                descuento = 0;

                lblMembresia.setText("Membresía: Ninguna");
                lblDescuento.setText("Descuento: 0%");

                btnNuevaMembresia.setVisible(true);

            }else{

                descuento = m.getDescuento();

                lblMembresia.setText("Membresía: " + m.getTipo());
                lblDescuento.setText("Descuento: " + descuento + "%");

                btnNuevaMembresia.setVisible(false);
            }

            actualizarTotal();

        }catch(Exception e){
            e.printStackTrace();
            descuento = 0;
        }
    }

    private void actualizarTotal(){

        double total = 0;

        for(ItemVenta item : lista){
            total += item.getSubtotal();
        }

        double montoDescuento = total * (descuento / 100);

        double totalFinal = total - montoDescuento;

        lblTotal.setText(String.format("$%.2f", totalFinal));
    }

    @FXML
    private void abrirRegistroCliente(){

        try{

            var url = getClass().getResource("/com/example/pharmaonecaja/views/registro-cliente.fxml");
            System.out.println("URL: " + url);

            FXMLLoader loader = new FXMLLoader(url);

            Parent root = loader.load();

            Stage stage = new Stage();
            stage.setTitle("Registrar Cliente");
            stage.setScene(new Scene(root,600,400));
            stage.show();

        }catch(Exception e){
            e.printStackTrace();
        }
    }

    private void limpiarVenta() {
        lista.clear();
        actualizarTotal();
    }

    private void mostrarAlerta(String mensaje) {
        Alert alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setContentText(mensaje);
        alert.showAndWait();
    }
}