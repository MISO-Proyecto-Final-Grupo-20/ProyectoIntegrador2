﻿using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Inventarios;

public record InventarioValidado(Guid IdProceso, SolicitudDePedido SolicitudValiada);