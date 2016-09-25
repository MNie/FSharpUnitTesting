namespace QueryBuilder.FSharp.Test
open FsharpTest
open System
open Xunit

type test_query_builder() =
    let closeBracket = "%5d";
    let date = DateTime.UtcNow.ToString("yyyy-MM-dd");
    let dot = "%2c";
    let maxPrice = 20;
    let minPrice = 10;
    let openBracket = "%5b";
    let tickets = "tickets";
    
    let arrayEncode(array: int[]) =
        let arrayBody = 
            array 
            |> Seq.map(sprintf "%A")
            |> String.concat(dot)
        sprintf "%s%s%s" openBracket arrayBody closeBracket

    let createBaseQuery = 
        new FsharpTest.QueryBuilder()
        |> fun x -> x.WhereStartDateIs date
        |> fun x -> x.WhereEndDateIs date
        |> fun x -> x.WhereStartTicketPriceIs minPrice
        |> fun x -> x.WhereEndTicketPriceIs maxPrice
        |> fun x -> x.WhereTypeOfTicketsIs tickets

    [<Fact>]
    let ``when generating query without params should generate empty query``() =
        let finalQuery = 
            new FsharpTest.QueryBuilder()
            |> fun x -> x.CreateQuery()
        Assert.Equal(finalQuery, "", true)

    [<Fact>]
    let ``when generating query with null in arrays it should create query without this params``() =
        let organizers = [||];
        let categories = [||];
        let places = [||];
         
        let finalQuery =
            createBaseQuery
            |> fun x -> x.WhereOrganizerIs organizers
            |> fun x -> x.WhereCategoryIs categories
            |> fun x -> x.WherePlaceIs places
            |> fun x -> x.CreateQuery()

        let expectedResult = sprintf "start_date=%s&end_date=%s&startTicket=%d&endTicket=%d&tickets=%s" date date minPrice maxPrice tickets
        Assert.Equal(finalQuery, expectedResult, true)

    [<Fact>]
    let ``when generating query it should create valid query``() =
        let organizers = [| 2; 3 |];
        let categories = [| 5; 6 |];
        let places = [| 7; 4 |];
         
        let finalQuery =
            createBaseQuery
            |> fun x -> x.WhereOrganizerIs organizers
            |> fun x -> x.WhereCategoryIs categories
            |> fun x -> x.WherePlaceIs places
            |> fun x -> x.CreateQuery()

        let expectedResult = 
            sprintf "start_date=%s&end_date=%s&startTicket=%d&endTicket=%d&tickets=%s&organizer=%s&category=%s&place=%s" 
                date date minPrice maxPrice tickets (arrayEncode organizers) (arrayEncode categories) (arrayEncode places)
        Assert.Equal(finalQuery, expectedResult, true)