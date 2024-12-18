﻿using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Application.DiasSemana.List;

/// <summary>
/// Lista todos los días de semana.
/// </summary>
public sealed record ListDiasSemana : IQueryListOf<DiaSemana>;
