cat > README.md << 'EOF'
# Barber Booking System

This is a web-based booking system for a barbershop, developed using ASP.NET Core MVC and Identity. The application allows customers to register, log in, and book appointments with barbers. Each appointment includes the date, time, selected barber, and haircut type. An admin panel is included for managing barbers, services, and all reservations.

**Status: This project is currently under active development. Some features may still be incomplete or subject to change.**

## Features

### Customers

- Register and log in securely using ASP.NET Identity
- Book appointments by selecting:
  - Date and time
  - Barber
  - Haircut type (service)
- View a list of their own bookings
- Cancel their own bookings

### Admin Panel

- Add, edit, and delete barbers
- Define and manage haircut types (services)
- View and manage all reservations made by all users

## Technologies Used

- ASP.NET Core 8.0
- ASP.NET Core Identity for authentication and user management
- Entity Framework Core with Pomelo for MySQL
- Razor Views (MVC)
- MySQL as the database
- Bootstrap for styling and responsive design

## Project Structure

Controllers:
- HomeController.cs – Handles pages like login, registration, and home
- ReservaController.cs – API controller for booking operations (REST endpoints)
- ReservaViewController.cs – MVC controller for booking pages (user interface)
- AdminController.cs – Handles admin functionalities and views

Models:
- MyUser.cs – Custom user class that extends IdentityUser
- Reserva.cs – Reservation entity model
- Barbeiro.cs – Barber entity model
- Servico.cs – Haircut type or service entity model

Views:
- Home – Login, registration, and general pages
- Reserva – Booking creation and viewing (for logged-in users)
- Admin – Admin dashboard and management pages

Data:
- ApplicationDbContext.cs – Entity Framework database context

wwwroot:
- Contains static assets like CSS, JavaScript, and images

## Getting Started

Step 1 – Clone the repository:

git clone https://github.com/tonythecoder1/dotnet_barberBooking
cd barber-booking

Step 2 – Configure the database connection:

Open the file appsettings.json and update the connection string:

"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=BarberDB;user=root;password=yourpassword;"
}

Step 3 – Apply the database migrations:

Install EF Core CLI if needed:

dotnet tool install --global dotnet-ef

Then apply the migrations:

dotnet ef database update

Step 4 – Run the application:

dotnet run

Open your browser and visit:

http://localhost:5062

## Notes

- Admin features are only accessible to users with the admin role.
- Razor Views are used for rendering both the public and admin interfaces.
- Data is stored in a MySQL database using Entity Framework Core.


