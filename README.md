# OFMAssistentverktyg
Verktyg för sluträkning Överförmyndarverksamheten (VS2022, C# 11, Windows Forms, T-SQL - Stored Procedures, Dapper)

Verktyget är till för att spara tid för boendestödjare när Överförmyndarverksamheten årligen begär in underlag för alla transaktioner som hör till en huvudman.

Programmet läser in en exporterad lista (CSV) från Handelsbanken för hela året för att sedan kategorisera och summera alla transaktionsraderna till kategorier.

Användaren kan själv mata in dessa kategorier för automatisk kategorisering vid inläsning från CSV-filen eller själv definera dem manuellt.

Därefter kan användaren med hjälp av befintliga Excelmallar sedan mata ut hela sluträkningen  till definerade celler för varje kategori.
