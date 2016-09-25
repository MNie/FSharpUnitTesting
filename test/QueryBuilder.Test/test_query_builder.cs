using Shouldly;
using System;
using Xunit;

namespace QueryBuilder.Test
{
    public class test_query_builder
    {
        private const int minPrice = 10;
        private const int maxPrice = 20;
        private const string tickets = "tickets";
        private readonly string date;
        private const string dot = "%2c";
        private const string openBracket = "%5b";
        private const string closeBracket = "%5d";

        public test_query_builder()
        {
            date = DateTime.UtcNow.ToString("yyyy-MM-dd");
        }

        [Fact]
        public void should_create_emtpy_query()
        {
            var query = new FsharpTest.QueryBuilder();

            var finalQuery = query.CreateQuery();

            finalQuery.ShouldBe("");
        }

        [Fact]
        public void should_return_valid_criteria()
        {
            var organizers = new [] { 2, 3 };
            var categories = new [] { 5, 6 };
            var places = new [] { 7, 4 };

            var query = CreateBaseQuery()
                .WhereOrganizerIs(organizers)
                .WhereCategoryIs(categories)
                .WherePlaceIs(places);

            var finalQuery = query.CreateQuery();

            finalQuery.ShouldBe($"start_date={date}&" +
                                $"end_date={date}&" +
                                $"startTicket={minPrice}&" +
                                $"endTicket={maxPrice}&" +
                                $"tickets={tickets}&" +
                                $"organizer={openBracket}{string.Join(dot, organizers)}{closeBracket}&" +
                                $"category={openBracket}{string.Join(dot, categories)}{closeBracket}&" +
                                $"place={openBracket}{string.Join(dot, places)}{closeBracket}");
        }

        private FsharpTest.QueryBuilder CreateBaseQuery()
        {
            return new FsharpTest.QueryBuilder()
                .WhereStartDateIs(date)
                .WhereEndDateIs(date)
                .WhereStartTicketPriceIs(minPrice)
                .WhereEndTicketPriceIs(maxPrice)
                .WhereTypeOfTicketsIs(tickets);
        }

        [Fact]
        public void should_return_valid_criteria_when_passing_null_values()
        {
            var organizers = new int[] {  };
            var categories = new int[] {  };
            var places = new int[] {  };

            var query = CreateBaseQuery()
                .WhereOrganizerIs(organizers)
                .WhereCategoryIs(categories)
                .WherePlaceIs(places);

            var finalQuery = query.CreateQuery();

            finalQuery.ShouldBe($"start_date={date}&" +
                                   $"end_date={date}&" +
                                   $"startTicket={minPrice}&" +
                                   $"endTicket={maxPrice}&" +
                                   $"tickets={tickets}");
        }
    }
}
