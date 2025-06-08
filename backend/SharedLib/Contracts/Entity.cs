using System;

namespace SharedLib.Contracts;

/// <summary>
/// Represents an abstract data model that has an ID property
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Gets or set the unique identifier of the entity
    /// </summary>
    public Guid? Id { get; set; }
}
