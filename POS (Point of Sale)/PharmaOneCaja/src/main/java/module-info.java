module com.example.pharmaonecaja {

    requires javafx.controls;
    requires javafx.fxml;
    requires com.google.gson;

    opens com.example.pharmaonecaja.controller to javafx.fxml;
    opens com.example.pharmaonecaja.model to javafx.base, com.google.gson;
    opens com.example.pharmaonecaja.dto to com.google.gson;

    exports com.example.pharmaonecaja;
}