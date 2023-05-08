namespace Theatre.DataProcessor
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;
    using Trucks.Utilities;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            //var dtos = context.Theatres
            //    .ToArray()
            //    .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
            //    .Select(t => new
            //    {
            //        Name = t.Name,
            //        Halls = t.NumberOfHalls,
            //        TotalIncome = t.Tickets.Where(x => x.RowNumber <= 5).Sum(x => x.Price),
            //        Tickets = t.Tickets
            //        .Where(x => x.RowNumber <= 5)
            //        .Select(x => new
            //        {
            //            Price = x.Price,
            //            RowNumber = x.RowNumber
            //        })
            //        .OrderByDescending(x => x.Price)
            //        .ToArray()
            //    })
            //    .OrderByDescending(t => t.Halls)
            //    .ThenBy(t => t.Name)
            //    .ToArray();

            //return JsonConvert.SerializeObject(dtos, Formatting.Indented);
            var result = context.Theatres.ToArray().Where(x => x.NumberOfHalls >= numbersOfHalls &&
 x.Tickets.Count() >= 20)
 .Select(x => new
 {
     Name = x.Name,
     Halls = x.NumberOfHalls,
     TotalIncome = x.Tickets.Where(x => x.RowNumber <= 5).Sum(x => x.Price),
     Tickets = x.Tickets.Where(x => x.RowNumber <= 5).Select(t => new
     {
         Price = t.Price,
         RowNumber = t.RowNumber
     })
     .OrderByDescending(p => p.Price)
     .ToArray()
 })
 .OrderByDescending(h => h.Halls)
 .ThenBy(n => n.Name);


            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var result = context.Plays.Where(x => x.Rating <= raiting)
            .OrderBy(x => x.Title)
            .ThenByDescending(x => x.Genre)
            .Select(x => new ExportPlayDto()
            {
                Title = x.Title,
                Duration = x.Duration.ToString("c"),
                Rating = x.Rating == 0 ? "Premier" : x.Rating.ToString(),
                Genre = x.Genre.ToString(),
                Actors = x.Casts.Where(x => x.IsMainCharacter)
                .Select(a => new Actor()
                {
                    FullName = a.FullName,
                    MainCharacter = $"Plays main character in '{x.Title}'."
                })
                .OrderByDescending(x => x.FullName)
                .ToArray()
            }).ToArray();

            return xmlHelper.Serialize<ExportPlayDto[]>(result, "Plays");
        }
    }
}
