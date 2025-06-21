using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poke.battle.infraestructure.filters;
using poke.battle.Models;
using poke_battle_infraestructure.filters;
using System.Reflection;
using System.Text.Json;

namespace poke.battle.infraestructure.repositories.impl
{
    public abstract class GenericRepository<T, F> : IRepository<T, F>
        where T : IModel
        where F : IFilter<T>, new()
    {
        protected string STORED_FILE = string.Empty;
        private string DIRECTORY_PATH = Path.Combine(AppContext.BaseDirectory, "PBS");
        protected List<Func<T, bool>> predicates = [];
        public GenericRepository() {
            if(!Directory.Exists(DIRECTORY_PATH)) 
            {
                Directory.CreateDirectory(DIRECTORY_PATH);
            }
        }

        public PageResponse<T> GetAll(F filter, PageRequest? pageRequest)
        {
            PreparePredicates(filter);
            var list = ApplyPredicates(Load());

            int total = list.Count;
            int pageSize = 25;
            int page = 1;
            ApplyPageRequest(pageRequest, ref list, ref pageSize, ref page);

            return new()
            {
                Results = list,
                PageSize = pageSize,
                Page = page,
                Total = total,
            };
        }

        private void ApplyPageRequest(PageRequest? pageRequest, ref List<T> list, ref int pageSize, ref int page)
        {
            if (pageRequest != null)
            {
                list = ApplyOrderBy(list, pageRequest.OrderBy);

                if (pageRequest.PageSize > 0)
                {
                    page = pageRequest.Page;
                    pageSize = pageRequest.PageSize;
                    int skip = (pageRequest.Page - 1) * pageRequest.PageSize;
                    list = [.. list.Skip(skip).Take(pageRequest.PageSize)];
                }
            }
        }

        protected abstract void CreatePredicates(F filter);

        public T FindById(int id)
        {
            if(!File.Exists(GetFullPath()))
                throw new RepositoryException($"Error initialization repository");


            var entity = Load().FirstOrDefault(x => x.Id == id);
            return entity ?? throw new RepositoryException($"Entity not found with id '{id}'");
        }

        public T FindByName(string name)
        {
            if(!File.Exists(GetFullPath()))
                throw new RepositoryException($"Error initialization repository");
            if(string.IsNullOrEmpty(name)) 
                throw new RepositoryException("Argument name can not empty");

            var entity = Load().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return entity ?? throw new RepositoryException($"Entity not found with name '{name}'");
        }

        public T Save(T entity)
        {     
            var list = Load();
            if(list.Any(x => x.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase)))
                throw new RepositoryException($"Entity with name '{entity.Name}' already exists");

            entity.Id = GetId(list);
            list.Add(entity);
            Save(list);

            return entity;
            
        }
        public T Update(T entity)
        {
            var list = Load();
            var existing = list.FirstOrDefault(x => x.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase));
            if(existing != null && existing.Id != entity.Id) 
                throw new RepositoryException($"Another entity with name '{entity.Name}' already exists");


            list.RemoveAll(e => e.Id == entity.Id);
            list.Add(entity);
            Save(list);        
            
            return entity;
        }

        public bool Delete(T entity)
        {   
            T entityToDelete = FindById(entity.Id);

            var list = Load();
            var removed = list.RemoveAll(e => e.Id == entityToDelete.Id);
            Save(list);
            return removed > 0;
        }

        protected void SetDefaultPredicate(F filter) 
        {
            predicates.Clear();

        }

        protected List<T> ApplyOrderBy(List<T> data, OrderBy? orderBy) 
        {
            if(orderBy == null || string.IsNullOrEmpty(orderBy.Property))
                return [.. data.OrderBy(x => x.Id)];

            var propInfo = typeof(T).GetProperty(orderBy.Property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if(propInfo == null || propInfo.PropertyType.IsArray)
                return data;


            return orderBy.Direction == OrderDirection.desc ?
            [.. data.OrderByDescending(e => propInfo.GetValue(e, null))] :
            [.. data.OrderBy(e => propInfo.GetValue(e, null))];
        }

        private int GetId(List<T> list) {
            return list.Count != 0 ? list.Max(e => e.Id) + 1 : 1;
        }

        private void PreparePredicates(F filter)
        {
            predicates.Clear();
            if(filter != null && filter.Entries != null && filter.Entries.Count != 0)
            {
                CreatePredicates(filter.Entries);
            }

            if(filter != null)
            {
                CreatePredicates(filter!);
            }
        }

        private void CreatePredicates(List<FilterEntry> entries)
        {
            foreach (FilterEntry entry in entries)
            {
                var propInfo = typeof(T).GetProperty(entry.Property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propInfo == null) continue;

                AddPredicateForProperty(propInfo, entry);
            }
        }
        private void AddPredicateForProperty(PropertyInfo propInfo, FilterEntry entry) 
        {

            if (entry.Value == null)
            {
                if(IsValidTypeForNull(entry.Type))
                {
                    predicates.Add(x => propInfo.GetValue(x) == null && entry.Type == FilterType.isNull ||
                                         propInfo.GetValue(x) != null && entry.Type == FilterType.isNotNull);
                }else
                {
                    return;
                }

            }
                

            var type = propInfo.PropertyType;

            if (type == typeof(string) && IsValidTypeForText(entry.Type))
            {
                var strValue = ExtractStringValue(entry.Value!);
                if (!string.IsNullOrEmpty(strValue))
                    predicates.Add(CreateForText(strValue, propInfo, entry.Type));
            }
            else if (IsNumericType(type) && IsValidTypeForNumber(entry.Type))
            {
                if (TryExtractNumber(entry.Value!, out double number))
                    predicates.Add(CreateNumericPredicate(number, propInfo, entry.Type));
            }
            else if (!IsPrimitiveType(type) && type.IsArray && IsValidTypeForList(entry.Type))
            {
                var list = ExtractList(entry.Value!, type);
                if (list != null && list.Count > 0)
                    predicates.Add(CreateInListPredicate(list, propInfo, entry.Type));
            }
        }

        private static bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }

        private static bool TryExtractNumber(object value, out double result)
        {
            result = 0;

            switch (value)
            {
                case JsonElement json when json.ValueKind == JsonValueKind.Number:
                    return json.TryGetDouble(out result);
                case IConvertible conv:
                    try
                    {
                        result = Convert.ToDouble(conv);
                        return true;
                    }
                    catch { return false; }
                default:
                    return double.TryParse(value.ToString(), out result);
            }
        }

        private static Func<T, bool> CreateNumericPredicate(double value, PropertyInfo prop, FilterType type)
        {
            return type switch
            {
                FilterType.equals => x => Convert.ToDouble(prop.GetValue(x)!) == value,
                FilterType.notEquals => x => Convert.ToDouble(prop.GetValue(x)!) != value,
                FilterType.greaterThan => x => Convert.ToDouble(prop.GetValue(x)!) > value,
                FilterType.greaterThanOrEqual => x => Convert.ToDouble(prop.GetValue(x)!) >= value,
                FilterType.lessThanOrEqual => x => Convert.ToDouble(prop.GetValue(x)!) < value,
                FilterType.lessEqual => x => Convert.ToDouble(prop.GetValue(x)!) <= value,
                _ => throw new ArgumentException($"Tipo de filtro numérico no soportado: {type}")
            };
        }

        private static bool IsNumericType(Type type)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(float) ||
                   type == typeof(double) || type == typeof(decimal) || type == typeof(byte) ||
                   type == typeof(short) || type == typeof(uint) || type == typeof(ulong) ||
                   type == typeof(ushort) || type == typeof(sbyte);
        }


        private static List<object>? ExtractList(object rawValue, Type targetType)
        {
            try
            {
                if (rawValue is JsonElement json && json.ValueKind == JsonValueKind.Array)
                {
                    var list = new List<object>();
                    foreach (var el in json.EnumerateArray())
                    {
                        object? val = el.ValueKind switch
                        {
                            JsonValueKind.String => el.GetString(),
                            JsonValueKind.Number when targetType == typeof(int) && el.TryGetInt32(out var i) => i,
                            JsonValueKind.Number when targetType == typeof(long) && el.TryGetInt64(out var l) => l,
                            JsonValueKind.Number when targetType == typeof(double) && el.TryGetDouble(out var d) => d,
                            JsonValueKind.Number when targetType == typeof(decimal) && el.TryGetDecimal(out var dec) => dec,
                            _ => null
                        };
                        if (val != null) list.Add(val);
                    }
                    return list;
                }

                if (rawValue is IEnumerable<object> directList)
                    return [.. directList];

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static  Func<T, bool> CreateInListPredicate(List<object> values, PropertyInfo prop, FilterType type)
        {
            return type switch
            {
                FilterType.inList => x =>
                {
                    var val = prop.GetValue(x);
                    return val != null && values.Contains(val);
                }
                ,
                FilterType.notInList => x =>
                {
                    var val = prop.GetValue(x);
                    return val == null || !values.Contains(val);
                }
                ,
                _ => throw new ArgumentException("Tipo de lista no soportado.")
            };
        }

        private static string? ExtractStringValue(object value)
        {
            return value switch
            {
                string s => s,
                JsonElement json => json.ValueKind == JsonValueKind.String ? json.GetString() : null,
                _ => value.ToString()
            };
        }


        private static Func<T, bool> CreateForText(string value, PropertyInfo prop, FilterType type)
        {

            return type switch
            {
                FilterType.contains => x => prop.GetValue(x)?.ToString()?.Contains(value, StringComparison.OrdinalIgnoreCase) ?? false,
                FilterType.startsWith => x => prop.GetValue(x)?.ToString()?.StartsWith(value, StringComparison.OrdinalIgnoreCase) ?? false,
                FilterType.endsWith => x => prop.GetValue(x)?.ToString()?.EndsWith(value, StringComparison.OrdinalIgnoreCase) ?? false,
                FilterType.equals => x => string.Equals(prop.GetValue(x)?.ToString(), value, StringComparison.OrdinalIgnoreCase),
                FilterType.notEquals => x => !string.Equals(prop.GetValue(x)?.ToString(), value, StringComparison.OrdinalIgnoreCase),
                _ => throw new ArgumentException($"Tipo de filtro no soportado: {type}")
            };
        }

        private static bool IsValidTypeForNull(FilterType type) => type is FilterType.isNull or FilterType.isNotNull;
        private static bool  IsValidTypeForNumber(FilterType type) => type is FilterType.equals or FilterType.notEquals or FilterType.greaterThan or FilterType.greaterThanOrEqual or FilterType.lessThanOrEqual or FilterType.lessEqual;
        private static bool IsValidTypeForText(FilterType type) => type is FilterType.contains or FilterType.startsWith or FilterType.endsWith or FilterType.equals or FilterType.notEquals;
        private static bool IsValidTypeForList(FilterType type) => type is FilterType.inList or FilterType.notInList;

        private List<T> ApplyPredicates(List<T> data) 
        {
            if(predicates.Count == 0) return data;

            var combined = predicates.Aggregate((p1, p2) => x => p1(x) && p2(x));
            return [.. data.Where(combined)];
        }

        protected List<T> Load() 
        {
            if(!File.Exists(GetFullPath()))
                return [];
            return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(GetFullPath())) ?? [];
        }

        protected void Save(List<T> entities)
        {
            File.WriteAllText(GetFullPath(), JsonConvert.SerializeObject(entities, Formatting.Indented));
        }
        
        protected string GetFullPath()  {
            return Path.Combine(DIRECTORY_PATH, STORED_FILE);
        }


    }
}