namespace OzyParkAdmin.Application;

/// <summary>
/// Representa el contexto de OzyParkAdmin.
/// </summary>
public interface IOzyParkAdminContext
{
    /// <summary>
    /// Begins tracking the given entity, and any other reachable entities that are
    /// not already being tracked, in the added state such that
    /// they will be inserted into the database when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to add.</param>
    void Add<TEntity>(TEntity entity)
        where TEntity : class;

    /// <summary>
    /// Begins tracking the given entities, and any other reachable entities that are
    /// not already being tracked, in the added state such that they will
    ///     be inserted into the database when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    void AddRange(IEnumerable<object> entities);

    /// <summary>
    /// Begins tracking the given entity, and any other reachable entities that are
    /// not already being tracked, in the added state such that they will
    /// be inserted into the database when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous Add operation.
    /// </returns>
    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class;

    /// <summary>
    /// Begins tracking the given entity, and any other reachable entities that are
    /// not already being tracked, in the added state such that they will
    /// be inserted into the database when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins tracking the given entity and entries reachable from the given entity using
    /// the unchanged state by default, but see below for cases
    /// when a different state will be used.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Generally, no database interaction will be performed until <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    ///     </para>
    ///     <para>
    ///         A recursive search of the navigation properties will be performed to find reachable entities
    ///         that are not already being tracked by the context. All entities found will be tracked
    ///         by the context.
    ///     </para>
    ///     <para>
    ///         For entity types with generated keys if an entity has its primary key value set
    ///         then it will be tracked in the unchanged state. If the primary key
    ///         value is not set then it will be tracked in the added state.
    ///         This helps ensure only new entities will be inserted.
    ///         An entity is considered to have its primary key value set if the primary key property is set
    ///         to anything other than the CLR default for the property type.
    ///     </para>
    ///     <para>
    ///         For entity types without generated keys, the state set is always unchanged.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to attach.</param>
    void Attach<TEntity>(TEntity entity)
        where TEntity : class;

    /// <summary>
    /// Begins tracking the given entities and entries reachable from the given entities using
    /// the unchanged state by default, but see below for cases
    /// when a different state will be used.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Generally, no database interaction will be performed until <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    ///     </para>
    ///     <para>
    ///         A recursive search of the navigation properties will be performed to find reachable entities
    ///         that are not already being tracked by the context. All entities found will be tracked
    ///         by the context.
    ///     </para>
    ///     <para>
    ///         For entity types with generated keys if an entity has its primary key value set
    ///         then it will be tracked in the unchanged state. If the primary key
    ///         value is not set then it will be tracked in the added state.
    ///         This helps ensure only new entities will be inserted.
    ///         An entity is considered to have its primary key value set if the primary key property is set
    ///         to anything other than the CLR default for the property type.
    ///     </para>
    ///     <para>
    ///         For entity types without generated keys, the state set is always unchanged.
    ///     </para>
    /// </remarks>
    /// <param name="entities">The entities to attach.</param>
    void AttachRange(IEnumerable<object> entities);

    /// <summary>
    /// Begins tracking the given entities and entries reachable from the given entities using
    /// the unchanged state by default, but see below for cases
    /// when a different state will be used.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Generally, no database interaction will be performed until <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    ///     </para>
    ///     <para>
    ///         A recursive search of the navigation properties will be performed to find reachable entities
    ///         that are not already being tracked by the context. All entities found will be tracked
    ///         by the context.
    ///     </para>
    ///     <para>
    ///         For entity types with generated keys if an entity has its primary key value set
    ///         then it will be tracked in the unchanged state. If the primary key
    ///         value is not set then it will be tracked in the added state.
    ///         This helps ensure only new entities will be inserted.
    ///         An entity is considered to have its primary key value set if the primary key property is set
    ///         to anything other than the CLR default for the property type.
    ///     </para>
    ///     <para>
    ///         For entity types without generated keys, the state set is always unchanged.
    ///     </para>
    /// </remarks>
    /// <param name="entities">The entities to attach.</param>
    void AttachRange(params object[] entities);

    /// <summary>
    /// Begins tracking the given entity in the deleted state such that it will
    /// be removed from the database when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If the entity is already tracked in the added state then the context will
    ///         stop tracking the entity (rather than marking it as deleted) since the
    ///         entity was previously added to the context and does not exist in the database.
    ///     </para>
    ///     <para>
    ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
    ///         they would be if <see cref="Attach{TEntity}(TEntity)" /> was called before calling this method.
    ///         This allows any cascading actions to be applied when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to remove.</param>
    void Remove<TEntity>(TEntity entity)
        where TEntity : class;

    /// <summary>
    /// Begins tracking the given entity in the deleted state such that it will
    /// be removed from the database when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If any of the entities are already tracked in the added state then the context will
    ///         stop tracking those entities (rather than marking them as deleted) since those
    ///         entities were previously added to the context and do not exist in the database.
    ///     </para>
    ///     <para>
    ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
    ///         they would be if <see cref="AttachRange(IEnumerable{object})" /> was called before calling this method.
    ///         This allows any cascading actions to be applied when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    ///     </para>
    /// </remarks>
    /// <param name="entities">The entities to remove.</param>
    void RemoveRange(IEnumerable<object> entities);

    /// <summary>
    /// Begins tracking the given entity and entries reachable from the given entity using
    /// the modified state by default, but see below for cases
    /// when a different state will be used.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Generally, no database interaction will be performed until <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    ///     </para>
    ///     <para>
    ///         A recursive search of the navigation properties will be performed to find reachable entities
    ///         that are not already being tracked by the context. All entities found will be tracked
    ///         by the context.
    ///     </para>
    ///     <para>
    ///         For entity types with generated keys if an entity has its primary key value set
    ///         then it will be tracked in the modified state. If the primary key
    ///         value is not set then it will be tracked in the added state.
    ///         This helps ensure new entities will be inserted, while existing entities will be updated.
    ///         An entity is considered to have its primary key value set if the primary key property is set
    ///         to anything other than the CLR default for the property type.
    ///     </para>
    ///     <para>
    ///         For entity types without generated keys, the state set is always modified.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to update.</param>
    void Update<TEntity>(TEntity entity)
        where TEntity : class;

    /// <summary>
    /// Begins tracking the given entities and entries reachable from the given entities using
    /// the modified state by default, but see below for cases
    /// when a different state will be used.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Generally, no database interaction will be performed until <see cref="SaveChangesAsync(CancellationToken)" /> is called.
    ///     </para>
    ///     <para>
    ///         A recursive search of the navigation properties will be performed to find reachable entities
    ///         that are not already being tracked by the context. All entities found will be tracked
    ///         by the context.
    ///     </para>
    ///     <para>
    ///         For entity types with generated keys if an entity has its primary key value set
    ///         then it will be tracked in the modified state. If the primary key
    ///         value is not set then it will be tracked in the added state.
    ///         This helps ensure new entities will be inserted, while existing entities will be updated.
    ///         An entity is considered to have its primary key value set if the primary key property is set
    ///         to anything other than the CLR default for the property type.
    ///     </para>
    ///     <para>
    ///         For entity types without generated keys, the state set is always modified.
    ///     </para>
    /// </remarks>
    /// <param name="entities">The entities to update.</param>
    void UpdateRange(IEnumerable<object> entities);

    /// <summary>
    /// Saves all changes made in this context to the persistence mechanism.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous save operation. The task result contains the
    /// number of state entries written to the database.
    /// </returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
