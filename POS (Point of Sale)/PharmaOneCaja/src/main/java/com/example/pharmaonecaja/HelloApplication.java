package com.example.pharmaonecaja;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.stage.Stage;

import java.io.IOException;

public class HelloApplication extends Application {
    @Override
    public void start(Stage stage) throws Exception {

        FXMLLoader loader = new FXMLLoader(
                HelloApplication.class.getResource("/com/example/pharmaonecaja/login-view.fxml")
        );

        Scene scene = new Scene(loader.load(), 700, 400);
        stage.setTitle("PharmaOne Caja");
        stage.setScene(scene);
        stage.show();
    }
}
