using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ClaimDataManager
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ClaimContext db = new ClaimContext())
            {
                // Create the database and table if they do not exist
                db.Database.EnsureCreated();

                // Add sample records only if the table is empty
                if (!db.Claims.Any())
                {
                    db.Claims.Add(new Claim
                    {
                        LecturerName = "Thabo Mokoena",
                        ModuleCode = "PROG6212",
                        HoursWorked = 20,
                        HourlyRate = 350,
                        ClaimMonth = "September 2026",
                        Status = "Draft"
                    });

                    db.Claims.Add(new Claim
                    {
                        LecturerName = "Lerato Dlamini",
                        ModuleCode = "DBMS6212",
                        HoursWorked = 15,
                        HourlyRate = 300,
                        ClaimMonth = "September 2026",
                        Status = "Draft"
                    });

                    db.SaveChanges();

                    Console.WriteLine("Sample claims added.");
                }
            }

            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine("====================================");
                Console.WriteLine("       CONTRACT CLAIM DATA MANAGER");
                Console.WriteLine("====================================");
                Console.WriteLine("1. Add Claim");
                Console.WriteLine("2. View Claims");
                Console.WriteLine("3. Update Claim Status");
                Console.WriteLine("4. Delete Claim");
                Console.WriteLine("5. Export Claims");
                Console.WriteLine("6. Show ADO.NET Claim Count");
                Console.WriteLine("7. Exit");
                Console.WriteLine("====================================");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        AddClaim();
                        break;

                    case "2":
                        ViewClaims();
                        break;

                    case "3":
                        UpdateClaim();
                        break;

                    case "4":
                        DeleteClaim();
                        break;

                    case "5":
                        ExportClaims();
                        break;

                    case "6":
                        ShowClaimCount();
                        break;

                    case "7":
                        running = false;
                        Console.WriteLine("Goodbye.");
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        // Add a new claim
        static void AddClaim()
        {
            Console.Clear();
            Console.WriteLine("ADD CLAIM");
            Console.WriteLine("=========");

            Console.Write("Lecturer name: ");
            string lecturerName = Console.ReadLine() ?? "";

            Console.Write("Module code: ");
            string moduleCode = Console.ReadLine() ?? "";

            decimal hoursWorked;

            while (true)
            {
                Console.Write("Hours worked (1 - 160): ");

                if (decimal.TryParse(Console.ReadLine(), out hoursWorked)
                    && hoursWorked >= 1 && hoursWorked <= 160)
                {
                    break;
                }

                Console.WriteLine("Please enter hours between 1 and 160.");
            }

            decimal hourlyRate;

            while (true)
            {
                Console.Write("Hourly rate: ");

                if (decimal.TryParse(Console.ReadLine(), out hourlyRate)
                    && hourlyRate > 0)
                {
                    break;
                }

                Console.WriteLine("Hourly rate must be greater than zero.");
            }

            Console.Write("Claim month: ");
            string claimMonth = Console.ReadLine() ?? "";

            Claim newClaim = new Claim
            {
                LecturerName = lecturerName,
                ModuleCode = moduleCode,
                HoursWorked = hoursWorked,
                HourlyRate = hourlyRate,
                ClaimMonth = claimMonth,
                Status = "Draft"
            };

            try
            {
                using (ClaimContext db = new ClaimContext())
                {
                    db.Claims.Add(newClaim);
                    db.SaveChanges();
                }

                Console.WriteLine();
                Console.WriteLine("Claim added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }

            Pause();
        }

        // Display all claims
        static void ViewClaims()
        {
            Console.Clear();
            Console.WriteLine("ALL CLAIMS");
            Console.WriteLine("==========");

            try
            {
                using (ClaimContext db = new ClaimContext())
                {
                    List<Claim> claims = db.Claims.ToList();

                    if (claims.Count == 0)
                    {
                        Console.WriteLine("No claims found.");
                    }
                    else
                    {
                        foreach (Claim claim in claims)
                        {
                            Console.WriteLine("------------------------------------");
                            Console.WriteLine("Claim ID:      " + claim.ClaimId);
                            Console.WriteLine("Lecturer:      " + claim.LecturerName);
                            Console.WriteLine("Module:        " + claim.ModuleCode);
                            Console.WriteLine("Hours Worked:  " + claim.HoursWorked);
                            Console.WriteLine("Hourly Rate:   R" + claim.HourlyRate);
                            Console.WriteLine("Claim Month:   " + claim.ClaimMonth);
                            Console.WriteLine("Status:        " + claim.Status);
                            Console.WriteLine("Total Amount:  R" + claim.TotalAmount);
                        }

                        Console.WriteLine("------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not load claims: " + ex.Message);
            }

            Pause();
        }

        // Update the status of a claim
        static void UpdateClaim()
        {
            Console.Clear();
            Console.WriteLine("UPDATE CLAIM STATUS");
            Console.WriteLine("===================");

            Console.Write("Enter claim ID: ");

            if (!int.TryParse(Console.ReadLine(), out int claimId))
            {
                Console.WriteLine("Please enter a valid claim ID.");
                Pause();
                return;
            }

            try
            {
                using (ClaimContext db = new ClaimContext())
                {
                    Claim? claim = db.Claims.Find(claimId);

                    if (claim == null)
                    {
                        Console.WriteLine("Claim was not found.");
                        Pause();
                        return;
                    }

                    Console.WriteLine("Current status: " + claim.Status);
                    Console.Write("Enter new status: ");

                    string newStatus = Console.ReadLine() ?? "";

                    if (string.IsNullOrWhiteSpace(newStatus))
                    {
                        Console.WriteLine("Status cannot be empty.");
                    }
                    else
                    {
                        claim.Status = newStatus;
                        db.SaveChanges();

                        Console.WriteLine("Claim status updated.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not update claim: " + ex.Message);
            }

            Pause();
        }

        // Delete a claim
        static void DeleteClaim()
        {
            Console.Clear();
            Console.WriteLine("DELETE CLAIM");
            Console.WriteLine("============");

            Console.Write("Enter claim ID: ");

            if (!int.TryParse(Console.ReadLine(), out int claimId))
            {
                Console.WriteLine("Please enter a valid claim ID.");
                Pause();
                return;
            }

            try
            {
                using (ClaimContext db = new ClaimContext())
                {
                    Claim? claim = db.Claims.Find(claimId);

                    if (claim == null)
                    {
                        Console.WriteLine("Claim was not found.");
                        Pause();
                        return;
                    }

                    Console.WriteLine();
                    Console.WriteLine("Lecturer: " + claim.LecturerName);
                    Console.WriteLine("Module: " + claim.ModuleCode);

                    Console.Write("Are you sure you want to delete this claim? (Y/N): ");
                    string answer = Console.ReadLine() ?? "";

                    if (answer.ToUpper() == "Y")
                    {
                        db.Claims.Remove(claim);
                        db.SaveChanges();

                        Console.WriteLine("Claim deleted.");
                    }
                    else
                    {
                        Console.WriteLine("Delete cancelled.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not delete claim: " + ex.Message);
            }

            Pause();
        }

        // Export claims to a text file
        static void ExportClaims()
        {
            Console.Clear();
            Console.WriteLine("EXPORTING CLAIMS");
            Console.WriteLine("================");

            try
            {
                string reportsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Reports");

                Directory.CreateDirectory(reportsFolder);

                string filePath = Path.Combine(
                    reportsFolder,
                    "claim_summary.txt");

                using (ClaimContext db = new ClaimContext())
                {
                    List<Claim> claims = db.Claims.ToList();

                    // Write the claims to the text file
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine("CONTRACT CLAIM SUMMARY");
                        writer.WriteLine("======================");
                        writer.WriteLine();

                        foreach (Claim claim in claims)
                        {
                            writer.WriteLine("Claim ID: " + claim.ClaimId);
                            writer.WriteLine("Lecturer: " + claim.LecturerName);
                            writer.WriteLine("Module Code: " + claim.ModuleCode);
                            writer.WriteLine("Hours Worked: " + claim.HoursWorked);
                            writer.WriteLine("Hourly Rate: R" + claim.HourlyRate);
                            writer.WriteLine("Claim Month: " + claim.ClaimMonth);
                            writer.WriteLine("Status: " + claim.Status);
                            writer.WriteLine("Total Amount: R" + claim.TotalAmount);
                            writer.WriteLine("-----------------------------");
                        }
                    }
                }

                Console.WriteLine("Report created successfully.");
                Console.WriteLine();
                Console.WriteLine("REPORT CONTENTS");
                Console.WriteLine("===============");

                // Read the file back and display it
                string reportContents = File.ReadAllText(filePath);

                Console.WriteLine(reportContents);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not export claims: " + ex.Message);
            }

            Pause();
        }

        // Direct ADO.NET query
        static void ShowClaimCount()
        {
            Console.Clear();
            Console.WriteLine("ADO.NET CLAIM COUNT");
            Console.WriteLine("===================");

            try
            {
                using (SqliteConnection connection =
                    new SqliteConnection("Data Source=claims.db"))
                {
                    connection.Open();

                    string sql = "SELECT COUNT(*) FROM Claims";

                    using (SqliteCommand command =
                        new SqliteCommand(sql, connection))
                    {
                        object? result = command.ExecuteScalar();

                        Console.WriteLine(
                            "Number of claim records: " + result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not run ADO.NET query: " + ex.Message);
            }

            Pause();
        }

        // Keeps the menu from immediately appearing again
        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}