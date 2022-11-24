using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<PetContext>(options =>
{
	options.UseSqlite("Filename=Pets.db");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseHttpsRedirection();

var petGroup = app.MapGroup("/pet");
petGroup.WithOpenApi();
petGroup.MapGet("/", async (PetContext ctx) =>
{
	return await ctx.Pets.ToListAsync();
});
petGroup.MapGet("/{id}", async (int id, PetContext ctx) =>
{
	var pet = await ctx.Pets.FirstOrDefaultAsync(p => p.ID == id);

	return pet != null ? Results.Ok(pet) : Results.NotFound();
});
petGroup.MapPost("/", async (string name, string type, PetContext ctx) =>
{
	var p = new Pet(0, name, type);

	await ctx.Pets.AddAsync(p);
	await ctx.SaveChangesAsync();

	return Results.Ok(p);
});
petGroup.MapDelete("/{id}", async (int id, PetContext ctx) =>
{
	int deleted_count = await ctx.Pets.Where(p => p.ID == id)
		.ExecuteDeleteAsync();

	return deleted_count > 0 ? Results.Ok() : Results.NotFound();
});

//ensure db is created and migrations applied
using (var scope = app.Services.CreateScope())
{
	using var ctx = scope.ServiceProvider.GetRequiredService<PetContext>();
	await ctx.Database.EnsureCreatedAsync();
}
app.Run();



//
// Model
//
internal record Pet(int ID, string Name, string Type);

//
// Db Context
//
internal class PetContext : DbContext
{
	public DbSet<Pet> Pets { get; set; }

	public PetContext(DbContextOptions<PetContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Pet>(e =>
		{
			e.Property(p => p.ID).ValueGeneratedOnAdd();
			e.HasKey(p => p.ID);

			e.Property(p => p.Name);
			e.Property(p => p.Type);

			e.HasData(new Pet[]
			{
				new Pet(1, "Fluffy", "cat 🐱"),
				new Pet(2, "Wolfie", "dog 🐕")
			});

		});
	}
}
