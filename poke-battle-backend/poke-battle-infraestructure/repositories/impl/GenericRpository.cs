using System.Reflection;
using Newtonsoft.Json;
using poke.battle.infraestructure.filters;
using poke.battle.Models;

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
                return data;

            var propInfo = typeof(T).GetProperty(orderBy.Property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if(propInfo == null || propInfo.PropertyType.IsArray)
                return data;


            return orderBy.Direction == OrderDirection.desc ? 
            data.OrderByDescending(e => propInfo.GetValue(e, null)).ToList() : 
            data.OrderBy(e => propInfo.GetValue(e, null)).ToList();
        }

        private int GetId(List<T> list) {
            return list.Count != 0 ? list.Max(e => e.Id) + 1 : 1;
        }

        private void PreparePredicates(F filter)
        {
            predicates.Clear();
            if(filter != null && !string.IsNullOrEmpty(filter.Search))
            {
                predicates.Add(x => x.DisplayName.Contains(filter.Search, StringComparison.OrdinalIgnoreCase));
            }

            if(filter != null)
            {
                CreatePredicates(filter!);
            }
        }

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