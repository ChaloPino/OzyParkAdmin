﻿using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.ExclusionesCupo;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.ExclusionesCupo.Delete;

/// <summary>
/// Elimina varias fechas excluidas.
/// </summary>
/// <param name="FechasExcluidas">La información de las fechas excluidas de cupos que se quieren eliminar.</param>
public sealed record DeleteFechasExcluidasCupo(ImmutableArray<FechaExcluidaCupoFullInfo> FechasExcluidas) : ICommand;
