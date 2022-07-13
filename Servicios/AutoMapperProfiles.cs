using AutoMapper;
using gestorPresupuestos.Models;

namespace ManejoPresupuesto.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();//Desde, Hasta
            CreateMap<TransaccionActualizacionViewModel, Transaccion>().ReverseMap();
            //ReverseMap: Permite que el mapeo sea ida y vuelta:
            //                      A -> B // B -> A
        }
    }
}

