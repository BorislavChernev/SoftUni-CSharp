namespace SoftUni;

using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;


public class StartUp
{
    static void Main()
    {
        SoftUniContext dbContext = new SoftUniContext();

        string result = RemoveTown(dbContext);
        Console.WriteLine(result);
    }

    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToArray();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .OrderBy(e => e.FirstName)
            .Select(e => new
            {
                e.FirstName,
                e.Salary
            })
            .Where(e => e.Salary > 50000)
            .ToArray();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                DepartmentName = e.Department.Name,
                e.Salary
            })
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .ToArray();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };

        Employee? employee = context.Employees
            .FirstOrDefault(e => e.LastName == "Nakov");
        employee!.Address = newAddress;

        context.SaveChanges();

        string[] employeeAddresses = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e => e.Address!.AddressText)
            .ToArray();

        return String.Join(Environment.NewLine, employeeAddresses);
    }

    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employeesWithProjects = context.Employees
            //.Where(e => e.EmployeesProjects
            //    .Any(ep => ep.Project.StartDate.Year >= 2001 &&
            //               ep.Project.StartDate.Year <= 2003))
            .Take(10)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                ManagerFirstName = e.Manager!.FirstName,
                ManagerLastName = e.Manager!.LastName,
                Projects = e.EmployeesProjects
                    .Where(ep => ep.Project.StartDate.Year >= 2001 &&
                                 ep.Project.StartDate.Year <= 2003)
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = ep.Project.EndDate.HasValue
                            ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                            : "not finished"
                    })
                    .ToArray()
            })
            .ToArray();

        foreach (var e in employeesWithProjects)
        {
            sb
                .AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

            foreach (var p in e.Projects)
            {
                sb
                    .AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetAddressesByTown(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var Addresses = context.Addresses
            .Select(a => new
            {
                a.AddressText,
                TownName = a.Town!.Name,
                EmployeesCount = a.Employees.Count
            })
            .OrderByDescending(a => a.EmployeesCount)
            .ThenBy(a => a.TownName)
            .Take(10);

        foreach (var a in Addresses)
        {
            sb
                .AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees");
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetEmployee147(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        Employee e = context.Employees
            .FirstOrDefault(e => e.EmployeeId == 147)!;

        sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");

        foreach (EmployeeProject ep in e.EmployeesProjects.OrderBy(p => p.Project.Name))
        {
            sb.AppendLine($"{ep.Project.Name}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var departments = context.Departments
            .Select(d => new
            {
                d.Name,
                ManagerFirstName = d.Manager.FirstName,
                ManagerLastName = d.Manager.LastName,
                d.Employees
            })
            .OrderBy(d => d.Employees.Count)
            .Where(d => d.Employees.Count > 5);

        foreach (var d in departments)
        {
            sb
                .AppendLine($"{d.Name} - {d.ManagerFirstName} {d.ManagerLastName}");

            foreach (var e in d.Employees)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetLatestProjects(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var projects = context.Projects
            .Select(p => new
            {
                p.StartDate,
                p.Name,
                p.Description
            })
            .OrderByDescending(p => p.StartDate)
            .OrderBy(p => p.Name)
            .Take(10);

        foreach (var p in projects)
        {
            sb
                .AppendLine(p.Name)
                .AppendLine(p.Description)
                .AppendLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
        }

        return sb.ToString().TrimEnd();
    }

    public static string IncreaseSalaries(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .Where(e =>
                e.Department.Name == "Engineering"
                || e.Department.Name == "Tool Design"
                || e.Department.Name == "Marketing"
                || e.Department.Name == "Information Services"
                )
            .OrderBy(e => e.FirstName)
            .ThenByDescending(e => e.LastName);


        foreach (var e in employees)
        {
            e.Salary += e.Salary * 0.12m;

            sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Salary,
                e.JobTitle
            })
            .Where(e => e.FirstName.StartsWith("Sa"))
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName);

        foreach (var e in employees)
        {
            sb
                .AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
        }

        return sb.ToString().TrimEnd();
    }

    public static string DeleteProjectById(SoftUniContext context)
    {
        var epToDelete = context.EmployeesProjects
    .Where(ep => ep.ProjectId == 2);

        context.EmployeesProjects.RemoveRange(epToDelete);

        Project projectToDelete = context.Projects.Find(2)!;
        context.Projects.Remove(projectToDelete);
        context.SaveChanges();

        string[] projectNames = context.Projects
            .Take(10)
            .Select(p => p.Name)
            .ToArray();
        return String.Join(Environment.NewLine, projectNames);
    }

    public static string RemoveTown(SoftUniContext context)
    {
        Town townToDelete = context.Towns
            .FirstOrDefault(t => t.Name == "Seattle")!;

        IQueryable<Address> addressesToDelete = context.Addresses
                .Where(a => a.TownId == townToDelete.TownId);
        int addressesCount = addressesToDelete.Count();

        IQueryable<Employee> employeesOnDeletedAddresses =
            context
                .Employees
                .Where(e => addressesToDelete.Any(a => a.AddressId == e.AddressId));

        foreach (var employee in employeesOnDeletedAddresses)
        {
            employee.AddressId = null;
        }

        foreach (var address in addressesToDelete)
        {
            context.Addresses.Remove(address);
        }

        context.Remove(townToDelete);

        context.SaveChanges();

        return $"{addressesCount} addresses in Seattle were deleted";

        //IQueryable<Address> addressesToDelete = context.Addresses
        //    .Where(t => t.TownId == townToDelete.TownId);

        //IQueryable<Employee> employees = context.Employees
        //    .Where(e => addressesToDelete.Any(a => a.AddressId == e.AddressId));

        //foreach (var e in employees)
        //{
        //    e.AddressId = null;
        //}

        //context.Addresses.RemoveRange(addressesToDelete);

        //context.Towns.Remove(townToDelete);
        //context.SaveChanges();

        //return $"{addressesToDelete.Count()} addresses in Seattle were deleted";
    }
}