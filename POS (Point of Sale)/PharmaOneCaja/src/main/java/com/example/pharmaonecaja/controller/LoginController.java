package com.example.pharmaonecaja.controller;

import com.example.pharmaonecaja.model.LoginResponse;
import com.example.pharmaonecaja.service.AuthService;
import com.example.pharmaonecaja.service.CajaService;
import com.example.pharmaonecaja.util.Session;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

public class LoginController {

    @FXML
    private TextField txtUsuario;

    @FXML
    private PasswordField txtPassword;

    @FXML
    private void login() {

        String usuario = txtUsuario.getText();
        String password = txtPassword.getText();

        if (usuario.isEmpty() || password.isEmpty()) {
            mostrarAlerta("Debe ingresar usuario y contraseña");
            return;
        }

        try {

            LoginResponse response = AuthService.login(usuario, password);

            if (response == null) {
                mostrarAlerta("Credenciales incorrectas");
                return;
            }

            // Guardar sesión
            Session.setUsuarioId(response.getUsuarioId());
            Session.setRol(response.getRol());

            System.out.println("Usuario logueado: " + Session.getUsuarioId());

            // Si es empleado abrir caja
            if (response.getRol().equalsIgnoreCase("Empleado")) {

                int cajaId = CajaService.abrirCaja(100, Session.getUsuarioId());

                Session.setCajaId(cajaId);

                System.out.println("Caja abierta: " + cajaId);
            }

            abrirHome();

        } catch (Exception e) {

            e.printStackTrace();
            mostrarAlerta("Error conectando con la API");

        }
    }

    private void abrirHome() {

        try {

            FXMLLoader loader = new FXMLLoader(
                    getClass().getResource("/com/example/pharmaonecaja/views/ventas-view.fxml")
            );

            Scene scene = new Scene(loader.load(), 1200, 700);

            Stage stage = (Stage) txtUsuario.getScene().getWindow();
            stage.setScene(scene);

        } catch (Exception e) {

            e.printStackTrace();
            mostrarAlerta("Error abriendo la pantalla principal");

        }
    }

    private void mostrarAlerta(String mensaje) {

        Alert alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setHeaderText(null);
        alert.setContentText(mensaje);
        alert.showAndWait();

    }
}