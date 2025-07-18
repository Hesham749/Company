﻿using System.Dynamic;
using System.Reflection;
using Contracts;

namespace Service.DataShaping;

public class DataShaper<T> : IDataShaper<T> where T : class
{
    public DataShaper() => Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    public PropertyInfo[] Properties { get; set; }

    public IEnumerable<ExpandoObject> ShapeDate(IEnumerable<T> entities, string? fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);
        return FetchData(entities, requiredProperties);
    }

    public ExpandoObject ShapeDate(T entity, string? fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);
        return FetchDataForEntity(entity, requiredProperties);
    }

    private List<PropertyInfo> GetRequiredProperties(string? fieldsString)
    {
        var requiredProperties = new List<PropertyInfo>();

        if (!string.IsNullOrWhiteSpace(fieldsString))
        {
            var fields = fieldsString.Split(',',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var field in fields)
            {
                var property = Properties
                .FirstOrDefault(pi => pi.Name.Equals(field,
                StringComparison.InvariantCultureIgnoreCase));

                if (property is not null)
                    requiredProperties.Add(property);
            }
        }
        else
        {
            requiredProperties = [.. Properties];
        }

        return requiredProperties;
    }

    private List<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        => [.. entities.Select(entity => FetchDataForEntity(entity, requiredProperties))];

    private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ExpandoObject();

        foreach (var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            shapedObject.TryAdd(property.Name, objectPropertyValue);
        }

        return shapedObject;
    }
}