### SklepMvc
## Opis projektu

SklepMvc to aplikacja webowa typu sklep internetowy, wykonana w technologii ASP.NET Core MVC z wykorzystaniem Entity Framework Core oraz SQLite jako bazy danych.

Projekt został zrealizowany jako praca zaliczeniowa z przedmiotu Programowanie zaawansowane i przedstawia kompletny, działający przepływ zakupowy użytkownika – od przeglądania produktów, przez koszyk, aż do składania i przeglądania zamówień.

# Zastosowane technologie

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQLite
- Razor Views (CSHTML)
- Session (koszyk użytkownika)
- ASP.NET Identity (logowanie użytkownika)
- Git + GitHub

# Funkcjonalności aplikacji
Produkty

wyświetlanie listy produktów

dodawanie, edycja i usuwanie produktów

prezentacja ceny oraz stanu magazynowego

Koszyk (Session)

dodawanie produktów do koszyka

usuwanie pojedynczych pozycji

czyszczenie koszyka

automatyczne liczenie sumy

Zamówienia

składanie zamówienia na podstawie koszyka

zapis zamówienia i pozycji zamówienia w bazie danych

lista zamówień zalogowanego użytkownika

szczegóły zamówienia (produkty, ilości, ceny)

Użytkownicy

logowanie i wylogowanie użytkownika

przypisanie zamówień do konkretnego użytkownika

Struktura projektu

SklepMvc/
├── Controllers/
│ ├── ProductsController.cs
│ ├── CartController.cs
│ └── OrdersController.cs
├── Models/
│ ├── Product.cs
│ ├── CartItem.cs
│ ├── Order.cs
│ └── OrderItem.cs
├── Helpers/
│ └── SessionExtensions.cs
├── Views/
│ ├── Products/
│ ├── Cart/
│ └── Orders/
├── Data/
│ └── ApplicationDbContext.cs
├── Program.cs
└── app.db

Uruchomienie projektu lokalnie

1. Sklonuj repozytorium:
git clone https://github.com/krystianmarciniak/SklepMvc.git

2. Przejdź do katalogu projektu:
cd SklepMvc

3. Uruchom aplikację:
dotnet run

4. Otwórz w przeglądarce:
http://localhost:5245

Stan projektu

Projekt jest ukończony w zakresie wymaganym do zaliczenia i prezentuje:

poprawną architekturę MVC

rozdzielenie odpowiedzialności (Controllers / Models / Views)

praktyczne użycie sesji i bazy danych

działający proces zakupowy end‑to‑end