using ElasticsearchSample.API.Dtos;
using ElasticsearchSample.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchSample.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EcommercesController : ControllerBase
{
    private readonly ECommerceRepository _eCommerceRepository;

    public EcommercesController(ECommerceRepository eCommerceRepository)
    {
        _eCommerceRepository = eCommerceRepository;
    }

    [HttpGet("term-query")]
    public async Task<IActionResult> TermQuery(string customerFirstName)
    {
        var result = await _eCommerceRepository.TermQueryAsync(customerFirstName);

        return Ok(result);
    }

    [HttpPost("terms-query")]
    public async Task<IActionResult> TermQuery(List<string> customerFirstName)
    {
        var result = await _eCommerceRepository.TermsQueryAsync(customerFirstName);

        return Ok(result);
    }

    [HttpGet("prefix-query")]
    public async Task<IActionResult> PrefixQuery(string customerFullName)
    {
        var result = await _eCommerceRepository.PrefixQueryAsync(customerFullName);

        return Ok(result);
    }

    [HttpGet("range-query")]
    public async Task<IActionResult> RangeQuery(double fromPrice,double toPrice)
    {
        var result = await _eCommerceRepository.RangeQueryAsync(fromPrice,toPrice);

        return Ok(result);
    }

    [HttpGet("match-all-query")]
    public async Task<IActionResult> MatchAllQuery()
    {
        var result = await _eCommerceRepository.MatchAllQueryAsync();

        return Ok(result);
    }

    [HttpGet("match-all-query/{page}/{take}")]
    public async Task<IActionResult> MatchAllQuery(int page=1,int take=20)
    {
        var result = await _eCommerceRepository.MatchAllQueryWithPagingAsync(page,take);

        return Ok(result);
    }

    [HttpGet("wildcard-query")]
    public async Task<IActionResult> WildcardQuery(string customerFullName)
    {
        var result = await _eCommerceRepository.WildCardQueryAsync(customerFullName);

        return Ok(result);
    }

    [HttpGet("fuzzy-query")]
    public async Task<IActionResult> FuzzyQuery(string customerName)
    {
        var result = await _eCommerceRepository.FuzzyQueryAsync(customerName);

        return Ok(result);
    }

    [HttpGet("fuzzy-order-query")]
    public async Task<IActionResult> FuzzyOrderQuery(string customerName)
    {
        var result = await _eCommerceRepository.FuzzyQueryWithOrderAsync(customerName);

        return Ok(result);
    }

    [HttpGet("match-full-text-query")]
    public async Task<IActionResult> MatchFullText(string categoryName)
    {
        var result = await _eCommerceRepository.MatchFullTextQueryAsync(categoryName);

        return Ok(result);
    }

    [HttpGet("match-bool-full-text-query")]
    public async Task<IActionResult> MatchBoolFullText(string categoryName)
    {
        var result = await _eCommerceRepository.MatchBoolPrefixFullTextQueryAsync(categoryName);

        return Ok(result);
    }

    [HttpGet("match-phraze-full-text-query")]
    public async Task<IActionResult> MatchPhrazeFullText(string categoryName)
    {
        var result = await _eCommerceRepository.MatchPhraseFullTextQueryAsync(categoryName);

        return Ok(result);
    }

    [HttpGet("compound-query")]
    public async Task<IActionResult> CompoundQuery(string cityName, double textFulTotalPrice, string categoryName, string menufacturer)
    {
        var result = await _eCommerceRepository.CompoundQueryAsync(cityName,textFulTotalPrice,categoryName,menufacturer);

        return Ok(result);
    }

    [HttpGet("multi-match-query")]
    public async Task<IActionResult> MultiMatchQuery(string text)
    {
        var result = await _eCommerceRepository.MultiMatchQueryAsync(text);

        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchAsync([FromBody]EcommerceSearchDto searchDto, int page, int size)
    {
        var result = await _eCommerceRepository.SearhAsync(searchDto,page,size);

        return Ok(result);
    }
}
