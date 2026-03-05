package com.example.pharmaonecaja.service;

import com.example.pharmaonecaja.model.LoginRequest;
import com.example.pharmaonecaja.model.LoginResponse;
import com.google.gson.Gson;

import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Scanner;

public class AuthService {

    private static final String BASE_URL = "http://localhost:5221/api/auth/login";

    public static LoginResponse login(String usuario, String password) throws Exception {

        URL url = new URL(BASE_URL);
        HttpURLConnection conn = (HttpURLConnection) url.openConnection();

        conn.setRequestMethod("POST");
        conn.setRequestProperty("Content-Type", "application/json");
        conn.setDoOutput(true);

        LoginRequest request = new LoginRequest(usuario, password);
        String json = new Gson().toJson(request);

        try (OutputStream os = conn.getOutputStream()) {
            os.write(json.getBytes());
        }

        if (conn.getResponseCode() != 200) {
            return null;
        }

        Scanner scanner = new Scanner(conn.getInputStream());
        String response = scanner.useDelimiter("\\A").next();
        scanner.close();

        return new Gson().fromJson(response, LoginResponse.class);
    }
}