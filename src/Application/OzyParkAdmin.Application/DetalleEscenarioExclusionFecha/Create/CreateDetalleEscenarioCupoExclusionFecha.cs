using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.Create;
public sealed record CreateDetalleEscenarioCupoExclusionFecha(int EscenarioCupoId, IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> Exclusiones) : ICommand;
