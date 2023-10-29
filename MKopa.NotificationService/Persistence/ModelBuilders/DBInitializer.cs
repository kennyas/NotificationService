namespace MKopaMessageBox.Persistence.ModelBuilders
{
    public static class DBInitializer
    {
        public static async Task SeedDefaultsData(this IHost host)
        {
            var serviceProvider = host.Services.CreateScope().ServiceProvider;
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var accessKeys = AccessKeyData.GetAccessKeys();

            if (!context.AccessKeys.Any())
            {
                await context.AccessKeys.AddRangeAsync(accessKeys);
                await context.SaveChangesAsync();
            }

        }
    }
}
