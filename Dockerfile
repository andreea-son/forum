# Stage 1: Build the project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the project files
COPY ./Forum/Forum.csproj ./Forum/
COPY ./Forum.UI/Forum.UI.csproj ./Forum.UI/

# Restore packages
RUN dotnet restore ./Forum/Forum.csproj

# Copy the rest of the files
COPY . .

# Build the solution
RUN dotnet publish -c Release -o /app

# Stage 2: Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app .

EXPOSE 80
ENTRYPOINT ["dotnet", "Forum.UI.dll"]
