package com.example.pharmaonecaja.controller;

import com.example.pharmaonecaja.service.ClienteService;
import com.example.pharmaonecaja.service.MembresiaService;
import javafx.fxml.FXML;
import javafx.scene.control.*;

import java.time.LocalDate;

public class RegistroClienteController {

    @FXML private TextField txtUsuario;
    @FXML private PasswordField txtPassword;
    @FXML private TextField txtNombre;
    @FXML private TextField txtApellidos;
    @FXML private TextField txtTelefono;
    @FXML private TextField txtDireccion;

    @FXML private ComboBox<String> cmbMembresia;
    @FXML private DatePicker dpFechaFin;

    @FXML
    public void initialize(){

        cmbMembresia.getItems().addAll(
                "Black",
                "Dorada",
                "Azul"
        );

        cmbMembresia.getSelectionModel().selectFirst();
    }

    @FXML
    private void registrarCliente(){

        try{

            String usuario = txtUsuario.getText();
            String password = txtPassword.getText();
            String nombre = txtNombre.getText();
            String apellidos = txtApellidos.getText();
            String telefono = txtTelefono.getText();
            String direccion = txtDireccion.getText();

            String tipoMembresia = cmbMembresia.getValue();
            LocalDate fechaFin = dpFechaFin.getValue();

            int clienteId = ClienteService.registrar(
                    usuario,
                    password,
                    nombre,
                    apellidos,
                    telefono,
                    direccion
            );

            if(tipoMembresia != null && fechaFin != null){
                MembresiaService.crear(clienteId,tipoMembresia,fechaFin);
            }

            Alert alert = new Alert(Alert.AlertType.INFORMATION);
            alert.setContentText("Cliente registrado correctamente");
            alert.showAndWait();

            // cerrar ventana
            txtUsuario.getScene().getWindow().hide();

        }catch(Exception e){
            e.printStackTrace();
        }
    }
}