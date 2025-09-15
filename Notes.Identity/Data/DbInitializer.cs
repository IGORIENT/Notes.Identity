namespace Notes.Identity.Data
{
    public class DbInitializer
    {
        public static void Initialize(AuthDbContext context)
        {
            context.Database.EnsureCreated();  //убедись что создана БД. Если база уже есть - ничего не делает.
        }
    }
}
