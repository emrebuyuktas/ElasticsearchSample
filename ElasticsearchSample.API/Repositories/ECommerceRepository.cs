using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticsearchSample.API.Dtos;
using ElasticsearchSample.API.Models.ECommerceModels;
using System.Collections.Immutable;

namespace ElasticsearchSample.API.Repositories;

public class ECommerceRepository
{
    private const string _indexName = "kibana_sample_data_ecommerce";
    private readonly ElasticsearchClient _client;

    public ECommerceRepository(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task<IImmutableList<ECommerce>> TermQueryAsync(string customerFirstName)
    {
        //first way
        //var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));

        //second way
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Query(q => q.Term(t => t.CustomerFirstName.Suffix("keyword"), customerFirstName)));

        //third way
        //var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Query(q => q.Term(t => t.Field(f=>f.CustomerFirstName).Value(customerFirstName))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> TermsQueryAsync(List<string> customerFirstNames)
    {
        var terms = new List<FieldValue>();

        customerFirstNames.ForEach(x => terms.Add(x));
        //first way
        //var termsQuery = new TermsQuery
        //{
        //    Field = "customer_first_name.keyword",
        //    Terms = new TermsQueryField(terms.AsReadOnly())
        //};

        //var result = await _client.SearchAsync<ECommerce>(s=>s.Index(_indexName).Query(termsQuery));

        //second way
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Size(100)
        .Query(q => q
        .Terms(t => t
        .Field(f => f.CustomerFirstName
        .Suffix("keyword"))
        .Terms(new TermsQueryField(terms.AsReadOnly())))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> PrefixQueryAsync(string customerFullName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName)
        .Query(q => q.Prefix(p => p.Field(f => f.CustomerFullName.Suffix("keyword"))
        .Value(customerFullName))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> RangeQueryAsync(double fromPrice, double toPrice)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName)
        .Query(q => q
        .Range(r => r
        .NumberRange(nr => nr.
        Field(f => f.TaxfulTotalPrice)
        .Gte(fromPrice)
        .Lte(toPrice)))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> MatchAllQueryAsync()
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Size(100).Query(q => q.MatchAll()));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> MatchAllQueryWithPagingAsync(int page, int take)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).From((page - 1) * take).Size(take).Query(q => q.MatchAll()));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> WildCardQueryAsync(string customerFullName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName)
        .Query(q => q.Wildcard(w => w.Field(f => f.CustomerFullName
        .Suffix("keyword")).Wildcard(customerFullName))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> FuzzyQueryAsync(string customerName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).
        Query(q => q.Fuzzy(f => f.Field(fi => fi.CustomerFirstName.Suffix("keyword")).Value(customerName).Fuzziness(new Fuzziness(1)))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> FuzzyQueryWithOrderAsync(string customerName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).
        Query(q => q.Fuzzy(f => f.Field(fi => fi.CustomerFirstName.Suffix("keyword")).Value(customerName).Fuzziness(new Fuzziness(1)))).Sort(s =>
        s.Field(f => f.TaxfulTotalPrice, new FieldSort() { Order = SortOrder.Asc })));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> MatchFullTextQueryAsync(string categoryName)
    {
        var result = await _client.SearchAsync<ECommerce>(s =>
        s.Index(_indexName).Query(q =>
        q.Match(m =>
        m.Field(f => f.Category)
        .Query(categoryName))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> MatchBoolPrefixFullTextQueryAsync(string customerFullName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Query(q => q
        .MatchBoolPrefix(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> MatchPhraseFullTextQueryAsync(string customerFullName)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).Query(q => q
        .MatchPhrase(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> CompoundQueryAsync(string cityName, double textFulTotalPrice, string categoryName, string menufacturer)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName)
                        .Size(1000).Query(q => q
                            .Bool(b => b
                                .Must(m => m
                                    .Term(t => t
                                        .Field("geoip.city_name")
                                        .Value(cityName)))
                                .MustNot(mn => mn
                                    .Range(r => r
                                        .NumberRange(nr => nr
                                            .Field(f => f.TaxfulTotalPrice)
                                            .Lte(textFulTotalPrice))))
                                .Should(s => s.Term(t => t
                                    .Field(f => f.Category.Suffix("keyword"))
                                    .Value(categoryName)))
                                .Filter(f => f
                                    .Term(t => t
                                        .Field("manufacturer.keyword")
                                        .Value(menufacturer))))


                        ));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    public async Task<IImmutableList<ECommerce>> MultiMatchQueryAsync(string name)
    {
        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName)
        .Query(q => q.MultiMatch(mm=>mm.Fields(new Field("customer_first_name").And(new Field("customer_last_name"))
        .And(new Field("customer_full_name"))).Query(name))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();
    }

    #region search
    public async Task<IImmutableList<ECommerce>> SearhAsync(EcommerceSearchDto searchDto,int page,int size)
    {
        List<Action<QueryDescriptor<ECommerce>>> queryList = new List<Action<QueryDescriptor<ECommerce>>>();

        if(!string.IsNullOrEmpty(searchDto.Category))
        {
            queryList.Add((q)=>q.Match(m=>m.Field(f=>f.Category).Query(searchDto.Category)));
            //Action<QueryDescriptor<ECommerce>> categoryQuery =
            //    (q)=>q.Match(m=>m.Field(f=>f.Category).Query(searchDto.Category));
        }

        if (!string.IsNullOrEmpty(searchDto.CustomerFullName))
        {
            queryList.Add((q) => q.Match(m => m.Field(f => f.CustomerFullName).Query(searchDto.CustomerFullName)));
        }

        if (searchDto.OrderDateStart.HasValue)
        {
            queryList.Add((q)=>q.Range(r=>r.DateRange(dr=>dr.Field(f=>f.OrderDate).Gte(searchDto.OrderDateStart))));
        }

        if (searchDto.OrderDateEnd.HasValue)
        {
            queryList.Add((q) => q.Range(r => r.DateRange(dr => dr.Field(f => f.OrderDate).Lte(searchDto.OrderDateEnd))));
        }

        if (!string.IsNullOrEmpty(searchDto.Gender))
        {
            queryList.Add((q) => q.Term(t => t.Field(f => f.Gender).Value(searchDto.Gender)));
        }

        var pageFrom = (page - 1) * size;

        var result = await _client.SearchAsync<ECommerce>(s => s.Index(_indexName).From(pageFrom).Size(size).Query(q => q.Bool(b => b.Must(queryList.ToArray()))));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

        return result.Documents.ToImmutableList();

    }
    #endregion
}