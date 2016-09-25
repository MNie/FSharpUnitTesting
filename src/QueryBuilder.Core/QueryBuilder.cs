using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FsharpTest
{
    public class QueryBuilder
    {
        private readonly NameValueCollection _queryParameters;
        public QueryBuilder()
        {
            _queryParameters = HttpUtility.ParseQueryString(string.Empty);
        }
        public QueryBuilder WhereStartDateIs(string startDate)
        {
            return WhereSingleOptionIs(EventsParametersConstants.StartDate, startDate);
        }

        public QueryBuilder WhereEndDateIs(string endDate)
        {
            return WhereSingleOptionIs(EventsParametersConstants.EndDate, endDate);
        }

        public QueryBuilder WhereOrganizerIs(int[] ids)
        {
            return WhereMultiOptionalIs(EventsParametersConstants.Organizer, ids);
        }

        public QueryBuilder WhereCategoryIs(int[] ids)
        {
            return WhereMultiOptionalIs(EventsParametersConstants.Category, ids);
        }

        public QueryBuilder WherePlaceIs(int[] ids)
        {
            return WhereMultiOptionalIs(EventsParametersConstants.Place, ids);
        }

        public QueryBuilder WhereStartTicketPriceIs(int minPrice)
        {
            return WhereSingleOptionIs(EventsParametersConstants.StartTicket, minPrice.ToString());
        }

        public QueryBuilder WhereEndTicketPriceIs(int maxPrice)
        {
            return WhereSingleOptionIs(EventsParametersConstants.EndTicket, maxPrice.ToString());
        }

        public QueryBuilder WhereTypeOfTicketsIs(string type)
        {
            return WhereSingleOptionIs(EventsParametersConstants.Tickets, type);
        }

        private QueryBuilder WhereMultiOptionalIs(string key, int[] ids)
        {
            return ids.Any() ? 
                WhereSingleOptionIs(key, JsonConvert.SerializeObject(ids)) : 
                this;
        }

        private QueryBuilder WhereSingleOptionIs(string key, string value)
        {
            _queryParameters.Add(key, value);
            return this;
        }

        public string CreateQuery()
        {
            return _queryParameters.ToString();
        }
    }
}
