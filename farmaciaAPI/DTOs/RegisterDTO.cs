namespace farmaciaAPI.DTOs
{
    public class RegisterDTO
    {
        public string UsuarioNombre { get; set; }
        public string Password { get; set; }
        public int RolId { get; set; }

        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}
