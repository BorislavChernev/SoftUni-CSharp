// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Utilities;
    using System.Text;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;
    using System.Security;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();

            StringBuilder sb = new StringBuilder();

            ImportProjectXmlDto[] dtos =
                xmlHelper.Deserialize<ImportProjectXmlDto[]>(xmlString, "Projects");

            ICollection<Project> projects = new List<Project>();
            foreach (ImportProjectXmlDto projectXmlDto in dtos)
            {
                if (!IsValid(projectXmlDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime projectOpenDate;
                bool projectOpenDateValid = DateTime.TryParseExact(projectXmlDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out projectOpenDate);

                if (!projectOpenDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime projectDueDate;
                bool projectDueDateValid = DateTime.TryParseExact(projectXmlDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out projectDueDate);

                if (!string.IsNullOrWhiteSpace(projectXmlDto.DueDate) && !projectDueDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Project project = new Project()
                {
                    Name = projectXmlDto.Name,
                    OpenDate = projectOpenDate,
                    DueDate = projectDueDate,
                };

                foreach (ImportTaskXmlDto taskXmlDto in projectXmlDto.Tasks)
                {
                    if (!IsValid(taskXmlDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskOpenDate;
                    bool taskOpenDateValid = DateTime.TryParseExact(taskXmlDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskOpenDate);

                    if (!taskOpenDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskDueDate;
                    bool taskDueDateValid = DateTime.TryParseExact(taskXmlDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskDueDate);

                    if (!taskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate < projectOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(projectXmlDto.DueDate) && taskDueDate > projectDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task()
                    {
                        Name = taskXmlDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)taskXmlDto.ExecutionType,
                        LabelType = (LabelType)taskXmlDto.LabelType,
                    };

                    project.Tasks.Add(task);
                }
                projects.Add(project);
                sb.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }
            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            ImportEmployeeJsonDto[] dtos
                = JsonConvert.DeserializeObject<ImportEmployeeJsonDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            ICollection<Employee> employees = new List<Employee>();
            foreach (ImportEmployeeJsonDto employeeJsonDto in dtos)
            {
                if (!IsValid(employeeJsonDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee()
                {
                    Username = employeeJsonDto.Username,
                    Email = employeeJsonDto.Email,
                    Phone = employeeJsonDto.Phone,
                };

                foreach (int id in employeeJsonDto.Tasks.Distinct())
                {
                    Task current = context.Tasks.Find(id);

                    if (current == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    employee.EmployeesTasks.Add(new EmployeeTask()
                    {
                        Task = current
                    });
                }
                employees.Add(employee);
                sb.AppendLine(String.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }
            context.Employees.AddRange(employees);
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