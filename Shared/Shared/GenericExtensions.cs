using Shared.Network.MapperModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Shared.Extensions
{
    public static class GenericExtensions
    {
        public static bool IsNull(this object item)
        {
            return item == null;
        }
        public static T IsNull<T>(this T item, T result)
        {
            return item.IsNull() ? result : item;
        }
        public static bool IsNotNull(this object item)
        {
            return item != null;
        }

        public static bool IsIn<T>(this T item, IEnumerable<T> list)
        {
            return list.Contains(item);
        }
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return item.IsIn(list.ToList());
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> item)
        {
            return item == null || !item.Any();
        }

        public static R Switch<T, R>(this T item, params KeyValuePair<T, R>[] list)
        {
            return Switch(item, default(R), list);
        }
        public static R Switch<T, R>(this T item, R defaultReturn, params KeyValuePair<T, R>[] list)
        {
            if (list == null || !list.Any(x => x.Key.Equals(item))) return defaultReturn;
            return list.FirstOrDefault(x => x.Key.Equals(item)).Value;
        }
        public static void MapNetworkOperation<T, X>(this ServiceResponse<T> target, ServiceResponse<X> source)
        {
            target.Map(source, x => new
            {
                x.NetworkOperationStatus,
                x.NetworkOperationRequestBody,
                x.NetworkOperationRequestMethod,
                x.NetworkOperationRequestUri,
                x.NetworkOperationResponseBody,
                x.NetworkOperationResponseContentType,
                x.NetworkOperationResponseRawBody
            });
        }
        public static string[] GetExpressionFields(this Expression expression)
        {
            var ret = new List<string>();

            // Lambda type
            if (expression is LambdaExpression lambdaExpression)
            {
                if (lambdaExpression.Body is NewArrayExpression arrayBody)
                {
                    foreach (var exp in arrayBody.Expressions)
                    {
                        ret.Add(GetExpressionField(exp));
                    }
                }

                if (lambdaExpression.Body is NewExpression expressionBody)
                {
                    foreach (var exp in expressionBody.Arguments)
                    {
                        ret.Add(GetExpressionField(exp));
                    }
                }
            }

            return ret.ToArray();
        }

        public static string GetExpressionField(this Expression expression)
        {
            // Reference type
            if (expression is MemberExpression memberExpression)
            {
                if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    return GetExpressionField(memberExpression.Expression)
                           + "."
                           + memberExpression.Member.Name;
                }

                return memberExpression.Member.Name;
            }

            // Value Type
            if (expression is UnaryExpression unaryExpression)
            {
                if (unaryExpression.NodeType != ExpressionType.Convert)
                    throw new Exception(string.Format("Cannot interpret member from {0}", expression));

                return GetExpressionField(unaryExpression.Operand);
            }

            // Anonymous type
            if (expression is NewExpression newType)
            {
                return newType.Members.First().Name;
            }

            throw new Exception(string.Format("Could not determine member from {0}", expression));
        }


        public static List<int> GetDependencyIds(string dependencies)
        {
            if (string.IsNullOrEmpty(dependencies))
                return new List<int>();

            return dependencies.Split(',').Select(int.Parse).ToList();
        }


        public static T FilterEmptyFields<T>(T data)
        {
            var properties = typeof(T).GetProperties();
            var filteredData = Activator.CreateInstance<T>();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(data);
                if (value != null)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        var stringValue = value.ToString();
                        if (!string.IsNullOrEmpty(stringValue))
                        {
                            prop.SetValue(filteredData, stringValue);
                        }
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        int intValue;
                        if (int.TryParse(value.ToString(), out intValue))
                        {
                            prop.SetValue(filteredData, intValue);
                        }
                    }
                    // Agrega más casos para otros tipos de datos según sea necesario
                }
            }

            return filteredData;
        }
    }
}
