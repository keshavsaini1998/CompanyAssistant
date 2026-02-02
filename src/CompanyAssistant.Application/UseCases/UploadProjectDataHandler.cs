using CompanyAssistant.Application.Interfaces;

namespace CompanyAssistant.Application.UseCases
{
    public record UploadProjectDataCommand(Guid ProjectId);
    public class UploadProjectDataHandler
    {
        private readonly IVectorStore _vector;

        public UploadProjectDataHandler(
            IVectorStore vector)
        {
            _vector = vector;
        }

        public async Task Handle(UploadProjectDataCommand command)
        {
            //var project = await _db.Projects.FindAsync(command.ProjectId);
            //if (project == null) throw new Exception("Project not found");


            //using var conn = new SqliteConnection(project.DatabaseConnection);
            //await conn.OpenAsync();


            //var cmd = conn.CreateCommand();
            //cmd.CommandText = "SELECT Name, Role FROM Employees";


            //using var reader = await cmd.ExecuteReaderAsync();
            //while (await reader.ReadAsync())
            //{
            //    var text = $"Employee {reader.GetString(0)} works as {reader.GetString(1)}";
            //    await _vector.StoreAsync(text, project.Id);
            //}
        }
    }
}
