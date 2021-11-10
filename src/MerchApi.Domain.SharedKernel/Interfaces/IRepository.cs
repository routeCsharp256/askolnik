﻿using System.Threading;
using System.Threading.Tasks;

namespace MerchApi.Domain.SharedKernel.Interfaces
{
    /// <summary>
    /// Базовый интерфейс репозитория
    /// </summary>
    /// <typeparam name="TAggregationRoot">Объект сущности для управления</typeparam>
    public interface IRepository<TAggregationRoot>
    {
        /// <summary>
        /// Объект <see cref="IUnitOfWork"/>
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Создать новую сущность
        /// </summary>
        /// <param name="itemToCreate">Объект для создания</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Созданная сущность</returns>
        Task<TAggregationRoot> CreateAsync(TAggregationRoot itemToCreate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновить существующую сущность
        /// </summary>
        /// <param name="itemToUpdate">Объект для обновления</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Обновленная сущность</returns>
        Task<TAggregationRoot> UpdateAsync(TAggregationRoot itemToUpdate, CancellationToken cancellationToken = default);
    }
}