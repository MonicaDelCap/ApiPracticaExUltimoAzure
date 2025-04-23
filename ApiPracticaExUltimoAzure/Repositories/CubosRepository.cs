using ApiPracticaExUltimoAzure.Data;
using ApiPracticaExUltimoAzure.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPracticaExUltimoAzure.Repositories
{
    public class CubosRepository
    {
        private CubosContext context;

        public CubosRepository(CubosContext con)
        {
            this.context = con;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            List<Cubo> cubos = await this.context.Cubos.ToListAsync();
            foreach(Cubo c in cubos)
            {
                c.Imagen = "https://storageexamenmdc.blob.core.windows.net/cubosimg/" + c.Imagen;
            }
            return cubos;
        }

        public async Task<Cubo> FindCuboByIdAsync(int idcubo)
        {
            Cubo cubo = await this.context.Cubos.FirstOrDefaultAsync(x => x.IdCubo == idcubo);
            cubo.Imagen = "https://storageexamenmdc.blob.core.windows.net/cubosimg/" + cubo.Imagen;
            return cubo;
        }

        public async Task CreateUsuarioAsync(string nombre, string email, string password, string imagen)
        {
            int idmax = await (this.context.Usuarios).MaxAsync(x => x.IdUsuario);
            UsuariosCubo usuario = new UsuariosCubo
            {
                IdUsuario = idmax + 1,
                Nombre = nombre,
                Email = email,
                Password = password, 
                Imagen = imagen
            };

            await this.context.Usuarios.AddAsync(usuario);
            await this.context.SaveChangesAsync();
        }

        public async Task<UsuariosCubo> PerfilUsuario(int idusuario)
        {
            UsuariosCubo usuario =  await this.context.Usuarios.Where(x => x.IdUsuario == idusuario).FirstOrDefaultAsync();
            usuario.Imagen = "https://storageexamenmdc.blob.core.windows.net/cubosimg/" + usuario.Imagen;
            return usuario;
        }

        public async Task<List<CompraCubos>> ComprasCubos(int idusuario)
        {
            return await this.context.CompraCubos.Where(x => x.IdUsuario == idusuario).ToListAsync();
        }

        public async Task RealizarPedido(int idusuario, int idcubo)
        {
            int idmax = await (this.context.CompraCubos).MaxAsync(x => x.IdUsuario);
            CompraCubos compra = new CompraCubos
            {
                IdPedido = idmax+ 1,
                IdCubo = idcubo,
                IdUsuario = idusuario,
                FechaPedido = DateTime.Now
            };

            await this.context.CompraCubos.AddAsync(compra);
            await this.context.SaveChangesAsync();
        }
        public async Task<UsuariosCubo> LoginUsuario(string email, string password)
        {
            return await this.context.Usuarios.Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }
    }
}
