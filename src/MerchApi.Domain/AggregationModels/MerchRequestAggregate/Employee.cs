﻿using System.Collections.Generic;

using MerchApi.Domain.SharedKernel.Models;

namespace MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    /// <summary>
    /// Информация о сотруднике
    /// </summary>
    public class Employee : ValueObject
    {
        /// <summary>
        /// Адрес электронной почты. В нашем случае для упрощения выступает идентификатором сотрудника
        /// </summary>
        public Email Email { get; }

        private Employee(Email email) =>
            Email = email;

        public static Employee Create(Email email)
        {
            return new Employee(email);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Email;
        }
    }
}