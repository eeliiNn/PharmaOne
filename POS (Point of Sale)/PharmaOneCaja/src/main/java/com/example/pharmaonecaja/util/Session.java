package com.example.pharmaonecaja.util;

public class Session {

    private static int usuarioId;
    private static int cajaId;
    private static String rol;

    public static int getUsuarioId() {
        return usuarioId;
    }

    public static void setUsuarioId(int id) {
        usuarioId = id;
    }

    public static int getCajaId() {
        return cajaId;
    }

    public static void setCajaId(int id) {
        cajaId = id;
    }

    public static String getRol() {
        return rol;
    }

    public static void setRol(String r) {
        rol = r;
    }

    public static void clear(){
        usuarioId = 0;
        cajaId = 0;
        rol = null;
    }
}