namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utlities;
    using Newtonsoft.Json;
   
    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportCreatorXmlDto[] dtos = context.Creators
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorXmlDto()
                {
                    CreatorName = c.FirstName + ' ' + c.LastName,
                    BoardgamesCount = c.Boardgames.Count,
                    Boardgames = c.Boardgames.Select(b => new ExportBoardgameXmlDto()
                    {
                        BoardgameName = b.Name,
                        BoardgameYearPublished = b.YearPublished,
                    })
                    .OrderBy(b => b.BoardgameName)
                    .ToArray()
                })
                .OrderByDescending(b => b.BoardgamesCount)
                .ThenBy(b => b.CreatorName)
                .ToArray();

            return xmlHelper.Serialize<ExportCreatorXmlDto[]>(dtos, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .ToArray()
                .Where(c => c.BoardgamesSellers
                .Any(b => b.Boardgame.YearPublished >= year
                && b.Boardgame.Rating <= rating))
                .Select(c => new
                {
                    Name = c.Name,
                    Website = c.Website,
                    Boardgames = c.BoardgamesSellers.
                    Where(b => b.Boardgame.YearPublished >= year
                    && b.Boardgame.Rating <= rating)
                    .Select(b => new
                    {
                        Name = b.Boardgame.Name,
                        Rating = b.Boardgame.Rating,
                        Mechanics = b.Boardgame.Mechanics,
                        Category = b.Boardgame.CategoryType.ToString(),
                    })
                    .OrderByDescending(b => b.Rating)
                    .ThenBy(b => b.Name)
                    .ToArray()
                })
                .OrderByDescending(c => c.Boardgames.Count())
                .ThenBy(c => c.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}