using System;
using System.Linq.Expressions;
using Backend.Common.Dtos;
using Backend.Controllers;
using Backend.Infra.Data.Entities;

namespace Backend.Converters
{
    public static partial class OrderMapper
    {
        public static OrderShortDto AdaptToShortDto(this Order p1)
        {
            return p1 == null ? null : new OrderShortDto(new OrderNumber(0l, null), p1.Price, p1.Address, p1.Status);
        }
        public static OrderShortDto AdaptTo(this Order p2, OrderShortDto p3)
        {
            return p2 == null ? null : new OrderShortDto(new OrderNumber(0l, null), p2.Price, p2.Address, p2.Status);
        }
        public static Expression<Func<Order, OrderShortDto>> ProjectToShortDto => p4 => new OrderShortDto(new OrderNumber(0l, null), p4.Price, p4.Address, p4.Status);
    }
}