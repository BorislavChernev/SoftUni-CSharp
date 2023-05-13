namespace Footballers.DataProcessor
{
    using AutoMapper;
    using Castle.Core.Internal;
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Utilities;
    using Microsoft.Win32.SafeHandles;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();

            ImportCoachDto[] importCoachDtos =
                xmlHelper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

            ICollection<Coach> coaches = new List<Coach>();
            foreach (ImportCoachDto c in importCoachDtos)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(c.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(c.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = c.Name,
                    Nationality = c.Nationality,
                };

                foreach (ImportFootballerDto f in c.Footballers)
                {
                    if (!IsValid(f))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime start;
                    if (!DateTime.TryParseExact(f.ContractStartDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out start))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime end;
                    if (!DateTime.TryParseExact(f.ContractEndDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out end))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (start > end)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    try
                    {
                        Footballer footballer = new Footballer()
                        {
                            Name = f.Name,
                            ContractStartDate = start,
                            ContractEndDate = end,
                            BestSkillType = (BestSkillType)f.BestSkillType,
                            PositionType = (PositionType)f.PositionType
                        };

                        coach.Footballers.Add(footballer);
                    }
                    catch (Exception)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                }
                coaches.Add(coach);
                sb.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));

            }
            context.Coaches.AddRange(coaches);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTeamDto[] importTeamDtos =
                JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            ICollection<Team> teams = new List<Team>();

            int i = 0;
            foreach (ImportTeamDto t in importTeamDtos)
            {
                i++;
                if (i == 16)
                {
                    Console.WriteLine();
                }

                if (!IsValid(t))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(t.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(t.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (t.Footballers.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (t.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = t.Name,
                    Nationality = t.Nationality,
                    Trophies = t.Trophies,
                };

                foreach (int fId in t.Footballers.Distinct())
                {
                    Footballer footballer = context.Footballers.Find(fId);
                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    team.TeamsFootballers.Add(new TeamFootballer()
                    {
                        Footballer = footballer
                    });
                }
                teams.Add(team);
                sb.AppendLine(String.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }
            context.Teams.AddRange(teams);
            context.SaveChanges();
            Console.WriteLine(teams.Count + " " + teams.Sum(x => x.TeamsFootballers.Count));
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static IMapper CreateAutoMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<FootballersProfile>();
            }));
    }
}
