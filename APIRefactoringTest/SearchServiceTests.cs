using API.services;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace APIRefactoringTest
{
    public class SearchServiceTests
    {
        private readonly SearchService _sut;
        public SearchServiceTests()
        {
            _sut = new SearchService();
        }

        [Theory]
        [InlineData("Head Office")]
        [InlineData("Produktionsstätte")]
        public void SearchItem_Should_Return_Maximum_Wighted_Item_When_Exact_Building_Search(string searchTerm)
        {

            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.Should().NotBeEmpty();
            var building = result.buildings.FirstOrDefault(x => x.name == searchTerm);
            building.weight.Should().Be(95);
            var locks = result.locks.Where(x => x.buildingId == building.id).ToList();
            locks.ForEach(x => x.weight.Should().Be(80));
            result.groups.Should().BeEmpty();
            result.media.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Head")]
        [InlineData("Logistikzentrum")]
        public void SearchItem_Should_Return_Minimum_Wighted_Item_When_Partial_Building_Search(string searchTerm)
        {
            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.ForEach(item => item.weight.Should().Be(14));
            result.locks.ForEach(item => item.weight.Should().Be(8));
            result.groups.Should().BeEmpty();
            result.media.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Gästezimmer 4.OG")]
        [InlineData("WC Herren 3.OG süd")]
        public void SearchItem_Should_Return_Maximum_Wighted_Item_When_Exact_Lock_Search(string searchTerm)
        {
            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.Should().BeEmpty();
            result.locks.Should().NotBeEmpty();
            result.locks.ForEach(x => x.weight.Should().Be(100));
            result.groups.Should().BeEmpty();
            result.media.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Gästezimmer")]
        [InlineData("Besprechungsraum")]
        public void SearchItem_Should_Return_Minimum_Wighted_Item_When_Partial_Lock_Search(string searchTerm)
        {
            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.Should().BeEmpty();
            result.locks.Should().NotBeEmpty();
            result.locks.ForEach(item => item.weight.Should().Be(10));
            result.groups.Should().BeEmpty();
            result.media.Should().BeEmpty();
        }

        [Theory]
        [InlineData("4.OG")]
        [InlineData("3.OG")]
        public void SearchItem_Should_Return_Varied_Wighted_Item_When_Exact_Floor_Search(string searchTerm)
        {
            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.Should().BeEmpty();
            var nameMatching = result.locks.Where(x => x.name.Contains(searchTerm)).ToList();
            nameMatching.ForEach(x => x.weight.Should().Be(70));
            var nameNotMatching = result.locks.Where(x => !x.name.Contains(searchTerm)).ToList();
            nameNotMatching.ForEach(x => x.weight.Should().Be(60));
            result.groups.Should().BeEmpty();
            result.media.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Cylinder")]
        [InlineData("SmartHandle")]
        public void SearchItem_Should_Return_Maximum_Wighted_Item_When_Eaxact_Lock_Type_Search(string searchTerm)
        {
            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.Should().BeEmpty();
            result.locks.ForEach(item => item.weight.Should().Be(30));
            result.groups.Should().BeEmpty();
            result.media.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Vorstand")]
        [InlineData("Einkauf")]
        public void SearchItem_Should_Return_Maximum_Wighted_Item_When_Eaxact_Group_Search(string searchTerm)
        {
            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.Should().BeEmpty();
            result.locks.Should().BeEmpty();
            result.groups.Should().NotBeEmpty();
            result.groups.ForEach(x => x.weight.Should().Be(90));
            result.media.ForEach(item => item.weight.Should().Be(80));
        }

        [Theory]
        [InlineData("Project")]
        [InlineData("Mechanik")]
        public void SearchItem_Should_Return_Minimum_Wighted_Item_When_Partial_Group_Search(string searchTerm)
        {
            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.Should().BeEmpty();
            result.locks.Should().BeEmpty();
            result.groups.Should().NotBeEmpty();
            result.groups.ForEach(x => x.weight.Should().Be(9));
            result.media.ForEach(item => item.weight.Should().Be(8));
        }

        [Theory]
        [InlineData("TransponderWithCardInlay")]
        [InlineData("Card")]
        public void SearchItem_Should_Return_Maximum_Wighted_Item_When_Eaxact_Media_Type_Search(string searchTerm)
        {
            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.Should().BeEmpty();
            result.locks.Should().BeEmpty();
            result.groups.Should().BeEmpty();
            var typeMatching = result.media.Where(x => x.type == searchTerm).ToList();
            typeMatching.ForEach(x => x.weight.Should().Be(30));
            var typeNotMatching = result.media.Where(x => x.type != searchTerm).ToList();
            typeNotMatching.ForEach(x => x.weight.Should().Be(3));
        }

        [Theory]
        [InlineData("Modesto Bradtke")]
        [InlineData("Mrs. Quinton Stanton")]
        public void SearchItem_Should_Return_Maximum_Wighted_Item_When_Exact_Media_Owner_Search(string searchTerm)
        {
            // Act
            var result = _sut.searchItem(searchTerm);

            //Assert
            result.buildings.Should().BeEmpty();
            result.locks.Should().BeEmpty();
            result.groups.Should().BeEmpty();
            result.media.Count.Should().Be(1);
            result.media.ForEach(x => x.weight.Should().Be(100));
        }
    }
}
