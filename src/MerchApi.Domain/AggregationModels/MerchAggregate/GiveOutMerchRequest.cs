﻿using System;
using System.Collections.Generic;

using MerchApi.Domain.Exceptions;
using MerchApi.Domain.SharedKernel.Models;

namespace MerchApi.Domain.AggregationModels.MerchAggregate
{
    /// <summary>
    /// Заявка на выдачу мерча
    /// </summary>
    public class GiveOutMerchRequest : Entity
    {
        /// <summary>
        /// Статус заявки
        /// </summary>
        public RequestStatus Status { get; private set; }

        /// <summary>
        /// Сотрудник
        /// </summary>
        public int EmployeeId { get; private set; }

        /// <summary>
        /// Тип выданного мерча
        /// </summary>
        public MerchType MerchType { get; private set; }

        /// <summary>
        /// Дата выдачи мерча
        /// </summary>
        public DateTime? IssueDate { get; private set; }

        /// <summary>
        /// Набор товаров в мерче
        /// </summary>
        public MerchPack MerchPack { get; private set; }

        public GiveOutMerchRequest(int employeeId, MerchType merchType, RequestStatus? requestStatus = null, DateTime? issueDate = null)
        {
            EmployeeId = employeeId > 0 ? employeeId : throw new Exception("EmploeeId must be more 0");
            MerchType = merchType ?? throw new Exception("MerchType must be not null");
            IssueDate = issueDate;
            Status = requestStatus ?? RequestStatus.Draft;
        }

        public void LoadMerchPack(IEnumerable<Item> items)
        {
            if (!Status.Equals(RequestStatus.Created))
            {
                throw new RequestStatusException($"Request is not created. Change MerchPack unavailable");
            }

            MerchPack = new MerchPack(items);
        }

        /// <summary>
        /// Смена статуса у заявки
        /// </summary>
        /// <param name="status">Статус запроса</param>
        /// <exception cref="RequestStatusException">Исключение при неверном запросе</exception>
        public void ChangeStatus(RequestStatus status)
        {
            if (Status.Equals(RequestStatus.Done))
            {
                throw new RequestStatusException($"Request is done. Change status unavailable");
            }

            Status = status;
        }

        /// <summary>
        /// Завершить работу по заявке
        /// </summary>
        public void Complete()
        {
            if (Status != RequestStatus.InWork)
            {
                throw new Exception("Incorrect request status");
            }

            Status = RequestStatus.Done;
        }
    }
}