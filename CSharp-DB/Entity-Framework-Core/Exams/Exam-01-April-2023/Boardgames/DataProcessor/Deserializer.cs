namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utlities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();

            StringBuilder sb = new StringBuilder();

            ImportCreatorXmlDto[] dtos =
                xmlHelper.Deserialize<ImportCreatorXmlDto[]>(xmlString, "Creators");

            ICollection<Creator> creators = new List<Creator>();
            foreach (ImportCreatorXmlDto dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                };

                foreach (ImportBoardgameXmlDto dtoChild in dto.Boardgames)
                {
                    if (!IsValid(dtoChild))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new Boardgame()
                    {
                        Name = dtoChild.Name,
                        Rating = dtoChild.Rating,
                        YearPublished = dtoChild.YearPublished,
                        CategoryType = (CategoryType)dtoChild.CategoryType,
                        Mechanics = dtoChild.Mechanics,
                    };

                    creator.Boardgames.Add(boardgame);
                }
                creators.Add(creator);
                sb.AppendLine(String.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
            }
            context.Creators.AddRange(creators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportSellerJsonDto[] dtos =
                JsonConvert.DeserializeObject<ImportSellerJsonDto[]>(jsonString);

            ICollection<Seller> sellers = new List<Seller>();
            foreach (ImportSellerJsonDto dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    Country = dto.Country,
                    Website = dto.Website,
                };

                foreach (int id in dto.Boardgames.Distinct())
                {
                    Boardgame boardgame = context.Boardgames.Find(id);
                    if (boardgame == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        Boardgame = boardgame,
                        Seller = seller,
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);
                }
                sellers.Add(seller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }
            context.Sellers.AddRange(sellers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
