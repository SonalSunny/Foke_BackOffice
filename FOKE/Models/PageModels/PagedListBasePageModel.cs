using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using X.PagedList;
using X.PagedList.Extensions;

namespace FOKE.Models.PageModels
{
    public class PagedListBasePageModel : BasePageModel
    {
        [BindProperty]
        public int? pageNo { get; set; }
        [BindProperty]
        public int? pageSize { get; set; }
        public bool hasPagination { get; set; }
        public string sortDirection { get; set; }
        public string sortText { get; set; }
        [BindProperty]
        public string sortColumn { get; set; }
        [BindProperty]
        public string sortOrder { get; set; }

        [BindProperty]
        public string searchField { get; set; }
        [BindProperty]
        public string globalSearch { get; set; }

        [BindProperty]
        public string globalSearchColumn { get; set; }

        public List<PageListFilterColumns> pageListFilterColumns { get; set; }

        public IPagedList<T> PagedList<T>(List<T> sourceData, long? TotalCount = null)
        {
            var pn = pageNo ?? 1;
            var ps = pageSize ?? 10;

            try
            {
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    var param = sortColumn;
                    var propertyInfo = typeof(T).GetProperty(param);

                    if (sortOrder == "0")
                    {
                        var orderBy = sourceData.OrderByDescending(x => propertyInfo?.GetValue(x, null));
                        sourceData = orderBy.ToList();
                    }
                    else
                    {
                        var orderBy = sourceData.OrderBy(x => propertyInfo?.GetValue(x, null));
                        sourceData = orderBy.ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            try
            {
                if (TotalCount == null)
                {
                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        globalSearch = globalSearch?.ToLower();

                        if (!string.IsNullOrEmpty(searchField) && searchField != "All")
                        {
                            var param = searchField;
                            var propertyInfo = typeof(T).GetProperty(param);

                            if (propertyInfo != null)
                            {
                                var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                                var filterData = sourceData
                                    .Where(x =>
                                    {
                                        var value = propertyInfo.GetValue(x, null);

                                        if (value == null)
                                            return false;

                                        if (type == typeof(string))
                                        {
                                            return value.ToString().ToLower().Contains(globalSearch.ToLower());
                                        }
                                        else if (type == typeof(long) || type == typeof(int) || type == typeof(short))
                                        {
                                            if (long.TryParse(globalSearch, out long longSearch))
                                            {
                                                return Convert.ToInt64(value) == longSearch;
                                            }
                                            return false;
                                        }

                                        // Optional: Handle other types like DateTime, decimal, etc.

                                        return false;
                                    })
                                    .ToList();

                                if (filterData != null)
                                {
                                    sourceData = filterData;
                                }
                            }
                        }
                        else
                        {
                            if (pageListFilterColumns.Count > 0)
                            {
                                string sqlFilter = "";
                                var dSource = sourceData.AsQueryable();
                                foreach (var column in pageListFilterColumns)
                                {
                                    var propertyInfo = typeof(T).GetProperty(column.ColumName);
                                    if (propertyInfo != null)
                                    {
                                        if (!string.IsNullOrEmpty(sqlFilter))
                                        {
                                            sqlFilter = $"{sqlFilter} or ";
                                        }
                                        sqlFilter = $"{sqlFilter} ({column.ColumName.Trim()} == null ? \"\":  {column.ColumName.Trim()}).ToLower().Contains(@0)";
                                    }
                                }

                                if (!string.IsNullOrEmpty(sqlFilter))
                                {
                                    var filterData = dSource.Where(sqlFilter, globalSearch).ToList();
                                    sourceData = filterData.ToList();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            IPagedList<T> pagedListData;
            if (TotalCount == null)
            {
                pagedListData = sourceData.ToPagedList(pn, ps);
                hasPagination = sourceData.Count() > ps ? true : false;
            }
            else
            {
                pagedListData = new StaticPagedList<T>(sourceData, (int)pn, (int)ps, (int)TotalCount);
                hasPagination = sourceData.Count() < TotalCount ? true : false;
            }

            pageNo = pn;
            pageSize = ps;
            return pagedListData;
        }


    }
}
