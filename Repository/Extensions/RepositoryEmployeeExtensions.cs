﻿using System.Linq.Dynamic.Core;
using Entities.Models;
using Repository.Utility;

namespace Repository.Extensions;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
        => employees.Where(e => e.Age >= minAge && e.Age <= maxAge);

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return employees;

        return employees.Where(e => e.Name!.Contains(searchTerm));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return employees.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return employees.OrderBy(e => e.Name);

        return employees.OrderBy(orderQuery);

    }

}