using API.services;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace API.Utility
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class SearchBenchMark
    {
        private const string searchTerm = "Head Office";
        private static readonly SearchService searchService = new SearchService();

        [Benchmark]
        public void GetRankedList()
        {
            searchService.searchItem(searchTerm);
        }
    }
}
