namespace RotatingChoresData
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class RotatingChoresContext : DbContext
    {

        public RotatingChoresContext()
            : base("name=RotatingChoresContext")
        {
            //TODO Delete this when database finished.
            Database.SetInitializer<RotatingChoresContext>(new DropCreateDatabaseIfModelChanges<RotatingChoresContext>());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Chore> Chores { get; set; }

        public virtual DbSet<ChoreDoer> ChoreDoers { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        
    }


}