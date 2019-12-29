using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

using TinySeoSharp.Web.Tokenizer;
using TinySeoSharp.Web.Utils;

namespace TinySeoSharp.Web {

	public class Route {
		public static Route For(string route) => new Route(route);
		private readonly IEnumerable<RouteTokens> _routeTokens;
		private readonly string _route;
		private readonly string _path;
		private readonly string _queryString;

		private Route(string route) {
			var routeParts = route.ToLower().Split('?');
			var path = routeParts.First();
			var queryString = routeParts.ElementAtOrDefault(1) ?? string.Empty;

			queryString = string.Join("&", queryString.Split('&').OrderBy(s => s)); 

			_route = $"{path}{(queryString == string.Empty ? "" : $"?{queryString}")}";
			_path = path;
			_queryString = queryString;
			_routeTokens = RouteTokenizer.For(_route).Tokenize();
		}

		public bool IsMatch(string url) {
			var path = url.ToLower().Split('?').First();
			return _routeTokens.Any(routeTokens => routeTokens.IsMatch(path));
		}

		public T Match<T>(string url) where T : new() {
			var urlParts = url.ToLower().Split('?');
			var path = urlParts.First();
			var queryString = urlParts.ElementAtOrDefault(1) ?? string.Empty;
			var routeTokens = _routeTokens.FirstOrDefault(rt => rt.IsMatch(path));

			if (routeTokens == default) {
				return default;
			}

			var instance = new T();

			var properties = ReflectionUtils.GetProperties(typeof(T));
			var pathMatch = routeTokens.PathTokens.Match(path);

			var routeParameterTokens = routeTokens.PathTokens.OfType<RouteParameterToken>().ToArray();

			for (var i = 0; i < routeParameterTokens.Length; ++i) {
				var name = routeParameterTokens[i].ParameterName;
				var value = pathMatch.Groups[1 + i].Value;
				SetValue(instance, name, value);
			}

			var queryParameterTokens = routeTokens.QueryTokens.OfType<QueryParameterToken>().ToArray();
			for (var i = 0; i < queryParameterTokens.Length; ++i) {
				var token = queryParameterTokens[i];
				var name = token.ParameterName;
				var match = Regex.Match(queryString, token.Pattern);
				if (match.Success) {
					var value = match.Groups[1].Value;
					SetValue(instance, name, value);
				}
			}

			return instance;
		}

		public string Build<T>(T query) where T : new() {
			var path = _path;
			var queryString = _queryString;
			var type = typeof(T);
			var properties = ParserUtils.GetAssignableProperties(type);

			foreach (var property in properties) {
				var propertyType = property.PropertyType;
				var name = property.Name.ToLower();
				var value = property.GetValue(query, null);
				var defaultValue = ReflectionUtils.GetDefaultValue(propertyType);
				var pattern = new Regex($@"\{{\??{name}\}}");

				if (!pattern.IsMatch(_route) || value == null || value.Equals(defaultValue)) {
					continue;
				}
				
				if (ConverterUtils.IsPrimitive(propertyType) || propertyType.IsEnum) {
					value = UrlBuilderUtils.CreateFragment(propertyType, value);
				}

				if (ConverterUtils.IsPrimitiveArray(propertyType) && value != null) {
					var items = (IList)value;
					var values = new List<string>();
					foreach (var item in items) {
						values.Add((string)Convert.ChangeType(item, typeof(string)));
					}
					value = string.Join(",", values);
				}

				if (ConverterUtils.IsEnumArray(propertyType) && value != null) {
					value = UrlBuilderUtils.CreateFragment(propertyType, value);
				}

				path = pattern.Replace(path, (string)value);
				queryString = pattern.Replace(queryString, (string)value);
			}
			var url = $"{path}{(string.IsNullOrEmpty(queryString) ? "" : $"?{queryString}")}";
			url = Regex.Replace(url, @"\w+=\{\??\w+\}&?", string.Empty);

			if (IsMatch(url) == false) {
				throw new ArgumentException();
			}

			return url;
		}

		private void SetValue<T>(T instance, string name, string value) where T : new() {
			value = WebUtility.UrlDecode(value);
			var properties = ParserUtils.GetAssignableProperties(typeof(T));
			var property = properties.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			var propertyType = property.PropertyType;

			if (ConverterUtils.IsPrimitive(propertyType)) {
				propertyType = ReflectionUtils.EnsureNotNullableType(propertyType);
				property.SetValue(instance, Convert.ChangeType(value, propertyType), null);
			}

			if (propertyType.IsEnum) {
				if (int.TryParse(value, out var enumValue)) {
					property.SetValue(instance, Enum.ToObject(propertyType, enumValue), null);
				} else {
					property.SetValue(instance, EnumFuzzy.Parse(propertyType, value), null);
				}
			}

			if (ConverterUtils.IsPrimitiveArray(propertyType)) {
				var elementType = ReflectionUtils.EnsureNotNullableType(propertyType.GetElementType());
				var values = value.Split(',').ToList();
				var array = Array.CreateInstance(elementType, values.Count);

				for (var i = 0; i < values.Count; ++i) {
					array.SetValue(Convert.ChangeType(values[i], elementType), i);					
				}

				property.SetValue(instance, array, null);
			}

			if (ConverterUtils.IsEnumArray(propertyType)) {
				var elementType = ReflectionUtils.EnsureNotNullableType(propertyType.GetElementType());
				var values = value.Split(',').ToList();
				var array = Array.CreateInstance(elementType, values.Count);

				for (var i = 0; i < values.Count; ++i) {
					if (int.TryParse(values[i], out var enumValue)) {
						array.SetValue(Enum.ToObject(elementType, enumValue), i);
					} else {
						array.SetValue(EnumFuzzy.Parse(elementType, values[i]), i);
					}
				}
				property.SetValue(instance, array, null);
			}
		}
	}
}