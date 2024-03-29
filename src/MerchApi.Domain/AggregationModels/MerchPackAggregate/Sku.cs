﻿using System.Collections.Generic;

using MerchApi.Domain.Exceptions;
using MerchApi.Domain.SharedKernel.Models;

namespace MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    /// <summary>
    /// Товарная позиция на складе одного из товаров мерча
    /// </summary>
    public class Sku : ValueObject
    {
        public int Value { get; }

        private Sku(int value)
        {
            Value = value;
        }

        public static Sku Create(int value)
        {
            if (value > 0)
            {
                return new Sku(value);
            }

            throw new ItemException("ItemId is invalid!");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}