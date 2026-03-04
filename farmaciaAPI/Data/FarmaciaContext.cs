using Microsoft.EntityFrameworkCore;
using farmaciaAPI.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Rol> Roles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Membresia> Membresias { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<DetalleCompra> DetalleCompras { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<Inventario> Inventarios { get; set; }
    public DbSet<Caja> Cajas { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetalleVentas { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<DetallePedido> DetallePedidos { get; set; }
}
