package com.example.pharmaonecaja.service;

import java.io.*;
import java.net.HttpURLConnection;
import java.net.URL;

public class Api {

    private static final String BASE_URL = "http://localhost:5221/api/";

    public static String post(String endpoint, String jsonInput) {

        try {

            URL url = new URL(BASE_URL + endpoint);
            HttpURLConnection conn = (HttpURLConnection) url.openConnection();

            conn.setRequestMethod("POST");
            conn.setRequestProperty("Content-Type", "application/json");
            conn.setDoOutput(true);

            if (jsonInput != null && !jsonInput.isEmpty()) {
                try (OutputStream os = conn.getOutputStream()) {
                    byte[] input = jsonInput.getBytes("utf-8");
                    os.write(input, 0, input.length);
                }
            }

            int status = conn.getResponseCode();

            InputStream stream;

            if (status >= 200 && status < 300) {
                stream = conn.getInputStream();
            } else {
                stream = conn.getErrorStream();
            }

            if (stream == null) {
                return "";
            }

            BufferedReader br = new BufferedReader(
                    new InputStreamReader(stream, "utf-8")
            );

            StringBuilder response = new StringBuilder();
            String line;

            while ((line = br.readLine()) != null) {
                response.append(line.trim());
            }

            conn.disconnect();

            System.out.println("Status: " + status);
            System.out.println("Response: " + response);

            return response.toString();

        } catch (Exception e) {
            e.printStackTrace();
            throw new RuntimeException("Error API: " + e.getMessage());
        }
    }

    public static String get(String endpoint) {

        try {

            URL url = new URL(BASE_URL + endpoint);

            HttpURLConnection conn = (HttpURLConnection) url.openConnection();
            conn.setRequestMethod("GET");

            BufferedReader br = new BufferedReader(
                    new InputStreamReader(conn.getInputStream(), "utf-8"));

            StringBuilder response = new StringBuilder();
            String line;

            while ((line = br.readLine()) != null) {
                response.append(line.trim());
            }

            conn.disconnect();

            return response.toString();

        } catch (Exception e) {
            throw new RuntimeException("Error API GET: " + e.getMessage());
        }
    }
}