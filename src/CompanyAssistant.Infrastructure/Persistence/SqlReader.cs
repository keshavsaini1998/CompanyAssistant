using CompanyAssistant.Application.Interfaces;
using CompanyAssistant.Application.Vector;
using CompanyAssistant.Domain.Entities;
using CompanyAssistant.Infrastructure.Constants;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CompanyAssistant.Infrastructure.Persistence
{
    public class SqlReader : ISqlReader
    {
        private readonly AppDbContext _db;

        public SqlReader(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<VectorDocument>> ReadAsync(Guid projectId)
        {
            var project = await _db.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new Exception("Project not found");
            }

            var docs = new List<VectorDocument>();
            await using var con = new NpgsqlConnection(project.DatabaseConnection);
            await con.OpenAsync();
            return project.Name switch
            {
                var name when name == ProjectNames.ECommerce => await ECommerceAsync(con, project.Id),
                var name when name == ProjectNames.Clinic => await ClinicAsync(con, project.Id),
                _ => throw new Exception("Unsupported project")
            };
        }

        private async Task<List<VectorDocument>> ECommerceAsync(NpgsqlConnection con, Guid projectId)
        {
            var docs = new List<VectorDocument>();
            // Users
            var users = await con.QueryAsync(@"SELECT name, email, created_at FROM users");


            foreach (var u in users)
            {
                docs.Add(new VectorDocument
                {
                    ProjectId = projectId.ToString(),
                    Entity = "User",
                    Content = $"User {u.name}, email {u.email}, joined {u.created_at:yyyy-MM-dd}"
                });
            }


            // Products
            var products = await con.QueryAsync(@"SELECT name, category, price, stock FROM products");


            foreach (var p in products)
            {
                docs.Add(new VectorDocument
                {
                    ProjectId = projectId.ToString(),
                    Entity = "Product",
                    Content = $"Product {p.name}, category {p.category}, price {p.price}, stock {p.stock}"
                });
            }


            // Orders
            var orders = await con.QueryAsync(@"
                                    SELECT o.order_date, o.status, u.name AS user
                                    FROM orders o
                                    JOIN users u ON u.user_id = o.user_id");


            foreach (var o in orders)
            {
                docs.Add(new VectorDocument
                {
                    ProjectId = projectId.ToString(),
                    Entity = "Order",
                    Content = $"Order by {o.user}, date {o.order_date:yyyy-MM-dd}, status {o.status}"
                });
            }

            return docs;
        }

        private async Task<List<VectorDocument>> ClinicAsync(NpgsqlConnection con, Guid ProjectId)
        {
            var docs = new List<VectorDocument>();
            // Patients
            var patients = await con.QueryAsync(@"SELECT patient_id, name, phone, date_of_birth FROM patients");


            foreach (var p in patients)
            {
                docs.Add(new VectorDocument
                {
                    ProjectId = ProjectId.ToString(),
                    Entity = "Patient",
                    Content = $"Patient {p.name}, phone {p.phone}, born {p.date_of_birth:yyyy-MM-dd}"
                });
            }


            // Doctors
            var doctors = await con.QueryAsync(@"SELECT doctor_id, name, specialty FROM doctors");


            foreach (var d in doctors)
            {
                docs.Add(new VectorDocument
                {
                    ProjectId = ProjectId.ToString(),
                    Entity = "Doctor",
                    Content = $"Doctor {d.name}, specialty {d.specialty}"
                });
            }


            // Appointments
            var appts = await con.QueryAsync(@"
                                    SELECT a.appointment_date, a.status, p.name AS patient, d.name AS doctor
                                    FROM appointments a
                                    JOIN patients p ON p.patient_id = a.patient_id
                                    JOIN doctors d ON d.doctor_id = a.doctor_id");


            foreach (var a in appts)
            {
                docs.Add(new VectorDocument
                {
                    ProjectId = ProjectId.ToString(),
                    Entity = "Appointment",
                    Content = $"Appointment on {a.appointment_date:yyyy-MM-dd}, patient {a.patient}, doctor {a.doctor}, status {a.status}"
                });
            }

            return docs;
        }
    }
}
