por [Diego Diaz](https://github.com/diego-devs)

# YuGiOh! Creador de Decks | Deck Builder
![image](/YGODeckBuilder/wwwroot/images/deckbuilder1.png)
<p><img src="/YGODeckBuilder/wwwroot/images/logo-header-2.jpg" alt="img" width="300" /></p>

Esta solución consta de tres proyectos:

1.  **YGODeckBuilder**: Aplicación ASP.NET Core con Razor Pages. Proyecto principal. Buscador de cartas, Visualizador y Creador de Decks.
2.  **YugiohDB**: Aplicación de consola para descargar cartas y mapearlas localmente, descargar imágenes, mapear banlist y poblar la base de datos. Ahora con un menú interactivo.
3.  **YGODeckBuilder.Tests**: Proyecto de pruebas unitarias (xUnit) que cubre las capas de datos, juego, páginas, controladores de API y el proyecto YugiohDB.

## Contenido

- [YuGiOh! Creador de Decks | Deck Builder](#yugioh-creador-de-decks--deck-builder)
  - [Contenido](#contenido)
    - [Proyecto YugiohDB](#proyecto-yugiohdb)
    - [Proyecto YGODeckBuilder](#proyecto-ygodeckbuilder)
    - [Proyecto YGODeckBuilder.Tests](#proyecto-ygodeckbuildertests)
  - [Pre requisitos](#pre-requisitos)
    - [Instalación de herramientas de Entity Framework y SQL](#instalación-de-herramientas-de-entity-framework-y-sql)
  - [Pasos para ejecutar](#pasos-para-ejecutar)
  - [Ejecutar las pruebas](#ejecutar-las-pruebas)
  - [API Utilizada](#api-utilizada)
  - [Contribuciones](#contribuciones)
  - [Historial de Cambios](#historial-de-cambios)

### Proyecto YugiohDB
Aplicación de consola que centraliza las tareas de administración de datos: descarga del catálogo completo de cartas desde la API de ygoProDeck, descarga de imágenes (tamaños grande, pequeño y recortado), mapeo de rutas locales y banlist, y carga masiva de cartas a la base de datos mediante EF Core.

Al ejecutar el proyecto se muestra un menú interactivo:

```
==== YugiohDB Tool ====
[1] Download all cards from API → local JSON
[2] Download all card images (small, large, cropped)
[3] Map local image paths + banlist into cards JSON
[4] Push cards JSON into database
[5] Interactive card search
[Q] Quit
```

Las rutas (`Paths:CardIdsFilePath`, `Paths:ImagesFolder`) y la cadena de conexión (`ConnectionStrings:YGODatabase`) se configuran en `YugiohDB/appsettings.json`.

### Proyecto YGODeckBuilder
Sitio web principal. Incluye un Buscador de cartas, un Visualizador de Cartas, un Constructor de Decks (Deck Builder) y un Administrador de Decks con operaciones de guardar, renombrar, duplicar y eliminar.

### Proyecto YGODeckBuilder.Tests
Proyecto xUnit con pruebas unitarias para:

- **Datos**: `Deck`, `DeckUtility` (validación de nombres, seguridad de rutas, carga y exportación de `.ydk`), `DeckFormatConverter` (serialización JSON ↔ YDK).
- **Juego**: `Player`, `Turn`, `GameManager` y las zonas (Deck, ExtraDeck, Hand, Graveyard, Monster, MagicTrap).
- **Páginas Razor**: `MainPageModel`, `CardViewerModel`, `DecksManager`.
- **Controladores API**: `CardsController`, `DecksManagerController` (guardar, duplicar, eliminar, renombrar, crear).
- **Proveedores**: `YgoDBProvider` con base de datos EF Core In-Memory.
- **YugiohDB**: `YgoProDeckTools` — round-trip JSON, mapeo de imágenes y banlist, y `DownloadImagesAsync` usando un `HttpMessageHandler` falso para simular respuestas HTTP.

## Pre requisitos

- .NET SDK 8.0
- SQL Server local (por ejemplo, SQLEXPRESS)

### Instalación de herramientas de Entity Framework y SQL
La aplicación YugiohDB utiliza Entity Framework Core Migrations para crear y mantener la base de datos. El script `setup-database.ps1` instala `dotnet-ef` automáticamente si no está disponible, pero si prefieres hacerlo manualmente:

```bash
dotnet tool install --global dotnet-ef
dotnet ef
```

## Pasos para ejecutar

1. **Clona o haz Fork del repositorio.**

2. **Actualiza la cadena de conexión** según tu servidor local en:
    - `YGODeckBuilder/appsettings.json`
    - `YugiohDB/appsettings.json`

3. **Crea la base de datos con un comando.**

    Desde la raíz del repositorio ejecuta el script:

    ```powershell
    powershell -ExecutionPolicy Bypass -File .\setup-database.ps1
    ```

    El script:
    - Instala `dotnet-ef` como herramienta global si aún no lo está.
    - Aplica las migraciones EF Core sobre el proyecto `YugiohDB` y crea la base de datos `YgoDB` en tu servidor local.

    Si prefieres hacerlo manualmente, navega a `YugiohDB/` y ejecuta:

    ```bash
    dotnet ef database update
    ```

4. **Carga las cartas en la base de datos.**

    El repositorio incluye un snapshot del catálogo en `YGODeckBuilder/TestData/allCards.json`. Puedes usarlo directamente o descargar las cartas más recientes desde la API de ygoProDeck.

    Ejecuta el proyecto de consola:

    ```bash
    dotnet run --project YugiohDB
    ```

    En el menú interactivo:
    - `[1]` descarga el catálogo más reciente a tu archivo JSON local.
    - `[2]` descarga las imágenes (grande, pequeña y recortada).
    - `[3]` mapea las rutas locales de las imágenes y la información del banlist sobre el JSON.
    - `[4]` carga el JSON resultante en la base de datos.

    Si solo quieres poblar la base de datos usando el JSON incluido, ve directamente a la opción `[4]`.

5. **Ejecuta la aplicación web.**

    Navega al directorio del proyecto y haz build/run:

    ```bash
    dotnet run --project YGODeckBuilder
    ```

    La aplicación web estará disponible en http://localhost:5000 (o http://localhost:5001 en HTTPS).

¡Listo! Has configurado y construido la solución.

## Ejecutar las pruebas

El proyecto `YGODeckBuilder.Tests` usa xUnit, Moq y EF Core In-Memory. Desde la raíz del repositorio:

```bash
dotnet test YGODeckBuilder.Tests/YGODeckBuilder.Tests.csproj
```

Las pruebas no requieren una base de datos real ni acceso a red: toda dependencia externa se mockea o se sustituye por un proveedor en memoria.

## API Utilizada
Ambas aplicaciones utilizan la API ygoProDeck para obtener información sobre las cartas de Yu-Gi-Oh!. Puedes consultar la documentación de la API para obtener más detalles sobre su uso.
[ygoProDeck API](https://db.ygoprodeck.com/api-guide/)

## Contribuciones
¡Contribuciones y mejoras son bienvenidas! Si deseas contribuir a este proyecto, por favor sigue las pautas de contribución.
Por favor, si encuentras algún bug o error, siéntete libre de contactarme para resolverlo.

## Historial de Cambios

[Unreleased]
- Proyecto `YGODeckBuilder.Tests` con 95 pruebas unitarias cubriendo datos, juego, páginas, API, proveedores y YugiohDB.
- Script `setup-database.ps1` para crear la base de datos en un solo paso.
- YugiohDB rediseñado con un menú interactivo en lugar de bloques de código comentados.
- Fondo con gradiente oscuro y tipografía Orbitron/Rajdhani.

[0.0.2] - 01/08/2024
- Se han resuelto diversos bugs y errores visuales y de backend.
- El sitio de Deck Builder es funcional para crear y editar decks. Tiene fallos y le hacen falta características aun.

[0.0.1] - 07/09/2023
- Versión inicial del proyecto.
- Funcionalidades básicas de búsqueda y visualización de cartas.
