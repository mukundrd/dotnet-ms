Once you create a model class, go to `Package Console Manager` from `Tools` toolbar option and run;

1. `add-migration AddPProductModelToDb` : To generate code for migration for creating table from the model class created.
2. `update-database` : To run the migration which creates the tables in the MS SQL Server. 