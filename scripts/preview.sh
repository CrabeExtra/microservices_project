dotnet run --project ./Identity/Identity.csproj &
dotnet run --project ./Activity/Activity.csproj &
dotnet run --project ./Audit/Audit.csproj &
dotnet run --project ./Contact/Contact.csproj &
npm --prefix ./Frontend/ run start &
./nats-server &
wait