using AutoMapper;
using gestorPresupuestos.Models;

namespace ManejoPresupuesto.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();//Desde, Hasta
            CreateMap<TransaccionCreacionViewModel, Transaccion>().ReverseMap();
            //ReverseMap: Permite que el mapeo sea idea y vuelta:
            //                      A -> B // B -> A
        }
    }
}

